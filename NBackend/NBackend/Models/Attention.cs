using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class Attention
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 1), ForeignKey("Student")]
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        // 班级外键
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 2), ForeignKey("Section")]
        public int secId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 3), ForeignKey("Section")]
        public int courseId { get; set; }
        public virtual Course Course { get; set; }

        [Key, Column(Order = 4), ForeignKey("Section")]
        [MaxLength(20)]
        public string semester { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 5), ForeignKey("Section")]
        public int year { get; set; }

        public Section Section { get; set; }

       // 第几节课
        public int timeId { get; set; }
        //出席状态，是否出席
        public int status { get; set; }
    }
}