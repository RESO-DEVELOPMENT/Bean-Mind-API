using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Documents;
using Bean_Mind.API.Payload.Response.Documents;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Implement
{
    public class DocumentService : BaseService<DocumentService>, IDocumentService
    {
        public DocumentService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<DocumentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
        public async Task<CreateNewDocumentResponse> CreateNewDocument(CreateNewDocumentRequest request, Guid activityId)
        {
            _logger.LogInformation($"Create new Document with {request.Title}");
            if (activityId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
            }
            Activity activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(activityId) && s.DelFlg != true);
            if (activity == null)
            {
                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
            }

            Document  newDocument  = new Document()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Url = request.Url,
                ActivityId = activityId,
                DelFlg = false,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
            };
            await _unitOfWork.GetRepository<Document>().InsertAsync(newDocument);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewDocumentResponse createNewDocumentResponse = null;
            if (isSuccessful)
            {
                createNewDocumentResponse = new CreateNewDocumentResponse()
                {
                    Id = newDocument.Id,
                    Title = newDocument.Title,
                    Description = newDocument.Description,
                    DelFlg = newDocument.DelFlg,
                    InsDate = newDocument.InsDate,
                    UpdDate = newDocument.UpdDate,
                    Url = newDocument.Url,
                };
            }

            return createNewDocumentResponse;
        }
        public async Task<IPaginate<GetDocumentResponse>> GetListDocument(int page, int size)
        {
            var documents = await _unitOfWork.GetRepository<Document>().GetPagingListAsync(
                selector: s => new GetDocumentResponse(s.Id, s.Title, s.Description, s.Url),
                predicate: s => s.DelFlg != true,
                page: page,
                size: size
                );
            if (documents == null)
            {
                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentIdEmpty);
            }
            return documents;
        }
        public async Task<GetDocumentResponse> GetDocumentById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
            }
            var document = await _unitOfWork.GetRepository<Document>().SingleOrDefaultAsync(
                selector: s => new GetDocumentResponse(s.Id, s.Title, s.Description,s.Url),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (document == null)
            {
                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
            }
            return document;
        }
        public async Task<bool> UpdateDocument(Guid id, UpdateDocumentRequest request, Guid activityId)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
            }
            var document  = await _unitOfWork.GetRepository<Document>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (document == null)
            {
                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
            }

            if (activityId != Guid.Empty)
            {
                var activity  = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(predicate: c => c.Id.Equals(activityId) && c.DelFlg != true);
                if (activity == null)
                {
                    throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
                }
                document.ActivityId = activityId;
            }


            document.Title = string.IsNullOrEmpty(request.Title) ? document.Title : request.Title;
            document.Description = string.IsNullOrEmpty(request.Description) ? document.Description : request.Description;
            document.Url =string.IsNullOrEmpty(request.Url) ? document.Url : request.Url;
            document.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Document>().UpdateAsync(document);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
        public async Task<bool> RemoveDocument(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
            }
            var document = await _unitOfWork.GetRepository<Document>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (document == null)
            {

                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
            }
           
            document.UpdDate = TimeUtils.GetCurrentSEATime();
            document.DelFlg = true;

            _unitOfWork.GetRepository<Document>().UpdateAsync(document);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
