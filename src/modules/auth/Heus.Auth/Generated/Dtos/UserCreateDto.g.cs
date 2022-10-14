using System;
using Heus.Auth.Domain;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos
{
    public partial class UserCreateDto
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Salt { get; set; }
        public UserStatus Status { get; set; }
        public long? Id { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}