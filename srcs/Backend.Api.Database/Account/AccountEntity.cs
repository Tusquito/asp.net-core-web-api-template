using System;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Api.Database.Context;
using Backend.Api.Database.Generic;
using Backend.Domain.Enums;

namespace Backend.Api.Database.Account
{
    [Table("account", Schema = DatabaseSchemas.ACCOUNTS)]
    public class AccountEntity : IUuidEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
        public Guid TestId { get; set; }
        public AuthorityType AuthorityType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        
        public virtual TestEntity TestEntity { get; set; }
        
    }
}