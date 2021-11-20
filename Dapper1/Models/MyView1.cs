
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    //[Table("View_1")]
    public class MyView1
    {
        public string Id { get; private set; }
        public string UserName { get; private set; }
        public string NormalizedUserName { get; private set; }
        public string Email { get; private set; }
        public string NormalizedEmail { get; private set; }
        public string PhoneNumber { get; private set; }
        public string UserId { get; private set; }
        public string RoleId { get; private set; }
        public string Name { get; private set; }
        public string NormalizedName { get; private set; }
    }
}
