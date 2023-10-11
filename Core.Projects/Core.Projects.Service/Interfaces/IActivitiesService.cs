using Core.Projects.Service.Activities.Command;
using Core.Projects.Service.Activities.Query.Responce;

namespace Core.Projects.Service.Interfaces
{
    public interface IActivitiesService
    {
        public Task<List<ActivityResponse>> GetActivities();
        public Task<ActivityResponse> GetActivity(int id);
        public Task<ActivityResponse> CreateActivity(ActivityCommand cmd);
        public Task<ActivityResponse> UpdateActivity(int id, ActivityCommand cmd);
        public Task<string> DeleteActivity(int id);
    }
}
