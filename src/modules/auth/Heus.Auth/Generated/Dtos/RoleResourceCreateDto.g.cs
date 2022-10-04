using System;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos
{
    public partial class RoleResourceCreateDto
    {
        public EntityId ResourceId { get; set; }
        public EntityId RoleId { get; set; }
        public EntityId? Id { get; set; }
        public EntityId? CreatedBy { get; set; }
        public EntityId? UpdateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}