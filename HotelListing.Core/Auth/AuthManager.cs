using HotelListing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using HotelListing.Core.DTOs;
using HotelListing.Core.Models;
using System.Linq;

namespace HotelListing.Core.Auth
{
    public class AuthManager : IAuthManager
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        private const string TokenProfile = "HotelListingApi";
        private const string RefreshToken = "RefreshToken";

        public AuthManager(Microsoft.AspNetCore.Identity.UserManager<ApiUser> userManager, IConfiguration configuration)
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

        public async Task<string> CreateRefreshToken()
        {
            // 1. remove any authentication token
            await _userManager.RemoveAuthenticationTokenAsync(_user, TokenProfile, RefreshToken);
            // 2. Generate a new refresh token
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, TokenProfile, RefreshToken);
            // 3. Store to the DB the new token and refresh token
            var result = await _userManager.SetAuthenticationTokenAsync(_user, TokenProfile, RefreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<TokenRequest> VerifyRefreshToken(TokenRequest request)
        {

            // look at the token and extract the user
            /////////////////////////////////////////
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            // read the token
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            // get the userName from the Claims
            var userName = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == ClaimTypes.NameIdentifier)?.Value;
            // get the user nased on userName
            _user = await _userManager.FindByNameAsync(userName);

            /////////////////////////////////////////
            // check if the existing refresh token is valid for this user
            var isValid = await _userManager.VerifyUserTokenAsync(_user, TokenProfile, RefreshToken, request.RefreshToken);
            if (isValid)
            {
                return new TokenRequest
                {
                    Token = await CreateToken(),
                    RefreshToken = await CreateRefreshToken()
                };
            }
            // if not valid then sign the user out
            await _userManager.UpdateSecurityStampAsync(_user);

            return null;
        }
    }
}
