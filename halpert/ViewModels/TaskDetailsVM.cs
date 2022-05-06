using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using halpert.Models;

namespace halpert.ViewModels
{
    public class TaskDetailsVM
    {
        public Task Task { get; set; }
        public List<PredecessorTask> PreTasks { get; set; }
    }
}