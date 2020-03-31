using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptersDownloadFile.Domain
{ 
    class DeliverableList
    {
        string projectName;
        string jobNumber;
        string transmittalNumber;
        string transmittalsRecordNumber;
        string actionRequired;
        string recordNumber;
        string remarks;
        string documentNumber;
        string reasonForIssue;
        string documentTitle;
        string approvalCode;
        string sheetnumber;
        string documentStatus;
        string revision;
        string uRL;

        public string ProjectName { get => projectName; set => projectName = value; }
        public string JobNumber { get => jobNumber; set => jobNumber = value; }
        public string TransmittalNumber { get => transmittalNumber; set => transmittalNumber = value; }
        public string TransmittalsRecordNumber { get => transmittalsRecordNumber; set => transmittalsRecordNumber = value; }
        public string ActionRequired { get => actionRequired; set => actionRequired = value; }
        public string RecordNumber { get => recordNumber; set => recordNumber = value; }
        public string Remarks { get => remarks; set => remarks = value; }
        public string DocumentNumber { get => documentNumber; set => documentNumber = value; }
        public string DocumentTitle { get => documentTitle; set => documentTitle = value; }
        public string ReasonForIssue { get => reasonForIssue; set => reasonForIssue = value; }
        public string ApprovalCode { get => approvalCode; set => approvalCode = value; }
        public string Sheetnumber { get => sheetnumber; set => sheetnumber = value; }
        public string DocumentStatus { get => documentStatus; set => documentStatus = value; }
        public string Revision { get => Revision1; set => Revision1 = value; }
        public string Revision1 { get => revision; set => revision = value; }
        public string URL { get => uRL; set => uRL = value; }

      
    }
}

