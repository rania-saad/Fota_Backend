using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fota.DataLayer.Models;
using SharedProjectDTOs.Admin;
namespace Fota.BusinessLayer.Interfaces
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Task<Admin?> GetByEmailAsync(string email);
        Task<IEnumerable<Admin>> GetActiveAdminsAsync();
    }
}
