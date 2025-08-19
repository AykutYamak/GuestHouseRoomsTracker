using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNBarbershop.Models.EnumClasses
{
    public enum ReservationStatus
    {
        Scheduled = 1,
        Completed = 2,
        Cancelled = 3,
        Current = 4
    }
}