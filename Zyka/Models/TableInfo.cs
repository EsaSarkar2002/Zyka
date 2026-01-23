using System;

using System.Collections.Generic; // Required for ICollection

using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Zyka.Models.Enums;

namespace Zyka.Models

{

    [Index(nameof(TableNumber), IsUnique = true)]

    public class TableInfo

    {

        [Key]

        public int TableId { get; set; }

        [Required, StringLength(10)]

        public string TableNumber { get; set; }

        [Required]

        public TableStatus Status { get; set; }

        [Required]

        public TableCategory Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Add this line below to fix the error

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }

}
