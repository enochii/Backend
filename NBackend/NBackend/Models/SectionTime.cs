using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBackend.Models
{
    public class SectionTime
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int SectionTimeId { get; set; }

        [Required]
        [MaxLength(20)]
        public string day { get; set; }
        [Required]
        public int start_section { get; set; }
        [Required]
        public int length { get; set; }
        [Required]
        public int start_week { get; set; }
        [Required]
        public int end_week { get; set; }

        // 1 表示单周，2表示双周，3表示单双周
        [Required]
        public int single_or_double { get; set; }
    }
}