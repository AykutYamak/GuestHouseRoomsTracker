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

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var rooms = _roomService.GetAll().OrderBy(x=>Convert.ToInt32(x.RoomNumber)).ToList();
            var model = new RoomViewModel { Rooms = rooms };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            var model = new RoomCreateViewModel();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Add(RoomCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Невалидни данни!";
                return RedirectToAction("Add", "Room");
            }

            var exists = _roomService.GetAll().Any(r => r.RoomNumber == model.RoomNumber);
            if (exists)
            {
                TempData["error"] = "Стая с този номер вече съществува.";
                return RedirectToAction("Add","Room");
            }

            var room = new Room
            {
                RoomNumber = model.RoomNumber,
                Capacity = model.Capacity,
                Description = model.Description,
                IsActive = true,
                Notes = model.Notes
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
                return RedirectToAction("Index","Room");
            }

            var model = new RoomEditViewModel
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Capacity = room.Capacity,
                Description = room.Description,
                IsActive = room.IsActive,
                Notes = room.Notes

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
                return RedirectToAction("Edit", "Room");
            }

            //var exists = _roomService.GetAll().Any(r => r.RoomNumber == model.RoomNumber);
            //if (exists)
            //{
            //    TempData["error"] = "Стая с този номер вече съществува.";
            //    return RedirectToAction("Add", "Room");
            //}

            var room = new Room
            {
                Id = model.Id,
                RoomNumber = model.RoomNumber,
                Capacity = model.Capacity,
                Description = model.Description,
                IsActive = model.IsActive,
                Notes = model.Notes
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

    }
}

