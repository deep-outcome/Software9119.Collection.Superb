using System;
using System.Diagnostics;

namespace Software9119.Collection.Superb.Numerics;

/// <summary>
/// Validated <see langword="int"/>-sized value.
/// </summary>
[DebuggerDisplay ( "(Int32, {value})" )]
readonly public struct PositiveInt32 : IEquatable<PositiveInt32>
{
  /// <summary>
  /// This struct constructor.
  /// </summary>
  /// <exception cref="ArgumentOutOfRangeException">When <paramref name="value"/> is less then <c>0</c>.</exception>
  public PositiveInt32 ( int value )
  {
    if (value < 1)
      throw new ArgumentOutOfRangeException ( paramName: nameof ( value ), "Value must be positive." );

    this.value = value;
  }

  readonly internal int value;

  /// <summary>
  /// Implicit operator for conversion from <see langword="int"/> to <see cref="PositiveInt32"/>.
  /// </summary>
  static public implicit operator PositiveInt32 ( int value ) => new ( value );

  /// <summary>
  /// Implicit operator for conversion from <see cref="PositiveInt32"/> to <see langword="int"/>.
  /// </summary>
  static public implicit operator int ( PositiveInt32 value ) => value.value;

  /// <summary>
  /// Conversion method.
  /// </summary>
  static public PositiveInt32 ToPositiveInt32 ( int value ) => value;

  /// <summary>
  /// Conversion method.
  /// </summary>
  static public int ToInt32 ( PositiveInt32 value ) => value;

  /// <summary>
  /// This struct hash code.
  /// </summary>
  override public int GetHashCode () => value.GetHashCode ();

  /// <summary>
  /// Equals operator.
  /// </summary>
  static public bool operator == ( PositiveInt32 left, PositiveInt32 right ) => left.value == right.value;

  /// <summary>
  /// Not equals operator.
  /// </summary>
  static public bool operator != ( PositiveInt32 left, PositiveInt32 right ) => left.value != right.value;

  /// <summary>
  /// Equals method.
  /// </summary>
  override public bool Equals ( object? obj ) => obj is PositiveInt32 value && value.value == this.value;

  /// <summary>
  /// Equals method.
  /// </summary>
  public bool Equals ( PositiveInt32 other ) => other == this;
}
