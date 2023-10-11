using Core.Projects.Service.Projects.Command;
using Core.Projects.Service.Projects.Query.Request;
using Core.Projects.Service.Projects.Query.Response;

namespace Core.Projects.Service.Interfaces
{
    public interface IProjectsService
    {
        public Task<List<ProjectsResponse>> GetProjects();
        public Task<ProjectsResponse> GetProject(int id);
        public Task<ProjectsResponse> CreateProject(AddProjectCommand cmd);
        public Task<ProjectsResponse> UpdateProject(int id, UpdateProjectCommand cmd);
        public Task<string> DeleteProject(int id);

    }
}
