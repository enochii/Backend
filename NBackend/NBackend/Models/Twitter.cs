using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class Twitter
    {
        [Key]
        public int TwitterId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ ForeignKey("User")]
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

        [Required]
        [MaxLength(100)]
        public string image { get; set; }
    }
}