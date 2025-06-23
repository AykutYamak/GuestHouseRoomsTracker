using GuestHouseRoomsTracker.Core.IServices;
using GuestHouseRoomsTracker.DataAccess.Repository;
using GuestHouseRoomsTracker.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GuestHouseRoomsTracker.Core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<Room> _roomRepo;
        public RoomService(IRepository<Room> roomRepo)
        {
            _roomRepo = roomRepo;
        }
        public async Task Add(Room entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            await _roomRepo.Add(entity);
        }

        public async Task Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty.", nameof(id));
            }

            await _roomRepo.Delete(id);
        }

        public async Task DeleteAll()
        {
            var allEntities = _roomRepo.GetAll();
            if (!allEntities.Any())
            {
                throw new InvalidOperationException("No entities exist to delete.");
            }

            await _roomRepo.DeleteAll();
        }

        public async Task<IEnumerable<Room>> Find(Expression<Func<Room, bool>> filter)
        {
            return await _roomRepo.Find(filter);
        }

        public async Task<Room> Get(Expression<Func<Room, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter expression cannot be null.");
            }

            var entity = await _roomRepo.Get(filter);
            if (entity == null)
            {
                throw new InvalidOperationException("No entity matches the specified filter.");
            }

            return entity;
        }

        public IQueryable<Room> GetAll()
        {
            var result = _roomRepo.GetAll();

            if (result == null)
            {
                throw new InvalidOperationException("Failed to retrieve entities.");
            }

            return result;
        }

        public async Task<IEnumerable<Guid>> GetAvailableRoomIdsAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date.");

            var rooms = await _roomRepo
                .GetAll()
                .Where(r => r.IsActive)
                .Include(r => r.Reservations)
                .ToListAsync();

            var availableRooms = rooms
                .Where(room => room.Reservations.All(res =>
                    endDate <= res.CheckInDate || startDate >= res.CheckOutDate
                ))
                .Select(room => room.Id).ToList();

            return availableRooms;
        }

        public async Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime startDate, DateTime endDate)
        {
            if (roomId == Guid.Empty)
            {
                throw new ArgumentException("Room ID cannot be empty.", nameof(roomId));
            }
                
            if (startDate >= endDate)
            {
                throw new ArgumentException("Start date must be before end date.");
            }

            var roomWithReservations = await _roomRepo
           .GetAll()
           .Include(r => r.Reservations)
           .FirstOrDefaultAsync(r => r.Id == roomId && r.IsActive);

            if (roomWithReservations == null)
            {
                throw new InvalidOperationException($"Room with ID {roomId} not found or is inactive.");
            }

            bool isOverlapping = roomWithReservations.Reservations.Any(res => startDate < res.CheckOutDate && endDate > res.CheckInDate);

            return !isOverlapping;
        }

        public async Task RemoveRange(IEnumerable<Room> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), "Entity collection cannot be null.");
            }

            if (!entities.Any())
            {
                throw new ArgumentException("Entity collection is empty.", nameof(entities));
            }

            await _roomRepo.RemoveRange(entities);
        }

        public async Task Update(Room entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity to update cannot be null.");
            }

            // Optionally check if the entity exists
            var idProperty = typeof(Room).GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(Guid))
            {
                var id = (Guid)idProperty.GetValue(entity);
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Entity ID cannot be empty.");
                } 
            }

            await _roomRepo.Update(entity);
        }
    }
}
