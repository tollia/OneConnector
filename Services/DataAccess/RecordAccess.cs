using Microsoft.Extensions.Logging;
using OneConnector.Services.Models;
using OneConnector.Services.Utils;
using OneRecord;
using System;

namespace OneConnector.Services.DataAccess
{
    public sealed class RecordAccess
    {
        private ILogger<RecordAccess> Logger { get; }
        private ApiTokenAccess ApiTokenAccess { get; }
        private OneSoap<OneRecordSoap> OneRecordSoap { get; }

        public RecordAccess(
            OneSoap<OneRecordSoap> oneRecordSoap,
            ApiTokenAccess apiTokenAccess,
            ILogger<RecordAccess> logger
        )
        {
            OneRecordSoap = oneRecordSoap;
            ApiTokenAccess = apiTokenAccess;
            Logger = logger;
        }

        public ItemProperties GetCase(string caseNumber)
        {
            using (DisposableWrapper<OneRecordSoap> OneRecordDisposableClient = OneRecordSoap.DisposableClient)
            {
                OneRecordSoap recordClient = OneRecordDisposableClient.Base;
                GetCaseInfoResponse caseInfoResponse = recordClient.GetCaseInfo(
                    new()
                    {
                        Body = new()
                        {
                            CaseNumber = caseNumber,
                            sToken = ApiTokenAccess.ApiToken
                        }
                    }
                );
                return Xml.Deserialize<ItemProperties>(caseInfoResponse.Body.GetCaseInfoResult);
            }
        }

        public CaseItems GetAllCaseSubItems(string caseNumber)
        {
            using (DisposableWrapper<OneRecordSoap> OneRecordDisposableClient = OneRecordSoap.DisposableClient)
            {
                OneRecordSoap recordClient = OneRecordDisposableClient.Base;
                GetAllCaseSubItemsResponse allCaseSubItemsResponse = recordClient.GetAllCaseSubItems(
                    new()
                    {
                        Body = new()
                        {
                            CaseNumber = caseNumber,
                            sToken = ApiTokenAccess.ApiToken
                        }
                    }
                );
                string caseItems = Xml.RenameNodes(
                    allCaseSubItemsResponse.Body.GetAllCaseSubItemsResult,
                    new()
                    {
                        { "/CaseItems/Item/Data/Item", "ItemDetails" },
                        { "/CaseItems/Item/Data", "ItemData" },
                        { "/CaseItems/Item/Attachments/Data", "AttachmentData" }
                    }
                );
                return Xml.Deserialize<CaseItems>(caseItems);
            }
        }

        public CustomerCases GetCustomerCases(string idNumber)
        {
            using (DisposableWrapper<OneRecordSoap> OneRecordDisposableClient = OneRecordSoap.DisposableClient)
            {
                OneRecordSoap recordClient = OneRecordDisposableClient.Base;
                GetCustomerCasesResponse customerCasesResponse = recordClient.GetCustomerCases(
                    new()
                    {
                        Body = new()
                        {
                            IDNumber = idNumber,
                            sToken = ApiTokenAccess.ApiToken
                        }
                    }
                );
                return Xml.Deserialize<CustomerCases>(customerCasesResponse.Body.GetCustomerCasesResult);
            }
        }



        public ItemInfo GetItemInfo(string itemId, string sContentClass)
        {
            using (DisposableWrapper<OneRecordSoap> OneRecordDisposableClient = OneRecordSoap.DisposableClient)
            {
                OneRecordSoap recordClient = OneRecordDisposableClient.Base;
                GetItemInfoResponse caseInfoResponse = recordClient.GetItemInfo(
                    new()
                    {
                        Body = new()
                        {
                            ItemId = itemId,
                            sContentClass = sContentClass,
                            sToken = ApiTokenAccess.ApiToken
                        }
                    }
                );
                string itemInfo = Xml.RenameNodes(
                    "<ItemInfo>" + caseInfoResponse.Body.GetItemInfoResult + "</ItemInfo>",
                    new()
                    {
                        { "/ItemInfo/Item/Attachments/Data", "AttachmentData" }
                    }
                );
                return Xml.Deserialize<ItemInfo>(itemInfo);
            }
        }

        public string GetDocumentInfo(string documentId)
        {
            using (DisposableWrapper<OneRecordSoap> OneRecordDisposableClient = OneRecordSoap.DisposableClient)
            {
                OneRecordSoap recordClient = OneRecordDisposableClient.Base;
                GetDocumentInfoResponse documentInfoResponse = recordClient.GetDocumentInfo(
                    new()
                    {
                        Body = new()
                        { 
                            DocumentId = documentId,
                            sToken = ApiTokenAccess.ApiToken
                        }
                    }
                );
                return documentInfoResponse.Body.GetDocumentInfoResult;
            }
        }

        public byte[] GetDocumentBytes(string DocumentId)
        {
            using (DisposableWrapper<OneRecordSoap> OneRecordDisposableClient = OneRecordSoap.DisposableClient)
            {
                OneRecordSoap recordClient = OneRecordDisposableClient.Base;
                GetDocumentBytesResponse documentBytesResponse = recordClient.GetDocumentBytes(
                    new()
                    {
                        Body = new()
                        {
                            DocumentId = DocumentId,
                            sToken = ApiTokenAccess.ApiToken
                        }
                    }
                );
                return documentBytesResponse.Body.GetDocumentBytesResult;
            }
        }

        public string AddDocument(
            string CaseNumber,
            string DocumentNumber,
            string FileName,
            string User,
            byte[] encodedData
        )
        {
            using (DisposableWrapper<OneRecordSoap> OneRecordDisposableClient = OneRecordSoap.DisposableClient)
            {
                OneRecordSoap recordClient = OneRecordDisposableClient.Base;
                AddDocumentResponse response = recordClient.AddDocument(
                    new()
                    {
                        Body = new()
                        {
                            CaseNumber = CaseNumber,
                            DocumentNumber = DocumentNumber,
                            FileName = FileName,
                            CreateDate = DateTime.Now,
                            document = encodedData,
                            User = User,
                            sToken = ApiTokenAccess.ApiToken
                        }
                    }
                );
                return response.Body.AddDocumentResult;
            }
        }

    }
}
