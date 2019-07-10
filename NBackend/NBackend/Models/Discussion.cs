using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class Discussion
    {
        
        [Key]
        public int DisscussionId { get; set; }

        public virtual ICollection<Disscussion> comments { get; set; }


        public Discussion()
        {
            this.comments = new List<Disscussion>();
        }

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

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("User")]
        public int userId { get; set; }
        public User User { get; set; }

        [Required]
        [MaxLength(400)]
        public string content { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(16)]
        //1997.11.12 12:01
        public string time { get; set; }
    }
}