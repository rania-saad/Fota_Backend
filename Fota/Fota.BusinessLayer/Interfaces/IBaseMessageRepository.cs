using Fota.DataLayer.Enum;
using Fota.DataLayer.Models;
using Fota.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.BusinessLayer.Interfaces
{
    public interface IBaseMessageRepository : IGenericRepository<BaseMessage> 
    {


        Task<IEnumerable<BaseMessage>> GetByStatusAsync(BaseMessageStatus status);
        Task<IEnumerable<BaseMessage>> GetByMessageTypeAsync(BaseMessageType messageType);
        Task<IEnumerable<BaseMessage>> GetByUploaderAsync(int uploaderId);
        Task<IEnumerable<BaseMessage>> GetByTopicAsync(int topicId);
        Task<IEnumerable<BaseMessage>> GetPublishedMessagesAsync();
        Task<IEnumerable<BaseMessage>> GetPendingApprovalAsync();
        Task<BaseMessage?> ApproveMessageAsync(int messageId, int approvedById);
        Task<BaseMessage?> RejectMessageAsync(int messageId, string rejectionReason);
        Task<BaseMessage?> PublishMessageAsync(int messageId, int publisherId);
        Task<int> GetTotalMessagesCountAsync();

    }


    
}
