using Bean_Mind.API.Payload.Request;
using Bean_Mind.API.Payload.Response;
using Bean_Mind_Business.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bean_Mind_Business.Business.Implement
{
    public class AccountService : BaseService, IAccountService
    {
        public Task<CreateNewAccountResponse> CreateNewBrand(CreateNewAccountRequest createNewAccountRequest)
        {
            throw new NotImplementedException();
        }
    }
}
