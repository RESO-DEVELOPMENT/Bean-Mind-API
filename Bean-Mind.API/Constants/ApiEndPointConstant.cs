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
            public const string GetCurriculumInSchool = SchoolEndpoint + "/{id}/curriculums";
            public const string GetQuestionLevelInSchool = SchoolEndpoint + "/{id}/questionlevels";
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

        public static class Subject
        {
            public const string SubjectEndpoint = ApiEndpoint + "/subjects";
            public const string Create = SubjectEndpoint;
            public const string GetAll = SubjectEndpoint;
            public const string GetById = SubjectEndpoint + "/{id}";
            public const string UpdateSubject = SubjectEndpoint + "/{id}";
            public const string DeleteSubject = SubjectEndpoint + "/{id}";
            public const string GetChapterInSubject = SubjectEndpoint + "/{id}/chapters";

        }

        public static class Chapter
        {
            public const string ChapterEndpoint = ApiEndpoint + "/chapters";
            public const string Create = ChapterEndpoint;
            public const string GetAll = ChapterEndpoint;
            public const string GetById = ChapterEndpoint + "/{id}";
            public const string UpdateChapter = ChapterEndpoint + "/{id}";
            public const string DeleteChapter = ChapterEndpoint + "/{id}";
            public const string GetTopicInChapter = ChapterEndpoint + "/{id}/topics";
        }

        public static class Topic
        {
            public const string TopicEndpoint = ApiEndpoint + "/topics";
            public const string Create = TopicEndpoint;
            public const string GetAll = TopicEndpoint;
            public const string GetById = TopicEndpoint + "/{id}";
            public const string UpdateTopic = TopicEndpoint + "/{id}";
            public const string DeleteTopic = TopicEndpoint + "/{id}";
        }

        public static class Course
        {
            public const string CourseEndpoint = ApiEndpoint + "/courses";
            public const string Create = CourseEndpoint;
            public const string GetAll = CourseEndpoint;
            public const string GetById = CourseEndpoint + "/{id}";
            public const string UpdateCourse = CourseEndpoint + "/{id}";
            public const string DeleteCourse = CourseEndpoint + "/{id}";
            public const string GetSubjectsInCourse  = CourseEndpoint + "/{id}/subjects";

        }

        public static class Curriculum
        {
            public const string CurriculumEndpoint = ApiEndpoint + "/curriculums";
            public const string Create = CurriculumEndpoint;
            public const string GetAll = CurriculumEndpoint;
            public const string GetById = CurriculumEndpoint + "/{id}";
            public const string DeleteCurriculum = CurriculumEndpoint + "/{id}";
            public const string UpdateCurriculum = CurriculumEndpoint + "/{id}";
            public const string GetCourseInCurriculum = CurriculumEndpoint + "/{id}/courses";
        }

        public static class Activity
        {
            public const string ActivityEndPoint = ApiEndpoint + "/activities";
            public const string Create = ActivityEndPoint;
            public const string GetAll = ActivityEndPoint;
            public const string GetById = ActivityEndPoint + "/{id}";
            public const string DeleteActivity = ActivityEndPoint + "/{id}";
            public const string UpdateActivity = ActivityEndPoint + "/{id}";
            public const string GetVideoInActivity = ActivityEndPoint + "/{id}/videos";
            public const string GetDocumentInActivity = ActivityEndPoint + "/{id}/documents";
        }
        public static class Video
        {
            public const string VideoEndpoint = ApiEndpoint + "/videos";
            public const string Create = VideoEndpoint;
            public const string GetAll = VideoEndpoint;
            public const string GetById = VideoEndpoint + "/{id}";
            public const string UpdateVideo = VideoEndpoint + "/{id}";
            public const string DeleteVideo = VideoEndpoint + "/{id}";
        }

        public static class Document
        {
            public const string DocumentEndpoint = ApiEndpoint + "/documents";
            public const string Create = DocumentEndpoint;
            public const string GetAll = DocumentEndpoint;
            public const string GetById = DocumentEndpoint + "/{id}";
            public const string UpdateDocument = DocumentEndpoint + "/{id}";
            public const string DeleteDocument = DocumentEndpoint + "/{id}";        
        }
      
        public static class Question
        {
            public const string QuestionEndpoint = ApiEndpoint + "/questions";
            public const string Create = QuestionEndpoint;
            public const string GetAll = QuestionEndpoint;
            public const string GetById = QuestionEndpoint + "/{id}";
            public const string UpdateQuestion = QuestionEndpoint + "/{id}";
            public const string DeleteQuestion = QuestionEndpoint + "/{id}";
            public const string GetAnswerInQuestion = QuestionEndpoint + "/{id}/answers";
        }


        public static class QuestionLevel
        {
            public const string QuestionLevelEndpoint = ApiEndpoint + "/questionlevels";
            public const string Create = QuestionLevelEndpoint;
            public const string GetAll = QuestionLevelEndpoint;
            public const string GetById = QuestionLevelEndpoint + "/{id}";
            public const string UpdateQuestionLevel = QuestionLevelEndpoint + "/{id}";
            public const string DeleteQuestionLevel = QuestionLevelEndpoint + "/{id}";
        }

        public static class WorkSheet
        {
            public const string WorkSheetEndPoint = ApiEndpoint + "/worksheets";
            public const string Create = WorkSheetEndPoint;
            public const string GetAll = WorkSheetEndPoint;
            public const string GetById = WorkSheetEndPoint + "/{id}";
            public const string UpdateWorkSheet = WorkSheetEndPoint + "/{id}";
            public const string DeleteWorkSheet = WorkSheetEndPoint + "/{id}";
        }

        public static class WorkSheetTemplate
        {
            public const string WorkSheetTemplateEndPoint = ApiEndpoint + "/worksheettemplates";
            public const string Create = WorkSheetTemplateEndPoint;
            public const string GetAll = WorkSheetTemplateEndPoint;
            public const string GetById = WorkSheetTemplateEndPoint + "/{id}";
            public const string UpdateWorkSheetTemplate = WorkSheetTemplateEndPoint + "/{id}";
            public const string DeleteWorkSheetTemplate = WorkSheetTemplateEndPoint + "/{id}";
        }
    }
}
    

