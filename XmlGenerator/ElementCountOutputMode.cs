namespace XmlGenerator;

/// <summary>
/// Режим выбора количества появлений элемента.
/// </summary>
public enum ElementCountOutputMode
{
  /// <summary>
  /// Минимальное количество раз.
  /// </summary>
  MinOccurs,
  /// <summary>
  /// Максимальное количество раз.
  /// </summary>
  MaxOccurs,
  /// <summary>
  /// Минимальное количество раз, но хотя-бы 1 раз, если минимум равен 0.
  /// </summary>
  MinButAtLeastSingleOccur
}
