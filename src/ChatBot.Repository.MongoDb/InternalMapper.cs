using ChatBot.Repository.Contracts.Models;
using ChatBot.Repository.MongoDb.Models;

namespace ChatBot.Repository.MongoDb
{
    public static class InternalMapper
    {
        public static User Map(this InternalUser user)
        {
            return new User
            {
                Department = user.Department,
                IsAdmin = user.IsAdmin,
                Name = user.Name,
                Email = user.Email,
                Uid = user.Uid
            };
        }

        public static InternalUser Map(this User user)
        {
            return new InternalUser
            {
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                Name = user.Name,
                Department = user.Department,
                Uid = user.Uid
            };
        }

        public static ChatMessage Map(this InternalMessage message)
        {
            return new ChatMessage
            {
                CreateDate = message.CreateDate,
                Message = message.Message,
                Recipient = message.Recipient,
                ConversationFinished = message.ConversationFinished
            };
        }

        public static InternalMessage Map(this ChatMessage message)
        {
            return new InternalMessage
            {
                CreateDate = message.CreateDate,
                Message = message.Message,
                Recipient = message.Recipient,
                ConversationFinished = message.ConversationFinished
            };
        }

        public static Department Map(this InternalDepartment department)
        {
            return new Department
            {
                DepartmentName = department.DepartmentName,
                DepartmentId = department.DepartmentId
            };
        }
    }
}