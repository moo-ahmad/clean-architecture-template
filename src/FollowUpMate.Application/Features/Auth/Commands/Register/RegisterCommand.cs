using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpMate.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand(string Email, string Password, string FirstName, string LastName) : IRequest<AuthResponseDto>
    {
    }
}
