namespace Zyka.Models.DTOs
{
    public class BookingsDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string TableCategory { get; set; } = string.Empty;
        public string TableNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
