using System;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos
{
    public partial class OrganDto
    {
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public string TreeCode { get; set; }
        public string TreePath { get; set; }
        public EntityId? ParentId { get; set; }
        public EntityId? Id { get; set; }
        public EntityId? CreatedBy { get; set; }
        public EntityId? UpdateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}