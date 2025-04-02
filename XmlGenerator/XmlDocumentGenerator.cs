using Fare;
using System.Xml.Schema;
using System.Xml;
using XmlGenerator;

public class XmlDocumentGenerator : IXmlGenerator
{
  public XmlGeneratorSettings Settings { get; set; }

  private readonly IXsdLoader _xsdLoader;
  
  public XmlDocument GenerateXml(string xsdPath)
  {
    var schemaSet = _xsdLoader.LoadXsdFromFile(xsdPath, Settings.Encoding);
    var xmlDoc = GenerateXml(schemaSet);

    return xmlDoc;
  }

  public XmlDocument GenerateXml(byte[] xsdBytes)
  {
    var schemaSet = _xsdLoader.LoadXsd(xsdBytes, Settings.Encoding);
    var xmlDoc = GenerateXml(schemaSet);

    return xmlDoc;
  }

  private XmlDocument GenerateXml(XmlSchemaSet schemaSet)
  {
    var xmlDoc = new XmlDocument();

    var rootElement = FindRootElement(schemaSet);

    if (rootElement == null)
      throw new InvalidOperationException("No root element found in the schema.");

    var root = xmlDoc.CreateElement(rootElement.Name);
    xmlDoc.AppendChild(root);

    GenerateElement(xmlDoc, root, rootElement, schemaSet);

    return xmlDoc;
  }

  private XmlSchemaElement FindRootElement(XmlSchemaSet schemaSet)
  {
    foreach (XmlSchema schema in schemaSet.Schemas())
    {
      foreach (XmlSchemaObject obj in schema.Items)
      {
        if (obj is XmlSchemaElement element)
        {
          return element;
        }
      }
    }

    return null;
  }

  private void GenerateElement(XmlDocument xmlDoc, XmlElement parent, XmlSchemaElement schemaElement, XmlSchemaSet schemaSet)
  {
    if (schemaElement.ElementSchemaType is XmlSchemaComplexType complexType)
    {
      GenerateComplexType(xmlDoc, parent, complexType, schemaSet);
    }
    else if (schemaElement.ElementSchemaType is XmlSchemaSimpleType simpleType)
    {
      parent.InnerText = GetSimpleElementValue(simpleType, schemaElement);
    }
    else
    {
      throw new NotSupportedException($"Unknown element schema type: {schemaElement.SchemaType}");
    }
  }

  private void GenerateComplexType(XmlDocument xmlDoc, XmlElement parent, XmlSchemaComplexType complexType, XmlSchemaSet schemaSet)
  {
    GenerateAttributes(xmlDoc, parent, complexType.Attributes);

    if (complexType.ContentModel is XmlSchemaComplexContent complexContent)
    {
      if (complexContent.Content is XmlSchemaComplexContentExtension extension)
      {
        GenerateAttributes(xmlDoc, parent, extension.Attributes);
        GenerateParticle(xmlDoc, parent, extension.Particle, schemaSet);
      }
      else if (complexContent.Content is XmlSchemaComplexContentRestriction restriction)
      {
        GenerateAttributes(xmlDoc, parent, restriction.Attributes);
        GenerateParticle(xmlDoc, parent, restriction.Particle, schemaSet);
      }
    }
    else if (complexType.ContentModel is XmlSchemaSimpleContent simpleContent)
    {
      if (simpleContent.Content is XmlSchemaSimpleContentExtension extension)
      {
        GenerateAttributes(xmlDoc, parent, extension.Attributes);
        parent.InnerText = GetSampleValue(complexType);
      }
      else if (simpleContent.Content is XmlSchemaSimpleContentRestriction restriction)
      {
        GenerateAttributes(xmlDoc, parent, restriction.Attributes);
        parent.InnerText = GetTypeValueWithRestrictions(complexType, restriction);
      }
    }
    else
    {
      GenerateParticle(xmlDoc, parent, complexType.Particle, schemaSet);
    }
  }

  private void GenerateAttributes(XmlDocument xmlDoc, XmlElement parent, XmlSchemaObjectCollection attributes)
  {
    foreach (XmlSchemaObject attributeObj in attributes)
    {
      if (attributeObj is XmlSchemaAttribute attribute)
      {
        if (ShouldAddAttribute(parent, attribute))
        {
          string attributeValue = GetSampleValueForAttribute(attribute);
          parent.SetAttribute(attribute.Name, attributeValue);
        }
      }
    }
  }

