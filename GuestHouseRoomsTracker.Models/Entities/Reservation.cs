using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GuestHouseRoomsTracker.Models.Entities
{
    public class Reservation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        [StringLength(100, ErrorMessage = nameof(GuestHouseRoomsTracker.Common.ErrorMessages.MaxLengthExceededErrorMessage))]
        public string GuestName { get; set; }

        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        public DateTime CheckInDate { get; set; }
        
        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        public DateTime CheckOutDate { get; set; }
        
        public string? Notes { get; set; }
        
        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [ForeignKey(nameof(Room))]
        public Guid RoomId { get; set; }

        public Room Room { get; set; }

        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
    }
}