using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fota.DataLayer.Models;
using Fota.Models;
namespace Fota.BusinessLayer.Interfaces
{
    public interface ITopicRepository : IGenericRepository<Topic>
    {
        Task<IEnumerable<Topic>> GetActiveTopicsAsync();
        Task<IEnumerable<Topic>> GetByTeamAsync(int teamId);
    }
}
