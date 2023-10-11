using Common.DTO;
using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Projects.Service.Projects.Query.Request
{
    public class ProjectRequest : BaseRequest
    {
        [Required]
        public string Name { get; set; }

        public string? ProjectPrefix { get; set; }

        public string? Description { get; set; }

        public ProjectType Type { get; set; }
    }
}
