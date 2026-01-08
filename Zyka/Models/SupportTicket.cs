using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zyka.Models;

namespace zyka.Models
{
    public class SupportTicket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [StringLength(30)]

        public string IssueType { get; set; }  // Booking / Payment / Others

        public string Description { get; set; }  // Issue details

        [StringLength(20)]
        public string Status { get; set; }  // Open / Closed

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        // Navigation property to Users table
        public virtual User User { get; set; }
    }
}