using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjName.Application.Interfaces.Auth
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string userId, string email, string fullName);
    }
}
