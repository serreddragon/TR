using AutoMapper;
using Core.Projects.DAL;
using Core.Projects.DAL.Entity;
using Core.Projects.Service.ActivityGroups.Command;
using Core.Projects.Service.ActivityGroups.Query.Request;
using Core.Projects.Service.ActivityGroups.Query.Responce;
using Core.Projects.Service.ActivityGroups.Query.Response;
using Core.Projects.Service.Interfaces;
using Core.Projects.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Core.Projects.Service.ActivityGroups
{
    public class ActivityGroupsService : BaseService<ActivityGroup, ActivityGroupResponse, ActivityGroupBaseResponse, ActivityGroupRequest>, IActivityGroupsService
    {
        private readonly ActivityGroupCommandValidator _activityGroupCommandValidator;
        public ActivityGroupsService(ProjectsDbContext ctx, ActivityGroupCommandValidator activityGroupCommandValidator, IMapper mapper) : base(ctx, mapper)
        {
            _activityGroupCommandValidator = activityGroupCommandValidator;
        }
        public async Task<List<ActivityGroupResponse>> GetActivityGroups()
        {
            List<ActivityGroup> activityGroups = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                activityGroups = await _ctx.ActivityGroups.ToListAsync();
            }

            return _mapper.Map<List<ActivityGroupResponse>>(activityGroups);
        }
        public async Task<ActivityGroupResponse> GetActivityGroup(int id)
        {
            ActivityGroup activityGroup = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                activityGroup = await _ctx.ActivityGroups.FirstAsync(x => x.Id == id);
            }

            return _mapper.Map<ActivityGroupResponse>(activityGroup);
        }
        public async Task<ActivityGroupResponse> CreateActivityGroup(ActivityGroupCommand cmd)
        {
            _activityGroupCommandValidator.Validate(cmd);
            ActivityGroup activityGroup = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                activityGroup = new ActivityGroup
                {
                    Name = cmd.Name,
                    OrderNo = cmd.OrderNo,
                    ProjectPhaseId = cmd.ProjectPhaseId,
                };

                await _ctx.ActivityGroups.AddAsync(activityGroup);
                await _ctx.SaveChangesAsync();

                scope.Complete();
            }

            activityGroup.CreatedById = activityGroup.Id;
            await _ctx.SaveChangesAsync();

            return _mapper.Map<ActivityGroupResponse>(activityGroup);
        }

        public async Task<ActivityGroupResponse> UpdateActivityGroup(int id, ActivityGroupCommand cmd)
        {
            _activityGroupCommandValidator.Validate(cmd);
            ActivityGroup activityGroup = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                activityGroup = await _ctx.ActivityGroups.FirstAsync(x => x.Id == id);

                activityGroup.Name = cmd.Name;
                activityGroup.OrderNo = cmd.OrderNo;
                activityGroup.ProjectPhaseId = cmd.ProjectPhaseId;

                activityGroup.UpdatedById = id;

                await _ctx.SaveChangesAsync();

                scope.Complete();
            }

            return _mapper.Map<ActivityGroupResponse>(activityGroup);
        }
        public async Task<string> DeleteActivityGroup(int id)
        {
            _ctx.ActivityGroups.Remove(_ctx.ActivityGroups.First(x => x.Id == id));

            var response = await _ctx.SaveChangesAsync();

            return response.ToString();
        }

    }
}
