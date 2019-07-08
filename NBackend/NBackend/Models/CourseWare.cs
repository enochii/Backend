using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class CourseWare
    {
        [Key]
        public int CourseWareId { get; set; }

        // 班级外键
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Section"), Column(Order = 2)]
        public int secId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Section"), Column(Order = 3)]
        public int courseId { get; set; }
        public virtual Course Course { get; set; }

        [ForeignKey("Section"), Column(Order = 4)]
        [MaxLength(20)]
        public string semester { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Section"), Column(Order = 5)]
        public int year { get; set; }

        public Section Section { get; set; }

        [Required]
        [MaxLength(40)]
        public string name { get; set; }

        [Required]
        [MaxLength(100)]
        public string location { get; set; }
    }
}