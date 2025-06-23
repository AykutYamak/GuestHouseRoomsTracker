using Microsoft.AspNetCore.Mvc;

namespace GuestHouseRoomsTracker.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
