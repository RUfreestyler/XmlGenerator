namespace XmlGenerator
{
  /// <summary>
  /// Ограчения на значение элемента или атрибута.
  /// </summary>
  internal class Restrictions
  {
    /// <summary>
    /// Базовое произвольное значение, к которому применяются ограничения.
    /// </summary>
    public string BaseValue { get; set; } = null;

    /// <summary>
    /// Шаблон значения.
    /// </summary>
    public string? Pattern { get; set; } = null;

    /// <summary>
    /// Минимальная длина.
    /// </summary>
    public int? MinLength
    {
      get { return _minLength; }
      set 
      { 
        if (value.HasValue && value < 0)
          throw new ArgumentOutOfRangeException("Min length must be equal to or greater than zero");

        _minLength = value; 
      }
    }

    private int? _minLength = null;

    /// <summary>
    /// Максимальная длина.
    /// </summary>
    public int? MaxLength
    {
      get { return _maxLength; }
      set 
      { 
        if (value.HasValue && value < 0)
          throw new ArgumentOutOfRangeException("Max length must be equal to or greater than zero");

        _maxLength = value; 
      }
    }

    private int? _maxLength = null;

    /// <summary>
    /// Минимальное значение отрезка.
    /// </summary>
    public decimal? MinInclusive { get; set; } = null;

    /// <summary>
    /// Максимальное значение отрезка.
    /// </summary>
    public decimal? MaxInclusive { get; set; } = null;

    /// <summary>
    /// Фиксированное количество цифр.
    /// </summary>
    public int? TotalDigits 
    { 
      get { return _totalDigits; }
      set
      {
        if (value.HasValue && value <= 0)
          throw new ArgumentOutOfRangeException("Total digits restriction must be greater than zero");

        _totalDigits = value;
      }
    }

    private int? _totalDigits = null;

    /// <summary>
    /// Список доступных значений.
    /// </summary>
    public List<string> Enumeration { get; set; } = null;

    public Restrictions(string baseValue)
    {
      BaseValue = baseValue;
    }
  }
}
