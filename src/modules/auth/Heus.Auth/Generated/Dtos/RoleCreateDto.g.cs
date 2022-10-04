using System;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos
{
    public partial class RoleCreateDto
    {
        public EntityId? Id { get; set; }
        public EntityId? CreatedBy { get; set; }
        public EntityId? UpdateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}