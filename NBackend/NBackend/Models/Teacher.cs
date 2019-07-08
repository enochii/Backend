using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBackend.Models
{
    public class Teacher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int TeacherId { get; set; }

        [Required]
        [MaxLength(20)]
        public string job_title { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool is_manager { get; set; }
    }
}