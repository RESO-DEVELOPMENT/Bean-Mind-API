using Bean_Mind.API.Constants;
using Bean_Mind.API.Enums;
using Bean_Mind.API.Payload.Request;
using Bean_Mind.API.Payload.Response;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;
        public AccountController(ILogger<AccountController> logger) : base(logger)
        {
        }

        [HttpPost(ApiEndPointConstant.Account.Register)]
        [ProducesResponseType(typeof(CreateNewAccountResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateAccount(CreateNewAccountRequest createNewAccountRequest)
        {
            CreateNewAccountResponse response =
                await _accountService.CreateNewAccount(createNewAccountRequest);
            if (response == null)
            {
                return Problem(MessageConstant.Account.CreateBrandAccountFailMessage);
            }

            return CreatedAtAction(nameof(CreateAccount), response);
        }

    }
}
