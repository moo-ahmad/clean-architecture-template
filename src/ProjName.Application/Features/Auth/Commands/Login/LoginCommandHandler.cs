using ProjName.Application.Interfaces.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjName.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IAuthService _authService;
        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);
            if (result == null)
                throw new UnauthorizedAccessException("Login failed. Please check your credentials.");

            return new AuthResponseDto
            {
                Token = result.Token,
                Email = result.Email,
                FullName = result.FullName
            };
        }
    }
}
