using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class TeamStudent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 1), ForeignKey("Team")]
        public int teamId { get; set; }
        public Team Team { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, Column(Order = 2), ForeignKey("Student")]
        public int studentId { get; set; }
        public Student Student { get; set; }
    }
}