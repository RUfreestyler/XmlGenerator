using System.Text;
using XmlGenerator;

namespace XmlGeneratorTests;

[TestFixture]
internal class Indicators
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
  public void GenerateXml_AllIndicator()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:all>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" />
      </xs:all>
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
  public void GenerateXml_ChoiceIndicator_WrittenFirst()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:choice>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" />
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>
  <subElem1>{_settings.DefaultStringValue}</subElem1>
</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_SequenceIndicator()
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
  public void GenerateXml_SequenceIndicatorWithMaxOccurs_MaxModeInSettings()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" maxOccurs=""10""/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>
  <subElem1>{_settings.DefaultStringValue}</subElem1>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);
    _settings.ElementCountOutputMode = ElementCountOutputMode.MaxOccurs;
    _generator.Settings = _settings;

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_SequenceIndicatorWithSameMinAndMaxOccurs()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" minOccurs=""5"" maxOccurs=""5""/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>
  <subElem1>{_settings.DefaultStringValue}</subElem1>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_SequenceIndicatorMinOccursIsZeroAndMaxIsTen_MinButAtLeastOneInSettings()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" minOccurs=""0"" maxOccurs=""10""/>
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
    _settings.ElementCountOutputMode = ElementCountOutputMode.MinButAtLeastSingleOccur;
    _generator.Settings = _settings;

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_SequenceIndicatorMinOccursIsZero_MinModeInSettings()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" minOccurs=""0""/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>
  <subElem1>{_settings.DefaultStringValue}</subElem1>
</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);
    _settings.ElementCountOutputMode = ElementCountOutputMode.MinOccurs;
    _generator.Settings = _settings;

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_SequenceIndicatorMinOccursIsZeroAndMaxOccursIsUnbounded_MaxModeInSettings()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" minOccurs=""0"" maxOccurs=""unbounded""/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>
  <subElem1>{_settings.DefaultStringValue}</subElem1>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);
    _settings.ElementCountOutputMode = ElementCountOutputMode.MaxOccurs;
    _settings.MaxUndoundedOccurs = 3;
    _generator.Settings = _settings;

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  [TestCase(ElementCountOutputMode.MinOccurs)]
  [TestCase(ElementCountOutputMode.MaxOccurs)]
  [TestCase(ElementCountOutputMode.MinButAtLeastSingleOccur)]
  public void GenerateXml_SequenceIndicatorMinOccursIsFiveAndMaxOccursIsUnbounded_MaxUnboundedIsThreeInSettings(ElementCountOutputMode outputMode)
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""subElem1"" type=""xs:string"" />
        <xs:element name=""subElem2"" type=""xs:string"" minOccurs=""5"" maxOccurs=""unbounded""/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>
  <subElem1>{_settings.DefaultStringValue}</subElem1>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
  <subElem2>{_settings.DefaultStringValue}</subElem2>
</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);
    _settings.ElementCountOutputMode = outputMode;
    _settings.MaxUndoundedOccurs = 3;
    _generator.Settings = _settings;

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }
}
