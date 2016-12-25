using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace K53Forum.Models
{
    [Table("Reply")]
    public class Reply
    {
        public Reply()
        {
            DateCreated = DateTime.Now;
        }
        [Key]
        [Display(Name = "Mã")]
        public long Id { get; set; }

        [Display(Name = "Nội dung")]
        [AllowHtml]
        public string Content { get; set; }

        [Display(Name = "Ngày trả lời")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Thành viên")]
        [ForeignKey("Member")]
        public long MemberId { get; set; }

        [ForeignKey("Discussion")]
        [Display(Name = "Thảo luận")]
        public long DiscussId { get; set; }

        [Display(Name = "Thảo luận")]
        public virtual Discussion Discussion { get; set; }

        [Display(Name = "Thành viên")]
        public virtual Member Member { get; set; }
    }
}