using HotelListing.Core.DTOs;
using HotelListing.Core.Models;
using System.Threading.Tasks;

namespace HotelListing.Core.Auth
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO userDto);
        Task<string> CreateToken();
        Task<string> CreateRefreshToken();
        Task<TokenRequest> VerifyRefreshToken(TokenRequest request);
    }
}
