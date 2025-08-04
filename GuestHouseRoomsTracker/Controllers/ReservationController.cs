using GuestHouseRoomsTracker.Core.IServices;
using GuestHouseRoomsTracker.Core.Services;
using GuestHouseRoomsTracker.Models.Entities;
using GuestHouseRoomsTracker.Models.Reservations;
using GuestHouseRoomsTracker.Models.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GuestHouseRoomsTracker.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        public ReservationController(IReservationService reservationService,IRoomService roomService)
        {
            _reservationService = reservationService;
            _roomService = roomService;
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(ReservationFilterViewModel? filter)
        {
            var reservations = _reservationService.GetAll()
            .Include(r => r.Room)
            .AsQueryable();

            if (filter.CheckInDate.HasValue)
            {
                reservations = reservations.Where(r => r.CheckInDate >= filter.CheckInDate.Value);
            }

            if (filter.CheckOutDate.HasValue)
            {
                reservations = reservations.Where(r => r.CheckInDate <= filter.CheckOutDate.Value);
            }


            filter.Reservations = reservations
                .OrderByDescending(r => r.CheckInDate)
                .ToList();

            return View(filter);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Index()
        {
            var reservations = _reservationService.GetAll()
                .Include(r => r.Room)
                .OrderBy(r => r.CheckInDate).ThenBy(r => r.CheckOutDate)
                .ToList();

            var model = new ReservationFilterViewModel
            {
                Reservations = reservations
            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            var model = new ReservationCreateViewModel();
            var rooms = _roomService.GetAll().ToList();
            if (!rooms.Any())
            {
                TempData["error"] = "Няма налични стаи.";
                return RedirectToAction("Index");
            }
            model.Rooms = rooms.ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Add(ReservationCreateViewModel model)
        {
            model.Rooms = _roomService.GetAll().ToList();

            if (!model.Rooms.Any())
            {
                TempData["error"] = "Няма налични стаи.";
                return RedirectToAction("Index");
            }

            if (model.CheckOutDate <= model.CheckInDate)
            {
                TempData["error"] = "Датата на напускане трябва да е след датата на настаняване.";
                return View(model);
            }

            var room = await _roomService.GetAll()
                .FirstOrDefaultAsync(r => r.RoomNumber == model.RoomNumber);

            if (room == null)
            {
                TempData["error"] = "Стая с такъв номер не съществува.";
                return View(model);
            }

            bool conflictExists = await _reservationService.GetAll()
                .AnyAsync(r => r.RoomId == room.Id &&
                        r.CheckInDate < model.CheckOutDate &&
                        r.CheckOutDate > model.CheckInDate);

            if (conflictExists)
            {
                TempData["error"] = $"Стая {room.RoomNumber} е резервирана за част от избрания период. Моля изберете други дати.";
                return View(model);
            }
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = room.Id,
                GuestName = model.GuestName,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                PhoneNumber = model.PhoneNumber,
                Notes = model.Notes,
                CreatedAt = DateTime.Now
            };

            await _reservationService.Add(reservation);
            TempData["success"] = "Резервацията е създадена успешно!";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var reservation = await _reservationService.Get(x=>x.Id == id);
            if (reservation == null)
                return NotFound();

            var rooms = _roomService.GetAll().ToList();
            var room = rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
            var viewModel = new ReservationEditViewModel
            {
                GuestName = reservation.GuestName,
                PhoneNumber = reservation.PhoneNumber,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                RoomNumber = room.RoomNumber,
                Notes = reservation.Notes,
                Rooms = rooms
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ReservationEditViewModel model)
        {
            var room = await _roomService.GetAll()
                .FirstOrDefaultAsync(r => r.RoomNumber == model.RoomNumber);

            if (room == null)
            {
                TempData["error"] = "Стая с такъв номер не съществува.!";

                return RedirectToAction("Edit", "Reservation");
            }
            var res = new Reservation
            {
                RoomId = room.Id,
                GuestName = model.GuestName,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                PhoneNumber = model.PhoneNumber,
                Notes = model.Notes,
                CreatedAt = DateTime.Now
            };
            await _reservationService.Update(res);

            return RedirectToAction(nameof(Index));
        }
    }
}
