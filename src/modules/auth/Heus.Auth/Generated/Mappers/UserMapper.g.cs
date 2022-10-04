using System;
using System.Linq.Expressions;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;

namespace Heus.Auth.Dtos
{
    public static partial class UserMapper
    {
        public static UserDto AdaptToDto(this User p1)
        {
            return p1 == null ? null : new UserDto()
            {
                Account = p1.Account,
                Password = p1.Password,
                Phone = p1.Phone,
                Salt = p1.Salt,
                Status = p1.Status,
                Id = p1.Id,
                CreatedBy = p1.CreatedBy,
                UpdateBy = p1.UpdateBy,
                CreatedDate = p1.CreatedDate,
                UpdateDate = p1.UpdateDate
            };
        }
        public static Expression<Func<User, UserDto>> ProjectToDto => p2 => new UserDto()
        {
            Account = p2.Account,
            Password = p2.Password,
            Phone = p2.Phone,
            Salt = p2.Salt,
            Status = p2.Status,
            Id = p2.Id,
            CreatedBy = p2.CreatedBy,
            UpdateBy = p2.UpdateBy,
            CreatedDate = p2.CreatedDate,
            UpdateDate = p2.UpdateDate
        };
    }
}