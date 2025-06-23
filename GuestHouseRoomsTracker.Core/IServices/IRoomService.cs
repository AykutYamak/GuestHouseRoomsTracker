using GuestHouseRoomsTracker.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestHouseRoomsTracker.Core.IServices
{
    public interface IRoomService : IGlobalService<Room>
    {
        
        Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime startDate, DateTime endDate);
        
        Task<IEnumerable<Guid>> GetAvailableRoomIdsAsync(DateTime startDate, DateTime endDate);
    }
}
