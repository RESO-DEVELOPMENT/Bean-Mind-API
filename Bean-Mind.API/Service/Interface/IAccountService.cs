using Bean_Mind.API.Payload.Request;
using Bean_Mind.API.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bean_Mind.API.Service.Interface
{
    public interface IAccountService
    {
        public Task<CreateNewAccountResponse> CreateNewAccount(CreateNewAccountRequest createNewAccountRequest);

    }
}
