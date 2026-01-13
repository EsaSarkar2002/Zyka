using System.ComponentModel.DataAnnotations;
using Zyka.Models.Enums;

namespace Zyka.Models
{
    
    public class TableInfo
    {

        [Key]
        public int TableId { get; set; }

        [Required,StringLength(10)]
        public string TableNumber { get; set; }

        [Required]
        public TableStatus Status { get; set; }

        [Required]
        public TableCategory Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; } 

    }
}