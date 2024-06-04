using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload;
using Bean_Mind.API.Payload.Request;
using Bean_Mind.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class AuthenticationController : BaseController<AuthenticationController>
    {

        private readonly IAccountService _accountService;
        public AuthenticationController(ILogger<AuthenticationController> logger, IAccountService accountService) : base(logger)
        {
            _accountService = accountService;
        }


        [HttpPost(ApiEndPointConstant.Authentication.Login)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(UnauthorizedObjectResult))]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var loginResponse = await _accountService.Login(loginRequest);
            if (loginResponse == null)
            {
                return Unauthorized(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Error = MessageConstant.LoginMessage.InvalidUsernameOrPassword,
                    TimeStamp = DateTime.Now
                });
            }
           // if (loginResponse.Status == AccountStatus.Deactivate)
              //  return Unauthorized(new ErrorResponse()
              //  {
              //      StatusCode = StatusCodes.Status401Unauthorized,
              //      Error = MessageConstant.LoginMessage.DeactivatedAccount,
              //      TimeStamp = DateTime.Now
              //});
            return Ok(loginResponse);
        }
    }
}
