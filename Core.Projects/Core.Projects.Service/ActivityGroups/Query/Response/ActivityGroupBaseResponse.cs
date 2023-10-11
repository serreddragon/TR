using Common.Model.Response;

namespace Core.Projects.Service.ActivityGroups.Query.Response
{
    public class ActivityGroupBaseResponse : BaseResponse
    {
        public string? Name { get; set; }
        public int? OrderNo { get; set; }

        public int ProjectPhaseId { get; set; }
    }
}
