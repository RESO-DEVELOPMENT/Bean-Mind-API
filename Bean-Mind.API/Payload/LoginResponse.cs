using Bean_Mind_Data.Enums;

namespace Bean_Mind.API.Payload
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public Guid UserId { get; set; }
        //public bool IsFirstLogin { get; set; }
        //public string Phone { get; set; }
        //public string ImageUrl { get; set; }
        public string Name { get; set; }
        //public string Email { get; set; }

        //public AccountStatus Status { get; set; }
        public RoleEnum Role { get; set; }
    }
}
