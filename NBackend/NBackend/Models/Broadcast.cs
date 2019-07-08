using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class Broadcast
    {
        [Key]
        public int BroadcastId { get; set; }

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
        //广播类型，1表示作业，2表示活动
        public int type { set; get; }

        [Required]
        [MaxLength(200)]
        public string content { get; set; }
        [Required]
        //1表示班级，2表示全局
        public int scope { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(16)]
        //1997.11.12 12:01
        public string publish_time { get; set; }

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