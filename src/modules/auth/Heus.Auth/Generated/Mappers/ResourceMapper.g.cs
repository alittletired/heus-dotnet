using System;
using System.Linq.Expressions;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;

namespace Heus.Auth.Dtos
{
    public static partial class ResourceMapper
    {
        public static ResourceDto AdaptToDto(this Resource p1)
        {
            return p1 == null ? null : new ResourceDto()
            {
                ActionCode = p1.ActionCode,
                Type = p1.Type,
                IsDeleted = p1.IsDeleted,
                Sort = p1.Sort,
                TreeCode = p1.TreeCode,
                TreePath = p1.TreePath,
                ParentId = p1.ParentId,
                Id = p1.Id,
                CreatedBy = p1.CreatedBy,
                UpdateBy = p1.UpdateBy,
                CreatedDate = p1.CreatedDate,
                UpdateDate = p1.UpdateDate
            };
        }
        public static Expression<Func<Resource, ResourceDto>> ProjectToDto => p2 => new ResourceDto()
        {
            ActionCode = p2.ActionCode,
            Type = p2.Type,
            IsDeleted = p2.IsDeleted,
            Sort = p2.Sort,
            TreeCode = p2.TreeCode,
            TreePath = p2.TreePath,
            ParentId = p2.ParentId,
            Id = p2.Id,
            CreatedBy = p2.CreatedBy,
            UpdateBy = p2.UpdateBy,
            CreatedDate = p2.CreatedDate,
            UpdateDate = p2.UpdateDate
        };
    }
}