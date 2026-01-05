using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fota.DataLayer.Models;
using Fota.Models;
namespace Fota.BusinessLayer.Interfaces
{
    public interface IDeveloperRepository : IGenericRepository<Developer>
    {
        Task<Developer?> GetByEmailAsync(string email);
        Task<IEnumerable<Developer>> GetActiveDevelopersAsync();
        Task<IEnumerable<Developer>> GetByTeamAsync(int teamId);
        Task<IEnumerable<Team>> GetTeamsByDeveloperIdAsync(int developerId);
    }
}
