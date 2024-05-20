namespace Bean_Mind.API.Constants
{
    public static class MessageConstant
    {
        public static class LoginMessage
        {
            public const string InvalidUsernameOrPassword = "Tên đăng nhập hoặc mật khẩu không chính xác";
            public const string DeactivatedAccount = "Tài khoản đang bị vô hiệu hoá";
        }

        public static class Account
        {
            public const string CreateAccountWithWrongRoleMessage = "Please create with acceptent role";
            public const string CreateBrandAccountFailMessage = "Tạo tài khoản mới cho nhãn hiệu thất bại";
            public const string CreateStaffAccountFailMessage = "Tạo tài khoản nhân viên thất bại";
            public const string UserUnauthorizedMessage = "Bạn không được phép cập nhật status cho tài khoản này";

            public const string UpdateAccountStatusRequestWrongFormatMessage =
                "Cập nhật status tài khoản request sai format";

            public const string AccountNotFoundMessage = "Không tìm thấy tài khoản";
            public const string UpdateAccountStatusSuccessfulMessage = "Cập nhật status tài khoản thành công";
            public const string UpdateAccountStatusFailedMessage = "Cập nhật status tài khoản thất bại";
            public const string EmptyAccountId = "Account id bị trống";
            public const string InvalidPassword = "Mật khẩu cũ không chính xác";
            public const string UpdateAccountPasswordSuccessfulMessage = "Cập nhật status tài khoản thành công";
            public const string UpdateAccountPassFailedMessage = "Cập nhật status tài khoản thất bại";
        }

        public static class User
        {
            public const string CreateNewUserFailedMessage = "Tạo mới Người dùng thất bại";
            public const string UpdateUserFailedMessage = "Cập nhật thông tin Người dùng thất bại";
            public const string UpdateUserSuccessfulMessage = "Cập nhật thông tin Người dùng thành công";
            public const string EmptyUserId = "Id của Người dùng bị trống";
            public const string PaymentUserFail = "Thanh toán đơn hàng thất bại, lỗi hệ thống, vui lòng thửu lại sau";

            public const string UserNotFound =
                "Người dùng không tồn tại trong hệ thống hoặc ngừng hoạt động, vui lòng đăng ký tài khoản";

            public const string MembershipNotFound = "Không tồn tại thông tin thành viên, vui lòng đăng kí tài khoản";
            public const string MembershipFound = "Đã tồn tại thông tin thành viên, vui lòng đăng nhập";
            public const string MembershipPasswordFail = "Sai mã pin, vui lòng thử lại";
            public const string UpdatePin = "Vui lòng tạo mã pin để đăng nhập và hệ thống nhé";
            public const string InputPin = "Nhập mã pin để đăng nhập";
        }
        public static class Teacher
        {
            public const string EmptyCategoryIdMessage = "The category ID cannot be empty.";
            public const string InvalidTeacherData = "Invalid teacher data provided.";
            public const string TeacherNotFound = "Teacher not found.";
            public const string UpdateTeacherFailedMessage = "Failed to update teacher information.";
            public const string UpdateTeacherSuccessfulMessage = "Teacher information updated successfully.";
            // Thêm các thông điệp khác liên quan đến Teacher tại đây
        } 
        public static class School
        {
            public const string CreateNewSchoolFailedMessage = "Tạo mới trường học thất bại";
        }
    }


}
