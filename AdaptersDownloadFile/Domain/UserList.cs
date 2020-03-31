using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptersDownloadFile.Domain
{
    class UserList
    {
        string projectname;
        string jobnumber;
        string transmittalnumber;
        string transmittalrecordnumber;
        string email;
        string recipientname;
        string action;

        public string Projectname { get => projectname; set => projectname = value; }
        public string Jobnumber { get => jobnumber; set => jobnumber = value; }
        public string Transmittalnumber { get => transmittalnumber; set => transmittalnumber = value; }
        public string Transmittalrecordnumber { get => transmittalrecordnumber; set => transmittalrecordnumber = value; }
        public string Email { get => email; set => email = value; }
        public string Recipientname { get => recipientname; set => recipientname = value; }
        public string Action { get => action; set => action = value; }
    }
}
