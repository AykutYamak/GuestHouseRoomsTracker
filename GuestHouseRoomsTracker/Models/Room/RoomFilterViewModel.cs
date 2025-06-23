using GuestHouseRoomsTracker.Common;
using System.ComponentModel.DataAnnotations;

namespace GuestHouseRoomsTracker.Models.Room
{
    public class RoomFilterViewModel
    {
        [Range(1, 10, ErrorMessage = "Капацитетът трябва да е между 1 и 10.")]
        public int? Capacity { get; set; }

        public List<GuestHouseRoomsTracker.Models.Entities.Room> Rooms { get; set; }
    }
}
