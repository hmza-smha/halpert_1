using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using halpert.Models;

namespace halpert.ViewModels
{
    public class DeveloperProjectDetails
    {
        public List<Task> Tasks { get; set; }
        public string ProjectName { get; set; }
    }
}