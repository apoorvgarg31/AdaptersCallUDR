using Newtonsoft.Json.Linq;
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using System.Numerics;
using System.Net;
using System.Security.Policy;
using System.Dynamic;
using System.Globalization;
using AdaptersDownloadFile.Domain;

namespace AdaptersDownloadFile
{
    class Program
    {
        static void Main(string[] args)
        {
            polling();
        }
        public static void polling()
        {
            String DocumentListUDR = "Integration Transmittals Distribution DOc";

            int Count = 0;
            IRestResponse Response;
            //List<DeliverableList> DL = new List<DeliverableList>();
            CallUDR(DocumentListUDR,  out Response, out Count);
            Console.WriteLine(Count);
            if (Count > 0)
            {
                Console.WriteLine("Success");
                FetchDocumentList(DocumentListUDR, Response, Count);
            }
            else
            {
                Console.WriteLine("No row found");
                Thread.Sleep(15000);
                polling();
            }
        }

        public static int CallUDR(String ReportName, out IRestResponse response, out int count)
        {
            var client = new RestClient("https://petrofac-test-unifier-ws.oracleindustry.com/ws/rest/service/v1/data/udr/get/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer eyJ0eXAiOiJEQiJ9.eyJ1c2VybmFtZSI6IiQkYWdhcmcifQ==.3823C934-3EC7-48F5-2F65-0AE7AC55195A8430406F54E65CD3D35746409CE2C4AC");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\n    \"reportname\": \"" + ReportName + "\"\n}", ParameterType.RequestBody);
            response = client.Execute(request);
            var FileListJSON = JObject.Parse(response.Content.ToString());
            String FileListJSONResult = JsonConvert.SerializeObject(FileListJSON);
            var obj = ToExpando(FileListJSONResult);
            string ResponseStatus = (string)FileListJSON["status"];
            String FResponseCode = ResponseStatus;
            Console.WriteLine(FResponseCode);
            count = (obj as dynamic).data[0].report_row.Count;
            return count;

        }

        private static void FetchDocumentList(String ReportName, IRestResponse response, int count)
        {
            var DocumentListJson = JObject.Parse(response.Content.ToString());
            String DocumentListJsonResult = JsonConvert.SerializeObject(DocumentListJson);
            var obj = ToExpando(DocumentListJsonResult);
            List<DeliverableList> DocumentList = new List<DeliverableList>();

            for (int i = 0; i < count; i++)
            {
                var C1 = (obj as dynamic).data[0].report_row[i].c1;
                var C2 = (obj as dynamic).data[0].report_row[i].c2;
                var C3 = (obj as dynamic).data[0].report_row[i].c3;
                var C4 = (obj as dynamic).data[0].report_row[i].c4;
                var C5 = (obj as dynamic).data[0].report_row[i].c5;
                var C6 = (obj as dynamic).data[0].report_row[i].c6;
                var C7 = (obj as dynamic).data[0].report_row[i].c7;
                var C8 = (obj as dynamic).data[0].report_row[i].c8;
                var C9 = (obj as dynamic).data[0].report_row[i].c9;
                var C10 = (obj as dynamic).data[0].report_row[i].c10;
                var C11 = (obj as dynamic).data[0].report_row[i].c11;
                var C12 = (obj as dynamic).data[0].report_row[i].c12;
                var C13 = (obj as dynamic).data[0].report_row[i].c13;
                var C14 = (obj as dynamic).data[0].report_row[i].c14;
                var C15 = (obj as dynamic).data[0].report_row[i].c15;
                DeliverableList uf = new DeliverableList();
                uf.ProjectName = C1;
                uf.JobNumber = C2;
                uf.TransmittalNumber = C3;
                uf.TransmittalsRecordNumber = C4;
                uf.ActionRequired = C5;
                uf.RecordNumber = C6;
                uf.Remarks = C7;
                uf.DocumentNumber = C8;
                uf.ReasonForIssue = C9;
                uf.DocumentTitle = C10;
                uf.ApprovalCode = C11;
                uf.Sheetnumber = C12;
                uf.DocumentStatus = C13;
                uf.Revision = C14;
                uf.URL = C15;

                DocumentList.Add(uf);
            }

                List<string> TransmittalNumberDistinct = new List<string>();
                
                foreach (var Document in DocumentList)
                {
                    TransmittalNumberDistinct.Add(Document.TransmittalNumber);
                }
                TransmittalNumberDistinct = TransmittalNumberDistinct.Distinct().ToList();
                
                List<UserList> userLists = new List<UserList>();
                List<FileList> fileLists = new List<FileList>();
                GetUsers(out userLists);
                GetFileUDR(out fileLists);
                SendGrid sendGrid= new SendGrid();

                    foreach ( var TransmittalNumber in TransmittalNumberDistinct)
                    {
                        foreach (var file in fileLists )
                        {
                            if (TransmittalNumber == file.TransNumber)
                            {
                                GetFileFromUnifier(file.File_id.Replace(",", ""), out String fileLocation);
                                //sendGrid.BinaryFile = UnifierFileResponse.RawBytes;
                                SendGridEmail(TransmittalNumber, DocumentList, userLists , fileLocation);
                            }
                        }
                    
                    }
        }

