using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.DBContext;
using Fota.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
namespace Fota.BusinessLayer.Repositories
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public AdminRepository(FOTADbContext context) : base(context) { }


        public override async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _dbSet
                .Include(a => a.AssignedDiagnostics)
                .Include(a => a.CreatedTeams)
                .ToListAsync();
        }


        public override async Task<IEnumerable<Admin>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(a => a.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<IEnumerable<Admin>> GetActiveAdminsAsync()
        {
            return await _dbSet
                .Where(a => a.IsActive)
                .ToListAsync();
        }

        public override async Task<Admin?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.AssignedDiagnostics)
                .Include(a => a.CreatedTeams)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
