using Bean_Mind.API.Payload.Request.WorkSheetTemplates;
using Bean_Mind.API.Payload.Response.WorkSheetTemplates;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IWorkSheetTemplateService
    {
        public Task<CreateNewWorkSheetTemplateResponse> CreateNewWorkSheetTemplate(CreateNewWorkSheetTemplateRequest request, Guid subjectId);
        public Task<IPaginate<GetWorkSheetTemplateResponse>> GetListWorkSheetTemplate(int page, int size);
        public Task<GetWorkSheetTemplateResponse> GetWorkSheetTemplateById(Guid id);
        public Task<bool> UpdateWorkSheetTemplate(Guid id, UpdateWorkSheetTemplateRequest request, Guid subjectId);
        public Task<bool> RemoveWorkSheetTemplate(Guid id);
    }
}
