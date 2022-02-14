using OneConnector.Services.Utils;
using System.Xml.Serialization;

namespace OneConnector.Services.Models
{
    [XmlRoot("Data")]
    public class CustomerCases
    {
        [XmlElement("Item")]
        public CustomerCaseItem[] Items { get; set; }
    }

    public class CustomerCaseItem
    {
        public Subject Subject { get; set; }
        public CategoryName CategoryName { get; set; }
        public Status Status { get; set; }
        public UID UID { get; set; }
    }

    public class TestCustomerCases
    {
        public static string GetTestSerialize()
        {
            CustomerCases ci = new()
            {
                Items = new CustomerCaseItem[]
                {
                    new()
                    {
                        CategoryName = new()
                        {
                             DataType = "System.String",
                             FullName = "CategoryName",
                             Value = ""
                        },
                        Status = new()
                        {
                             DataType = "System.String",
                             FullName = "Status",
                             Value = "Í Úrvinnslu"
                        },
                        Subject = new()
                        {
                             DataType = "System.String",
                             FullName = "Subject",
                             Value = "Sumarstörf í skjalavörslu"
                        },
                        UID = new()
                        {
                             DataType = "System.String",
                             FullName = "UID",
                             Value = "21071766"
                        }
                    },
                    new()
                    {
                        CategoryName = new()
                        {
                            DataType = "System.String",
                            FullName = "CategoryName",
                            Value = "211.00 Grunnskólar almennt"
                        },
                        Status = new()
                        {
                            DataType = "System.String",
                            FullName = "Status",
                            Value = "Nýskráð"
                        },
                        Subject = new()
                        {
                            DataType = "System.String",
                            FullName = "Subject",
                            Value = "Grunnskóladeild-Samstarf og samvinna foreldra og skóla"
                        },
                        UID = new()
                        {
                            DataType = "System.String",
                            FullName = "UID",
                            Value = "1512001"
                        }
                    }
                }

            };
            return Xml.Serialize(ci);
        }
    }

}
