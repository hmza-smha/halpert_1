using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using halpert.Models;

namespace halpert.ViewModels
{
    public class ManagerIndexVM
    {
        public List<Project> Projects { get; set; }

        [Required]
        [Display(Name = "Project name")]
        public string  NewProjectName { get; set; }
    }
}