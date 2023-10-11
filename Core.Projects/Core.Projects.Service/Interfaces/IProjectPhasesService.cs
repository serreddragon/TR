using Core.Projects.Service.ProjectPhases.Command;
using Core.Projects.Service.ProjectPhases.Query.Response;

namespace Core.Projects.Service.Interfaces
{
    public interface IProjectPhasesService
    {
        public Task<List<ProjectPhaseResponse>> GetProjectPhases();
        public Task<ProjectPhaseResponse> GetProjectPhase(int id);
        public Task<ProjectPhaseResponse> CreateProjectPhase(AddProjectPhaseCommand cmd);
        public Task<ProjectPhaseResponse> UpdateProjectPhase(int id, UpdateProjectPhaseCommand cmd);
        public Task<string> DeleteProjectPhase(int id);
    }
}
