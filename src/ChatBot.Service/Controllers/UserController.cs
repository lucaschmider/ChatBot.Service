using System;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.User;
using ChatBot.Business.Contracts.User.Models;
using ChatBot.Repository.Contracts;
using ChatBot.Service.Mappers;
using ChatBot.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace ChatBot.Service.Controllers
{
    /// <summary>
    ///     Provided methods to manage users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserBusiness _userBusiness;
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger,
            IUserBusiness userBusiness)
        {
            userRepository.ShouldNotBeNull();
            logger.ShouldNotBeNull();
            userBusiness.ShouldNotBeNull();

            _userRepository = userRepository;
            _logger = logger;
            _userBusiness = userBusiness;
        }

        /// <summary>
        ///     Returns details about the user that is currently signed in
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(UserDetails), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserDataAsync()
        {
            try
            {
                _logger.LogInformation("Getting user details");
                var userId = User.Claims.FirstOrDefault(claim => claim.Type == "user_id")?.Value;

                if (userId == null)
                {
                    _logger.LogWarning("Could not determine userid from token");
                    return BadRequest();
                }


                var userData = await _userRepository.GetUserDetailsAsync(userId);

                if (userData != null) return Ok(userData);

                _logger.LogWarning($"Could not find data for uid {userId}.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occured while getting user details", ex);
                return StatusCode(500);
            }
        }


        /// <summary>
        ///     Creates a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CreateUserResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest user)
        {
            try
            {
                _logger.LogInformation("Trying to create a new user.");

                var newUserModel = await _userBusiness
                    .CreateUserAsync(user.Map())
                    .ConfigureAwait(false);

                return Ok(newUserModel.Map());
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occured while creating a new user.", ex);
                return StatusCode(500);
            }
        }
    }
}