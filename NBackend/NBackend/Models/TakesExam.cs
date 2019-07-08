using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class TakesExam
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 1), ForeignKey("Student")]
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 2), ForeignKey("Exam")]
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public double score { get; set; }
    }
}