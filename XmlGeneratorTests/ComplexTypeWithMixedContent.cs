using System.Text;
using XmlGenerator;

namespace XmlGeneratorTests;

[TestFixture]
internal class ComplexTypeWithMixedContent
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
  public void GenerateXml_MixedContent()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType mixed=""true"">
      <xs:sequence>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" />
        <xs:element name=""subElem3"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>
  <subElem1>{_settings.DefaultStringValue}</subElem1>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem3>{_settings.DefaultStringValue}</subElem3>
</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }
}
