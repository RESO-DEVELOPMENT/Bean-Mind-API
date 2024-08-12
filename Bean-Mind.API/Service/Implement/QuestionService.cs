using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.QuestionAnswers;
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
            if (img != null)
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
                predicate: q => q.QuestionId == id && q.DelFlg == false
            );


            var questionAnswerResponses = questionAnswers.Select(a => new GetQuestionAnswerResponse
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList();

            return questionAnswerResponses;
        }
        public async Task<bool> RemoveQuestion(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionMessage.QuestionNotFound);
            }

            var question = await _unitOfWork.GetRepository<Question>().SingleOrDefaultAsync(predicate: q => q.Id.Equals(Id) && q.DelFlg != true);
            if (question == null)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionMessage.QuestionNotFound);
            }

            // Lấy danh sách các câu trả lời liên quan đến câu hỏi này
            var questionAnswers = await _unitOfWork.GetRepository<QuestionAnswer>().GetListAsync(
                predicate: qa => qa.QuestionId.Equals(Id) && qa.DelFlg != true
            );

            // Đánh dấu xóa các câu trả lời liên quan
            foreach (var answer in questionAnswers)
            {
                answer.UpdDate = TimeUtils.GetCurrentSEATime();
                answer.DelFlg = true;
                _unitOfWork.GetRepository<QuestionAnswer>().UpdateAsync(answer);
            }

            // Lấy danh sách các WorksheetQuestion liên quan đến câu hỏi này
            var worksheetQuestions = await _unitOfWork.GetRepository<WorksheetQuestion>().GetListAsync(
                predicate: wq => wq.QuestionId.Equals(Id) && wq.DelFlg != true
            );

            // Đánh dấu xóa các WorksheetQuestion liên quan
            foreach (var worksheetQuestion in worksheetQuestions)
            {
                worksheetQuestion.UpdDate = TimeUtils.GetCurrentSEATime();
                worksheetQuestion.DelFlg = true;
                _unitOfWork.GetRepository<WorksheetQuestion>().UpdateAsync(worksheetQuestion);
            }

            // Đánh dấu xóa câu hỏi
            question.UpdDate = TimeUtils.GetCurrentSEATime();
            question.DelFlg = true;
            _unitOfWork.GetRepository<Question>().UpdateAsync(question);

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
        public async Task<Question> UpdateQuestionDetails(Guid questionId, IFormFile img, string text, int orderIndex, QuestionType questionType, Guid questionLevelId)
        {
            _logger.LogInformation($"Updating Question details with ID: {questionId}");

            var existingQuestion = await _unitOfWork.GetRepository<Question>().SingleOrDefaultAsync(
                predicate: q => q.Id == questionId && q.DelFlg != true
            );
            if (existingQuestion == null)
                throw new BadHttpRequestException(MessageConstant.QuestionMessage.QuestionNotFound);

            if (questionLevelId == Guid.Empty)
                throw new BadHttpRequestException(MessageConstant.QuestionMessage.EmptyQuestion);

            var questionLevel = await _unitOfWork.GetRepository<QuestionLevel>().SingleOrDefaultAsync(predicate: q => q.Id == questionLevelId);
            if (questionLevel == null)
                throw new BadHttpRequestException(MessageConstant.QuestionMessage.QuestionLevelNotFound);

            // Update image if provided
            string url = existingQuestion.Image;
            if (img != null)
            {
                url = await _driveService.UploadToGoogleDriveAsync(img);
            }

            // Update question details
            existingQuestion.Text = text;
            existingQuestion.Image = url;
            existingQuestion.OrderIndex = orderIndex;
            existingQuestion.QuestionType = (int)questionType;
            existingQuestion.QuestionLevelId = questionLevelId;
            existingQuestion.UpdDate = TimeUtils.GetCurrentSEATime();
            _unitOfWork.GetRepository<Question>().UpdateAsync(existingQuestion);

            // Save changes
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
                throw new Exception("Failed to update the question details");

            return existingQuestion;
        }
        public async Task UpdateQuestionAnswers(Guid questionId, List<UpdateQuestionAnswerRequest> answerRequests)
        {
            _logger.LogInformation($"Updating Question Answers for Question ID: {questionId}");

            var existingAnswers = await _unitOfWork.GetRepository<QuestionAnswer>().GetListAsync(
                predicate: a => a.QuestionId == questionId && a.DelFlg != true
            );

            var answerRequestsDict = answerRequests.ToDictionary(a => a.Text, a => a); 

            foreach (var existingAnswer in existingAnswers)
            {
                if (answerRequestsDict.TryGetValue(existingAnswer.Text, out var requestAnswer))
                {
                    existingAnswer.IsCorrect = requestAnswer.IsCorrect;
                    existingAnswer.UpdDate = TimeUtils.GetCurrentSEATime();
                    _unitOfWork.GetRepository<QuestionAnswer>().UpdateAsync(existingAnswer);
                    answerRequestsDict.Remove(existingAnswer.Text); // Xóa câu trả lời đã cập nhật khỏi từ điển
                }
                else
                {
                    existingAnswer.DelFlg = true;
                    existingAnswer.UpdDate = TimeUtils.GetCurrentSEATime();
                    _unitOfWork.GetRepository<QuestionAnswer>().UpdateAsync(existingAnswer);
                }
            }

            foreach (var newAnswerRequest in answerRequestsDict.Values)
            {
                var newAnswerEntity = new QuestionAnswer()
                {
                    Id = Guid.NewGuid(),
                    Text = newAnswerRequest.Text,
                    QuestionId = questionId,
                    IsCorrect = newAnswerRequest.IsCorrect,
                    IndDate = TimeUtils.GetCurrentSEATime(),
                    UpdDate = TimeUtils.GetCurrentSEATime(),
                    DelFlg = false
                };
                await _unitOfWork.GetRepository<QuestionAnswer>().InsertAsync(newAnswerEntity);
            }

            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            if (!isSuccessful)
                throw new Exception("Failed to update the question answers");
        }





    }
}





