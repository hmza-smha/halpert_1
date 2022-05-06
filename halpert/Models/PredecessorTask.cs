using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace halpert.Models
{
    public class PredecessorTask
    {
        [Key, Column(Order = 0)]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Task Task { get; set; }
        
        [ForeignKey("PreTaskId")]
        public Task PreTask { get; set; }

        [Key, Column(Order = 1)]
        public int PreTaskId { get; set; }
    }
}