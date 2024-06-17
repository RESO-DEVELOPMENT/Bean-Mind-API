using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.QuestionAnswers;
using Bean_Mind.API.Payload.Request.Questions;
using Bean_Mind.API.Payload.Response.Question;
using Bean_Mind.API.Payload.Response.QuestionAnswers;
using Bean_Mind.API.Payload.Response.Questions;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;


namespace Bean_Mind.API.Service.Implement
{
    public class QuestionService : BaseService<QuestionService>, IQuestionService
    {
        private readonly GoogleDriveService _driveService;
        public QuestionService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<QuestionService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, GoogleDriveService driveService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _driveService = driveService;
        }

        public async Task<CreateNewQuestionResponse> CreateNewQuestion(IFormFile img, string text, int orderIndex, List<CreateNewQuestionAnswerRequest> answerRequests, QuestionType questionType, Guid questionLevelId)
        {
            _logger.LogInformation($"Create new Question with {text}");

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            if (questionLevelId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionMessage.EmptyQuestion);
            }
            QuestionLevel questionLevel = await _unitOfWork.GetRepository<QuestionLevel>().SingleOrDefaultAsync(predicate: q => q.Id == questionLevelId);
            if (questionLevel == null)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionMessage.QuestionLevelNotFound);
            }
            string url = null;
            if(img != null)
            {
                url = await _driveService.UploadToGoogleDriveAsync(img);
               
            }
            Question newQuestion = new Question
            {
                Id = Guid.NewGuid(),
                Text = text,
                Image = url,
                OrderIndex = orderIndex,
                QuestionType = (int)questionType,
                QuestionLevelId = questionLevelId,
                SchoolId = account.SchoolId.Value,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false,
                QuestionAnswers = new List<QuestionAnswer>()
            };

            foreach (var answerText in answerRequests)
            {
                QuestionAnswer newAnswer = new QuestionAnswer()
                {
                    Id = Guid.NewGuid(),
                    Text = answerText.Text,
                    QuestionId = newQuestion.Id,
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
                    QuestionType = newQuestion.QuestionType,
                    QuestionLevelId = newQuestion.QuestionLevelId,
                    SchoolId = newQuestion.SchoolId,
                    InsDate = newQuestion.InsDate,
                    UpdDate = newQuestion.UpdDate,
                    DelFlg = newQuestion.DelFlg
                };
            }

            return createNewQuestionResponse;

        }
        public async Task<List<GetQuestionResponse>> GetAllQuestion(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var questions = await _unitOfWork.GetRepository<Question>().GetPagingListAsync(
                selector: q => q,
                predicate: s => s.DelFlg != true && s.SchoolId.Equals(account.SchoolId),
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
                    }).ToList(),
                    QuestionLevelId = question.QuestionLevelId,
                    SchoolId = question.SchoolId,
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





