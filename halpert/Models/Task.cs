using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace halpert.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Details { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public int ProjectId { get; set; }

        [ForeignKey("DevId")]
        public User User { get; set; }

        [Display(Name = "Assign to")]
        public int? DevId { get; set; }

        [Display(Name = "Optimistic time")]
        public int? Optimistic { get; set; }

        [Display(Name = "Most Likely time")]
        public int? MostLikely { get; set; }

        [Display(Name = "Pessimistic time")]
        public int? Pessimistic { get; set; }

        public int Duration { get; set; }

        public DateTime? StartTime { get; set; }

        public int Progress { get; set; }

        public bool? Accepted { get; set; }

        public string SorceCode { get; set; }

        public int? Length { get; set; }

        public int? Vocabulary { get; set; }

        public int? Volume { get; set; }

        public bool? InsideTheBoundaries { get; set; }

        public int? Difficulty { get; set; }

        public int? Effort { get; set; }

        public int? TimeToUnderStand { get; set; }

        public double? Bugs { get; set; }

        public string Operators { get; set; }

        public string Operands { get; set; }

    }
}