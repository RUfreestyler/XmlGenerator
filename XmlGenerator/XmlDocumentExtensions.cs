using System.Text;
using System.Xml;

namespace XmlGenerator
{
  public static class XmlDocumentExtensions
  {
    public static string ToHumanReadableXml(this XmlDocument xmlDocument, Encoding encoding = null)
    {
      if (encoding == null) encoding = Encoding.UTF8;
      var sb = new StringBuilder();
      var settings = new XmlWriterSettings { Encoding = encoding, Indent = true };
      using (var xmlWriter = XmlWriter.Create(sb, settings))
        xmlDocument.WriteTo(xmlWriter);

      return sb.ToString();

    }
  }
}
