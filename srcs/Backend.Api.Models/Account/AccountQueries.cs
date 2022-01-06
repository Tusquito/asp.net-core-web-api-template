using System;
using Backend.Domain.Enums;

namespace Backend.Api.Models.Account
{
    public class AccountRequestQuery
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Ip { get; set; }
        public AuthorityType AuthorityType { get; set; } 
    }
}