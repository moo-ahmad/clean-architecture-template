<#
.SYNOPSIS
    Renames a FollowUpMate clean-architecture template to a new project name.

.DESCRIPTION
    Replaces namespaces, usings, project references, and configuration values, then
    renames solution/projects/folders/files that contain the template name.
    Run from the repository root after cloning the template.

.PARAMETER NewName
    PascalCase project name (e.g. AcmeBilling). Used for assemblies, namespaces,
    folders, and the SQL catalog name in appsettings.

.PARAMETER OldName
    Template name to replace. Default: FollowUpMate

.PARAMETER SourceRoot
    Folder containing the .sln (default: ./src next to this script)

.PARAMETER WhatIf
    Shows planned changes without modifying files.

.EXAMPLE
    .\Rename-Project.ps1 -NewName AcmeBilling

.EXAMPLE
    .\Rename-Project.ps1 -NewName MyApp -OldName FollowUpMate -WhatIf
#>
[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern('^[A-Za-z][A-Za-z0-9]*$')]
    [string]$NewName,

    [ValidatePattern('^[A-Za-z][A-Za-z0-9]*$')]
    [string]$OldName = 'FollowUpMate',

    [string]$SourceRoot = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if ($OldName -eq $NewName) {
    throw "NewName '$NewName' must differ from OldName '$OldName'."
}

if ([string]::IsNullOrWhiteSpace($SourceRoot)) {
    $candidate = Join-Path $PSScriptRoot 'src'
    if (-not (Test-Path -LiteralPath $candidate)) {
        throw "Source root not found. Pass -SourceRoot pointing at the folder that contains the .sln file."
    }
    $SourceRoot = (Resolve-Path -LiteralPath $candidate).Path
}
else {
    if (-not (Test-Path -LiteralPath $SourceRoot)) {
        throw "SourceRoot does not exist: $SourceRoot"
    }
    $SourceRoot = (Resolve-Path -LiteralPath $SourceRoot).Path
}

$excludeDirectoryNames = [System.Collections.Generic.HashSet[string]]::new(
    [string[]]@('bin', 'obj', '.git', '.vs', 'node_modules'),
    [System.StringComparer]::OrdinalIgnoreCase
)

$textExtensions = [System.Collections.Generic.HashSet[string]]::new(
    [string[]]@(
        '.cs', '.csproj', '.sln', '.json', '.http', '.md', '.xml',
        '.props', '.targets', '.config', '.editorconfig', '.runsettings', '.user'
    ),
    [System.StringComparer]::OrdinalIgnoreCase
)

# Longest match first so "FollowUpMate.Application" is not partially corrupted.
$replacements = @(
    @{ Old = "${OldName}.Infrastructure"; New = "${NewName}.Infrastructure" }
    @{ Old = "${OldName}.Application";    New = "${NewName}.Application" }
    @{ Old = "${OldName}.Domain";        New = "${NewName}.Domain" }
    @{ Old = "${OldName}.API";            New = "${NewName}.API" }
    @{ Old = "${OldName}_";               New = "${NewName}_" }
    @{ Old = $OldName;                    New = $NewName }
)

function Test-ExcludedPath {
    param([string]$FullPath)
    foreach ($segment in $FullPath.Split([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar)) {
        if ($excludeDirectoryNames.Contains($segment)) {
            return $true
        }
    }
    return $false
}

function Get-RenameCandidates {
    param([string]$Root)
    Get-ChildItem -LiteralPath $Root -Recurse -Force -ErrorAction SilentlyContinue |
        Where-Object {
            -not (Test-ExcludedPath $_.FullName) -and
            $_.Name -like "*$OldName*"
        } |
        Sort-Object { $_.FullName.Length } -Descending
}

function Invoke-TextReplace {
    param([string]$Content)
    foreach ($pair in $replacements) {
        $Content = $Content.Replace($pair.Old, $pair.New)
    }
    return $Content
}

Write-Host "Template: $OldName -> $NewName"
Write-Host "Source:   $SourceRoot"
Write-Host ''

# --- Phase 1: file contents ---
$files = Get-ChildItem -LiteralPath $SourceRoot -Recurse -File -Force |
    Where-Object {
        -not (Test-ExcludedPath $_.FullName) -and
        $textExtensions.Contains($_.Extension)
    }

$contentUpdates = 0
foreach ($file in $files) {
    $original = [IO.File]::ReadAllText($file.FullName)
    $updated = Invoke-TextReplace -Content $original
    if ($updated -eq $original) { continue }

    $contentUpdates++
    if ($PSCmdlet.ShouldProcess($file.FullName, 'Update file content')) {
        $utf8NoBom = New-Object System.Text.UTF8Encoding($false)
        [IO.File]::WriteAllText($file.FullName, $updated, $utf8NoBom)
    }
}

Write-Host "Content: $contentUpdates file(s) would be updated."

# --- Phase 2: paths (deepest first) ---
$pathRenames = 0
foreach ($item in (Get-RenameCandidates -Root $SourceRoot)) {
    $targetName = $item.Name.Replace($OldName, $NewName)
    if ($targetName -eq $item.Name) { continue }

    $parentPath = Split-Path -Parent $item.FullName
    $targetPath = Join-Path $parentPath $targetName
    $pathRenames++

    if ($PSCmdlet.ShouldProcess($item.FullName, "Rename to $targetName")) {
        if (Test-Path -LiteralPath $targetPath) {
            throw "Cannot rename '$($item.FullName)' -> '$targetPath': target already exists."
        }
        Rename-Item -LiteralPath $item.FullName -NewName $targetName
    }
}

Write-Host "Paths:   $pathRenames file(s)/folder(s) would be renamed."
Write-Host ''

if ($WhatIfPreference) {
    Write-Host 'Dry run complete. Re-run without -WhatIf to apply changes.'
}
else {
    Write-Host 'Done. Open the renamed .sln and run: dotnet build'
}
