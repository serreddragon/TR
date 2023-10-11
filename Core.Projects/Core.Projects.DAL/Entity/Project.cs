using Common.Enums;
using Common.Model;

namespace Core.Projects.DAL.Entity
{
    public class Project : BaseEntity
    {
        public string? ProjectPrefix { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ProjectType Type { get; set; }

        public List<ProjectPhase>? ProjectPhases { get; set; } = new();
    }
}
