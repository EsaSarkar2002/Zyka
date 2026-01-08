//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Zyka.Models
//{
//    public class Payment
//    {
//        [Key]
//        public int PaymentId { get; set; }  // PK

//        [Required]
//        [ForeignKey("Reservation")]
//        public int ReservationId { get; set; }  // FK to Reservation table

//        [Required]
//        [Column(TypeName = "decimal(10,2)")]
//        public decimal Amount { get; set; }  // Payment Amount

//        [Required]
//        [StringLength(20)]
//        public string PaymentMode { get; set; }  // CARD / UPI / CASH / NET BANKING

//        [Required]
//        [StringLength(20)]
//        public string PaymentStatus { get; set; }  // Success / Failed

//        [Required]
//        public DateTime PaymentDate { get; set; }  // Transaction Date

//        // Navigation Property2
//        public Reservation Reservation { get; set; }
//    }
//}