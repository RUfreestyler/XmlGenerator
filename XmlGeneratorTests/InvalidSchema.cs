using System.Text;
using XmlGenerator;

namespace XmlGeneratorTests;

[TestFixture]
internal class InvalidSchema
{
  private XmlDocumentGenerator _generator;
  private XmlGeneratorSettings _settings;

  [OneTimeSetUp]
  public void OneTimeSetUp()
  {
    _settings = new XmlGeneratorSettings();
    _generator = new XmlDocumentGenerator(new XsdLoader(), _settings);
  }

  [SetUp]
  public void SetUp()
  {
    _settings = new XmlGeneratorSettings();
  }

  [Test]
  public void GenerateXml_InvalidSchema_ThrowsException()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?><xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema""></xs:schema>";

    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    Assert.Throws<InvalidOperationException>(() => _generator.GenerateXml(xsdBytes));
  }
}