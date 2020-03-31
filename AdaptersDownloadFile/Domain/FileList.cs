using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptersDownloadFile.Domain
{
    class FileList
    {
        string projectnumber;
        string record_No;
        string transNumber;
        string file_id;

        public string Projectnumber { get => projectnumber; set => projectnumber = value; }
        public string Record_No { get => record_No; set => record_No = value; }
        public string TransNumber { get => transNumber; set => transNumber = value; }
        public string File_id { get => file_id; set => file_id = value; }
    }
}
