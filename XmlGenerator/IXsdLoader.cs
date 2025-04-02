using System.Text;
using System.Xml.Schema;

namespace XmlGenerator;

public interface IXsdLoader
{
  XmlSchemaSet LoadXsd(byte[] xsdBytes, Encoding xsdEncoding);

  XmlSchemaSet LoadXsdFromFile(string xsdPath, Encoding xsdEncoding);
}
