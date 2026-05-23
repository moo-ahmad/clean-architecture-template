using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjName.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>
    {
    }
}
