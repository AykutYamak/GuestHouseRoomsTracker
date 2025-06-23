using Microsoft.AspNetCore.Mvc;

namespace GuestHouseRoomsTracker.Controllers
{
    public class RoomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
