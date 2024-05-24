namespace Bean_Mind.API.Payload.Request.Parents
{
    public class UpdateParentRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public void TrimString()
        {
            // Kiểm tra và loại bỏ khoảng trắng ở đầu và cuối chuỗi
            FirstName = FirstName?.Trim();
            LastName = LastName?.Trim();
            Phone = Phone?.Trim();
            Email = Email?.Trim();
            Address = Address?.Trim();
        }
    }
}
