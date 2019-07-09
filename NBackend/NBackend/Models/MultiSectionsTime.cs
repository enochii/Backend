using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class MultiSectionsTime
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 1)]
        public int SecId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 2), ForeignKey("Course")]
        public int courseId { get; set; }
        public virtual Course Course { get; set; }

        [Key, Column(Order = 3)]
        [MaxLength(20)]
        public string semester { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 4)]
        public int year { get; set; }

        public Section Section { get; set; }

        [Required, ForeignKey("SectionTime")]
        public int section_timeId { get; set; }
        public virtual SectionTime SectionTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string day { get; set; }

        // 1 表示单周，2表示双周，3表示单双周
        [Required]
        public int single_or_double { get; set; }
    }
}