        private static void GetFileUDR(out List<FileList> fileLists)
        {
                String GetFileUDR = "Test Transmittall CoverSheet";
                 int Count = 0;
                 IRestResponse FileResponse = null;
                 CallUDR(GetFileUDR, out FileResponse, out  Count);
                if (Count > 0)
                {
                    var FileListJSON = JObject.Parse(FileResponse.Content.ToString());
                    String FileListJSONResult = JsonConvert.SerializeObject(FileListJSON);
                    var obj = ToExpando(FileListJSONResult);
                    Console.WriteLine(FileResponse.Content);
                    List<FileList> filelist = new List<FileList>();
                    for (int i = 0; i < Count; i++)
                    {
                        Console.WriteLine("I am in inside GetFileUDR For loop");
                        var CF1 = (obj as dynamic).data[0].report_row[i].c1;
                        var CF2 = (obj as dynamic).data[0].report_row[i].c2;
                        var CF3 = (obj as dynamic).data[0].report_row[i].c3;
                        var CF4 = (obj as dynamic).data[0].report_row[i].c4;
                        FileList fl = new FileList();
                        fl.Projectnumber = CF1;
                        fl.Record_No = CF2;
                        fl.TransNumber = CF3;
                        fl.File_id = CF4;
                        filelist.Add(fl);
                    }
                    fileLists = filelist;
                }
                else
                {
                    Console.WriteLine("I am in inside GetFileUDR For loop but if statement");
                    fileLists = null;
                }

           
            

        }

        private static void GetFileFromUnifier(string UnifierFileID, out String FileLocation)
        {

            
            string UnifierTestLink = "https://petrofac-test-unifier-ws.oracleindustry.com/ws/rest/service/v1/dm//file/download/" + UnifierFileID;
            var GetUnifierFileClient = new RestClient(UnifierTestLink);
            var GetUnifierFileRequest = new RestRequest(Method.GET);
            GetUnifierFileRequest.AddHeader("Authorization", "Bearer eyJ0eXAiOiJEQiJ9.eyJ1c2VybmFtZSI6IiQkYWdhcmcifQ==.3823C934-3EC7-48F5-2F65-0AE7AC55195A8430406F54E65CD3D35746409CE2C4AC");
            GetUnifierFileRequest.AddHeader("Content-Type", "application/json");
            GetUnifierFileRequest.AddHeader("SendChunked", "chunked");
            GetUnifierFileRequest.AddHeader("Accept-Encoding", "gzip, deflate");
            GetUnifierFileRequest.AddHeader("Content-Disposition", "attachment; filename=Autovue_reply.txt");
            GetUnifierFileRequest.AddHeader("Content-Type", "application/octet-stream");
            GetUnifierFileRequest.AddHeader("Accept", "*/*");
            IRestResponse   UnifierFileResponse = GetUnifierFileClient.Execute(GetUnifierFileRequest);
            string FilePath = @"C:\temp\";
            string FileName =     "test.pdf";
            FileLocation = FilePath + FileName;
            File.WriteAllBytes(FileLocation, UnifierFileResponse.RawBytes);
                
                
           
        }

        private static void GetUsers(out List<UserList> userLists)
        {
            try
            {
                String GetUserUDR = "Integration Transmittals  Distribution User";
                int Count = 0;
                IRestResponse UserResponse = null;
                CallUDR(GetUserUDR, out UserResponse, out Count);
                if (Count > 0)
                {
                    var FileListJSON = JObject.Parse(UserResponse.Content.ToString());
                    String FileListJSONResult = JsonConvert.SerializeObject(FileListJSON);
                    var obj = ToExpando(FileListJSONResult);
                    Console.WriteLine(UserResponse.Content);
                    List<UserList> userlist = new List<UserList>();
                    for (int i = 0; i < Count; i++)
                    {
                        var CF1 = (obj as dynamic).data[0].report_row[i].c1;
                        var CF2 = (obj as dynamic).data[0].report_row[i].c2;
                        var CF3 = (obj as dynamic).data[0].report_row[i].c3;
                        var CF4 = (obj as dynamic).data[0].report_row[i].c4;
                        var CF5 = (obj as dynamic).data[0].report_row[i].c2;
                        var CF6 = (obj as dynamic).data[0].report_row[i].c3;
                        var CF7 = (obj as dynamic).data[0].report_row[i].c4;

                        UserList ul = new UserList();
                        ul.Projectname = CF1;
                        ul.Jobnumber = CF2;
                        ul.Transmittalnumber = CF3;
                        ul.Transmittalrecordnumber = CF4;
                        ul.Email = CF2;
                        ul.Recipientname = CF3;
                        ul.Action = CF4;
                        userlist.Add(ul);
                        
                    }
                    userLists = userlist.ToList();
                }
                else
                {
                    userLists = null;
                }
            }
            catch (Exception )
            {
                userLists = null;
            }


        }

