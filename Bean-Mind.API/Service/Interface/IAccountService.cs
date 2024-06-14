using Bean_Mind.API.Payload;
using Bean_Mind.API.Payload.Request;
using Bean_Mind.API.Payload.Response.Accounts;

namespace Bean_Mind.API.Service.Interface
{
    public interface IAccountService
    {
        public Task<CreateNewAccountResponse> CreateNewAccount(CreateNewAccountRequest createNewAccountRequest);

        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
