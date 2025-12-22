using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.DBContext;
using Fota.Models;
using Microsoft.EntityFrameworkCore;

namespace Fota.BusinessLayer.Repositories
{
    public class SubscriberRepository : GenericRepository<Subscriber>, ISubscriberRepository
    {
        public SubscriberRepository(FOTADbContext context) : base(context) { }

        public override async Task<IEnumerable<Subscriber>> GetAllAsync()
        {
            return await _dbSet
                .Include(s => s.TopicSubscriptions)
                .Include(s => s.Diagnostics)
                .Include(s => s.MessageDeliveries)
                .ToListAsync();
        }

        public override async Task<Subscriber?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(s => s.TopicSubscriptions)
                .Include(s => s.Diagnostics)
                .Include(s => s.MessageDeliveries)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<IEnumerable<Subscriber>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(s => s.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<Subscriber?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<IEnumerable<Subscriber>> GetActiveSubscribersAsync()
        {
            return await _dbSet
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subscriber>> GetByTopicAsync(int topicId)
        {
            return await _context.TopicSubscribers
                .Where(ts => ts.TopicId == topicId && ts.IsActive)
                .Include(ts => ts.Subscriber)
                .Select(ts => ts.Subscriber)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subscriber>> GetSubscribersWithDiagnosticsAsync()
        {
            return await _dbSet
                .Include(s => s.Diagnostics)
                .Where(s => s.Diagnostics.Any())
                .ToListAsync();
        }

        public async Task<IEnumerable<Subscriber>> GetSubscribersWithDeliveriesAsync()
        {
            return await _dbSet
                .Include(s => s.MessageDeliveries)
                .Where(s => s.MessageDeliveries.Any())
                .ToListAsync();
        }
    }
}