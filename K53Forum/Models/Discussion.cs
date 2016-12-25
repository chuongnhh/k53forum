using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace K53Forum.Models
{
    [Table("Discussion")]
    public class Discussion
    {
        public Discussion()
        {
            DateCreated = DateTime.Now;
            Replies = new List<Reply>();
            Title = "";
            Content = "";
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
        public DateTime DateCreated { get; set; }

        [Display(Name = "Lượt xem")]
        public long Count { get; set; }

        [ForeignKey("Member")]
        [Display(Name = "Thành viên")]
        public long MemberId { get; set; }

        [ForeignKey("Category")]
        [Display(Name = "Chuyên mục")]
        public long CatagoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Member Member { get; set; }
        public virtual List<Reply> Replies { get; set; }
    }
}