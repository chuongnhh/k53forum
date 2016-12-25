using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace K53Forum.Models
{
    [Table("Member")]
    public class Member
    {
        public Member()
        {
            Articles = new List<Article>();
            Tutorials = new List<Tutorial>();
            Discussions = new List<Discussion>();
            Likeds = new List<Models.Liked>();
            Comments = new List<Comment>();
            DateCreated = DateTime.Now;
            Birthday = DateTime.Now;

            NewPassword = "";
            ConfirmPassword = "";
        }
        [Key]
        [Display(Name = "Mã")]
        public long Id { get; set; }

        [Display(Name = "Tài khoản")]
        [Index(IsUnique = true)]
        [StringLength(50)]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Tên của bạn")]
        public string Fullname { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Giới tính")]
        public string Gender { get; set; }

        [Display(Name = "Địa chỉ mail")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Ngày tham gia")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }

        [Display(Name = "Lượt xem")]
        public long Count { get; set; }

        [ForeignKey("Role")]
        [Display(Name = "Chức vụ")]
        public long RoleId { get; set; }

        [Display(Name = "Chức vụ")]
        public virtual Role Role { get; set; }

        public virtual List<Article> Articles { get; set; }
        public virtual List<Discussion> Discussions { get; set; }
        public virtual List<Tutorial> Tutorials { get; set; }
        public virtual List<Reply> Replies { get; set; }
        public virtual List<Models.Liked> Likeds { get; set; }
        public virtual List<Comment> Comments { get; set; }

        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận lại mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}