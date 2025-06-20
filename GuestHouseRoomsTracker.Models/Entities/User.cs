using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestHouseRoomsTracker.Models.Entities
{
    public class User : IdentityUser
    {
        public ICollection<Reservation> Reservation { get; set; } = new HashSet<Reservation>();

    }
}
