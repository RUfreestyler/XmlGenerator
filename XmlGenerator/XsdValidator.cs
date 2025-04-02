using System.Xml;
using System.Xml.Schema;

namespace XmlGenerator;

public class XsdValidator : IXsdValidator
{
  public bool ValidateXml(XmlDocument xmlDoc, XmlSchemaSet schemaSet)
  {
    try
    {
      xmlDoc.Schemas.Add(schemaSet);
      xmlDoc.Validate(ValidationEventHandler);
      return true;
    }
    catch (XmlSchemaValidationException ex)
    {
      Console.WriteLine($"Validation error: {ex.Message}");
      return false;
    }
  }

  private void ValidationEventHandler(object sender, ValidationEventArgs e)
  {
    if (e.Severity == XmlSeverityType.Warning)
      Console.WriteLine("WARNING: " + e.Message);
    else
      Console.WriteLine("ERROR: " + e.Message);
  }
}
