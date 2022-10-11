using System;
using Heus.Auth.Domain;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos
{
    public partial class UserDto
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Salt { get; set; }
        public UserStatus Status { get; set; }
        public EntityId? Id { get; set; }
        public EntityId? CreatedBy { get; set; }
        public EntityId? UpdateBy { get; set; }
      
    }
}