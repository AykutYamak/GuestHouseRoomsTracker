using DNBarbershop.Models.EnumClasses;
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
        [Required(ErrorMessage = "Моля, въведете номер на стая.")]
        [Display(Name = "Номер на стая")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "Моля, въведете дата на настаняване.")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата на настаняване")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Моля, въведете дата на напускане.")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата на напускане")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Моля, въведете телефонен номер.")]
        [Phone(ErrorMessage = "Невалиден телефонен номер.")]
        [Display(Name = "Телефонен номер")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Моля, въведете името на госта.")]
        [Display(Name = "Име на гост")]
        public string GuestName { get; set; }

        [Display(Name = "Бележки")]
        public string? Notes { get; set; }

        [Required]
        public ReservationStatus Status { get; set; }

        public List<GuestHouseRoomsTracker.Models.Entities.Room> Rooms { get; set; }
    }
}
