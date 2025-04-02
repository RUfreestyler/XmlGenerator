using System.Text;
using System.Xml.Schema;
using System.Xml;

namespace XmlGenerator
{
  public class XsdLoader : IXsdLoader
  {
    public XmlSchemaSet LoadXsd(byte[] xsdBytes, Encoding xsdEncoding)
    {
      using (var stream = new MemoryStream(xsdBytes))
      using (var reader = new StreamReader(stream, xsdEncoding))
        return ReadAndCompileSchema(reader);
    }

    public XmlSchemaSet LoadXsdFromFile(string xsdPath, Encoding xsdEncoding)
    {
      if (!File.Exists(xsdPath))
        throw new FileNotFoundException(xsdPath);

      if (Path.GetExtension(xsdPath) != ".xsd")
        throw new ArgumentException($"Schema file must have .xsd extension. Actual: {xsdPath}");

      using (var reader = new StreamReader(xsdPath, xsdEncoding))
        return ReadAndCompileSchema(reader);
    }

    private XmlSchemaSet ReadAndCompileSchema(StreamReader xsdStreamReader)
    {
      using (var xmlReader = XmlReader.Create(xsdStreamReader))
      {
        var schemaSet = new XmlSchemaSet();
        var schema = XmlSchema.Read(xmlReader, ValidationEventHandler);
        schemaSet.Add(schema);
        schemaSet.Compile();
        return schemaSet;
      }
    }

    private void ValidationEventHandler(object sender, ValidationEventArgs e)
    {
      if (e.Severity == XmlSeverityType.Warning)
        Console.WriteLine("WARNING: " + e.Message);
      else
        Console.WriteLine("ERROR: " + e.Message);
    }

    public XsdLoader()
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
  }
}
