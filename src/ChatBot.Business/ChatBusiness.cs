using System;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.Chat;
using ChatBot.MessageInterpreter.Contract;
using ChatBot.MessageInterpreter.Contract.Models;
using ChatBot.Repository.Contracts;
using ChatBot.Repository.Contracts.Models;
using Shouldly;

namespace ChatBot.Business
{
    public class ChatBusiness : IChatBusiness
    {
        private readonly IChatRepository _chatRepository;
        private readonly IKnowledgeRepository _knowledgeRepository;
        private readonly IMessageInterpreter _messageInterpreter;

        public ChatBusiness(IMessageInterpreter messageInterpreter, IChatRepository chatRepository, IKnowledgeRepository knowledgeRepository)
        {
            messageInterpreter.ShouldNotBeNull();
            chatRepository.ShouldNotBeNull();
            knowledgeRepository.ShouldNotBeNull();

            _messageInterpreter = messageInterpreter;
            _chatRepository = chatRepository;
            _knowledgeRepository = knowledgeRepository;
        }

        public async Task HandleMessageAsync(string message, string userId)
        {
            var messageInterpretation = await _messageInterpreter
                .InterpretMessageAsync(message, userId)
                .ConfigureAwait(false);
            
            await _chatRepository.SendMessageAsync(new ChatMessage
            {
                ConversationFinished = messageInterpretation.IsCompleted,
                CreateDate = DateTime.Now,
                Message = await GetAnswerAsync(messageInterpretation),
                Recipient = userId
            }).ConfigureAwait(false);
        }

        private async Task<string> GetAnswerAsync(InterpretationResult interpretation)
        {
            if (interpretation.IsCompleted)
            {
                return   await _knowledgeRepository.GetDefinitionAsync(
                        interpretation.Parameters["definitiontype"],
                        interpretation.Parameters["keyword"])
                    .ConfigureAwait(false) ?? "Entschuldige, das kann ich leider nicht erklären!";
            }

            return interpretation.AnswerString;
        }
    }
}