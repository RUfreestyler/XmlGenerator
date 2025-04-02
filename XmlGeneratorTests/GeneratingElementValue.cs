using System.Text;
using XmlGenerator;

namespace XmlGeneratorTests;

[TestFixture]
internal class GeneratingElementValue
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
  public void GenerateXml_OneSimpleStringElement()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"" type=""xs:string""/>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>{_settings.DefaultStringValue}</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  [TestCase("int")]
  [TestCase("integer")]
  public void GenerateXml_OneSimpleIntegerElement(string integerTypeCode)
  {
    var xsdContent = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"" type=""xs:{integerTypeCode}""/>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>{_settings.DefaultIntegerValue}</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_OneSimpleBooleanElement()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"" type=""xs:boolean""/>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>{_settings.DefaultBooleanValue}</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_OneSimpleDateElement()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"" type=""xs:date""/>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>{_settings.DefaultDateValue}</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_OneSimpleDateTimeElement()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"" type=""xs:dateTime""/>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>{_settings.DefaultDateTimeValue.ToString("g")}</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_OneSimpleTimeElement()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"" type=""xs:time""/>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>{_settings.DefaultTimeValue}</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_OneSimpleDefaultStringElement()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"" type=""xs:string"" default=""test default string""/>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>test default string</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }

  [Test]
  public void GenerateXml_OneSimpleFixedStringElement()
  {
    var xsdContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""elem"" type=""xs:string"" fixed=""test fixed string""/>
</xs:schema>";

    var expectedXml = $@"<?xml version=""1.0"" encoding=""utf-16""?>
<elem>test fixed string</elem>";
    var xsdBytes = Encoding.UTF8.GetBytes(xsdContent);

    var xmlDoc = _generator.GenerateXml(xsdBytes);
    var actualXml = xmlDoc.ToHumanReadableXml();

    TestContext.WriteLine(actualXml);
    Assert.That(actualXml, Is.EqualTo(expectedXml));
  }
}
