using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace K53Forum.Models
{
    [Table("Carousel")]
    public class Carousel
    {
        public Carousel()
        {
            DateCreated = DateTime.Now;
            Title = "";
            Content = "";
        }
        [Key]
        [Display(Name = "Mã")]
        public long Id { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Người cập nhật")]
        public string CreatedBy { get; set; }
    }
}