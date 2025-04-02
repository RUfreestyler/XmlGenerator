using System.Text;

namespace XmlGenerator;

/// <summary>
/// Настройки XML генератора.
/// </summary>
public class XmlGeneratorSettings
{
  /// <summary>
  /// Кодировка XSD схемы.
  /// </summary>
  public Encoding Encoding { get; set; } = Encoding.UTF8;

  /// <summary>
  /// Количество создаваемых элементов, если их количество не ограничено.
  /// </summary>
  public int MaxUndoundedOccurs { get; set; } = 3;

  /// <summary>
  /// Режим выбора количества появлений элемента.
  /// </summary>
  public ElementCountOutputMode ElementCountOutputMode { get; set; } = ElementCountOutputMode.MinButAtLeastSingleOccur;

  /// <summary>
  /// Вероятность добавления необязательного атрибута (от 0 до 1).
  /// </summary>
  public double OptionalAttributesAddingProbability
  {
    get { return _optionalAttributesAddingProbability; }
    set
    {
      if (value < 0 || value > 1)
        throw new ArgumentOutOfRangeException("Probability must be between 0 and 1 inclusive.");
      _optionalAttributesAddingProbability = value; 
    }
  }

  private double _optionalAttributesAddingProbability = 1;

  /// <summary>
  /// Строковое значение по умолчанию.
  /// </summary>
  public string DefaultStringValue { get; set; } = "SampleText";

  /// <summary>
  /// Целое число по умолчанию.
  /// </summary>
  public int DefaultIntegerValue { get; set; } = 1;

  /// <summary>
  /// Булево значение по умолчанию.
  /// </summary>
  public bool DefaultBooleanValue { get; set; } = true;

  /// <summary>
  /// Дробное значение по умолчанию.
  /// </summary>
  public decimal DefaultDecimalValue { get; set; } = 1.00m;

  /// <summary>
  /// Дата и время по умолчанию.
  /// </summary>
  public DateTime DefaultDateTimeValue { get; set; } = new DateTime(new DateOnly(2025, 1, 1), new TimeOnly(0));

  /// <summary>
  /// Дата по умолчанию.
  /// </summary>
  public DateOnly DefaultDateValue { get; set; } = new DateOnly(2025, 1, 1);

  /// <summary>
  /// Время по умолчанию.
  /// </summary>
  public TimeOnly DefaultTimeValue { get; set; } = new TimeOnly(1, 1);
}
