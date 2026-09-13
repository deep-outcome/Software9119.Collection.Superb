using System;
using System.Diagnostics;

namespace Software9119.Collection.Superb.Numerics;

/// <summary>
/// Validated <see langword="int"/>-sized value.
/// </summary>
[DebuggerDisplay ( "value={Size}" )]
readonly public struct NonNegativeInt32 : IEquatable<NonNegativeInt32>
{
  /// <summary>
  /// This struct constructor.
  /// </summary>
  /// <exception cref="ArgumentOutOfRangeException">When <paramref name="value"/> is less then <c>0</c>.</exception>
  public NonNegativeInt32 ( int value )
  {
    if (value < 0)
      throw new ArgumentOutOfRangeException ( paramName: nameof ( value ), "Value must be non-negative." );

    this.value = value;
  }

  readonly internal int value;

  /// <summary>
  /// Implicit operator for conversion from <see langword="int"/> to <see cref="NonNegativeInt32"/>.
  /// </summary>
  static public implicit operator NonNegativeInt32 ( int value ) => new ( value );

  /// <summary>
  /// Implicit operator for conversion from <see cref="NonNegativeInt32"/> to <see langword="int"/>.
  /// </summary>
  static public implicit operator int ( NonNegativeInt32 value ) => value.value;

  /// <summary>
  /// Conversion method.
  /// </summary>
  static public NonNegativeInt32 ToNonNegativeInt32 ( int value ) => value;

  /// <summary>
  /// Conversion method.
  /// </summary>
  static public int ToInt32 ( NonNegativeInt32 value ) => value;

  /// <summary>
  /// This struct hash code.
  /// </summary>
  override public int GetHashCode () => value.GetHashCode ();

  /// <summary>
  /// Equals operator.
  /// </summary>
  static public bool operator == ( NonNegativeInt32 left, NonNegativeInt32 right ) => left.value == right.value;

  /// <summary>
  /// Not equals operator.
  /// </summary>
  static public bool operator != ( NonNegativeInt32 left, NonNegativeInt32 right ) => left.value != right.value;

  /// <summary>
  /// Equals method.
  /// </summary>
  override public bool Equals ( object? obj ) => obj is NonNegativeInt32 value && value.value == this.value;

  /// <summary>
  /// Equals method.
  /// </summary>
  public bool Equals ( NonNegativeInt32 other ) => other == this;
}
