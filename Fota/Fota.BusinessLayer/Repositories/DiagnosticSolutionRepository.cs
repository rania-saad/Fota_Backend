using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.DBContext;
using Fota.DataLayer.Models;
using Fota.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.BusinessLayer.Repositories
{



    public class DiagnosticSolutionRepository
        : GenericRepository<DiagnosticSolution>, IDiagnosticSolutionRepository
    {
        public DiagnosticSolutionRepository(FOTADbContext context) : base(context) { }

        public override async Task<IEnumerable<DiagnosticSolution>> GetAllAsync()
        {
            return await _dbSet
                .Include(d => d.Diagnostic)
                .Include(d => d.Developer)
                .ToListAsync();
        }

        public async Task<IEnumerable<DiagnosticSolution>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(d => d.Diagnostic)
                .Include(d => d.Developer)
                .Where(d => d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<DiagnosticSolution>> GetByDeveloperAsync(int devId)
        {
            return await _dbSet
                .Include(d => d.Diagnostic)
                .Where(d => d.DeveloperId == devId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DiagnosticSolution>> GetSubmittedAsync()
        {
            return await _dbSet
                .Include(d => d.Diagnostic)
                .Include(d => d.Developer)
                .Where(d => d.Status == "Submitted")
                .OrderBy(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<DiagnosticSolution?> ApproveAsync(int id, int approvedByDevId)
        {
            var sol = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (sol == null || sol.Status != "Pending") return null;

            sol.Status = "Approved";
            sol.ApprovedAt = DateTime.UtcNow;
            sol.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return sol;
        }


        //public async Task<DiagnosticSolution?> RejectAsync(int id, string reason)
        //{
        //    var sol = await GetByIdAsync(id);
        //    if (sol == null || sol.Status != "Pending") return null;

        //    sol.Status = "Rejected";
        //    sol.RejectedAt = DateTime.UtcNow;
        //    sol.RejectionReason = reason;
        //    sol.UpdatedAt = DateTime.UtcNow;

        //    await _context.SaveChangesAsync();
        //    return sol;
        //}

        public async Task<DiagnosticSolution?> RejectAsync(int messageId, string rejectionReason)
        {
            var message = await GetByIdAsync(messageId);
            if (message == null || message.Status != "Pending")
                return null;

            message.Status = "Rejected";
            message.RejectedAt = DateTime.UtcNow;
            message.RejectionReason = rejectionReason;
            message.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }



    }



}  


