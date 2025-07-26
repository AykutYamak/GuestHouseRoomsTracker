using GuestHouseRoomsTracker.Common;
using System.ComponentModel.DataAnnotations;

namespace GuestHouseRoomsTracker.Models.Room
{
    public class RoomCreateViewModel
    {
        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        [Range(1, 10, ErrorMessage = "Капацитетът трябва да е между 1 и 10.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Описанието трябва да е между 10 и 500 символа.")]
        public string Description { get; set; }

        [Required(ErrorMessage = GuestHouseRoomsTracker.Common.ErrorMessages.RequiredErrorMessage)]
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
    }
}
