using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Repository.Contracts;
using ChatBot.Repository.Contracts.Models;
using ChatBot.Repository.MongoDb.Configurations;
using ChatBot.Repository.MongoDb.Models;
using MongoDB.Driver;
using Shouldly;

namespace ChatBot.Repository.MongoDb
{
    /// <inheritdoc cref="IDepartmentRepository" />
    public class DepartmentRepository : RepositoryBase<InternalDepartment>, IDepartmentRepository
    {
        public DepartmentRepository(MongoDbConfiguration configuration) : base(configuration)
        {
            configuration.ShouldNotBeNull();
        }

        public bool DepartmentExists(string departmentName)
        {
            return Collection
                .Find(department => department.DepartmentName == departmentName)
                .Any();
        }

        public async Task<Department> CreateDepartmentAsync(string departmentName)
        {
            var newDepartment = new InternalDepartment
            {
                DepartmentName = departmentName
            };
            await Collection.InsertOneAsync(newDepartment).ConfigureAwait(false);

            return newDepartment.Map();
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            var departments = await Collection
                .FindAsync(department => true)
                .ConfigureAwait(false);
            return departments
                .ToList()
                .Select(department => department.Map());
        }
    }
}