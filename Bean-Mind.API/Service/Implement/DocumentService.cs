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
        private readonly GoogleDriveService _driveService;
        public DocumentService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<DocumentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, GoogleDriveService driveService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _driveService = driveService;
        }
        public async Task<CreateNewDocumentResponse> CreateNewDocument(CreateNewDocumentRequest request, Guid activityId)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            _logger.LogInformation($"Creating new Document with title: {request.Title}");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            string url = await _driveService.UploadToGoogleDriveAsync(request.Url);

            var newDocument = new Document()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Url = url,
                SchoolId = account.SchoolId.Value,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                TeacherId = teacher.Id,
                DelFlg = false
            };

            if (activityId != Guid.Empty)
            {
                var activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(
                    predicate: s => s.Id.Equals(activityId) && s.DelFlg != true);

                if (activity == null)
                {
                    throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
                }

                newDocument.ActivityId = activityId;
            }

            CreateNewDocumentResponse createNewDocumentResponse = null;

            await _unitOfWork.GetRepository<Document>().InsertAsync(newDocument);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessful)
            {
                createNewDocumentResponse = new CreateNewDocumentResponse()
                {
                    Id = newDocument.Id,
                    Title = newDocument.Title,
                    Description = newDocument.Description,
                    ActivityId = newDocument.ActivityId,
                    SchoolId = newDocument.SchoolId,
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
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            var documents = await _unitOfWork.GetRepository<Document>().GetPagingListAsync(
                selector: s => new GetDocumentResponse(s.Id, s.Title, s.Description, s.Url, s.ActivityId, s.SchoolId),
                predicate: s => s.DelFlg != true && s.SchoolId.Equals(account.SchoolId) && s.TeacherId.Equals(teacher.Id),
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

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            var document = await _unitOfWork.GetRepository<Document>().SingleOrDefaultAsync(
                selector: s => new GetDocumentResponse(s.Id, s.Title, s.Description, s.Url, s.ActivityId, s.SchoolId),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true && s.TeacherId.Equals(teacher.Id));
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

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            var document = await _unitOfWork.GetRepository<Document>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true && s.TeacherId.Equals(teacher.Id));
            if (document == null)
            {
                throw new BadHttpRequestException(MessageConstant.DocumentMessage.DocumentNotFound);
            }

            if (activityId != Guid.Empty)
            {
                var activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(predicate: c => c.Id.Equals(activityId) && c.DelFlg != true);
                if (activity == null)
                {
                    throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
                }
                document.ActivityId = activityId;
            }

            document.Title = string.IsNullOrEmpty(request.Title) ? document.Title : request.Title;
            document.Description = string.IsNullOrEmpty(request.Description) ? document.Description : request.Description;

            if (request.Url != null)
            {
                try
                {
                    // Assuming request.Url is the new file to be uploaded
                    document.Url = await _driveService.UploadToGoogleDriveAsync(request.Url);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error uploading file to Google Drive: {ex.Message}");
                    throw new BadHttpRequestException("Error uploading file to Google Drive.");
                }
            }

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

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            var document = await _unitOfWork.GetRepository<Document>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true && s.TeacherId.Equals(teacher.Id));
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
