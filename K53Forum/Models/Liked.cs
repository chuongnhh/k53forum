using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace K53Forum.Models
{
    [Table("Liked")]
    public class Liked
    {
        public Liked()
        {
            DateCreated = DateTime.Now;
        }
        [Column(Order = 1),Key, ForeignKey("Member")]
        public long MemberId { get; set; }
        [Column(Order = 2), Key, ForeignKey("Post")]
        public long PostId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Member Member { get; set; }
        public virtual Post Post { get; set; }
    }
}