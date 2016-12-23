using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
        public string Content { get; set; }
        [Display(Name = "Ngày bình luận")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Thành viên")]
        [ForeignKey("Member")]
        public long MemberId { get; set; }

        [ForeignKey("Comment")]
        [Display(Name = "Bình luận")]
        public long CommentID { get; set; }

        [Display(Name = "Bình luận")]
        public virtual Comment Comment { get; set; }
        [Display(Name = "Thành viên")]
        public virtual Member Member { get; set; }
    }
}