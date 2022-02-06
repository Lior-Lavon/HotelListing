using HotelListing.Data;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Configuration;
using System.Text;

namespace HotelListing.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;
        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> ValidateUser(LoginUserDTO userDto)
        {
            _user = await _userManager.FindByNameAsync(userDto.Email);
            // check if user (from db) is valid and password (passed from login) is correct
            return (_user != null) && await _userManager.CheckPasswordAsync(_user, userDto.Password);    
        }

        public async Task<string> CreateToken()
        {
            // create signing credentials
            SigningCredentials signingCredentials = GetSigningCredentials();
            // get cleims
            var claims = await GetClaims();
            // add to the token options
            var token = GenerateTokenOptions(signingCredentials, claims);

            // serielise the token object
            return new JwtSecurityTokenHandler().WriteToken((SecurityToken)token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, _user.Email)
                    };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        // combine the signing credentials and the claims and create the actual tokens
        private object GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            // define the token object
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signingCredentials);

            return token;
        }

    }
}
