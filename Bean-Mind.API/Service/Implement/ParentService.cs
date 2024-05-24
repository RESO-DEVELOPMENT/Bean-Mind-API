﻿using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Parent;
using Bean_Mind.API.Payload.Request.Teacher;
using Bean_Mind.API.Payload.Response.Parent;
using Bean_Mind.API.Payload.Response.Teacher;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Implement
{
   
        public class ParentService : BaseService<ParentService>, IParentService
        {
            public ParentService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<ParentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
            {
            }

            public async Task<CreateNewParentResponse> AddParent(CreateNewParentResquest newParentRequest)
            {
                _logger.LogInformation($"Creating new parent with {newParentRequest.FirstName} {newParentRequest.LastName}");

                Parent newParent = new Parent()
                {
                    Id = Guid.NewGuid(),
                    FirstName = newParentRequest.FirstName,
                    LastName = newParentRequest.LastName,
                    Phone = newParentRequest.Phone,
                    Email = newParentRequest.Email,
                    Address = newParentRequest.Address,
                    InsDate = DateTime.UtcNow,
                    UpdDate = DateTime.UtcNow,
                    DelFlg = false
                };

                await _unitOfWork.GetRepository<Parent>().InsertAsync(newParent);

                var success = await _unitOfWork.CommitAsync() > 0;
                if (!success)
                {
                    return null; // Or return appropriate error response
                }

                return new CreateNewParentResponse
                {
                    Id = newParent.Id,
                    FirstName = newParent.FirstName,
                    LastName = newParent.LastName,
                    Email = newParent.Email,
                    Phone = newParent.Phone,
                    Message = "Parent created successfully"
                };
            }
        public async Task<IPaginate<ParentResponse>> GetAllParents(int page, int size)
        {
            var parents = await _unitOfWork.GetRepository<Parent>().GetPagingListAsync(
                selector: x => new ParentResponse()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address,
                    InsDate = x.InsDate,
                    UpdDate = x.UpdDate,
                    DelFlg = x.DelFlg
                },
                predicate: x => x.DelFlg == false,
                size: size,
                page: page);
            return parents;
        }

        public async Task<ParentResponse> GetParentById(Guid parentId)
        {
            var parent = await _unitOfWork.GetRepository<Parent>().SingleOrDefaultAsync(
                selector: x => new ParentResponse()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,
                    Address = x.Address,
                    InsDate = x.InsDate,
                    UpdDate = x.UpdDate,
                    DelFlg = x.DelFlg
                },
                predicate: x => x.Id == parentId);

            return parent;
        }

        public async Task<bool> UpdateParent(Guid parentId, UpdateParentRequest request)
        {
            if (parentId == Guid.Empty)
                throw new ArgumentNullException(nameof(parentId), "Parent ID cannot be empty");

            var parent = await _unitOfWork.GetRepository<Parent>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(parentId)); ;

            if (parent == null)
                throw new ArgumentException("Parent not found", nameof(parentId));

            _logger.LogInformation($"Updating parent {parent.Id}");

            // Trim and update fields
            request.TrimString();

            parent.FirstName = string.IsNullOrEmpty(request.FirstName) ? parent.FirstName : request.FirstName;
            parent.LastName = string.IsNullOrEmpty(request.LastName) ? parent.LastName : request.LastName;
            parent.Phone = string.IsNullOrEmpty(request.Phone) ? parent.Phone : request.Phone;
            parent.Email = string.IsNullOrEmpty(request.Email) ? parent.Email : request.Email;
            parent.Address = string.IsNullOrEmpty(request.Address) ? parent.Address : request.Address;
            parent.UpdDate = DateTime.UtcNow;

            _unitOfWork.GetRepository<Parent>().UpdateAsync(parent);
            var isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }

        public async Task<bool> RemoveParent(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.Parent.ParentIdEmpty);
            }
            var parent = await _unitOfWork.GetRepository<Parent>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id));
            if (parent == null)
            {

                throw new BadHttpRequestException(MessageConstant.Parent.ParentNotFound);
            }
            parent.DelFlg = true;
            _unitOfWork.GetRepository<Parent>().UpdateAsync(parent);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }


    }
}
