using System;

namespace Bean_Mind.API.Payload.Request.QuestionAnswers
{
    public class UpdateQuestionAnswerRequest
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
