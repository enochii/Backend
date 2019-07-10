using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class Exam
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ExamId { get; set; }


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
        [MaxLength(100)]
        public string scope { get; set; }

        [Required]
        [MaxLength(50)]
        //同济大学2019年数据库重修考试
        public string title { get; set; }

        [Required]
        //[MaxLength(20)]
        //考试类型：1，2，3，4 ->
        public int type { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(16)]
        //1997.11.12 12:01
        public string start_time { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(16)]
        //1997.11.12 12:01
        public string end_time { get; set; }
    }
}