  private bool ShouldAddAttribute(XmlElement parent, XmlSchemaAttribute attribute)
  {
    if (attribute.Use == XmlSchemaUse.Required)
    {
      return true;
    }

    var random = new Random(Guid.NewGuid().GetHashCode());
    return random.NextDouble() <= Settings.OptionalAttributesAddingProbability;
  }

  private string GetSampleValueForAttribute(XmlSchemaAttribute attribute)
  {
    if (!string.IsNullOrEmpty(attribute.FixedValue))
    {
      return attribute.FixedValue;
    }
    else if (attribute.SchemaType is XmlSchemaSimpleType simpleType)
    {
      return GetSimpleAttributeValue(simpleType, attribute);
    }
    else if (attribute.AttributeSchemaType is XmlSchemaSimpleType attributeSimpleType)
    {
      return GetSimpleAttributeValue(attributeSimpleType, attribute);
    }

    return attribute.DefaultValue;
  }

  private void GenerateParticle(XmlDocument xmlDoc, XmlElement parent, XmlSchemaParticle particle, XmlSchemaSet schemaSet)
  {
    if (particle is XmlSchemaSequence sequence)
    {
      GenerateSequence(xmlDoc, parent, sequence, schemaSet);
    }
    else if (particle is XmlSchemaChoice choice)
    {
      GenerateChoice(xmlDoc, parent, choice, schemaSet);
    }
    else if (particle is XmlSchemaAll all)
    {
      GenerateAll(xmlDoc, parent, all, schemaSet);
    }
  }

  private void GenerateSequence(XmlDocument xmlDoc, XmlElement parent, XmlSchemaSequence sequence, XmlSchemaSet schemaSet)
  {
    foreach (XmlSchemaObject item in sequence.Items)
    {
      if (item is XmlSchemaElement element)
      {
        GenerateElementSequence(xmlDoc, parent, element, schemaSet);
      }
      else if (item is XmlSchemaSequence nestedSequence)
      {
        GenerateSequence(xmlDoc, parent, nestedSequence, schemaSet);
      }
      else if (item is XmlSchemaChoice nestedChoice)
      {
        GenerateChoice(xmlDoc, parent, nestedChoice, schemaSet);
      }
      else if (item is XmlSchemaGroupRef groupRef)
      {
        GenerateGroupRef(xmlDoc, parent, groupRef, schemaSet);
      }
    }
  }

  private void GenerateChoice(XmlDocument xmlDoc, XmlElement parent, XmlSchemaChoice choice, XmlSchemaSet schemaSet)
  {
    var selectedItem = choice.Items[0];

    if (selectedItem is XmlSchemaElement element)
    {
      GenerateElementSequence(xmlDoc, parent, element, schemaSet);
    }
    else if (selectedItem is XmlSchemaSequence nestedSequence)
    {
      GenerateSequence(xmlDoc, parent, nestedSequence, schemaSet);
    }
    else if (selectedItem is XmlSchemaChoice nestedChoice)
    {
      GenerateChoice(xmlDoc, parent, nestedChoice, schemaSet);
    }
    else if (selectedItem is XmlSchemaGroupRef groupRef)
    {
      GenerateGroupRef(xmlDoc, parent, groupRef, schemaSet);
    }
  }

  private void GenerateAll(XmlDocument xmlDoc, XmlElement parent, XmlSchemaAll all, XmlSchemaSet schemaSet)
  {
    foreach (XmlSchemaObject item in all.Items)
    {
      if (item is XmlSchemaElement element)
      {
        GenerateElementSequence(xmlDoc, parent, element, schemaSet);
      }
    }
  }

  private void GenerateGroupRef(XmlDocument xmlDoc, XmlElement parent, XmlSchemaGroupRef groupRef, XmlSchemaSet schemaSet)
  {
    var group = FindGroupByName(groupRef.RefName.Name, schemaSet);
    if (group != null)
    {
      GenerateParticle(xmlDoc, parent, group.Particle, schemaSet);
    }
  }

  private XmlSchemaGroup FindGroupByName(string name, XmlSchemaSet schemaSet)
  {
    foreach (XmlSchema schema in schemaSet.Schemas())
    {
      foreach (XmlSchemaObject obj in schema.Items)
      {
        if (obj is XmlSchemaGroup group && group.Name == name)
        {
          return group;
        }
      }
    }
    return null;
  }

