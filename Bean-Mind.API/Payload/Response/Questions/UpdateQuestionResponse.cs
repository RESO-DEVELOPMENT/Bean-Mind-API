using System;
using System.Collections.Generic;
using Bean_Mind.API.Payload.Response.QuestionAnswers;

namespace Bean_Mind.API.Payload.Response.Question
{
    public class UpdateQuestionResponse
    {
        public Guid? Id { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }
        public int? OrderIndex { get; set; }
        public int? QuestionType { get; set; }
        public Guid? QuestionLevelId { get; set; }
        public Guid? SchoolId { get; set; }
       
    }
}
