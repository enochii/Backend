using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class TeacherBroadcast
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 1), ForeignKey("Teacher")]
        public int teacherId { get; set; }
        public Teacher Teacher { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 2), ForeignKey("Broadcast")]
        public int broadcastId { get; set; }
        public Broadcast Broadcast { get; set; }
    }
}