using System.Text;
using XmlGenerator;

namespace XmlGeneratorTests;

[TestFixture]
internal class ComplexTextOnlyElement
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
  public void GenerateXml_ExtensionElement()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:simpleContent>
        <xs:extension base=""xs:integer"">
          <xs:attribute name=""attr"" type=""xs:string"" />
        </xs:extension>
      </xs:simpleContent>
    </xs:complexType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem attr=""{_settings.DefaultStringValue}"">{_settings.DefaultIntegerValue}</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }
}
