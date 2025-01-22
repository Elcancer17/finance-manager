using System.IO;
using System.Xml.Serialization;

namespace FinanceManager.Import
{
    public static class XMLManager
    {
        public static TObject DeserializeXML<TObject>(string xmlContent, string rootElementName) where TObject : class, new()
        {
            if (!string.IsNullOrEmpty(xmlContent))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TObject));

                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = rootElementName;
                xRoot.IsNullable = true;
                using (TextReader reader = new StringReader(xmlContent))
                {
                    return (TObject)new XmlSerializer(typeof(TObject), xRoot).Deserialize(reader);
                }
            }
            return new TObject();
        }
    }
}
