using System;
using System.ComponentModel.DataAnnotations;

namespace OwinAuth.Models
{
    public class Annoucement
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Details { get; set; }
        public byte[] Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsPopup { get; set; }
        public bool IsPublished { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}