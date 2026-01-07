using System.ComponentModel.DataAnnotations;

namespace Zyka.Models
{
    public class TableInfo
    {

        [Key]
        public int TableId { get; set; }

        [Required]
        public int TableNumber { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]

        public string Status { get; set; } = string.Empty; //Availabe, Reserved, maintainance

    }
}
