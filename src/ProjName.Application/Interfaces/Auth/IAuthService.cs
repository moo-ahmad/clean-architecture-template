using ProjName.Application.Features.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjName.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(string firstName, string lastName, string email, string password);
        Task<AuthResponseDto> LoginAsync(string email, string password);
    }
}
