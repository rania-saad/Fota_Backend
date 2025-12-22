using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fota.DataLayer.Models;

namespace Fota.BusinessLayer.Interfaces
{
    public interface IDiagnosticRepository : IGenericRepository<Diagnostic>
    {
        Task<IEnumerable<Diagnostic>> GetByStatusAsync(string status);
        Task<IEnumerable<Diagnostic>> GetByPriorityAsync(string priority);
        Task<IEnumerable<Diagnostic>> GetBySubscriberAsync(int subscriberId);
        Task<IEnumerable<Diagnostic>> GetByTopicAsync(int topicId);
        Task<IEnumerable<Diagnostic>> GetByAssignedDeveloperAsync(int developerId);
        Task<IEnumerable<Diagnostic>> GetOpenDiagnosticsAsync();
        Task<IEnumerable<Diagnostic>> GetUnassignedDiagnosticsAsync();
        Task<Diagnostic?> AssignToDeveloperAsync(int diagnosticId, int developerId, int adminId);
        Task<Diagnostic?> UpdateStatusAsync(int diagnosticId, string status);
        Task<Diagnostic?> ResolveDiagnosticAsync(int diagnosticId);
        Task<Diagnostic?> CloseDiagnosticAsync(int diagnosticId);
        Task<IEnumerable<Diagnostic>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}