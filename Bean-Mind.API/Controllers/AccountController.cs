using Bean_Mind.API.Constants;
using Microsoft.AspNetCore.Mvc;
using Bean_Mind.API.Service.Interface;

using Bean_Mind.API.Payload.Response.Accounts;
using Bean_Mind.API.Payload.Request;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;
        public AccountController(ILogger<AccountController> logger, IAccountService accountService) : base(logger)
        {
            _accountService = accountService;
        }

        [HttpPost(ApiEndPointConstant.Account.Register)]
        [ProducesResponseType(typeof(CreateNewAccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateAccount([FromBody] CreateNewAccountRequest createNewAccountRequest)
        {
            
            CreateNewAccountResponse response =
                await _accountService.CreateNewAccount(createNewAccountRequest);
            if (response == null)
            {
                return Problem(MessageConstant.Account.CreateSchoolAccountFailMessage);
            }

            return CreatedAtAction(nameof(CreateAccount), response);
        }
    }
}
