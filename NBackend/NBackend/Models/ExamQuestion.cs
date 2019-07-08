using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class ExamQuestion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 1), ForeignKey("Exam")]
        public int  examId{ get; set; }
        public Exam Exam { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 2), ForeignKey("Question")]
        public int questionId { get; set; }
        public Question Question { get; set; }

        //试题序号
        [Required]
        public int index { get; set; }
    }
}