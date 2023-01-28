using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using AddressBook.Logger.IManager;
using AddressBook.Entities.DTOs;
using AddressBook.Contracts;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace AddressBook.Controllers{
    [Authorize]
    [Route("api")]

    public class UserController : ControllerBase{
        private readonly ILoggerManager _logger;
        private readonly IUserService _userService;

        public UserController(ILoggerManager logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        #region GET Methods

        /// <summary>
        /// API to get user Details
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        /// <response code="404">NotFound</response>
        /// <response code="500">InternalServerError</response>
        
        [HttpGet]
        [Route("account")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserDto))]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 409, type: typeof(string))]
        [SwaggerResponse(statusCode: 500)]
        public IActionResult GetUserDetails()
        {
            _logger.LogDebug("Entering into GetUserDetails in UserController");
            try{
                    var result = _userService.GetUserDetails();
                    if(result.Item1)
                    {
                        if(result.Item2.Count == 0)
                            return StatusCode((int)HttpStatusCode.NoContent,result);
                        else 
                            return StatusCode((int)HttpStatusCode.OK,result);
                    }
                    else
                        return StatusCode((int)HttpStatusCode.Conflict,"Error occured while getting user details");
                        
            }
            catch{
                _logger.LogError("An error occurred in GetUserDetails in UserController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        /// <summary>
        /// API to get user Details Count
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="500">InternalServerError</response>
        
        [HttpGet]
        [Route("account/count")]
        [SwaggerResponse(statusCode: 200, type: typeof(int))]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 500)]
        public IActionResult GetUserCount()
        {
            _logger.LogDebug("Entering into GetUserCount in UserController");
            try{
                    var token = _userService.GetUserCount();
                    return StatusCode((int)HttpStatusCode.OK,token);
            }
            catch{
                _logger.LogError("An error occurred in GetUserCount in UserController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        /// <summary>
        /// API to get specific user details
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">InternalServerError</response>

        [HttpGet]
        [Route("account/{id}")]
        [SwaggerResponse(statusCode: 200, type: typeof(UserDto))]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 409, type: typeof(string))]
        [SwaggerResponse(statusCode: 500)]
        public IActionResult GetUserDetails(Guid id)
        {
            _logger.LogDebug("Entering into GetUserDetails in UserController");
            try{
                    var result = _userService.GetUserDetails(id);
                    if(result.Item1)
                        return StatusCode((int)HttpStatusCode.OK,result.Item2);
                    else
                        return StatusCode((int)HttpStatusCode.Conflict,"Error occured during get details");
            }
            catch{
                _logger.LogError("An error occurred in GetUserDetails in UserController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        /// <summary>
        /// API to download user files
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="500">InternalServerError</response>
        
        [HttpGet]
        [Route("asset/{id}")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "File Downloaded successfully")]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 500)]
        public IActionResult DownloadFile(Guid id)
        {
            _logger.LogDebug("Entering into DownloadFile in UserController");
            try{
                    
                    var fileResult = _userService.DownloadFile(id);
                    if(fileResult.Item1)
                        return File(fileResult.Item2.FileContent,fileResult.Item2.Type);
                    else
                        return StatusCode((int)HttpStatusCode.Conflict,"Error occured during downloading file");
            }
            catch{
                _logger.LogError("An error occurred in DownloadFile in UserController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        #endregion

        #region POST Methods

        /// <summary>
        /// API to create user details
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">InternalServerError</response>
        
        [AllowAnonymous]
        [HttpPost]
        [Route("account")]
        [SwaggerResponse(statusCode: 201, type: typeof(Guid), description: "User Created successfully")]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 409, type: typeof(string))]
        [SwaggerResponse(statusCode: 500)]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            _logger.LogDebug("Entering into CreateUser in UserController");
            try{
                    var result = _userService.CreateUser(userDto);
                    if(result.Item1 == 1)
                        return StatusCode((int)HttpStatusCode.Conflict,"User Id already exists");
                    else if(result.Item1 == 2)
                        return StatusCode((int)HttpStatusCode.Conflict,"Meta data does not exists");
                    else if(result.Item1 == 3)
                        return StatusCode((int)HttpStatusCode.Conflict,"Email already exists");
                    else if(result.Item1 == 4)
                        return StatusCode((int)HttpStatusCode.Created,result.Item2);
                    else if(result.Item1 == 5)
                        return StatusCode((int)HttpStatusCode.Conflict,"Password must contain minimum 8 letters with numbers and capital letters included");
                    else
                        return StatusCode((int)HttpStatusCode.Conflict, "Error occured while creating user");
            }
            catch{
                _logger.LogError("An error occurred in CreateUser in UserController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        /// <summary>
        /// API to upload user file
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="500">InternalServerError</response>

        [HttpPost]
        [Route("asset/upload-file/{id}")]
        [SwaggerResponse(statusCode: 201, type: typeof(string), description: "User Uploaded successfully")]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 500)]
        public IActionResult UploadFile(Guid id, IFormFile file)
        {
            _logger.LogDebug("Entering into UploadFile in UserController");
            try{
                    var result = _userService.UploadFile(id, file);
                    return StatusCode((int)HttpStatusCode.Created);
            }
            catch{
                _logger.LogError("An error occurred in UploadFile in UserController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        #endregion

        #region PUT Methods
        /// <summary>
        /// API to Update user details
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">InternalServerError</response>

        [HttpPut]
        [Route("account/{id}")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "User Updated successfully")]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 409, type: typeof(string))]
        [SwaggerResponse(statusCode: 500)]
        public IActionResult UpdateUser(Guid id, [FromBody] UserDto userDto)
        {
            _logger.LogDebug("Entering into UpdateUser in UserController");
            try{
                    var result = _userService.UpdateUser(id, userDto);
                    if(result.Item1)
                        return StatusCode((int)HttpStatusCode.OK,result.Item2);
                    else 
                        return StatusCode((int)HttpStatusCode.Conflict, result.Item2);
            }
            catch{
                _logger.LogError("An error occurred in UpdateUser in UserController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        #endregion

        #region DELETE Methods

        /// <summary>
        /// API to Delete user details
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">NotFound</response>
        /// <response code="500">InternalServerError</response>
        
        [HttpDelete]
        [Route("account/{id}")]
        [SwaggerResponse(statusCode: 204, type: typeof(string), description: "User Deleted successfully")]
        [SwaggerResponse(statusCode: 404, type: typeof(string))]
        [SwaggerResponse(statusCode: 500)]
        public IActionResult DeleteUser(Guid id)
        {
            _logger.LogDebug("Entering into DeleteUser in UserController");
            try{
                    var result = _userService.DeleteUser(id);
                    if(result)
                        return StatusCode((int)HttpStatusCode.NoContent);
                    else
                        return StatusCode((int)HttpStatusCode.NotFound,"User not found ");
            }
            catch{
                _logger.LogError("An error occurred in DeleteUser in UserController");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error occured");
            }
        }

        #endregion
    }
}