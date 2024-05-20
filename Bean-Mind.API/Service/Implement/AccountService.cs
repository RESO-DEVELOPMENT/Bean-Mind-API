using Bean_Mind.API.Payload.Request;
using Bean_Mind.API.Payload.Response;
using Bean_Mind.API.Service.Interface;
using AutoMapper;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Enums;
using Bean_Mind.API.Utils;

namespace Bean_Mind.API.Service.Implement
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        public AccountService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<AccountService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewAccountResponse> CreateNewAccount(CreateNewAccountRequest createNewAccountRequest)
        {
            _logger.LogInformation($"Create new account with {createNewAccountRequest.UserName}");
            //Account newAccount = _mapper.Map<Account>(createNewAccountRequest);
            Account newAccount = new Account();
            newAccount.Id = Guid.NewGuid();
            newAccount.SchoolId = Guid.NewGuid();
            newAccount.UserName = createNewAccountRequest.UserName;
            newAccount.Password = PasswordUtil.HashPassword(createNewAccountRequest.Password);
            newAccount.InsDate = TimeUtils.GetCurrentSEATime();
            newAccount.UpdDate = TimeUtils.GetCurrentSEATime();
            newAccount.DelFlg = false;
            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewAccountResponse createNewAccountResponse = new CreateNewAccountResponse();
            createNewAccountResponse.Username = newAccount.UserName;
            createNewAccountResponse.Password = newAccount.Password;
            if (isSuccessful)
            {
                createNewAccountResponse = _mapper.Map<CreateNewAccountResponse>(newAccount);
            }

            return createNewAccountResponse;
        }
    }
}
