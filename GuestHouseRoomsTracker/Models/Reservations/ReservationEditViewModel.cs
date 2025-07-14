using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuestHouseRoomsTracker.Models.Reservations
{
    public class ReservationEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Моля, въведете името на госта.")]
        [Display(Name = "Име на гост")]
        public string GuestName { get; set; }

        [Required(ErrorMessage = "Моля, въведете телефонен номер.")]
        [Phone(ErrorMessage = "Невалиден телефонен номер.")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Изберете дата на настаняване.")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата на настаняване")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Изберете дата на напускане.")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата на напускане")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Моля, изберете стая.")]
        [Display(Name = "Стая")]
        public Guid RoomId { get; set; }

        [Display(Name = "Бележки")]
        public string? Notes { get; set; }

        // For dropdown in the view
        public IEnumerable<SelectListItem> Rooms { get; set; } = new List<SelectListItem>();
    }
}
