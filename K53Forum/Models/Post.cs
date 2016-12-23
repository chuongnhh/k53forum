using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace K53Forum.Models
{
    [Table("Post")]
    public class Post
    {
        public Post()
        {
            DateCreated = DateTime.Now;
            Comments = new List<Comment>();
            Likeds = new List<Models.Liked>();
            DisLikeds = new List<DisLiked>();
        }
        [Key]
        [Display(Name = "Mã")]
        public long Id { get; set; }
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }
        [Display(Name = "Nội dung")]
        [AllowHtml]
        public string Content { get; set; }
        [Display(Name = "Ngày viết")]
        //[DisplayFormat(DataFormatString = "{0:DD/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Lượt xem")]
        public long Count { get; set; }

        [ForeignKey("Member")]
        [Display(Name = "Thành viên")]
        public long MemberId { get; set; }
        [ForeignKey("Category")]
        [Display(Name = "Chuyên mục")]
        public long CatagoryId { get; set; }

        [Display(Name = "Chuyên mục")]
        public virtual Category Category { get; set; }
        [Display(Name = "Thành viên")]
        public virtual Member Member { get; set; }
        [Display(Name = "Danh sách bình luận")]
        public virtual List<Comment> Comments { get; set; }

        public virtual List<Liked> Likeds { get; set; }
        public virtual List<DisLiked> DisLikeds { get; set; }
    }
}