using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NBackend.Models
{
    public class DisscussionComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, ForeignKey("Disscussion")]
        public int questionId { get; set; }

        public Disscussion Disscussion { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, ForeignKey("Disscussion")]
        public int commentId { get; set; }

        public Disscussion Comment { get; set; }
    }
}