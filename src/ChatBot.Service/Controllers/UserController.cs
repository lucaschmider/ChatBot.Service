using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.AuthProvider.Contract.Exceptions;
using ChatBot.Business.Contracts.User;
using ChatBot.Business.Contracts.User.Exceptions;
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
        [HttpGet("/details")]
        [Authorize]
        [ProducesResponseType(typeof(UserDetails), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserDataAsync()
        {
            try
            {
                _logger.LogInformation("Getting user details");
                var userId = GetCurrentUserId();

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

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate user data.");
                    return Unauthorized();
                }

                var newUserModel = await _userBusiness
                    .CreateUserAsync(user.Map())
                    .ConfigureAwait(false);

                return Ok(newUserModel.Map());
            }
            catch (InvalidUserDataException ex)
            {
                _logger.LogWarning(ex, "The specified object contained errors", user);
                return BadRequest();
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.LogWarning("The user that was tried to create already exists", ex);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occured while creating a new user.", ex);
                return StatusCode(500);
            }
        }


        /// <summary>
        ///     Deletes the user specified in the route
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string userId)
        {
            try
            {
                _logger.LogInformation("Trying to delete user");

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate user data.");
                    return Unauthorized();
                }

                var userData = await _userRepository
                    .GetAllUsersAsync()
                    .ConfigureAwait(false);

                return Ok(userData);
            }
            catch (ShouldAssertException)
            {
                _logger.LogInformation("No userId was specified");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Unexpected error occured while deleting a user", userId);
                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Returns a list of all registered users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<UserDetails>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Trying to get all users data");

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate user data.");
                    return Unauthorized();
                }

                var allUsers = await _userRepository
                    .GetAllUsersAsync()
                    .ConfigureAwait(false);

                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occured while getting all users");
                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Returns the id of the current user id
        /// </summary>
        /// <returns></returns>
        private string GetCurrentUserId()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == "user_id")?.Value;
            userId.ShouldNotBeNullOrWhiteSpace();
            return userId;
        }
    }
}