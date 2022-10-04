using System;
using Heus.Auth.Domain;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos
{
    public partial class ResourceDto
    {
        public string ActionCode { get; set; }
        public ResourceType Type { get; set; }
        public bool IsDeleted { get; set; }
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