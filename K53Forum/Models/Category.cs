using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace K53Forum.Models
{
    [Table("Category")]
    public class Category
    {
        public Category()
        {
            Posts = new List<Post>();
            DateCreated = DateTime.Now;
        }
        [Key]
        [Display(Name = "Mã")]
        public long Id { get; set; }
        [Display(Name = "Tên chuyên mục")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Ngày khởi tạo")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Danh sách bài viết")]
        public virtual List<Post> Posts { get; set; }
    }
}