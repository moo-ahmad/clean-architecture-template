using ProjName.Application.Features.Auth;
using ProjName.Application.Interfaces.Auth;
using ProjName.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjName.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenGenerator jwtTokenGenerator) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ApplicationException("Invalid credentials.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new ApplicationException("Invalid credentials.");

            var token = _jwtTokenGenerator.GenerateToken(user.Id.ToString(), user.Email, user.FullName);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new ApplicationException(string.Join(", ", result.Errors.Select(e => e.Description)));

            var token = _jwtTokenGenerator.GenerateToken(user.Email, user.Email, user.FullName);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName
            };
        }
    }
}
