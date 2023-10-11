using Common.Model.Enitites;
using System.ComponentModel.DataAnnotations;

namespace Core.Tenants.DAL.Entity
{
    public class Tenant : BaseEntity
    {
        [Required]
        [StringLength(450)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
