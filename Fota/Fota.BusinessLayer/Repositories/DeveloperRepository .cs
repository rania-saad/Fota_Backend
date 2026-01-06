    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fota.BusinessLayer.Interfaces;
    using Fota.DataLayer.DBContext;
    using Fota.DataLayer.Models;
    using Fota.Models;
using Microsoft.EntityFrameworkCore;

    namespace Fota.BusinessLayer.Repositories
    {
    public class DeveloperRepository : GenericRepository<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(FOTADbContext context) : base(context) { }



        public virtual async Task<IEnumerable<Developer>> GetAllAsync()
        {
            return await _dbSet
                .Include(d => d.UploadedMessages)
                .Include(d => d.PublishedMessages)
                .Include(d => d.LeadingTeams)
                .Include(d => d.AssignedDiagnostics)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Developer>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(d => d.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<Developer?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<IEnumerable<Developer>> GetActiveDevelopersAsync()
        {
            return await _dbSet
                .Where(d => d.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Developer>> GetByTeamAsync(int teamId)
        {
            return await _context.TeamDevelopers
                .Where(td => td.TeamId == teamId && td.IsActive)
                .Include(td => td.Developer)
                .Select(td => td.Developer)
                .ToListAsync();
        }

        public override async Task<Developer?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(d => d.UploadedMessages)
                .Include(d => d.PublishedMessages)
                .Include(d => d.TeamMemberships)
                .Include(d => d.LeadingTeams)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Team>> GetTeamsByDeveloperIdAsync(int developerId)
        {
            return await _context.TeamDevelopers
                .Where(td => td.DeveloperId == developerId && td.IsActive)
                .Include(td => td.Team)
                    .ThenInclude(t => t.Lead)
                .Include(td => td.Team)
                    .ThenInclude(t => t.TeamTopics)
                .Include(td => td.Team)
                    .ThenInclude(t => t.TeamDevelopers)
                        .ThenInclude(td => td.Developer)
                .Select(td => td.Team)
                .Where(t => t.IsActive)
                .ToListAsync();
        }

        public async Task<int?> GetDeveloperIdByIdentityUserIdAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                return null;

            return await _dbSet
                .Where(d => d.IdentityUserId == identityUserId)
                .Select(d => (int?)d.Id)
                .FirstOrDefaultAsync();
        }

    }
}

