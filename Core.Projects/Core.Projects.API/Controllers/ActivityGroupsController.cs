using Core.Projects.Service.ActivityGroups.Command;
using Core.Projects.Service.ActivityGroups.Query.Responce;
using Core.Projects.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Core.Projects.API.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ActivityGroupsController : Controller
    {
        private readonly IActivityGroupsService _activityGroupService;

        public ActivityGroupsController(IActivityGroupsService activityGroupService) => _activityGroupService = activityGroupService;

        [HttpGet]
        public async Task<List<ActivityGroupResponse>> GetActivityGroups() => await _activityGroupService.GetActivityGroups();

        [HttpGet("{id:int}")]
        public async Task<ActivityGroupResponse> GetActivityGroup(int id) => await _activityGroupService.GetActivityGroup(id);

        [HttpPost]
        public async Task<ActivityGroupResponse> CreateActivityGroup([FromBody] ActivityGroupCommand cmd) => await _activityGroupService.CreateActivityGroup(cmd);

        [HttpPut("{id:int}")]
        public async Task<ActivityGroupResponse> UpdateActivityGroup(int id, ActivityGroupCommand cmd) => await _activityGroupService.UpdateActivityGroup(id, cmd);

        [HttpDelete("{id:int}")]
        public async Task<string> DeleteActivityGroup(int id) => await _activityGroupService.DeleteActivityGroup(id);
    }
}
