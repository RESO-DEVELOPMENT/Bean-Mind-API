using Bean_Mind.API.Payload.Request.Documents;
using Bean_Mind.API.Payload.Response.Documents;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IDocumentService
    {
        public Task<CreateNewDocumentResponse> CreateNewDocument(CreateNewDocumentRequest request, Guid activityId);
        public Task<IPaginate<GetDocumentResponse>> GetListDocument(int page, int size);
        public Task<GetDocumentResponse> GetDocumentById(Guid id);
        public Task<bool> UpdateDocument(Guid id, UpdateDocumentRequest  request, Guid activityId);
        public Task<bool> RemoveDocument(Guid id);
    }
}
