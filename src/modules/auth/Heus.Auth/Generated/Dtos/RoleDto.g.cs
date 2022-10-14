using System;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos
{
    public partial class RoleDto
    {
        public bool IsBuildIn { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string? Remarks { get; set; }
        public long? Id { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}