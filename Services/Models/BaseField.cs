using System;
using System.Globalization;

namespace OneConnector.Services.Models
{
    public class BaseField<T>
    {

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DataType { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FullName { get; set; }
        [System.Xml.Serialization.XmlText]
        public string Value { get; set; }
        public override string ToString()
        {
            return Value.ToString();
        }
        [System.Xml.Serialization.XmlIgnore]
        public DateTime GetDateTime
        {
            get
            {
                if (DataType == "System.DateTime")
                {
                    IFormatProvider culture = new CultureInfo("is-IS", true);
                    return DateTime.ParseExact(Value, "dd.MM.yyyy HH:mm:ss", culture);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }

        public static implicit operator DateTime(BaseField<T> m)
        {
            return m.GetDateTime;
        }
    }
}
