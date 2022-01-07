using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Api.Database.Context;
using Backend.Api.Database.Generic;

namespace Backend.Api.Database.Account
{
    [Table("test", Schema = DatabaseSchemas.ACCOUNTS)]
    public class TestEntity : IUuidEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public virtual AccountEntity AccountEntity { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}