using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Course")]
        public int courseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        public string chapter { get; set; }
        [Required]
        [MaxLength(100)]
        public string content { get; set; }
        [Required]
        [MaxLength(200)]
        public string options { get; set; }
        [Required]
        [MaxLength(20)]
        public string answer { get; set; }
        //[Required]
        //public int single_score { get; set; }

    }
}