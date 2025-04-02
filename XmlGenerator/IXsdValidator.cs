using System.Xml.Schema;
using System.Xml;

namespace XmlGenerator;

public interface IXsdValidator
{
  bool ValidateXml(XmlDocument xmlDoc, XmlSchemaSet schemaSet);
}
