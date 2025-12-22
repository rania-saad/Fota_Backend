using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fota.DataLayer.Models;

namespace Fota.BusinessLayer.Interfaces
{
    public interface IGeneric<T>
          where T : class
    {
        IQueryable<T> GetAll();
        T GetByID(int id);
        T GetByID(string id);
        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void save();

        // Async Methods
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIDAsync(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(
            Expression<Func<T, bool>> criteria,
            string[] includes = null
        );
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync<TId>(TId id, T entity);
        Task DeleteAsync(int id);
        public bool Exists(Func<T, bool> predicate);
    }
}
