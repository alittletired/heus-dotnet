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
        public long? ParentId { get; set; }
        public long? Id { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}