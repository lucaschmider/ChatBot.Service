using System;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Repository.Contracts;
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
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            userRepository.ShouldNotBeNull();
            logger.ShouldNotBeNull();
            _userRepository = userRepository;
            _logger = logger;
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
    }
}