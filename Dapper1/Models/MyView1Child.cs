
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    //[Table("View_1")]
    public class MyView1Child
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<MyViewRole> Roles { get; set; }
       
    }

    public class MyViewRole
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }

}
