using Core.Projects.Service.Activities.Command;
using Core.Projects.Service.Activities.Query.Responce;
using Core.Projects.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Core.Projects.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActivitiesController : Controller
    {
        private readonly IActivitiesService _activityService;

        public ActivitiesController(IActivitiesService activityService) => _activityService = activityService;

        [HttpGet]
        public async Task<List<ActivityResponse>> GetActivities() => await _activityService.GetActivities();

        [HttpGet("{id:int}")]
        public async Task<ActivityResponse> GetActivity(int id) => await _activityService.GetActivity(id);

        [HttpPost]
        public async Task<ActivityResponse> CreateActivity([FromBody] ActivityCommand cmd) => await _activityService.CreateActivity(cmd);

        [HttpPut("{id:int}")]
        public async Task<ActivityResponse> UpdateActivity(int id, ActivityCommand cmd) => await _activityService.UpdateActivity(id, cmd);

        [HttpDelete("{id:int}")]
        public async Task<string> DeleteActivity(int id) => await _activityService.DeleteActivity(id);

    }
}
