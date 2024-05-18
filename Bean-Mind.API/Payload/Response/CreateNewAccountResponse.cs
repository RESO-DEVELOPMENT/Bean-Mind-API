using Bean_Mind.API.Enums;

namespace Bean_Mind.API.Payload.Response
{
    public class CreateNewAccountResponse
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; }
    }
}
