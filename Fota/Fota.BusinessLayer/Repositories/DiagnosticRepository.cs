using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.DBContext;
using Fota.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Fota.BusinessLayer.Repositories
{
    public class DiagnosticRepository : GenericRepository<Diagnostic>, IDiagnosticRepository
    {
        public DiagnosticRepository(FOTADbContext context) : base(context) { }

        public override async Task<IEnumerable<Diagnostic>> GetAllAsync()
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.Topic)
                .Include(d => d.AssignedByAdmin)
                .Include(d => d.AssignedToDeveloper)
                .Include(d => d.Solutions)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public override async Task<Diagnostic?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.Topic)
                .Include(d => d.AssignedByAdmin)
                .Include(d => d.AssignedToDeveloper)
                .Include(d => d.Solutions)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public override async Task<IEnumerable<Diagnostic>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.Topic)
                .Where(d => d.Title.Contains(name) || (d.Description != null && d.Description.Contains(name)))
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Diagnostic>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.Topic)
                .Include(d => d.AssignedToDeveloper)
                .Where(d => d.Status == status)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Diagnostic>> GetByPriorityAsync(string priority)
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.Topic)
                .Include(d => d.AssignedToDeveloper)
                .Where(d => d.Priority == priority)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Diagnostic>> GetBySubscriberAsync(int subscriberId)
        {
            return await _dbSet
                .Include(d => d.Topic)
                .Include(d => d.AssignedToDeveloper)
                .Include(d => d.Solutions)
                .Where(d => d.SubscriberId == subscriberId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Diagnostic>> GetByTopicAsync(int topicId)
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.AssignedToDeveloper)
                .Where(d => d.TopicId == topicId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Diagnostic>> GetByAssignedDeveloperAsync(int developerId)
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.Topic)
                .Include(d => d.Solutions)
                .Where(d => d.AssignedToDeveloperId == developerId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Diagnostic>> GetOpenDiagnosticsAsync()
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.Topic)
                .Include(d => d.AssignedToDeveloper)
                .Where(d => d.Status == "Open" || d.Status == "InProgress")
                .OrderBy(d => d.Priority == "Critical" ? 1 : d.Priority == "High" ? 2 : d.Priority == "Medium" ? 3 : 4)
                .ThenByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Diagnostic>> GetUnassignedDiagnosticsAsync()
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.Topic)
                .Where(d => d.AssignedToDeveloperId == null && (d.Status == "Open" || d.Status == "InProgress"))
                .OrderBy(d => d.Priority == "Critical" ? 1 : d.Priority == "High" ? 2 : d.Priority == "Medium" ? 3 : 4)
                .ThenByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<Diagnostic?> AssignToDeveloperAsync(int diagnosticId, int developerId, int adminId)
        {
            var diagnostic = await GetByIdAsync(diagnosticId);
            if (diagnostic == null || diagnostic.Status == "Closed")
                return null;

            diagnostic.AssignedToDeveloperId = developerId;
            diagnostic.AssignedByAdminId = adminId;
            diagnostic.Status = "InProgress";
            diagnostic.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return diagnostic;
        }

        public async Task<Diagnostic?> UpdateStatusAsync(int diagnosticId, string status)
        {
            var diagnostic = await GetByIdAsync(diagnosticId);
            if (diagnostic == null)
                return null;

            diagnostic.Status = status;
            diagnostic.UpdatedAt = DateTime.UtcNow;

            if (status == "Resolved" && diagnostic.ResolvedAt == null)
            {
                diagnostic.ResolvedAt = DateTime.UtcNow;
            }
            else if (status == "Closed" && diagnostic.ClosedAt == null)
            {
                diagnostic.ClosedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return diagnostic;
        }

        public async Task<Diagnostic?> ResolveDiagnosticAsync(int diagnosticId)
        {
            var diagnostic = await GetByIdAsync(diagnosticId);
            if (diagnostic == null || diagnostic.Status == "Closed")
                return null;

            diagnostic.Status = "Resolved";
            diagnostic.ResolvedAt = DateTime.UtcNow;
            diagnostic.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return diagnostic;
        }

        public async Task<Diagnostic?> CloseDiagnosticAsync(int diagnosticId)
        {
            var diagnostic = await GetByIdAsync(diagnosticId);
            if (diagnostic == null)
                return null;

            diagnostic.Status = "Closed";
            diagnostic.ClosedAt = DateTime.UtcNow;
            diagnostic.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return diagnostic;
        }

        public async Task<IEnumerable<Diagnostic>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(d => d.Subscriber)
                .Include(d => d.Topic)
                .Include(d => d.AssignedToDeveloper)
                .Where(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }
    }
}