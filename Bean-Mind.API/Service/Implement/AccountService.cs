using Bean_Mind.API.Service.Interface;
using AutoMapper;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Enums;
using Bean_Mind.API.Utils;
using Bean_Mind.API.Payload.Response.Accounts;
using Bean_Mind.API.Payload;
using System.Linq.Expressions;
using Bean_Mind.API.Payload.Request;

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
            Account newAccount = new Account()
            {
                Id = Guid.NewGuid(),
                UserName = createNewAccountRequest.UserName,
                Password = PasswordUtil.HashPassword(createNewAccountRequest.Password),
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false,
                Role = RoleEnum.SysAdmin.GetDescriptionFromEnum()
            };
            await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewAccountResponse createNewAccountResponse = null;
            if (isSuccessful)
            {
                createNewAccountResponse = new CreateNewAccountResponse()
                {
                    Username = newAccount.UserName,
                    Password = newAccount.Password
                };
            }

            return createNewAccountResponse;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            Expression<Func<Account, bool>> searchFilter = p =>
                p.UserName.Equals(loginRequest.Username) &&
                p.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password)) &&
                ((p.Role == RoleEnum.SysAdmin.GetDescriptionFromEnum() ||
                p.Role == RoleEnum.Teacher.GetDescriptionFromEnum()) ||
                p.Role == RoleEnum.Parent.GetDescriptionFromEnum() ||
                p.Role == RoleEnum.Student.GetDescriptionFromEnum() ||
                p.Role == RoleEnum.SysSchool.GetDescriptionFromEnum());

            Account account = await _unitOfWork.GetRepository<Account>()
             .SingleOrDefaultAsync(predicate: searchFilter);

            if (account == null) return null;

            RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role);
            Tuple<string, Guid> guidClaim = new Tuple<string, Guid>("AccountId", account.Id);
            LoginResponse loginResponse = null;
            loginResponse = new LoginResponse()
            {
                Role = role,
                UserId = account.Id,
                Name = account.UserName

            };
            var token = JwtUtil.GenerateJwtToken(account, guidClaim);
            loginResponse.AccessToken = token;
            return loginResponse;
        }

    }
}
