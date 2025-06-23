namespace GuestHouseRoomsTracker.Models.Room
{
    public class RoomAvailabilityViewModel
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public IEnumerable<GuestHouseRoomsTracker.Models.Entities.Room> AvailableRooms { get; set; } = new List<GuestHouseRoomsTracker.Models.Entities.Room>();
    }
}
