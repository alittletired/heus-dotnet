using System;
using System.Linq.Expressions;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;

namespace Heus.Auth.Dtos
{
    public static partial class OrganMapper
    {
        public static OrganDto AdaptToDto(this Organ p1)
        {
            return p1 == null ? null : new OrganDto()
            {
                Address = p1.Address,
                Phone = p1.Phone,
                Name = p1.Name,
                Sort = p1.Sort,
                TreeCode = p1.TreeCode,
                ParentId = p1.ParentId,
                Id = p1.Id,
                CreatedBy = p1.CreatedBy,
                UpdateBy = p1.UpdateBy,
            
            };
        }
        public static Expression<Func<Organ, OrganDto>> ProjectToDto => p2 => new OrganDto()
        {
            Address = p2.Address,
            Phone = p2.Phone,
            Name = p2.Name,
            Sort = p2.Sort,
            TreeCode = p2.TreeCode,
            ParentId = p2.ParentId,
            Id = p2.Id,
            CreatedBy = p2.CreatedBy,
            UpdateBy = p2.UpdateBy,
          
        };
    }
}