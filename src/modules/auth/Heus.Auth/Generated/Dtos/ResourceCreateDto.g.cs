using System;
using Heus.Auth.Domain;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos
{
    public partial class ResourceCreateDto
    {
        public string ActionCode { get; set; }
        public ResourceType Type { get; set; }
        public int Sort { get; set; }
        public string TreeCode { get; set; }
        public string TreePath { get; set; }
        public long? ParentId { get; set; }
        public long? Id { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}