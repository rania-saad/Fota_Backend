//using Fota.BusinessLayer.Interfaces;
//using Fota.DataLayer.DBContext;
//using Fota.DataLayer.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Fota.BusinessLayer.Repositories
//{

//    public class TeamRepository : GenericRepository<Team>, ITeamRepository
//    {
//        public TeamRepository(FOTADbContext context) : base(context) { }

//        public override async Task<IEnumerable<Team>> SearchByNameAsync(string name)
//        {
//            return await _dbSet
//                .Where(t => t.Name.Contains(name))
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Team>> GetActiveTeamsAsync()
//        {
//            return await _dbSet
//                .Where(t => t.IsActive)
//                .Include(t => t.Lead)
//                .ToListAsync();
//        }

//        public async Task<IEnumerable<Team>> GetByLeadAsync(int leadId)
//        {
//            return await _dbSet
//                .Where(t => t.LeadId == leadId)
//                .Include(t => t.Lead)
//                .ToListAsync();
//        }

//        public override async Task<Team?> GetByIdAsync(int id)
//        {
//            return await _dbSet
//                .Include(t => t.Lead)
//                .Include(t => t.CreatedByAdmin)
//                .Include(t => t.TeamDevelopers)
//                    .ThenInclude(td => td.Developer)
//                .Include(t => t.TeamTopics)
//                    .ThenInclude(tt => tt.Topic)
//                .FirstOrDefaultAsync(t => t.Id == id);
//        }
//    }
//}





using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.DBContext;
using Fota.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Fota.BusinessLayer.Repositories
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(FOTADbContext context) : base(context) { }

        public override async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Lead)
                .Include(t => t.CreatedByAdmin)
                .Include(t => t.TeamDevelopers)
                    .ThenInclude(td => td.Developer)
                .Include(t => t.TeamTopics)
                    .ThenInclude(tt => tt.Topic)
                .ToListAsync();
        }

        public override async Task<Team?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Lead)
                .Include(t => t.CreatedByAdmin)
                .Include(t => t.TeamDevelopers)
                    .ThenInclude(td => td.Developer)
                .Include(t => t.TeamTopics)
                    .ThenInclude(tt => tt.Topic)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<IEnumerable<Team>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(t => t.Name.Contains(name))
                .Include(t => t.TeamDevelopers)
                .Include(t => t.TeamTopics)
                .ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetActiveTeamsAsync()
        {
            return await _dbSet
                .Where(t => t.IsActive)
                .Include(t => t.TeamDevelopers)
                .Include(t => t.TeamTopics)
                .ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetByLeadAsync(int leadId)
        {
            return await _dbSet
                .Where(t => t.LeadId == leadId)
                .Include(t => t.TeamDevelopers)
                .Include(t => t.TeamTopics)
                .ToListAsync();
        }
    }
}
