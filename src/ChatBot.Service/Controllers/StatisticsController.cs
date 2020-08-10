using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.Statistics;
using ChatBot.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace ChatBot.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsBusiness _statisticsBusiness;
        private readonly ILogger<StatisticsController> _logger;

        public StatisticsController(IStatisticsBusiness statisticsBusiness, ILogger<StatisticsController> logger)
        {
            statisticsBusiness.ShouldNotBeNull();
            logger.ShouldNotBeNull();

            _statisticsBusiness = statisticsBusiness;
            _logger = logger;
        }

        /// <summary>
        ///     Anonymizes and safes the provided feedback
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        [HttpPost("feedback")]
        [Authorize]
        public async Task<IActionResult> HandleUserFeedbackAsync([FromBody] UserFeedbackRequest feedback)
        {
            try
            {
                _logger.LogInformation("Trying to post user feedback");
                var currentUserId = GetCurrentUserId();
                await _statisticsBusiness.RegisterUserFeedback(currentUserId, feedback.Rating);
                return NoContent();
            }
            catch (ShouldAssertException ex)
            {
                _logger.LogWarning(ex, "Bad request while posting feedback");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected Error occured while posting user feedback.");
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
