using System.Text;
using XmlGenerator;

namespace XmlGeneratorTests;

[TestFixture]
internal class ElementRestrictions
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
  public void GenerateXml_Integer_MinInclusiveRestriction()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:simpleType>
      <xs:restriction base=""xs:integer"">
        <xs:minInclusive value=""10""/>
      </xs:restriction>
    </xs:simpleType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>10</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_Integer_MaxInclusiveRestriction()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:simpleType>
      <xs:restriction base=""xs:integer"">
        <xs:maxInclusive value=""1000000""/>
      </xs:restriction>
    </xs:simpleType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>1000000</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_Integer_MinAndMaxInclusiveRestriction()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:simpleType>
      <xs:restriction base=""xs:integer"">
        <xs:minInclusive value=""10""/>
        <xs:maxInclusive value=""1000000""/>
      </xs:restriction>
    </xs:simpleType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>1000000</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_EnumerationRestriction()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:simpleType>
      <xs:restriction base=""xs:string"">
        <xs:enumeration value=""value1""/>
        <xs:enumeration value=""value2""/>
        <xs:enumeration value=""value3""/>
      </xs:restriction>
    </xs:simpleType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>value1</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_PatternRestriction()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:simpleType>
      <xs:restriction base=""xs:string"">
        <xs:pattern value=""[A-Z]{2}\d{3}""/>
      </xs:restriction>
    </xs:simpleType>
  </xs:element>
</xs:schema>";

    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);
    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Does.Match(@"^<\?xml version=""1\.0"" encoding=""utf-16""\?>\r?\n<elem>[A-Z]{2}\d{3}</elem>$"));
  }

  [Test]
  public void GenerateXml_MinLengthRestriction()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:simpleType>
      <xs:restriction base=""xs:string"">
        <xs:minLength value=""5""/>
      </xs:restriction>
    </xs:simpleType>
  </xs:element>
</xs:schema>";

    _settings.DefaultStringValue = string.Empty;
    _generator.Settings = _settings;
    var expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>xxxxx</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_MaxLengthRestriction()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:simpleType>
      <xs:restriction base=""xs:string"">
        <xs:maxLength value=""3""/>
      </xs:restriction>
    </xs:simpleType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>{_settings.DefaultStringValue.Substring(0, 3)}</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_Decimal_TotalDigitsRestriction()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:simpleType>
      <xs:restriction base=""xs:decimal"">
        <xs:totalDigits value=""3""/>
      </xs:restriction>
    </xs:simpleType>
  </xs:element>
</xs:schema>";

    var expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>100</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }
}