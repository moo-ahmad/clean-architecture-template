using ProjName.Application.Interfaces.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjName.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request.FirstName, request.LastName, request.Email, request.Password);
            return await Task.FromResult(new AuthResponseDto
            {
                Token = result.Token,
                Email = result.Email,
                FullName = result.FullName
            });
        }
    }
}
