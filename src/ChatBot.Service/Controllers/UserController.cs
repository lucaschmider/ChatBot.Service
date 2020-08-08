using System.Linq;
using System.Threading.Tasks;
using ChatBot.Repository.Contracts;
using ChatBot.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Service.Controllers
{
    /// <summary>
    ///     Provided methods to manage users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == "user_id")?.Value;
            var userData = await _userRepository.GetUserDetailsAsync(userId);
            return Ok(userData);
        }
    }
}