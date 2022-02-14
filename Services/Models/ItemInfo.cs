using System.Xml.Serialization;

namespace OneConnector.Services.Models
{
    [XmlRoot("ItemInfo")]
    public class ItemInfo
    {
        [XmlElement("Data")]
        public ItemInfoData Data { get; set; }

        [XmlElement("Attachments")]
        public ItemInfoAttachments Attachments { get; set; }
    }

    public class ItemInfoData
    {
        [XmlElement("Item")]
        public ItemInfoItem Item { get; set; }
    }

    public class ItemInfoItem
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

    public class ItemInfoAttachments
    {
        [XmlArrayItem(ElementName = "AttachmentData", Type = typeof(string))]
        [XmlArray]
        public string[] AttachmentArray { get; set; }
    }
}
