namespace HotelReservation.WebAPI.Models
{
    public class CityModel
    {
        public Data[] data { get; set; }

        public class Data
        {
            public string dest_id { get; set; }
        }
    }
}

