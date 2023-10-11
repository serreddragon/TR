using Core.Projects.Service.ActivityGroups.Command;
using Core.Projects.Service.ActivityGroups.Query.Responce;

namespace Core.Projects.Service.Interfaces
{
    public interface IActivityGroupsService
    {
        public Task<List<ActivityGroupResponse>> GetActivityGroups();
        public Task<ActivityGroupResponse> GetActivityGroup(int id);
        public Task<ActivityGroupResponse> CreateActivityGroup(ActivityGroupCommand cmd);
        public Task<ActivityGroupResponse> UpdateActivityGroup(int id, ActivityGroupCommand cmd);
        public Task<string> DeleteActivityGroup(int id);
    }
}
