using Common.DTO;

namespace Core.Projects.Service.ProjectPhases.Query.Request
{
    public class ProjectPhaseRequest : BaseRequest
    {
        public string? Name { get; set; }
    }
}
