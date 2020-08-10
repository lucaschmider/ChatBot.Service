using System;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.Statistics;
using ChatBot.Service.Mappers;
using ChatBot.Service.Models;
using ChatBot.StatisticsProvider.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace ChatBot.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IStatisticsBusiness _statisticsBusiness;
        private readonly IStatisticsProvider _statisticsProvider;

        public StatisticsController(IStatisticsBusiness statisticsBusiness, ILogger<StatisticsController> logger,
            IStatisticsProvider statisticsProvider)
        {
            statisticsBusiness.ShouldNotBeNull();
            logger.ShouldNotBeNull();
            statisticsProvider.ShouldNotBeNull();

            _statisticsBusiness = statisticsBusiness;
            _logger = logger;
            _statisticsProvider = statisticsProvider;
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
        ///     Loads user satisfaction 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserSatisfactionAsync()
        {
            try
            {
                _logger.LogInformation("Loading all available user feedback");
                var statisticsData = await _statisticsProvider.GetFeedbackAsync();
                return Ok(statisticsData.Select(report => report.Map()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected Error occured while loading user feedback.");
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