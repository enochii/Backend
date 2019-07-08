using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBackend.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(30)]
        public string course_name { get; set; }
        [Required]
        public int credits { get; set; }
        [Required]
        [MaxLength(100)]
        public string avatar { get; set; }
        [Required]
        [MaxLength(400)]
        public string description { get; set; }
    }
}