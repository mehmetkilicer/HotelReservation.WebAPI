using HotelReservation.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace HotelReservation.WebAPI.Controllers
{
    public class BookingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(SearchHotelViewModel searchHotelViewModel)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query=" + searchHotelViewModel.City),
                Headers =
    {
        { "x-rapidapi-key", "6b50468ad8msh7508d044209a4e4p1d9cf8jsn2e3ac2d060bf" },
        { "x-rapidapi-host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var cityData = JsonConvert.DeserializeObject<CityModel>(body);
                int? destid = Convert.ToInt32(cityData.data[0].dest_id);
                var searchHotelWithId = new SearchHotelViewModel
                {
                    destId = destid,
                    City = searchHotelViewModel.City,
                    Checkin = searchHotelViewModel.Checkin,
                    Checkout = searchHotelViewModel.Checkout,
                    roomCount = searchHotelViewModel.roomCount,
                    adultCount = searchHotelViewModel.adultCount,
                };
                return RedirectToAction("HotelList", "Booking", searchHotelWithId);
            }

        }
        public async Task<IActionResult> HotelList(SearchHotelViewModel searchHotel)
        {
            var checkin = searchHotel.Checkin.ToString("yyyy-MM-dd");
            var checkout = searchHotel.Checkout.ToString("yyyy-MM-dd");
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(
                    $"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id={searchHotel.destId}&search_type=CITY&arrival_date={checkin}&departure_date={checkout}&adults={searchHotel.adultCount}&children_age=0%2C17&room_qty={searchHotel.roomCount}&page_number=1&languagecode=en-us&currency_code=EUR"
                ),
                Headers =
    {
        { "x-rapidapi-key", "6b50468ad8msh7508d044209a4e4p1d9cf8jsn2e3ac2d060bf" },
        { "x-rapidapi-host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values =JsonConvert.DeserializeObject<HotelListViewModel>(body);
               if(values.data.hotels != null)
                {
                    return View(values.data.hotels.ToList());

                }
                return View(null);
            }
        }

        public async Task<IActionResult> HotelDetail(string hotel_id, string checkin, string checkout)
        {
            var CheckIn = checkin;
            var ChectOut = checkout;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/getHotelDetails?hotel_id=" + hotel_id + "&arrival_date=" + CheckIn + "&departure_date=" + ChectOut + "&adults=1&children_age=1%2C17&room_qty=1&languagecode=en-us&currency_code=EUR"),
                Headers =
    {
        { "x-rapidapi-key", "6b50468ad8msh7508d044209a4e4p1d9cf8jsn2e3ac2d060bf" },
        { "x-rapidapi-host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<HotelDetailViewModel>(body);
                if (values.data != null)
                {
                    return View(values.data);
                }
            }
            return View();
        }

    }
}
