using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace halpert.Models
{
    public class UserRolesMapping
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Key, Column(Order = 1)]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}