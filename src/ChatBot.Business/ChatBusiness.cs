using System;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.Chat;
using ChatBot.MessageInterpreter.Contract;
using ChatBot.Repository.Contracts;
using ChatBot.Repository.Contracts.Models;
using Shouldly;

namespace ChatBot.Business
{
    public class ChatBusiness : IChatBusiness
    {
        private readonly IMessageInterpreter _messageInterpreter;
        private readonly IChatRepository _chatRepository;

        public ChatBusiness(IMessageInterpreter messageInterpreter, IChatRepository chatRepository)
        {
            messageInterpreter.ShouldNotBeNull();
            chatRepository.ShouldNotBeNull();

            _messageInterpreter = messageInterpreter;
            _chatRepository = chatRepository;
        }

        public async Task HandleMessageAsync(string message, string userId)
        {
            var messageInterpretation = await _messageInterpreter
                .InterpretMessageAsync(message, userId)
                .ConfigureAwait(false);

            if (messageInterpretation.IsCompleted)
            {
                return;
            }

            await _chatRepository.SendMessageAsync(new ChatMessage
            {
                ConversationFinished = false,
                CreateDate = DateTime.Now,
                Message = messageInterpretation.AnswerString,
                Recipient = userId
            }).ConfigureAwait(false);
        }
    }
}