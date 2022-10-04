using System;
using System.Linq.Expressions;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;

namespace Heus.Auth.Dtos
{
    public static partial class RoleResourceMapper
    {
        public static RoleResourceDto AdaptToDto(this RoleResource p1)
        {
            return p1 == null ? null : new RoleResourceDto()
            {
                ResourceId = p1.ResourceId,
                RoleId = p1.RoleId,
                Id = p1.Id,
                CreatedBy = p1.CreatedBy,
                UpdateBy = p1.UpdateBy,
                CreatedDate = p1.CreatedDate,
                UpdateDate = p1.UpdateDate
            };
        }
        public static Expression<Func<RoleResource, RoleResourceDto>> ProjectToDto => p2 => new RoleResourceDto()
        {
            ResourceId = p2.ResourceId,
            RoleId = p2.RoleId,
            Id = p2.Id,
            CreatedBy = p2.CreatedBy,
            UpdateBy = p2.UpdateBy,
            CreatedDate = p2.CreatedDate,
            UpdateDate = p2.UpdateDate
        };
    }
}