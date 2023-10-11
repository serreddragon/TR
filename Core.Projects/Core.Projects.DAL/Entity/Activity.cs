using Common.Enums;
using Common.Model;

namespace Core.Projects.DAL.Entity
{
    public class Activity : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsInternal { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public int? ParentId { get; set; }
        public ActivityPriority Priority { get; set; }

        public int ActivityGroupId { get; set; }
        public ActivityGroup? ActivityGroup { get; set; }
    }
}
