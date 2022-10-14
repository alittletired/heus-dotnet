using System;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos
{
    public partial class RoleCreateDto
    {
        public long? Id { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}