using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using halpert.Models;

namespace halpert.ViewModels
{
    public class CreateTaskVM
    {
        [Required]
        public string Name { get; set; }

        public string Details { get; set; }

        public List<User> Developers { get; set; }

        [Required]
        [Display(Name = "Assign to")]
        public int? DeveloperId { get; set; }

        [Required]
        [Display(Name = "Depends on")]
        public int[] PreTasksIds { get; set; }

        public List<Task> PreTasks { get; set; }

        [Display(Name = "Start date")]
        public DateTime? StartTime { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}