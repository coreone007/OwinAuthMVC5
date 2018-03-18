using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OwinAuth.ViewModels
{
    public class AnnouncementViewModel
    {
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Details { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Published Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Expired Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public byte[] Image { get; set; }
        public HttpPostedFileBase ImageStore { get; set; }
        public bool IsPopup { get; set; }
        public bool IsPublished { get; set; }
        public int Priority { get; set; }
        [Display(Name = "today")]
        public DateTime CreatedDateTime { get; set; }
    }
}