using AutoMapper;
using Bean_Mind.API.Service.Implement;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using System.Security.Claims;


namespace Bean_Mind.API.Service
{
    public abstract class BaseService<T> where T : class
    {
        protected IUnitOfWork<BeanMindContext> _unitOfWork;
        protected ILogger<T> _logger;
        protected IMapper _mapper;
        protected IHttpContextAccessor _httpContextAccessor;

        public BaseService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<T> logger, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected string GetUsernameFromJwt()
        {
            string username = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return username;
        }

        protected string GetRoleFromJwt()
        {
            string role = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            return role;
        }

        //Use for employee and store manager
        protected async Task<bool> CheckIsAccount(Account account)
        {
            ICollection<Account> listAccount = await _unitOfWork.GetRepository<Account>().GetListAsync(
                predicate: s => s.DelFlg == false);

            return listAccount.Select(x => x.Id).Contains(account.Id);
        }

        protected string GetAcountIdFromJwt()
        {
            return _httpContextAccessor?.HttpContext?.User?.FindFirstValue("Id");
        }
    }
}