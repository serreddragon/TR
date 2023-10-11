using AutoMapper;
using Core.Projects.DAL;
using Core.Projects.DAL.Entity;
using Core.Projects.Service.Activities.Command;
using Core.Projects.Service.Activities.Query.Request;
using Core.Projects.Service.Activities.Query.Responce;
using Core.Projects.Service.Activities.Query.Response;
using Core.Projects.Service.Interfaces;
using Core.Projects.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Core.Projects.Service.Activities
{
    public class 
        ActivitiesService : BaseService<Activity, ActivityResponse, ActivityBaseResponse, ActivityRequest>, IActivitiesService
    {
        ActivityCommandValidator _activityCommandValidator;
        public ActivitiesService(ProjectsDbContext ctx, ActivityCommandValidator activityCommandValidator, IMapper mapper) : base(ctx, mapper)
        {
            _activityCommandValidator = activityCommandValidator;
        }
        public async Task<List<ActivityResponse>> GetActivities()
        {
            List<Activity> activities = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                activities = await _ctx.Activities.ToListAsync();
            }

            return _mapper.Map<List<ActivityResponse>>(activities);
        }
        public async Task<ActivityResponse> GetActivity(int id)
        {
            Activity activity = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                activity = await _ctx.Activities.FirstAsync(x => x.Id == id);
            }

            return _mapper.Map<ActivityResponse>(activity);
        }
        public async Task<ActivityResponse> CreateActivity(ActivityCommand cmd)
        {
            _activityCommandValidator.Validate(cmd);
            Activity activity = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                activity = new Activity
                {
                    Name = cmd.Name,
                    Description = cmd.Description,
                    IsInternal = cmd.IsInternal,
                    DueDate = cmd.DueDate,
                    ParentId = cmd.ParentId,
                    Priority = cmd.Priority,
                    ActivityGroupId = cmd.ActivityGroupId,
                };

                await _ctx.Activities.AddAsync(activity);
                await _ctx.SaveChangesAsync();

                scope.Complete();
            }

            activity.CreatedById = activity.Id;
            await _ctx.SaveChangesAsync();

            return _mapper.Map<ActivityResponse>(activity);
        }

        public async Task<ActivityResponse> UpdateActivity(int id, ActivityCommand cmd)
        {

            _activityCommandValidator.Validate(cmd);
            Activity activity = new();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                activity = await _ctx.Activities.FirstAsync(x => x.Id == id);

                activity.Name = cmd.Name;
                activity.Description = cmd.Description;
                activity.IsInternal = cmd.IsInternal;
                activity.DueDate = cmd.DueDate;
                activity.ParentId = cmd.ParentId;
                activity.Priority = cmd.Priority;
                activity.ActivityGroupId = cmd.ActivityGroupId;

                activity.UpdatedById = id;

                await _ctx.SaveChangesAsync();

                scope.Complete();
            }

            return _mapper.Map<ActivityResponse>(activity);
        }
        public async Task<string> DeleteActivity(int id)
        {
            _ctx.Activities.Remove(_ctx.Activities.First(x => x.Id == id));

            var response = await _ctx.SaveChangesAsync();

            return response.ToString();
        }



    }
}
