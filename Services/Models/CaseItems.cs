using OneConnector.Services.Utils;
using System;
using System.Globalization;
using System.Xml.Serialization;

namespace OneConnector.Services.Models
{
    public class CaseItems
    {
        [XmlElement("Item")]
        public CaseItem[] Items { get; set; }
    }

    public class CaseItem
    {
        [XmlElement("Attachments")]
        public CaseAttachments Attachments { get; set; }
        [XmlElement("ItemData")]
        public CaseItemData ItemData { get; set; }
    }

    public class CaseAttachments
    {
        [XmlArrayItem(ElementName = "AttachmentData", Type = typeof(string))]
        [XmlArray]
        public string[] AttachmentArray { get; set; }
    }

    public class CaseItemData
    {
        [XmlElement("ItemDetails")]
        public CaseItemDetails ItemDetails { get; set; }
    }

    public class CaseItemDetails
    {
        public ContentClass ContentClass { get; set; }
        public Subject Subject { get; set; }
        public Company Company { get; set; }
        public UID UID { get; set; }
        public Modified Modified { get; set; }
        public CreateDate CreateDate { get; set; }
        public ItemID ItemID { get; set; }
        public ModifiedBy ModifiedBy { get; set; }
        public Status Status { get; set; }
        public Author Author { get; set; }
        public Creator Creator { get; set; }
        public DocCategory DocCategory { get; set; }
        public DocType DocType { get; set; }
    }

    public class TestCaseItems
    {
        public static string GetTestSerialize()
        {
            CaseItems ci = new()
            {
                Items = new CaseItem[]
                {
                            new()
                            {
                                Attachments = new CaseAttachments(),
                                ItemData = new()
                                {
                                    ItemDetails = new()
                                    {
                                        Author = new() { Value = "xxxAuthor" },
                                        Company = new() { Value = "xxxCompany" }
                                    }
                                }
                            },
                            new()
                            {
                                Attachments = new CaseAttachments(),
                                ItemData = new()
                                {
                                    ItemDetails = new()
                                    {
                                        Author = new() { Value = "qqqAuthor" },
                                        Company = new() { Value = "qqqCompany" }
                                    }
                                }
                            }

                }
            };
            return Xml.Serialize(ci);
        }
    }
}   
    
 
