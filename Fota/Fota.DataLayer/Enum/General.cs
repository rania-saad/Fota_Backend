using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.DataLayer.Enum
{
        public enum DiagnosticStatus
        {
            //Delivered,
            //Pending,
            //Failed

            Open,
            InProgress,
            Closed,
            Resolved
        }
        public enum DiagnosticPriority
        {
            High,
            Low,
            Medium ,
            Critical
        }

        public enum BaseMessageStatus
        {
            Draft,
            Pending,
            Approved,
            Published,
            Rejected,
            Received
    }
        public enum BaseMessageType
        {
            Standard,
            Diagnostic,
            Broadcast,
            BugFix,
            Update,
            Feature,
            Patch,
            


    }

        
    
}
