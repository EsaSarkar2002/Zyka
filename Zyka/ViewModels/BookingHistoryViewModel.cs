namespace Zyka.ViewModels

{

    public class BookingHistoryViewModel

    {

        public string BookingCode { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public int Guests { get; set; }

        public string TableType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        // ✅ NEW

        public DateTime ReservationDate { get; set; }

        public string TimeSlotText { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

    }

}

