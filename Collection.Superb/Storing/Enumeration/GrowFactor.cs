namespace Software9119.Collection.Superb.Storing;

/// <summary>
/// Internal store auto-grow factor used by Auto Capacitation.
/// </summary>
public enum GrowFactor
{
  /// <summary>
  /// 1.5 grow factor.
  /// </summary>
  OneAndHalf,
  /// <summary>
  /// 2.0 grow factor.
  /// </summary>
  Two,
  /// <summary>
  /// 2.5 grow factor.
  /// </summary>
  TwoAndHalf,
  /// <summary>
  /// 3.0 grow factor.
  /// </summary>
  Three,
  /// <summary>
  /// 3.5 grow factor.
  /// </summary>
  ThreeAndHalf,
  /// <summary>
  /// 4 grow factor.
  /// </summary>
  Four,
  /// <summary>
  /// 4.5 grow factor.
  /// </summary>
  FourAndHalf,
  /// <summary>
  /// 5.0 grow factor.
  /// </summary>
  Five
}

/// <summary>
/// Extension methods of <see cref="GrowFactor"/>.
/// </summary>
static public class GrowFactorExtension
{
  /// <summary>
  /// Converts <paramref name="factor"/> to <see langword="float"/>.
  /// </summary>
  /// <returns><see langword="true"/> when conversion exists, <see langword="false"/> otherwise.</returns>
  /// <remarks>When <see langword="false"/> returned, <paramref name="number"/> is <see langword="default"/>.</remarks>
  static public bool ToFloat ( this GrowFactor factor, out float number )
  {
    number = factor switch
    {
      GrowFactor.OneAndHalf => 1.5f,
      GrowFactor.Two => 2.0f,
      GrowFactor.TwoAndHalf => 2.5f,
      GrowFactor.Three => 3.0f,
      GrowFactor.ThreeAndHalf => 3.5f,
      GrowFactor.Four => 4f,
      GrowFactor.FourAndHalf => 4.5f,
      GrowFactor.Five => 5.0f,
      _ => default
    };

    return number != default;
  }
}