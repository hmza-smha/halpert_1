using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using halpert.Models;

namespace halpert.ViewModels
{
    public class ProjectDetailsVM
    {
        public List<Task> Tasks { get; set; }
        public Project Project { get; set; }
        public string Ids { get; set; }
        public string Names { get; set; }
        public string Durations { get; set; }
        public string Progresses { get; set; }
        public string Predecessors { get; set; }
        public string Dates { get; set; }
        public int  NumOfTasks { get; set; }
    }
}