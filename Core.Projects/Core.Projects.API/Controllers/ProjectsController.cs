using Core.Projects.Service.Interfaces;
using Core.Projects.Service.Projects.Command;
using Core.Projects.Service.Projects.Query.Response;
using Microsoft.AspNetCore.Mvc;

namespace Core.Projects.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectsController : Controller
    {
        private readonly IProjectsService _projectsService;
        public ProjectsController(IProjectsService projectsService) => _projectsService = projectsService;

        [HttpGet]
        public async Task<List<ProjectsResponse>> GetProjects() => await _projectsService.GetProjects();

        [HttpGet("{id:int}")]
        public async Task<ProjectsResponse> GetProject(int id) => await _projectsService.GetProject(id);

        [HttpPost]
        public async Task<ProjectsResponse> CreateProject(AddProjectCommand cmd) => await _projectsService.CreateProject(cmd);

        [HttpPut("{id:int}")]
        public async Task<ProjectsResponse> UpdateProject(int id, UpdateProjectCommand cmd) => await _projectsService.UpdateProject(id, cmd);

        [HttpDelete("{id:int}")]
        public async Task<string> DeleteProject(int id)
        {
            var responce = await _projectsService.DeleteProject(id);
            return responce;
        }

    }
}
