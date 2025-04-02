using System.Xml;

namespace XmlGenerator;

public interface IXmlGenerator
{
  XmlDocument GenerateXml(byte[] xsdBytes);

  XmlDocument GenerateXml(string xsdPath);
}
