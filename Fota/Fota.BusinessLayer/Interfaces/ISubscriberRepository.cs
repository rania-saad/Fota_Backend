using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fota.Models;

namespace Fota.BusinessLayer.Interfaces
{
    public interface ISubscriberRepository : IGenericRepository<Subscriber>
    {
        Task<Subscriber?> GetByEmailAsync(string email);
        Task<IEnumerable<Subscriber>> GetActiveSubscribersAsync();
        Task<IEnumerable<Subscriber>> GetByTopicAsync(int topicId);
        Task<IEnumerable<Subscriber>> GetSubscribersWithDiagnosticsAsync();
        Task<IEnumerable<Subscriber>> GetSubscribersWithDeliveriesAsync();
        Task<int> GetTotalCountAsync();
    }
}