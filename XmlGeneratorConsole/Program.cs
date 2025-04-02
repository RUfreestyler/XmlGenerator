using System.Text;
using XmlGenerator;

if (args.Length <= 0)
  throw new ArgumentException("Path to XSD-scheme in first parameter is missing");

var xsdPath = args[0];

var savePath = string.Empty;
if (args.Length > 1)
  savePath = args[1];
else
  savePath = Path.Combine(Path.GetDirectoryName(xsdPath), $"{Path.GetFileNameWithoutExtension(xsdPath)}_generated.xml");

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var xsdEncoding = Encoding.GetEncoding("windows-1251");
Console.OutputEncoding = Encoding.UTF8;

var validator = new XsdValidator();
var loader = new XsdLoader();
var settings = new XmlGeneratorSettings { Encoding = xsdEncoding };
var generator = new XmlDocumentGenerator(loader, settings);
var xmlDoc = generator.GenerateXml(xsdPath);

var schema = loader.LoadXsdFromFile(xsdPath, xsdEncoding);
var isValid = validator.ValidateXml(xmlDoc, schema);
Console.WriteLine("XML is valid: " + isValid);

xmlDoc.Save(savePath);

Console.WriteLine("XML generated successfully.");