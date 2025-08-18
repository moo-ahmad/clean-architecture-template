using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowUpMate.Application.Features.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
