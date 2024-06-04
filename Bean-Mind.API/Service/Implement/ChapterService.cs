using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Chapters;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Topics;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Implement
{
    public class ChapterService : BaseService<ChapterService>, IChapterService
    {
        public ChapterService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<ChapterService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewChapterResponse> CreateNewChapter(CreateNewChapterRequest request, Guid subjectId)
        {
            _logger.LogInformation($"Create new Chapter with {request.Title}");
            if (subjectId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            Subject subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(subjectId) && s.DelFlg != true);
            if (subject == null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }

            Chapter newChapter = new Chapter()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                DelFlg = false,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                SubjectId = subjectId,

            };
            await _unitOfWork.GetRepository<Chapter>().InsertAsync(newChapter);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewChapterResponse createNewChapterResponse = null;
            if (isSuccessful)
            {
                createNewChapterResponse = new CreateNewChapterResponse()
                {
                    Id = newChapter.Id,
                    Title = newChapter.Title,
                    Description = newChapter.Description,
                    DelFlg = newChapter.DelFlg,
                    InsDate = newChapter.InsDate,
                    UpdDate = newChapter.UpdDate
                };
            }

            return createNewChapterResponse;
        }
        public async Task<IPaginate<GetChapterResponse>> GetListChapter(int page, int size)
        {
            var chapters = await _unitOfWork.GetRepository<Chapter>().GetPagingListAsync(
                selector: s => new GetChapterResponse(s.Id, s.Title, s.Description),
                predicate: s => s.DelFlg != true,
                page: page,
                size: size
                );
            if (chapters == null)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChaptersIsEmpty);
            }
            return chapters;
        }
        public async Task<GetChapterResponse> GetChapterById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChapterNotFound);
            }
            var chapter = await _unitOfWork.GetRepository<Chapter>().SingleOrDefaultAsync(
                selector: s => new GetChapterResponse(s.Id, s.Title, s.Description),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (chapter == null)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChapterNotFound);
            }
            return chapter;
        }
        public async Task<bool> UpdateChapter(Guid id, UpdateChapterRequest request, Guid subjectId)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChapterNotFound);
            }
            var chapter = await _unitOfWork.GetRepository<Chapter>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (chapter == null)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChapterNotFound);
            }

            if (subjectId != Guid.Empty)
            {
                var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: c => c.Id.Equals(subjectId) && c.DelFlg != true);
                if (subject == null)
                {
                    throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
                }
                chapter.SubjectId = subjectId;
            }


            chapter.Title = string.IsNullOrEmpty(request.Title) ? chapter.Title : request.Title;
            chapter.Description = string.IsNullOrEmpty(request.Description) ? chapter.Description : request.Description;
            chapter.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Chapter>().UpdateAsync(chapter);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
        public async Task<bool> RemoveChapter(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChapterNotFound);
            }
            var chapter = await _unitOfWork.GetRepository<Chapter>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (chapter == null)
            {

                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChapterNotFound);
            }
            // Xóa chapter
            chapter.UpdDate = TimeUtils.GetCurrentSEATime();
            chapter.DelFlg = true;

            // Cập nhật DelFlg của các Topic
            var topics = await _unitOfWork.GetRepository<Topic>().GetListAsync(predicate: c => c.ChapterId.Equals(id) && c.DelFlg != true);
            foreach (var topic in topics)
            {
                topic.DelFlg = true;
                topic.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
            }

            _unitOfWork.GetRepository<Chapter>().UpdateAsync(chapter);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<IPaginate<GetTopicResponse>> GetListTopic(Guid id, int page, int size)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChapterNotFound);
            }
            var topics = await _unitOfWork.GetRepository<Topic>().GetPagingListAsync(
                selector: s => new GetTopicResponse(s.Id, s.Title, s.Description),
                predicate: s => s.ChapterId.Equals(id) && s.DelFlg != true,
                page: page,
                size: size
                );
            if (topics == null)
            {
                throw new BadHttpRequestException(MessageConstant.TopicMessage.ListIsEmpty);
            }
            return topics;
        }
    }
}
