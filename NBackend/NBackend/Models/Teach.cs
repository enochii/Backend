using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBackend.Models
{
    public class Teach
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 1), ForeignKey("Teacher")]
        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        // 班级外键
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 2), ForeignKey("Section")]
        public int SecId { get; set; }

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


        //[Required]
        //public int grade { get; set; }
    }
}