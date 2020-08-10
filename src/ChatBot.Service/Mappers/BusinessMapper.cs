using ChatBot.Business.Contracts.User.Models;
using ChatBot.Repository.Contracts.Models;
using ChatBot.Service.Models;

namespace ChatBot.Service.Mappers
{
    public static class BusinessMapper
    {
        public static UserModel Map(this CreateUserRequest user)
        {
            return new UserModel
            {
                Name = user.Name,
                Email = user.Email,
                Department = user.Department,
                IsAdmin = user.IsAdmin,
                Password = user.Password
            };
        }

        public static CreateUserResponse Map(this UserModel user)
        {
            return new CreateUserResponse
            {
                IsAdmin = user.IsAdmin,
                Email = user.Email,
                Department = user.Department,
                Name = user.Name,
                Uid = user.Uid
            };
        }

        public static ChatMessageResponse Map(this ChatMessage message)
        {
            return new ChatMessageResponse
            {
                CreateDate = message.CreateDate,
                ConversationFinished = message.ConversationFinished,
                Message = message.Message,
                Recipient = message.Recipient
            };
        }
    }
}