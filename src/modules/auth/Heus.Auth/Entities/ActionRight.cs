using Heus.Ddd.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heus.Auth.Entities
{
    [Index(nameof(ResourceId))]
    [Table("auth_action_right")]
    public class ActionRight : AuditEntity, ISoftDelete
    {
        public long ResourceId { get; set; }
        public string Name { get; set; } = null!;
        public string Display { get; set; } = null!;
        public string Url { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
