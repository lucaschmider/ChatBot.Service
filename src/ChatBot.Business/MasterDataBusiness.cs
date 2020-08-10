using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.MasterData;
using ChatBot.Business.Contracts.MasterData.Exceptions;
using ChatBot.Business.Contracts.MasterData.Models;
using ChatBot.Repository.Contracts;
using Shouldly;

namespace ChatBot.Business
{
    public class MasterDataBusiness : IMasterDataBusiness
    {
        private readonly IDepartmentRepository _departmentRepository;

        public MasterDataBusiness(IDepartmentRepository departmentRepository)
        {
            departmentRepository.ShouldNotBeNull();
            _departmentRepository = departmentRepository;
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

        public Task<IEnumerable<DepartmentModel>> GetDepartmentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteDepartmentAsync(string departmentId)
        {
            throw new NotImplementedException();
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