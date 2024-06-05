namespace Bean_Mind.API.Constants
{
    public static class MessageConstant
    {
        public static class LoginMessage
        {
            public const string InvalidUsernameOrPassword = "Tên đăng nhập hoặc mật khẩu không chính xác";
            public const string DeactivatedAccount = "Tài khoản đang bị vô hiệu hoá";
        }

        public static class AccountMessage
        {
            public const string UsernameExisted = "Tài khoản đã tồn tại";
            public const string CreateAccountWithWrongRoleMessage = "Please create with acceptent role";
            public const string CreateStudentAccountFailMessage = "Tạo tài khoản học sinh thất bại";
            public const string CreateParentAccountFailMessage = "Tạo tài khoản phụ huynh thất bại";
            public const string CreateTeacherAccountFailMessage = "Tạo tài khoản giáo viên thất bại";
            public const string UserUnauthorizedMessage = "Bạn không được phép cập nhật status cho tài khoản này";
            public const string CreateSchoolAccountFailMessage = "Tạo tài khoản trường học thất bại";

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

        public static class UserMessage
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

        public static class SchoolMessage
        {
            public const string CreateNewSchoolFailedMessage = "Tạo mới trường học thất bại";
            public const string SchoolNotFound = "Không tìm thấy trường.";
            public const string SchoolIdEmpty = "School Id bị trống o.";
            public const string UpdateSchoolFailedMessage = "Update trường học thất bại";

        }

        public static class TeacherMessage
        {
            public const string EmptyCategoryIdMessage = "The category ID cannot be empty.";
            public const string InvalidTeacherData = "Invalid teacher data provided.";
            public const string TeacherNotFound = "Teacher not found.";
            public const string UpdateTeacherFailedMessage = "Failed to update teacher information.";
            public const string UpdateTeacherSuccessfulMessage = "Teacher information updated successfully.";
            // Thêm các thông điệp khác liên quan đến Teacher tại đây
        }
        public static class ParentMessage
        {
            public const string CreateNewParentFailedMessage = "Tạo mới phụ huynh thất bại";
            public const string ParentNotFound = "Không tìm thấy phụ huynh.";
            public const string ParentIdEmpty = "Parent Id bị trống.";
            public const string UpdateParentFailedMessage = "Update thông tin phụ huynh thất bại";

            // Thêm các thông điệp khác liên quan đến phụ huynh tại đây
        }
        public static class StudentMessage
        {
            public const string CreateNewStudentFailedMessage = "Thêm học sinh mới thất bại";
            public const string StudentsIsEmpty = "Không có học sinh nào.";
            public const string StudentNotFound = "Không tìm thấy học sinh";
        }

        public static class SubjectMessage
        {
            public const string SubjectNotFound = "Không tìm thấy môn học";
            public const string SubjectsIsEmpty = "Không có môn học nào.";
            public const string CreateNewSubjectFailedMessage = "Tạo mới môn thất bại";
            public const string UpdateSubjectFailedMessage = "Update thông tin môn thất bại";
        }

        public static class ChapterMessage
        {
            public const string CreateNewChapterFailedMessage = "Thêm chương mới thất bại";
            public const string ChaptersIsEmpty = "Không có chương nào";
            public const string ChapterNotFound = "Không tìm thấy chương";
            public const string UpdateChapterFailedMessage = "Update thông tin chương thất bại";
        }

        public static class TopicMessage
        {
            public const string CreateNewTopicFailedMessage = "Tạo mới Chương thất bại";
            public const string UpdateTopicFailedMessage = "Cập nhật thất bại";
            public const string ListIsEmpty = "Không có gì cả";
            public const string TopicNotFound = "Không tìm thấy topic nào";
        }

        public static class CurriculumMessage
        {
            public const string CreateNewCurriculumFailedMessage = "Thêm chương trình học mới thất bại";
            public const string CurriculumsIsEmpty = "Không có chương trình học nào.";
            public const string CurriculumNotFound = "Không tìm thấy chương trình học";

        }

        public static class CourseMessage
        {
            public const string CreateNewCourseFailedMessage = "Tạo mới khóa học thất bại";
            public const string CoursesIsEmpty = "Không có khóa học nào.";
            public const string CourseNotFound = "Không tìm thấy khóa học";
        }

        public static class VideoMessage
        {
            public const string CreateNewVideoFailedMessage = "Tạo mới video thất bại";
            public const string VideoIsEmpty = "Không có video nào";
            public const string VideoNotFound = "Không tìm thấy video";
            public const string UpdateVideoFailedMessage = "Cập nhật video thất bại";
        }

        public static class ActivityMessage
        {
            public const string CreateNewActivityFailedMessage = "Tạo mới hoạt động thất bại";
            public const string ActivityIsEmpty = "Không có hoạt động nào";
            public const string ActivityNotFound = "Không tìm thấy hoạt động";
            public const string UpdateActivityFailedMessage = "Cập nhật hoạt động thất bại";
        }


        public static class DocumentMessage
        {
            public const string CreateNewDocumentFailedMessage = "Tạo mới tài liệu thất bại.";
            public const string DocumentNotFound = "Không có tài liệu nào";
            public const string DocumentIdEmpty = "Không tìm thấy tài liệu";
            public const string UpdateDocumentFailedMessage = "Cập nhật tài liệu thất bại.";
        }

        public static class WorkSheetMessage
        {
            public const string CreateNewWorkSheetFailedMessage = "Tạo mới worksheet thất bại.";
            public const string WorkSheetNotFound = "Không tìm thấy worksheet nào";
            public const string WorkSheetIsEmpty = "Không có worksheet nào";
            public const string UpdateWorkSheetFailedMessage = "Cập nhật worksheet thất bại.";
        }

        public static class WorkSheetTemplateMessage
        {
            public const string CreateNewWorkSheetTemplateFailedMessage = "Tạo mới worksheet template thất bại.";
            public const string WorkSheetTemplateNotFound = "Không tìm thấy worksheet template";
            public const string WorkSheetTemplateIsEmpty = "Không có worksheet template nào";
            public const string UpdateWorkSheetTemplateFailedMessage = "Cập nhật worksheet template thất bại";
        }

        public static class QuestionLevelMessage
        {
            public const string CreateNewQuestionLevelFailedMessage = "Tạo mới cấp độ câu hỏi thất bại.";
            public const string QuestionLevelNotFound = "Không có cấp độ câu hỏi nào";
            public const string QuestionLevelIsEmpty = "Không tìm thấy cấp độ câu hỏi";
            public const string UpdateQuestionLevelFailedMessage = "Cập nhật cấp độ câu hỏi thất bại.";
        }
    }
}
