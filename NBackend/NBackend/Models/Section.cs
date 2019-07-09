using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBackend.Models
{
    public class Section
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        [Required]
        [MaxLength(20)]
        public string building { get; set; }
        [Required]
        [MaxLength(20)]
        public string room_numer { get; set; }

        //[Required, ForeignKey("SectionTime")]
        //public int section_timeId { get; set; }
        //public virtual SectionTime SectionTime { get; set; }

        [Required]
        public int start_week { get; set; }
        [Required]
        public int end_week { get; set; }

        [Required]
        [MaxLength(100)]
        public string avatar { get; set; }
    }
}