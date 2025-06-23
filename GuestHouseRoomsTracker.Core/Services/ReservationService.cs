using GuestHouseRoomsTracker.Core.IServices;
using GuestHouseRoomsTracker.DataAccess.Repository;
using GuestHouseRoomsTracker.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GuestHouseRoomsTracker.Core.Services
{
    internal class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _resRepo;
        public ReservationService(IRepository<Reservation> repository)
        {
            _resRepo = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task Add(Reservation entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            await _resRepo.Add(entity);
        }

        public async Task Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty.", nameof(id));
            }

            await _resRepo.Delete(id);
        }

        public async Task DeleteAll()
        {
            var allEntities = _resRepo.GetAll();
            if (!allEntities.Any())
            {
                throw new InvalidOperationException("No entities exist to delete.");
            }

            await _resRepo.DeleteAll();
        }

        public async Task<IEnumerable<Reservation>> Find(Expression<Func<Reservation, bool>> filter)
        {
            return await _resRepo.Find(filter);
        }

        public async Task<Reservation> Get(Expression<Func<Reservation, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter expression cannot be null.");
            }

            var entity = await _resRepo.Get(filter);
            if (entity == null)
            {
                throw new InvalidOperationException("No entity matches the specified filter.");
            }

            return entity;
        }

        public IQueryable<Reservation> GetAll()
        {
            var result = _resRepo.GetAll();

            if (result == null)
            {
                throw new InvalidOperationException("Failed to retrieve entities.");
            }

            return result;
        }

        public async Task RemoveRange(IEnumerable<Reservation> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), "Entity collection cannot be null.");
            }

            if (!entities.Any())
            {
                throw new ArgumentException("Entity collection is empty.", nameof(entities));
            }

            await _resRepo.RemoveRange(entities);
        }

        public async Task Update(Reservation entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity to update cannot be null.");
            }

            var idProperty = typeof(Reservation).GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(Guid))
            {
                var id = (Guid)idProperty.GetValue(entity);
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Entity ID cannot be empty.");
                }
            }

            await _resRepo.Update(entity);
        }
    }
}
