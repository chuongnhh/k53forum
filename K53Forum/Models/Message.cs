using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace K53Forum.Models
{
    [Table("Message")]
    public class Message
    {
        [Key]
        [Display(Name = "Mã")]
        public long Id { get; set; }

        [Display(Name = "Người nhận")]
        [ForeignKey("Member")]
        public long RecipientId { get; set; }

        [Display(Name = "Người gửi")]
        [ForeignKey("Member")]
        public long SenderId { get; set; }

        [Display(Name ="Nội dung")]
        [AllowHtml]
        public string Content { get; set; }

        [Display(Name = "Thời gian")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}