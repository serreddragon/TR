using Common.Model.Search;
using System;
using System.ComponentModel.DataAnnotations;

namespace Common.DTO
{
    public abstract class BaseRequest : BaseSearchQuery
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
