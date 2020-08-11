using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.MasterData;
using ChatBot.Business.Contracts.MasterData.Exceptions;
using ChatBot.Business.Contracts.MasterData.Models;
using ChatBot.MessageInterpreter.Contract;
using ChatBot.Repository.Contracts;
using Shouldly;

namespace ChatBot.Business
{
    public class MasterDataBusiness : IMasterDataBusiness
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IKnowledgeRepository _knowledgeRepository;
        private readonly IMessageInterpreter _messageInterpreter;

        public MasterDataBusiness(IDepartmentRepository departmentRepository, IKnowledgeRepository knowledgeRepository,
            IMessageInterpreter messageInterpreter)
        {
            departmentRepository.ShouldNotBeNull();
            knowledgeRepository.ShouldNotBeNull();
            messageInterpreter.ShouldNotBeNull();

            _departmentRepository = departmentRepository;
            _knowledgeRepository = knowledgeRepository;
            _messageInterpreter = messageInterpreter;
        }

        public async Task<DepartmentModel> CreateDepartmentAsync(string departmentName)
        {
            try
            {
                departmentName.ShouldNotBeNullOrWhiteSpace();

                if (_departmentRepository
                    .DepartmentExists(departmentName))
                    throw new DepartmentAlreadyExistsException();

                var newDepartment = await _departmentRepository
                    .CreateDepartmentAsync(departmentName)
                    .ConfigureAwait(false);

                return newDepartment.Map();
            }
            catch (ShouldAssertException)
            {
                throw new MissingDataException();
            }
        }

        public async Task<DataSchemaModel> GetSchema(MasterDataType type)
        {
            await RefreshDepartmentSchemeAsync();
            return type switch
            {
                MasterDataType.Department => DataSchemaModel.Department,
                MasterDataType.Knowledge => DataSchemaModel.Knowledge,
                _ => throw new ArgumentException("Unknown department")
            };
        }

        public async Task<IEnumerable<KnowledgeModel>> GetKnowledgeBaseAsync()
        {
            var knownDefinitionsTask = _knowledgeRepository.GetAllDefinitionsAsync();
            var knownTermsTask = _messageInterpreter.GetAllKnownTermsAsync();
            await Task.WhenAll(knownTermsTask, knownDefinitionsTask);

            return knownDefinitionsTask.Result
                .Join(knownTermsTask.Result, knownDefinition => knownDefinition.Keyword,
                    knowledgeTerm => knowledgeTerm.Keyword,
                    (knownDefinition, knowledgeTerm) => new KnowledgeModel
                    {
                        Description = knownDefinition.Description,
                        DefinitionType = knownDefinition.DefinitionType,
                        Keywords = knowledgeTerm.Synonyms,
                        Name = knownDefinition.Keyword
                    });
        }

        private async Task RefreshDepartmentSchemeAsync()
        {
            var departments = await _departmentRepository.GetAllDepartmentsAsync().ConfigureAwait(false);
            var departmentNames = departments.Select(department => department.DepartmentName);

            var options = DataSchemaModel.Department.Fields.First(field =>
                field.Key.Equals("departmentName", StringComparison.InvariantCultureIgnoreCase)).Options;
            options.Clear();
            options.AddRange(departmentNames);
        }
    }
}