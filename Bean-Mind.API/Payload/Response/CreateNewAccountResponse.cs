using Bean_Mind_Data.Enums;

namespace Bean_Mind.API.Payload.Response
{
    public class CreateNewAccountResponse
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; }
    }
}