  private void GenerateElementSequence(XmlDocument xmlDoc, XmlElement parent, XmlSchemaElement element, XmlSchemaSet schemaSet)
  {
    var occursCount = GetElementOccursCount(element);

    for (var i = 0; i < occursCount; i++)
    {
      XmlElement newElement = xmlDoc.CreateElement(element.Name);
      parent.AppendChild(newElement);
      GenerateElement(xmlDoc, newElement, element, schemaSet);
    }
  }

  private int GetElementOccursCount(XmlSchemaElement element)
  {
    var maxOccurs = element.MaxOccurs == decimal.MaxValue 
      ? (int)Math.Max(element.MinOccurs, Settings.MaxUndoundedOccurs)
      : (int)element.MaxOccurs;
    if (Settings.ElementCountOutputMode == ElementCountOutputMode.MinOccurs)
      return (int)element.MinOccurs;
    else if (Settings.ElementCountOutputMode == ElementCountOutputMode.MaxOccurs)
      return maxOccurs;
    else if (Settings.ElementCountOutputMode == ElementCountOutputMode.MinButAtLeastSingleOccur &&
      element.MinOccurs == 0)
      return 1;

    return (int)element.MinOccurs;
  }

  private string GetSimpleElementValue(XmlSchemaSimpleType simpleType, XmlSchemaElement simpleElementSchema)
  {
    if (!string.IsNullOrEmpty(simpleElementSchema.FixedValue))
      return simpleElementSchema.FixedValue;

    if (!string.IsNullOrEmpty(simpleElementSchema.DefaultValue))
      return simpleElementSchema.DefaultValue;

    if (simpleType.Content is XmlSchemaSimpleTypeRestriction restriction)
      return GetTypeValueWithRestrictions(simpleType, restriction);

    return GetSampleValue(simpleType);
  }

  private string GetSimpleAttributeValue(XmlSchemaSimpleType simpleType, XmlSchemaAttribute simpleAttributeSchema)
  {
    if (!string.IsNullOrEmpty(simpleAttributeSchema.FixedValue))
      return simpleAttributeSchema.FixedValue;

    if (!string.IsNullOrEmpty(simpleAttributeSchema.DefaultValue))
      return simpleAttributeSchema.DefaultValue;

    if (simpleType.Content is XmlSchemaSimpleTypeRestriction restriction)
      return GetTypeValueWithRestrictions(simpleType, restriction);

    return GetSampleValue(simpleType);
  }

  private string GetTypeValueWithRestrictions(XmlSchemaType type, XmlSchemaSimpleTypeRestriction restriction)
  {
    return GetTypeValueWithRestrictions(type, restriction.Facets);
  }

  private string GetTypeValueWithRestrictions(XmlSchemaType type, XmlSchemaSimpleContentRestriction restriction)
  {
    return GetTypeValueWithRestrictions(type, restriction.Facets);
  }

  private string GetTypeValueWithRestrictions(XmlSchemaType type, XmlSchemaObjectCollection facets)
  {
    var restrictions = new Restrictions(GetSampleValue(type));

    foreach (var facet in facets)
    {
      if (facet is XmlSchemaPatternFacet patternFacet)
        restrictions.Pattern = patternFacet.Value;
      else if (facet is XmlSchemaMinLengthFacet minLengthFacet)
        restrictions.MinLength = int.Parse(minLengthFacet.Value);
      else if (facet is XmlSchemaMaxLengthFacet maxLengthFacet)
        restrictions.MaxLength = int.Parse(maxLengthFacet.Value);
      else if (facet is XmlSchemaMinInclusiveFacet minInclusiveFacet)
        restrictions.MinInclusive = decimal.Parse(minInclusiveFacet.Value);
      else if (facet is XmlSchemaMaxInclusiveFacet maxInclusiveFacet)
        restrictions.MaxInclusive = decimal.Parse(maxInclusiveFacet.Value);
      else if (facet is XmlSchemaTotalDigitsFacet totalDigitsFacet)
        restrictions.TotalDigits = int.Parse(totalDigitsFacet.Value);
      else if (facet is XmlSchemaEnumerationFacet enumerationFacet)
      {
        if (restrictions.Enumeration == null)
          restrictions.Enumeration = new List<string>();

        restrictions.Enumeration.Add(enumerationFacet.Value);
      }
    }

    return GenerateValueWithRestrictions(restrictions);
  }

