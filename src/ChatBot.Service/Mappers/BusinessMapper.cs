using System;
using System.Linq;
using ChatBot.Business.Contracts.MasterData.Models;
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

        public static DepartmentResponse Map(this DepartmentModel department)
        {
            return new DepartmentResponse
            {
                DepartmentName = department.DepartmentName,
                _id = department.DepartmentId
            };
        }

        public static MasterDataSchema Map(this DataSchemaModel model)
        {
            return new MasterDataSchema
            {
                Collection = model.Collection,
                Fields = model.Fields.Select(field => field.Map())
            };
        }

        public static MasterDataSchema.DataFieldModel Map(this DataSchemaModel.DataFieldModel model)
        {
            return new MasterDataSchema.DataFieldModel
            {
                Name = model.Name,
                Options = model.Options,
                Type = model.Type.Map(),
                Key = model.Key
            };
        }

        public static string Map(this DataSchemaModel.DataFieldModel.DataFieldType type)
        {
            return type switch
            {
                DataSchemaModel.DataFieldModel.DataFieldType.Options => "options",
                DataSchemaModel.DataFieldModel.DataFieldType.Text => "text",
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        public static KnowledgeResponse Map(this KnowledgeModel knowledge)
        {
            return new KnowledgeResponse
            {
                Description = knowledge.Description,
                DefinitionType = knowledge.DefinitionType,
                Keywords = knowledge.Keywords,
                Name = knowledge.Name
            };
        }
    }
}