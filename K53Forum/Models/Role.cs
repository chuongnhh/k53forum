using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace K53Forum.Models
{
    [Table("Role")]
    public class Role
    {
        public Role()
        {
            Members = new List<Member>();
            DateCreated = DateTime.Now;
        }
        [Key]
        [Display(Name = "Mã")]
        public long Id { get; set; }
        [Display(Name = "Chức vụ")]
        public string RoleName { get; set; }
        [Display(Name = "Ngày khởi tạo")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Danh sách thành viên")]
        public virtual List<Member> Members { get; set; }
    }
}