using AutoMapper;
using HotelListing.Core.Auth;
using HotelListing.Core.DTOs;
using HotelListing.Core.Models;
using HotelListing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // From Identity Core
        private readonly UserManager<ApiUser> _userManager;

        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;

        // Authentication Manager
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<ApiUser> userManager,
            ILogger<AccountController> logger, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        // Registration anew user
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration attempt for {userDTO.Email}");
            if (!ModelState.IsValid) // check validation
                return BadRequest(ModelState);

            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = user.Email; // set the email ass userName
                var result = await _userManager.CreateAsync(user, userDTO.Password); // register in db
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return BadRequest(ModelState);
                }

                // set the User-Role
                await _userManager.AddToRolesAsync(user, userDTO.Roles);

                return Accepted(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return StatusCode(500, "Internal Server Error !");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            _logger.LogInformation($"Login attempt for {loginUserDTO.Email}");
            if (!ModelState.IsValid) // check validation
                return BadRequest(ModelState);

            try
            {
                if (!await _authManager.ValidateUser(loginUserDTO))
                    return Unauthorized(loginUserDTO); // 401

                return Accepted(new TokenRequest { Token = await _authManager.CreateToken(),  // add token
                                      RefreshToken = await _authManager.CreateRefreshToken() }); // add refresh token
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
                return StatusCode(500, "Internal Server Error !");
            }
        }

        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
        {
            var tokenRequest = await _authManager.VerifyRefreshToken(request);
            if(tokenRequest == null)
            {
                return Unauthorized();
            }
            return Ok(tokenRequest);
        }

    }
}
