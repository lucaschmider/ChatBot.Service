using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.Chat;
using ChatBot.Repository.Contracts;
using ChatBot.Service.Mappers;
using ChatBot.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace ChatBot.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatBusiness _chatBusiness;
        private readonly IChatRepository _chatRepository;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatRepository chatRepository, ILogger<ChatController> logger,
            IChatBusiness chatBusiness)
        {
            chatRepository.ShouldNotBeNull();
            logger.ShouldNotBeNull();
            chatBusiness.ShouldNotBeNull();

            _chatRepository = chatRepository;
            _logger = logger;
            _chatBusiness = chatBusiness;
        }

        /// <summary>
        ///     Returns new chat messages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<ChatMessageResponse>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetChatMessagesAsync()
        {
            try
            {
                _logger.LogInformation("Loading new messages");
                var userId = GetCurrentUserId();
                var messages = await _chatRepository
                    .GetChatMessagesForUserAndDeleteAsync(userId)
                    .ConfigureAwait(false);
                return Ok(messages.Select(message => message.Map()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occured while getting new messages");
                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Handles incoming messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> HandleIncomingMessageAsync([FromBody] ChatMessageRequest message)
        {
            try
            {
                await _chatBusiness.HandleMessageAsync(message.Message, GetCurrentUserId())
                    .ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occured while creating new message");
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