using Common.Model;

namespace Core.Projects.DAL.Entity
{
    public class ActivityGroup : BaseEntity
    {
        public string? Name { get; set; }
        public int? OrderNo { get; set; }

        public int ProjectPhaseId { get; set; }
        public ProjectPhase? ProjectPhase { get; set; }
        public List<Activity>? Activities { get; set; }

    }
}
