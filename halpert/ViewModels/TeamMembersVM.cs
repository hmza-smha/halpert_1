using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using halpert.Models;

namespace halpert.ViewModels
{
    public class TeamMembersVM
    {
        public List<WorksOn> TeamMembers { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}