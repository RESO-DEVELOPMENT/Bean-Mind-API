using Microsoft.AspNetCore.Routing;

namespace Bean_Mind.API.Constants
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Authentication
        {
            public const string AuthenticationEndpoint = ApiEndpoint + "/auth";
            public const string Login = AuthenticationEndpoint + "/login";
            public const string UpdatePassword = AuthenticationEndpoint + "/changepass";
        }
        public static class Account
        {
            public const string AccountEndpoint = ApiEndpoint + "/account";
            public const string Register = AccountEndpoint + "/sys-admin";
        }

        public static class School
        {
            public const string SchoolEndpoint = ApiEndpoint + "/school";
            public const string CreateSchool = SchoolEndpoint + "/create-school";
            public const string GetListSchool = SchoolEndpoint + "/get-list-school";
            public const string GetSchool = SchoolEndpoint + "/{id}";
            public const string DeleteSchool = SchoolEndpoint + "/{id}";
            public const string UpdateSchool = SchoolEndpoint + "/{id}";
        }

        public static class Teacher
        {
            public const string TeacherEndpoint = ApiEndpoint + "/teacher";
            public const string Create = TeacherEndpoint + "/create";
            public const string GetAll = TeacherEndpoint + "/get-all";
            public const string GetById = TeacherEndpoint + "/{teacherId}";
            public const string DeleteTeacher = TeacherEndpoint + "/{teacherId}";
            public const string UpdateTeacher = TeacherEndpoint + "/{teacherId}";
        }
    }
}
