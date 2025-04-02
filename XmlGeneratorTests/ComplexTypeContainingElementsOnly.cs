using System.Text;
using XmlGenerator;

namespace XmlGeneratorTests;

[TestFixture]
internal class ComplexTypeContainingElementsOnly
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
  public void GenerateXml_ElementsSequence()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>
  <subElem1>{_settings.DefaultStringValue}</subElem1>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_ElementsSequenceTypeDeclaredSeparately()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"" type=""elemSequence""/>
  <xs:complexType name=""elemSequence"">
    <xs:sequence>
      <xs:element name=""subElem1"" type=""xs:string"" />
      <xs:element name=""subElem2"" type=""xs:string"" />
    </xs:sequence>
  </xs:complexType>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>
  <subElem1>{_settings.DefaultStringValue}</subElem1>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }
}
