using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace K53Forum.Models
{
    [Table("Comment")]
    public class Comment
    {
        public Comment()
        {
            DateCreated = DateTime.Now;
            Replies = new List<Reply>();
        }
        [Key]
        [Display(Name = "Mã")]
        public long Id { get; set; }
        [Display(Name = "Nội dung")]
        [AllowHtml]
        public string Content { get; set; }
        [Display(Name = "Ngày bình luận")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Thành viên")]
        [ForeignKey("Member")]
        public long MemberId { get; set; }

        [ForeignKey("Post")]
        [Display(Name = "Bài viết")]
        public long PostId { get; set; }

        [Display(Name = "Bài viết")]
        public virtual Post Post { get; set; }

        [Display(Name = "Thành viên")]
        public virtual Member Member { get; set; }

        [Display(Name = "Trả lời")]
        public virtual List<Reply> Replies { get; set; }
    }
}