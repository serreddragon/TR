using Core.Projects.Service.Interfaces;
using Core.Projects.Service.ProjectPhases.Command;
using Core.Projects.Service.ProjectPhases.Query.Response;
using Microsoft.AspNetCore.Mvc;

namespace Core.Projects.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectPhasesController : Controller
    {
        private readonly IProjectPhasesService _projectPhasesService;

        public ProjectPhasesController(IProjectPhasesService projectPhasesService) => _projectPhasesService = projectPhasesService;

        [HttpGet]
        public async Task<List<ProjectPhaseResponse>> GetProjectPhases() => await _projectPhasesService.GetProjectPhases();

        [HttpGet("{id:int}")]
        public async Task<ProjectPhaseResponse> GetProjectPhase(int id) => await _projectPhasesService.GetProjectPhase(id);

        [HttpPost]
        public async Task<ProjectPhaseResponse> CreateProjectPhase([FromBody] AddProjectPhaseCommand cmd) => await _projectPhasesService.CreateProjectPhase(cmd);

        [HttpPut("{id:int}")]
        public async Task<ProjectPhaseResponse> UpdateProjectPhase(int id, UpdateProjectPhaseCommand cmd) => await _projectPhasesService.UpdateProjectPhase(id, cmd);

        [HttpDelete("{id:int}")]
        public async Task<string> DeleteProjectPhase(int id) => await _projectPhasesService.DeleteProjectPhase(id);
    }
}