        private static void SendGridEmail(String TransmittalNumber, List<DeliverableList> documentList, List<UserList> userList , String fileLocation)
        {
            var client = new RestClient("https://api.sendgrid.com/v3/mail/send");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddFile("test.pdf",fileLocation);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer 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");
            request.AddHeader("Content-Type", "application/json");
            StringBuilder LoopDocumentList = new StringBuilder();
            StringBuilder LoopProjectInfoList = new StringBuilder();
            StringBuilder RecipientList = new StringBuilder();
            String prefix = "";
            foreach (var Transmittal in documentList)
            {
                if (TransmittalNumber == Transmittal.TransmittalNumber)
                {
                    LoopDocumentList.Append(prefix);
                    prefix = ",";
                    LoopDocumentList.Append("{\n \"SNo\": \"1\",\n\"DocumentNumber\": \"" + Transmittal.DocumentNumber + "\",\n                            \"SheetNumber\": \"" + Transmittal.Sheetnumber + "\",\n                            \"Revision\": \"" + Transmittal.Revision + "\",\n                            \"DocumentTitle\": \"" + Transmittal.DocumentTitle + "\",\n                            \"ApprovalCode\": \"" + Transmittal.ApprovalCode + "\",\n                            \"ReasonForIssue\": \"" + Transmittal.ReasonForIssue + "\",\n                            \"DeliverableStatus\": \"" + Transmittal.DocumentStatus + "\"\n                        }");
                    
                }
            }
            foreach (var Transmittal in userList)
            {
                if (TransmittalNumber == Transmittal.Transmittalnumber)
                {
                    RecipientList.Append(prefix);
                    prefix = ",";
                    RecipientList.Append(" {\n                            \"SNo\": \"1\",\n                            \"RecipientName\": \""+Transmittal.Recipientname+"\",\n                            \"Email\": \""+Transmittal.Email+"\",\n                            \"Action\": \""+Transmittal.Action+"\"\n                        }");

                }
            }


            LoopProjectInfoList.Append("{\r\n                    \"ProjectName\": \"UAT-001 Mumbai test\",\r\n                    \"JobNumber\": \"UAT-001\",\r\n                    \"TransNumber\": \"PE-106-PE-ADNOC-T-00001\",\r\n                    \"ActionRequired\": \"\",\r\n                    \"Remarks\": \"This is for TEST\",\r\n                    \"URL\": \"www.google.com\",\r\n         ");

            Console.WriteLine(LoopDocumentList);
            Console.WriteLine(LoopProjectInfoList);
            Console.WriteLine(RecipientList);
            Console.WriteLine("{\n    \"from\": {\n        \"email\": \"CDC-SPC@petrofac.com\",\n        \"name\": \"Transmittal Information\"\n    },\n    \"personalizations\": [\n        {\n            \"to\": [\n                {\n                    \"email\": \"agarg@enstoa.com\"\n                }\n            ],\n            \"dynamic_template_data\": {\n                \"subject\": \"PE-106-PE-ADNOC-T-00001 : TEST-TDR Received from PIL (2nd Rev.)\",\n                \"email\":  {  " + LoopProjectInfoList + "          ,\n                    \"document\": [\n                        \n" + LoopDocumentList + "  ],\n                    \"recipient\": [" + RecipientList + "         ]\n                }\n            }\n        }\n    ],\n    \"template_id\": \"d-d8c14bc619894c60aabd18c23e8f8707\"\n}");
            //request.AddParameter("application/json,application/json", "{\n    \"from\": {\n        \"email\": \"CDC-SPC@petrofac.com\",\n        \"name\": \"Transmittal Information\"\n    },\n    \"personalizations\": [\n        {\n            \"to\": [\n                {\n                    \"email\": \"agarg@enstoa.com\"\n                }\n            ],\n            \"dynamic_template_data\": {\n                \"subject\": \"PE-106-PE-ADNOC-T-00001 : TEST-TDR Received from PIL (2nd Rev.)\",\n                \"email\":  {  "+LoopProjectInfoList+"          ,\n                    \"document\": [\n                        \n" + LoopDocumentList + "  ],\n                    \"recipient\": ["+RecipientList+"         ]\n                }\n            }\n        }\n    ],\n    \"template_id\": \"d-d8c14bc619894c60aabd18c23e8f8707\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            Thread.Sleep(50000);




        }
        public static ExpandoObject ToExpando(string json)
        {
            if (string.IsNullOrEmpty(json))
                return null;
            return (ExpandoObject)ToExpandoObject(JToken.Parse(json));
        }

        private static object ToExpandoObject(JToken token)
        {

            switch (token.Type)
            {
                case JTokenType.Object:
                    var expando = new ExpandoObject();
                    var expandoDic = (IDictionary<string, object>)expando;
                    foreach (var prop in token.Children<JProperty>())
                        expandoDic.Add(prop.Name, ToExpandoObject(prop.Value));
                    return expando;
                case JTokenType.Array:
                    return token.Select(ToExpandoObject).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }

    }

}


