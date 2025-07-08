using GuestHouseRoomsTracker.Core.IServices;
using GuestHouseRoomsTracker.Models.Entities;
using GuestHouseRoomsTracker.Models.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuestHouseRoomsTracker.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;

        public RoomController(IRoomService roomService, IReservationService reservationService)
        {
            _roomService = roomService;
            _reservationService = reservationService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var rooms = _roomService.GetAll().ToList();
            var model = new RoomViewModel { Rooms = rooms };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View(new RoomCreateViewModel());
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Add(RoomCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Невалидни данни!";
                return RedirectToAction("Add", "RoomController");
            }

            var exists = _roomService.GetAll().Any(r => r.RoomNumber == model.RoomNumber);
            if (exists)
            {
                TempData["error"] = "Стая с този номер вече съществува.";
                return RedirectToAction("Add","RoomController");
            }

            var room = new Room
            {
                RoomNumber = model.RoomNumber,
                Capacity = model.Capacity,
                Description = model.Description,
                IsActive = true
            };

            await _roomService.Add(room);
            TempData["success"] = "Стаята е добавена успешно.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var room = await _roomService.Get(r => r.Id == id);
            if (room == null)
            {
                TempData["error"] = "Стаята не е намерена.";
                return NotFound();
            }

            var model = new RoomEditViewModel
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Capacity = room.Capacity,
                Description = room.Description
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(RoomEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Невалидни данни!";
                return RedirectToAction("Edit", "RoomController");
            }

            var duplicate = await _roomService.Get(r => r.RoomNumber == model.RoomNumber && r.Id != model.Id);
            if (duplicate != null)
            {
                TempData["error"] = "Стая с този номер вече съществува.";
                return RedirectToAction("Edit", "RoomController");
            }

            var room = new Room
            {
                Id = model.Id,
                RoomNumber = model.RoomNumber,
                Capacity = model.Capacity,
                Description = model.Description
            };

            await _roomService.Update(room);
            TempData["success"] = "Стаята е обновена успешно.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var room = await _roomService.Get(r => r.Id == id);
            if (room == null)
            {
                TempData["error"] = "Стаята не е намерена.";
                return RedirectToAction("Index");
            }

            try
            {
                await _roomService.Delete(id);
                TempData["success"] = "Стаята е изтрита успешно.";
            }
            catch
            {
                TempData["error"] = "Грешка при изтриване на стаята.";
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RoomList(RoomFilterViewModel? filter)
        {
            var rooms = _roomService.GetAll().AsQueryable();

            if (filter != null)
            {

                if (filter.Capacity.HasValue && filter.Capacity.Value > 0)
                    rooms = rooms.Where(r => r.Capacity >= filter.Capacity.Value);
            }

            var model = new RoomFilterViewModel
            {
                Capacity = filter?.Capacity,
                Rooms = rooms.ToList()
            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(Guid id)
        {
            var room = await _roomService.Get(r => r.Id == id);
            if (room == null)
            {
                TempData["error"] = "Стаята не е намерена.";
                return NotFound();
            }
            return View(room);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchAvailableRooms(DateTime? checkIn, DateTime? checkOut)
        {
            if (checkIn == null || checkOut == null || checkIn >= checkOut)
            {
                TempData["error"] = "Невалидни дати за търсене.";
                return RedirectToAction("Index");
            }

            var availableRoomIds = await _roomService.GetAvailableRoomIdsAsync(checkIn.Value, checkOut.Value);
            var availableRooms = (await _roomService.Find(x => availableRoomIds.Contains(x.Id)));

            var model = new RoomAvailabilityViewModel
            {
                CheckInDate = checkIn.Value,
                CheckOutDate = checkOut.Value,
                AvailableRooms = availableRooms
            };

            return View("AvailableRooms", model); 
        }
    }
}

