namespace HotelReservation.WebAPI.Models
{
    public class SearchHotelViewModel
    {
        public int? destId { get; set; }
        public string City { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        public int adultCount { get; set; }
        public int roomCount { get; set; }
    }
}
