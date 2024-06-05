using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Questions;
using Bean_Mind.API.Payload.Response.Question;
using Bean_Mind.API.Payload.Response.QuestionAnswers;
using Bean_Mind.API.Payload.Response.Questions;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Bean_Mind.API.Service.Implement
{
    public class QuestionService : BaseService<QuestionService>, IQuestionService
    {
        public QuestionService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<QuestionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewQuestionResponse> CreateNewQuestion(CreateNewQuestionRequest request, Guid questionLevelId)
        {
            _logger.LogInformation($"Create new Question with {request.Text}");
            if (questionLevelId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionMessage.EmptyQuestion);
            }
            QuestionLevel questionLevel = await _unitOfWork.GetRepository<QuestionLevel>().SingleOrDefaultAsync(predicate: q => q.Id == questionLevelId);
            if (questionLevel == null)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionMessage.QuestionLevelNotFound);
            }
            Question newQuestion = new Question
            {
                Id = Guid.NewGuid(),
                Text = request.Text,
                Image = request.Image,
                OrderIndex = request.OrderIndex,
                QuestionType = (int)request.QuestionType,
                QuestionLevelId = questionLevelId,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false,
                QuestionAnswers = new List<QuestionAnswer>()
            };

            foreach (var answerText in request.Answers)
            {
                QuestionAnswer newAnswer = new QuestionAnswer()
                {
                    Id = Guid.NewGuid(),
                    Text = answerText.Text,
                    IsCorrect = answerText.IsCorrect,
                    IndDate = TimeUtils.GetCurrentSEATime(),
                    UpdDate = TimeUtils.GetCurrentSEATime(),
                    DelFlg = false
                };
                newQuestion.QuestionAnswers.Add(newAnswer);
                await _unitOfWork.GetRepository<QuestionAnswer>().InsertAsync(newAnswer);
            }

            await _unitOfWork.GetRepository<Question>().InsertAsync(newQuestion);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewQuestionResponse createNewQuestionResponse = null;
            if (isSuccessful)
            {
                createNewQuestionResponse = new CreateNewQuestionResponse
                {
                    Id = newQuestion.Id,
                    Text = newQuestion.Text,
                    Image = newQuestion.Image,
                    OrderIndex = newQuestion.OrderIndex,
                    QuestionType = newQuestion.QuestionType
                };
            }

            return createNewQuestionResponse;

        }
        public async Task<List<GetQuestionResponse>> GetAllQuestion(int page, int size)
        {
            var questions = await _unitOfWork.GetRepository<Question>().GetPagingListAsync(
                selector: q => q,
                predicate:  q => q.WorksheetQuestions.ToList(),
                orderBy: null,
                include: null,
                page: page,
                size: size
            );

            var questionResponses = new List<GetQuestionResponse>();

            foreach (var question in questions.Items)
            {
                var questionAnswers = await _unitOfWork.GetRepository<QuestionAnswer>().GetListAsync(
        predicate: a => a.QuestionId == question.Id && a.DelFlg == false);

                var questionResponse = new GetQuestionResponse
                {
                    Id = question.Id,
                    Text = question.Text,
                    Image = question.Image,
                    OrderIndex = question.OrderIndex,
                    QuestionType = (QuestionType)question.QuestionType,
                    Answers = questionAnswers.Select(a => new GetQuestionAnswerResponse
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                };

                questionResponses.Add(questionResponse);
            }

            return questionResponses;
        }
        public async Task<List<GetQuestionAnswerResponse>> GetQuestionAnswersByQuestionId(Guid id)
        {
            var questionAnswers = await _unitOfWork.GetRepository<QuestionAnswer>().GetListAsync(
                predicate: q => q.QuestionId == id
            );

            
            var questionAnswerResponses = questionAnswers.Select(a => new GetQuestionAnswerResponse
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList();

            return questionAnswerResponses;
        }




    }
}





