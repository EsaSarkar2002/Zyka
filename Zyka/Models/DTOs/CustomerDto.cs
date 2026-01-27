namespace Zyka.Models.DTOs

{

    public class CustomerListDto

    {

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string? MobileNumber { get; set; }

    }

}

