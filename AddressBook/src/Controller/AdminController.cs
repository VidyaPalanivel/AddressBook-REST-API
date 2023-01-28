using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using AddressBook.Logger.IManager;
using AddressBook.Entities.DTOs;
using AddressBook.Contracts;
using System.Net;

namespace AddressBook.Controllers{
    [AllowAnonymous]
    [Route("api")]

    public class AdminController : Controller{
        private readonly ILoggerManager _logger;
        private readonly IAdminDetailsService _adminService;

        public AdminController(ILoggerManager logger, IAdminDetailsService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        #region POST Methods
        /// <summary>
        /// API to create authendication token
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">InternalServerError</response>
        
        [HttpPost]
        [Route("auth/sign-in")]
        [SwaggerResponse(statusCode: 200, type: typeof(LoginDto), description: "Token Generated successfully")]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 409, type: typeof(string))]
        [SwaggerResponse(statusCode: 500, type: typeof(string))]
        public IActionResult Login([FromBody] LoginDto LoginDto)
        {
            _logger.LogDebug("Entering into Login in AdminController");
            try{
                    var result = _adminService.Login(LoginDto);
                    if(result.Item1 == 1)
                        return StatusCode((int)HttpStatusCode.OK,result.Item2);
                    else if(result.Item1 == 2)
                        return StatusCode((int)HttpStatusCode.Conflict,"Password Incorrect");
                    else 
                        return StatusCode((int)HttpStatusCode.NotFound,"User Not Found");
            }
            catch{
                _logger.LogError("An error occurred in Login in AdminController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        /// <summary>
        /// API to insert meta data
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">InternalServerError</response>
        
        [HttpPost]
        [Route("meta-data")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Meta Data inserted successfully")]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 500, type: typeof(string))]
        public IActionResult CreateMetadata([FromBody] List<MetadataDto> metadataDto)
        {
            _logger.LogDebug("Entering into CreateMetadata in AdminController");
            try{
                    var result = _adminService.InsertMetaData(metadataDto);
                    if(result.Item1)
                        return StatusCode((int)HttpStatusCode.OK, "Meta Data inserted successfully");
                    else
                        return StatusCode((int)HttpStatusCode.Conflict, result.Item2);
            }
            catch{
                _logger.LogError("An error occurred in CreateMetadata in AdminController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }
        
        [HttpPost]
        [Route("meta-data/ref-term")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Meta Data inserted successfully")]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 500, type: typeof(string))]
        public IActionResult AddRefterm([FromBody] MetadataDto metadataDto)
        {
            _logger.LogDebug("Entering into CreateMetadata in AdminController");
            try{
                    var result = _adminService.AddRefTermToTheExistingRefset(metadataDto);
                    if(result.Item1)
                        return StatusCode((int)HttpStatusCode.OK, "Meta Data inserted successfully");
                    else
                        return StatusCode((int)HttpStatusCode.Conflict, result.Item2);
            }
            catch{
                _logger.LogError("An error occurred in CreateMetadata in AdminController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }
        
        #endregion
    }
}