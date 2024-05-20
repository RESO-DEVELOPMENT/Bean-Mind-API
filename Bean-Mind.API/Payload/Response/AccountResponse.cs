using Bean_Mind.API.Payload;
using Bean_Mind_Data.Enums;

namespace Bean_Mind.API.PayLoad.Response
{
    public class AccountResponse : LoginResponse
    {
        public string  UserName { get; set; }
        public RoleEnum RoleName { get; set; }

        // Constructor không tham số
        public AccountResponse()
        {
        }

        // Constructor với tham số để khởi tạo các thuộc tính
        public AccountResponse(string userName, RoleEnum roleName)
        {
            UserName = userName;
            RoleName = roleName;
        }

    }

    
}
