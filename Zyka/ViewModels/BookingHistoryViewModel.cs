namespace Zyka.ViewModels
{
    public class BookingHistoryViewModel
    {
        public string BookingCode { get; set; }

        public string CustomerName { get; set; }

        public int Guests { get; set; }

        public string TableType { get; set; }

        public string Status { get; set; }     // Completed / Upcoming / Cancelled

        public string Category { get; set; }   // date / family / meeting / celebrate

    }
}
