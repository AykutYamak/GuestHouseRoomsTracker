using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuestHouseRoomsTracker.Common;

namespace GuestHouseRoomsTracker.Models.Entities
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        [StringLength(20, ErrorMessage = nameof(GuestHouseRoomsTracker.Common.ErrorMessages.MaxLengthExceededErrorMessage))]
        public string RoomNumber { get; set; }
        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        [StringLength(50, ErrorMessage = nameof(GuestHouseRoomsTracker.Common.ErrorMessages.MaxLengthExceededErrorMessage))]
        public string Type { get; set; }
        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        [Range(1, 4, ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.MustBeWholeNumberErrorMessage)]
        public int Capacity { get; set; }
        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        public bool IsActive { get; set; } = true;
        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        public ICollection<Reservation> Reservations { get; set; }
    }
}
