using Common.Model.Response;
using Core.Projects.DAL.Entity;

namespace Core.Projects.Service.ProjectPhases.Query.Response
{
    public class ProjectPhaseBaseResponse : BaseResponse
    {
        public string? Name { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public List<ActivityGroup>? ActivityGroups { get; set; } = new();
    }
}
