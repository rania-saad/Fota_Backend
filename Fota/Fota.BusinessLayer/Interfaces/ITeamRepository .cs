using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fota.DataLayer.Models;
namespace Fota.BusinessLayer.Interfaces
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Task<IEnumerable<Team>> GetActiveTeamsAsync();
        Task<IEnumerable<Team>> GetByLeadAsync(int leadId);
    }
}
