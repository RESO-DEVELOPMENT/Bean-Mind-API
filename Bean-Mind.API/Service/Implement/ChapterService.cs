﻿using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Controllers;
using Bean_Mind.API.Payload.Request.Chapters;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IPaginate<GetChapterResponse>> getListChapter(int page, int size)
        {
            var chapters = await _unitOfWork.GetRepository<Chapter>().GetPagingListAsync(
                selector: s => new GetChapterResponse(s.Id, s.Title, s.Description),
                predicate: s => s.DelFlg != true,
                include: s => s.Include(s => s.Topics),
                page: page,
                size: size
                );
            if (chapters == null)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChaptersIsEmpty);
            }
            return chapters;
        }
        public async Task<GetChapterResponse> getChapterById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChapterNotFound);
            }
            var chapter = await _unitOfWork.GetRepository<Chapter>().SingleOrDefaultAsync(
                selector: s => new GetChapterResponse(s.Id, s.Title, s.Description),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true,
                include: s => s.Include(s => s.Topics));
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

            if (subjectId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: c => c.Id.Equals(subjectId) && c.DelFlg != true);
            if (subject == null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }

            chapter.Title = string.IsNullOrEmpty(request.Title) ? chapter.Title : request.Title;
            chapter.Description = string.IsNullOrEmpty(request.Description) ? chapter.Description : request.Description;
            chapter.SubjectId = subjectId;
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
            chapter.UpdDate = TimeUtils.GetCurrentSEATime();
            chapter.DelFlg = true;
            _unitOfWork.GetRepository<Chapter>().UpdateAsync(chapter);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}