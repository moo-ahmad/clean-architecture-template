using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpMate.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Here you would typically call your user service to register the user
            // and generate a token. For now, we will return a dummy response.
            return await Task.FromResult(new AuthResponseDto
            {
                Token = "dummy-token",
                Email = request.Email,
                FullName = $"{request.FirstName} {request.LastName}"
            });
        }
    }
}
