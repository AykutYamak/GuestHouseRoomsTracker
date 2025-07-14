namespace GuestHouseRoomsTracker.Models.Reservations
{
    public class AvailableRoomsViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<GuestHouseRoomsTracker.Models.Entities.Room> Rooms { get; set; }
    }
}
