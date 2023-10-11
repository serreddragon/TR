using Common.Enums;
using Common.Model.Response;

namespace Core.Projects.Service.Activities.Query.Response
{
    public class ActivityBaseResponse : BaseResponse
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsInternal { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public int? ParentId { get; set; }
        public ActivityPriority Priority { get; set; }

        public int ActivityGroupId { get; set; }
    }
}
