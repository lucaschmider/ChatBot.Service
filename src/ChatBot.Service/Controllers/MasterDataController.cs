using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate department data.");
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
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate department data.");
                    return Unauthorized();
                }

                var departments = await _departmentRepository
                    .GetAllDepartmentsAsync()
                    .ConfigureAwait(false);
                var response = departments.Select(RepositoryMapper.Map);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not load all departments");
                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Returns the schema for the specified model
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("scheme/{type}")]
        [ProducesResponseType(typeof(MasterDataSchema), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
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
                    _logger.LogInformation($"User with id {currentUserId} tried to get data illegally.");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not load schema");
                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Deletes the specified department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("data/departments/{departmentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteDepartmentAsync([FromRoute] string departmentId)
        {
            try
            {
                _logger.LogInformation($"Deleting department with id {departmentId}");

                if (string.IsNullOrWhiteSpace(departmentId))
                {
                    _logger.LogInformation("DepartmentId was invalid");
                    return NotFound();
                }

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate department data.");
                    return Unauthorized();
                }

                await _departmentRepository
                    .DeleteDepartmentAsync(departmentId)
                    .ConfigureAwait(false);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occured while deleting a user");
                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Returns all known terms with explanations
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("data/knowledge")]
        [ProducesResponseType(typeof(IEnumerable<KnowledgeResponse>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetKnowledgeAsync()
        {
            try
            {
                _logger.LogInformation("Loading knowledge base");

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate knowledge data.");
                    return Unauthorized();
                }

                var knowledgeBase = await _masterDataBusiness
                    .GetKnowledgeBaseAsync()
                    .ConfigureAwait(false);

                return Ok(knowledgeBase.Select(knowledge => knowledge.Map()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting knowledge base");
                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Deletes a term from the knowledge base
        /// </summary>
        /// <param name="definitionType"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("data/knowledge/{definitionType}/{keyword}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteKnowledgeAsync(string definitionType, string keyword)
        {
            try
            {
                _logger.LogInformation($"Trying to delete term ({definitionType} / {keyword})");

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate knowledge data.");
                    return Unauthorized();
                }

                await _masterDataBusiness.DeleteTermAsync(definitionType, keyword);
                return NoContent();
            }
            catch (MissingDataException ex)
            {
                _logger.LogWarning(ex, "The user provided insufficient information");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occured while deleting term");
                return StatusCode(500);
            }
        }

        /// <summary>
        ///     Creates the specified knowledge
        /// </summary>
        /// <param name="knowledge"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("data/knowledge")]
        [ProducesResponseType(typeof(KnowledgeResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateKnowledgeAsync([FromBody] CreateKnowledgeRequest knowledge)
        {
            try
            {
                _logger.LogInformation("Trying to create knowledge");

                var currentUserId = GetCurrentUserId();
                var isRequestingUserAdmin = await _userBusiness
                    .CheckAdminPrivilegesAsync(currentUserId)
                    .ConfigureAwait(false);

                if (!isRequestingUserAdmin)
                {
                    _logger.LogInformation($"User with id {currentUserId} tried to manipulate knowledge data.");
                    return Unauthorized();
                }

                var newKnowledge = await _masterDataBusiness
                    .CreateKnowledgeAsync(knowledge.Map())
                    .ConfigureAwait(false);

                return Ok(newKnowledge.Map());
            }
            catch (MissingDataException ex)
            {
                _logger.LogWarning(ex, "The user provided insufficient information");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occured while deleting term");
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

        /// <summary>
        ///     Converts a string to a master data type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static MasterDataType ParseMasterDataType(string type)
        {
            if (type.Equals("knowledge", StringComparison.InvariantCultureIgnoreCase))
                return MasterDataType.Knowledge;
            if (type.Equals("departments", StringComparison.InvariantCultureIgnoreCase))
                return MasterDataType.Department;
            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}