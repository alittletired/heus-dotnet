using System;
using System.Linq.Expressions;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;

namespace Heus.Auth.Dtos
{
    public static partial class UserRoleMapper
    {
        public static UserRoleDto AdaptToDto(this UserRole p1)
        {
            return p1 == null ? null : new UserRoleDto()
            {
                UserId = p1.UserId,
                RoleId = p1.RoleId,
                Id = p1.Id,
                CreatedBy = p1.CreatedBy,
                UpdateBy = p1.UpdateBy,
                CreatedDate = p1.CreatedDate,
                UpdateDate = p1.UpdateDate
            };
        }
        public static Expression<Func<UserRole, UserRoleDto>> ProjectToDto => p2 => new UserRoleDto()
        {
            UserId = p2.UserId,
            RoleId = p2.RoleId,
            Id = p2.Id,
            CreatedBy = p2.CreatedBy,
            UpdateBy = p2.UpdateBy,
            CreatedDate = p2.CreatedDate,
            UpdateDate = p2.UpdateDate
        };
    }
}