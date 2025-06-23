using GuestHouseRoomsTracker.Core.IServices;
using GuestHouseRoomsTracker.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GuestHouseRoomsTracker.Core.Services
{
    internal class GlobalService<T> : IGlobalService<T> where T : class
    {
        private readonly IRepository<T> _repository;
        public GlobalService(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            await _repository.Add(entity);
        }

        public async Task Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty.", nameof(id));
            }

            var entity = await _repository.Get(e => EF.Property<Guid>(e, "Id") == id);
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with ID {id} does not exist.");
            }

            await _repository.Delete(id);
        }

        public async Task DeleteAll()
        {
            var allEntities = _repository.GetAll();
            if (!allEntities.Any())
            {
                throw new InvalidOperationException("No entities exist to delete.");
            }

            await _repository.DeleteAll();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter expression cannot be null.");
            }

            var entity = await _repository.Get(filter);
            if (entity == null)
            {
                throw new InvalidOperationException("No entity matches the specified filter.");
            }

            return entity;
        }

        public IQueryable<T> GetAll()
        {
            var result = _repository.GetAll();

            if (result == null)
            {
                throw new InvalidOperationException("Failed to retrieve entities.");
            }

            return result;
        }

        public async Task RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), "Entity collection cannot be null.");
            }

            if (!entities.Any())
            {
                throw new ArgumentException("Entity collection is empty.", nameof(entities));
            }

            await _repository.RemoveRange(entities);
        }

        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity to update cannot be null.");
            }

            // Optionally check if the entity exists
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(Guid))
            {
                var id = (Guid)idProperty.GetValue(entity);
                if (id == Guid.Empty)
                {
                    throw new ArgumentException("Entity ID cannot be empty.");
                }

                var exists = await _repository.Get(e => EF.Property<Guid>(e, "Id") == id);
                if (exists == null)
                {
                    throw new InvalidOperationException($"Entity with ID {id} does not exist.");
                }
            }

            await _repository.Update(entity);
        }
        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter)
        {
           return await _repository.Find(filter);
        }
    }
}
