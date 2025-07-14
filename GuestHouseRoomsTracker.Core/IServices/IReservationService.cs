using GuestHouseRoomsTracker.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestHouseRoomsTracker.Core.IServices
{
    public interface IReservationService : IGlobalService<Reservation>
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task CreateReservationAsync(Reservation model);
        Task<IEnumerable<Room>> GetRoomsByIdsAsync(IEnumerable<Guid> roomIds);
    }
}