  private string GetSampleValue(XmlSchemaType type)
  {
    if (type.Datatype.TypeCode == XmlTypeCode.String)
      return Settings.DefaultStringValue;

    else if (type.Datatype.TypeCode == XmlTypeCode.Int ||
      type.Datatype.TypeCode == XmlTypeCode.Integer)
      return Settings.DefaultIntegerValue.ToString();

    else if (type.Datatype.TypeCode == XmlTypeCode.Decimal)
      return Settings.DefaultDecimalValue.ToString("F2");

    else if (type.Datatype.TypeCode == XmlTypeCode.Boolean)
      return Settings.DefaultBooleanValue.ToString();

    else if (type.Datatype.TypeCode == XmlTypeCode.Date)
      return Settings.DefaultDateValue.ToString();

    else if (type.Datatype.TypeCode == XmlTypeCode.DateTime)
      return Settings.DefaultDateTimeValue.ToString("g");

    else if (type.Datatype.TypeCode == XmlTypeCode.Time)
      return Settings.DefaultTimeValue.ToString();

    return string.Empty;
  }

  private string GenerateValueWithRestrictions(Restrictions restrictions)
  {
    if (restrictions.Enumeration != null && restrictions.Enumeration.Any())
      return restrictions.Enumeration.First();

    if (decimal.TryParse(restrictions.BaseValue, out decimal numericValue))
      return GenerateNumericValueWithRestrictions(numericValue, restrictions.MinInclusive, restrictions.MaxInclusive, restrictions.TotalDigits);

    if (restrictions.BaseValue is string stringValue)
      return GenerateStringValueWithRestrictions(stringValue, restrictions.Pattern, restrictions.MinLength, restrictions.MaxLength);

    return restrictions.BaseValue;
  }

  private string GenerateNumericValueWithRestrictions(decimal numericValue, decimal? minInclusive, decimal? maxInclusive, int? totalDigits)
  {
    if (maxInclusive.HasValue)
    {
      numericValue = maxInclusive.Value;
    }

    if (minInclusive.HasValue && numericValue < minInclusive.Value)
    {
      numericValue = minInclusive.Value;
    }

    if (totalDigits.HasValue)
    {
      string valueStr = Math.Abs(numericValue).ToString();
      if (valueStr.Replace(".", "").Length > totalDigits.Value)
      {
        numericValue = (decimal)Math.Pow(10, totalDigits.Value - 1);
      }
    }

    return numericValue.ToString();
  }

  private string GenerateStringValueWithRestrictions(string stringValue, string pattern, int? minLength, int? maxLength)
  {
    stringValue = ApplyLengthRestrictions(stringValue, minLength, maxLength);

    if (!string.IsNullOrEmpty(pattern))
    {
      stringValue = ApplyPatternRestriction(stringValue, pattern);
    }

    return stringValue;
  }

  private string ApplyLengthRestrictions(string value, int? minLength, int? maxLength)
  {
    if (minLength.HasValue)
      value = ApplyMinLengthRestriction(value, minLength.Value);

    if (maxLength.HasValue)
      value = ApplyMaxLengthRestriction(value, maxLength.Value);

    return value;
  }

  private string ApplyPatternRestriction(string value, string pattern)
  {
    try
    {
      var xeger = new Xeger(pattern, new Random());
      return xeger.Generate();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error generating value for pattern '{pattern}': {ex.Message}");
      return value;
    }
  }

  private string ApplyMinLengthRestriction(string value, int minLength)
  {
    if (value.Length < minLength)
      return value.PadRight(minLength, 'x');

    return value;
  }

  private string ApplyMaxLengthRestriction(string value, int maxLength)
  {
    if (value.Length > maxLength)
      return value.Substring(0, maxLength);

    return value;
  }

  /// <summary>
  /// Создать генератор XML документов с настройками по умолчанию.
  /// </summary>
  /// <param name="xsdLoader">Загрузчик XSD схемы.</param>
  public XmlDocumentGenerator(IXsdLoader xsdLoader) : this(xsdLoader, new XmlGeneratorSettings()) { }

  /// <summary>
  /// Создать генератор XML документов с произвольными настройками.
  /// </summary>
  /// <param name="xsdLoader">Загрузчик XSD схемы.</param>
  /// <param name="settings">Настройки генератора.</param>
  public XmlDocumentGenerator(IXsdLoader xsdLoader, XmlGeneratorSettings settings)
  {
    _xsdLoader = xsdLoader;
    Settings = settings;
  }
}
