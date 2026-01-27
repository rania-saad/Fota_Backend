using Fota.BusinessLayer.Interfaces;
using Fota.BusinessLayer.Repositories;
using Fota.DataLayer.DBContext;
using Fota.Models;
using Microsoft.EntityFrameworkCore;

namespace Fota.DataLayer.Repositories.Interfaces
{
    public interface ITopicRepository : IGenericRepository<Topic>
    {
        Task<IEnumerable<Topic>> GetActiveTopicsAsync();
        Task<IEnumerable<Topic>> GetByTeamAsync(int teamId);
    }
}

namespace Fota.DataLayer.Repositories.Implementation
{
    public class TopicRepository : GenericRepository<Topic>, ITopicRepository
    {
        public TopicRepository(FOTADbContext context) : base(context) { }
        public override async Task<IEnumerable<Topic>> GetAllAsync()
        {
            return await _dbSet
                 .Include(t => t.TopicSubscribers)
                 .Include(t => t.Diagnostics)
                 .Include(t => t.Messages)
                 .Include(t => t.TeamTopics)
                     .ThenInclude(tt => tt.Team)
                 .ToListAsync();
        }
        public override async Task<IEnumerable<Topic>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(t => t.Name.Contains(name) && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Topic>> GetActiveTopicsAsync()
        {
            return await _dbSet
                .Where(t => t.IsActive && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Topic>> GetByTeamAsync(int teamId)
        {
            return await _context.TeamTopics
                .Where(tt => tt.TeamId == teamId && tt.IsActive)
                .Include(tt => tt.Topic)
                .Select(tt => tt.Topic)
                .ToListAsync();
        }

        public override async Task<Topic?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(t => t.TopicSubscribers)
                    .ThenInclude(ts => ts.Subscriber)
                .Include(t => t.Messages)
                .Include(t => t.TeamTopics)
                    .ThenInclude(tt => tt.Team)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Soft delete implementation
        public override async Task<bool> DeleteAsync(int id)
        {
            var topic = await _dbSet.FindAsync(id);
            if (topic == null) return false;

            topic.IsDeleted = true;
            topic.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Topic?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.Name == name && !t.IsDeleted);
        }

    }
}