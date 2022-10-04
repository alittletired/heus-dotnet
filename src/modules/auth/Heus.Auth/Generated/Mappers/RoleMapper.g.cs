using System;
using System.Linq.Expressions;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;

namespace Heus.Auth.Dtos
{
    public static partial class RoleMapper
    {
        public static RoleDto AdaptToDto(this Role p1)
        {
            return p1 == null ? null : new RoleDto()
            {
                IsBuildIn = p1.IsBuildIn,
                IsDeleted = p1.IsDeleted,
                Name = p1.Name,
                Remarks = p1.Remarks,
                Id = p1.Id,
                CreatedBy = p1.CreatedBy,
                UpdateBy = p1.UpdateBy,
                CreatedDate = p1.CreatedDate,
                UpdateDate = p1.UpdateDate
            };
        }
        public static Expression<Func<Role, RoleDto>> ProjectToDto => p2 => new RoleDto()
        {
            IsBuildIn = p2.IsBuildIn,
            IsDeleted = p2.IsDeleted,
            Name = p2.Name,
            Remarks = p2.Remarks,
            Id = p2.Id,
            CreatedBy = p2.CreatedBy,
            UpdateBy = p2.UpdateBy,
            CreatedDate = p2.CreatedDate,
            UpdateDate = p2.UpdateDate
        };
    }
}