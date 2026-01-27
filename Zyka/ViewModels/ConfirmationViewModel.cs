using System;

namespace Zyka.ViewModels

{

    public class ConfirmationViewModel

    {

        public int ReservationId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public DateTime ReservationDate { get; set; }

        public string TimeSlotText { get; set; } = "N/A";

        public int Guests { get; set; }

        public string TableCategory { get; set; } = "N/A";

        public string TableNumber { get; set; } = "N/A";

        public decimal PaymentAmount { get; set; }

        public string PaymentMethod { get; set; } = "N/A";

        public string Status { get; set; } = "N/A";

    }

}
