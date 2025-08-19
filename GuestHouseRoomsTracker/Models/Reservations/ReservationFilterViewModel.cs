using GuestHouseRoomsTracker.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuestHouseRoomsTracker.Models.Reservations
{
    public class ReservationFilterViewModel
    {
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public List<Reservation> Reservations { get; set; } 
    }
}
