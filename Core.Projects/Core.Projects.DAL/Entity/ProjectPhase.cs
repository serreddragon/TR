using Common.Model;

namespace Core.Projects.DAL.Entity
{
    public class ProjectPhase : BaseEntity
    {
        public string? Name { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public List<ActivityGroup>? ActivityGroups { get; set; } = new();

    }
}
