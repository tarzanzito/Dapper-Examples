using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    //[Table("AspNetUsers")]
    public class MyIdentityUser
    {
        //[Key]
        //[Column("id")]
        public string Id { get; set; } = null;
        public string UserName { get; set; } = null;
        public string NormalizedUserName { get; set; } = null;
        public string Email { get; set; } = null;
        public string NormalizedEmail { get; set; } = null;
        public bool EmailConfirmed { get; set; } = true;
        public string PasswordHash { get; set; } = null;
        public string SecurityStamp { get; set; } = null;
        public string ConcurrencyStamp { get; set; } = null;
        public string PhoneNumber { get; set; } = null;
        public bool PhoneNumberConfirmed { get; set; } = true;
        public bool TwoFactorEnabled { get; set; } = false;
        public DateTimeOffset? LockoutEnd { get; set; } = null;
        public virtual bool LockoutEnabled { get; set; } = false;
        public int AccessFailedCount { get; set; } = 0;
    }
}
