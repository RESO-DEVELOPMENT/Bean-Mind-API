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
            public const string SchoolEndpoint = ApiEndpoint + "/schools";
            public const string CreateSchool = SchoolEndpoint;
            public const string GetListSchool = SchoolEndpoint;
            public const string GetSchool = SchoolEndpoint + "/{id}";
            public const string DeleteSchool = SchoolEndpoint + "/{id}";
            public const string UpdateSchool = SchoolEndpoint + "/{id}";
        }

        public static class Teacher
        {
            public const string TeacherEndpoint = ApiEndpoint + "/teachers";
            public const string Create = TeacherEndpoint;
            public const string GetAll = TeacherEndpoint;
            public const string GetById = TeacherEndpoint + "/{id}";
            public const string DeleteTeacher = TeacherEndpoint + "/{id}";
            public const string UpdateTeacher = TeacherEndpoint + "/{id}";
        }

        public static class Student
        {
            public const string StudentEndpoint = ApiEndpoint + "/students";
            public const string Create = StudentEndpoint;
            public const string GetAll = StudentEndpoint;
            public const string GetById = StudentEndpoint + "/{id}";
            public const string DeleteStudent = StudentEndpoint + "/{id}";
            public const string UpdateStudent = StudentEndpoint + "/{id}";
        }
        public static class Parent
        {
            public const string ParentEndpoint = ApiEndpoint + "/parents";
            public const string Create = ParentEndpoint;
            public const string GetAll = ParentEndpoint;
            public const string GetById = ParentEndpoint + "/{id}";
            public const string UpdateParent = ParentEndpoint + "/{id}";
            public const string DeleteParent = ParentEndpoint + "/{id}";
        }

        public static class Chapter
        {
            public const string ChapterEndpoint = ApiEndpoint + "/chapters";
            public const string Create = ChapterEndpoint;
            public const string GetAll = ChapterEndpoint;
            public const string GetById = ChapterEndpoint + "/{id}";
            public const string UpdateChapter = ChapterEndpoint + "/{id}";
            public const string DeleteChapter = ChapterEndpoint + "/{id}";
        }

        public static class Topic
        {
            public const string TopicEndpoint = ApiEndpoint + "/topics";
            public const string Create = TopicEndpoint;
            public const string GetAll = TopicEndpoint;
            public const string GetById = TopicEndpoint + "/{id}";
            public const string UpdateTopic = TopicEndpoint + "/{id}";
            public const string DeleteTopic = TopicEndpoint + "/{id}";
       
          public static class Course
        {
            public const string CourseEndpoint = ApiEndpoint + "/courses";
            public const string Create = CourseEndpoint;
            public const string GetAll = CourseEndpoint;
            public const string GetById = CourseEndpoint + "/{id}";
            public const string UpdateCourse = CourseEndpoint + "/{id}";
            public const string DeleteCourse = CourseEndpoint + "/{id}";
        }

        public static class Curriculum
        {
            public const string CurriculumEndpoint = ApiEndpoint + "/curriculums";
            public const string Create = CurriculumEndpoint;
            public const string GetAll = CurriculumEndpoint;
            public const string GetById = CurriculumEndpoint + "/{id}";
            public const string DeleteCurriculum = CurriculumEndpoint + "/{id}";
            public const string UpdateCurriculum = CurriculumEndpoint + "/{id}";
        }
    }
}
