using Common.Enums;
using Common.Model.Response;
using Core.Projects.DAL.Entity;

namespace Core.Projects.Service.Projects.Query.Response
{
    public class ProjectsBaseResponse : BaseResponse
    {
        public string? ProjectPrefix { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ProjectType Type { get; set; }

        public List<ProjectPhase>? ProjectPhases { get; set; } = new();
    }
}
