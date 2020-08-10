using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot.Business;
using ChatBot.Business.Contracts.MasterData;
using ChatBot.Business.Contracts.MasterData.Exceptions;
using ChatBot.Business.Contracts.MasterData.Models;
using ChatBot.Business.Contracts.User;
using ChatBot.Repository.Contracts;
using ChatBot.Repository.Contracts.Models;
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
    public class MasterDataController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<MasterDataController> _logger;
        private readonly IMasterDataBusiness _masterDataBusiness;
        private readonly IUserBusiness _userBusiness;

        public MasterDataController(IMasterDataBusiness masterDataBusiness, ILogger<MasterDataController> logger,
            IUserBusiness userBusiness, IDepartmentRepository departmentRepository)
        {
            masterDataBusiness.ShouldNotBeNull();
            userBusiness.ShouldNotBeNull();
            logger.ShouldNotBeNull();
            departmentRepository.ShouldNotBeNull();

            _masterDataBusiness = masterDataBusiness;
            _logger = logger;
            _userBusiness = userBusiness;
            _departmentRepository = departmentRepository;
        }

        /// <summary>
        ///     Creates a new department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("data/departments")]
        [ProducesResponseType(typeof(DepartmentResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmentRequest department)
        {
            try
            {
                _logger.LogInformation("Creating new department");

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate user data.");
                    return Unauthorized();
                }

                var newUser = await _masterDataBusiness
                    .CreateDepartmentAsync(department.DepartmentName)
                    .ConfigureAwait(false);

                return Ok(newUser.Map());
            }
            catch (DepartmentAlreadyExistsException)
            {
                _logger.LogInformation("Could not create department because it already exists");
                return BadRequest();
            }
            catch (MissingDataException)
            {
                _logger.LogInformation("Could not create department due to missing data");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating new user");
                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Returns all registered departments
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("data/departments")]
        [ProducesResponseType(typeof(IEnumerable<Department>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllDepartmentsAsync()
        {
            try
            {
                _logger.LogInformation("Loading all departments");

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate user data.");
                    return Unauthorized();
                }

                var departments = await _departmentRepository
                    .GetAllDepartmentsAsync()
                    .ConfigureAwait(false);

                return Ok(departments.Select(department => department.Map()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not load all departments");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet("scheme/{type}")]
        [ProducesResponseType(typeof(MasterDataSchema), 200)]
        public async Task<IActionResult> GetSchemaAsync([FromRoute] string type)
        {
            try
            {
                _logger.LogInformation("Trying to get scheme for master data type");
                var requestedType = ParseMasterDataType(type);

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate user data.");
                    return Unauthorized();
                }

                var schema = await _masterDataBusiness.GetSchema(requestedType);
                return Ok(schema.Map());
            }
            catch (ArgumentOutOfRangeException)
            {
                _logger.LogInformation("Got invalid master data type");
                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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

        private static MasterDataType ParseMasterDataType(string type)
        {
            if (type.Equals("knowledge", StringComparison.InvariantCultureIgnoreCase))
                return MasterDataType.Knowledge;
            if (type.Equals("departments", StringComparison.InvariantCultureIgnoreCase))
                return MasterDataType.Department;
            throw new ArgumentOutOfRangeException();
        }
    }
}