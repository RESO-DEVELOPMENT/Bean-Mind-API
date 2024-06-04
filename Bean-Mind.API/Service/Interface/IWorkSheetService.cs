using Bean_Mind.API.Payload.Request.WorkSheets;
using Bean_Mind.API.Payload.Response.WorkSheets;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IWorkSheetService
    {
        public Task<CreateNewWorkSheetResponse> CreateNewWorkSheet(CreateNewWorkSheetRequest request, Guid activityId, Guid worksheetTemplateId);
        public Task<IPaginate<GetWorkSheetResponse>> GetWorkSheet(int page, int size);
        public Task<GetWorkSheetResponse> GetWorkSheetById(Guid id);
        public Task<bool> DeleteWorkSheet(Guid id);
        public Task<bool> UpdateWorkSheet(Guid id, UpdateWorkSheetRequest request, Guid activityId, Guid worksheetTemplateId);
    }
}
