using Fota.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.BusinessLayer.Interfaces
{
    public interface IDiagnosticSolutionRepository : IGenericRepository<DiagnosticSolution>
    {
        Task<IEnumerable<DiagnosticSolution>> GetByStatusAsync(string status);
        Task<IEnumerable<DiagnosticSolution>> GetByDeveloperAsync(int devId);
        Task<IEnumerable<DiagnosticSolution>> GetSubmittedAsync();
        Task<DiagnosticSolution?> ApproveAsync(int id, int approvedByDevId);
        Task<DiagnosticSolution?> RejectAsync(int id, string reason);
    }
}
