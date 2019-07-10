using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NBackend.Models
{
    public class User
    {
        public virtual ICollection<User> following { get; set; }
        public virtual ICollection<User> followers { get; set; }

        public User()
        {
            this.following = new List<User>();
            this.followers = new List<User>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string user_name { get; set; }
        
        [MaxLength(20)]
        public string department { get; set; }
        [Required]
        [MaxLength(20)]
        public string password { get; set; }
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string phone_number { get; set; }
        [Required]
        [MaxLength(50)]
        public string mail { get; set; }
     
        [MaxLength(100)]
        public string avatar { get; set; }

        [MaxLength(20)]
        public string role { get; set; }
    }
}