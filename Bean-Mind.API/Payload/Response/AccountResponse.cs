using Bean_Mind.API.Payload;
using Bean_Mind_Data.Enums;

<<<<<<< HEAD
namespace Bean_Mind.API.Payload.Response
=======
namespace Bean_Mind.API.PayLoad.Response
>>>>>>> f6ca0e51d9563f5628c3cafe5b811e5d48100f5c
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
