using System;
using System.Diagnostics;

namespace Software9119.Collection.Superb.Numerics;

/// <summary>
/// Validated <see langword="int"/>-sized value.
/// </summary>
[DebuggerDisplay ( "(Int32, {value})" )]
readonly public struct NegativeInt32 : IEquatable<NegativeInt32>
{
  /// <summary>
  /// Validates current instance initialization and throws if it is default.
  /// </summary>
  /// <exception cref="InvalidOperationException"/>
  public void ThrowIfDefault ()
  {
    const string msg = $"Unitialized {nameof(NegativeInt32)} instance usage.";
    if (value == 0)
      throw new InvalidOperationException ( msg );
  }

  /// <summary>
  /// This struct constructor.
  /// </summary>
  /// <exception cref="ArgumentOutOfRangeException">When <paramref name="value"/> is less then <c>0</c>.</exception>
  public NegativeInt32 ( int value )
  {
    if (value > -1)
    {
      string msg = $"Value must be negative integer, but it is '{value}'.";
      throw new ArgumentOutOfRangeException ( paramName: null, message: msg );
    }

    this.value = value;
  }

  readonly internal int value;

  /// <summary>
  /// This instance value.
  /// </summary>  
  public int Value
  {
    get
    {
      ThrowIfDefault ();
      return value;
    }
  }

  /// <summary>
  /// Implicit operator for conversion from <see langword="int"/> to <see cref="NegativeInt32"/>.
  /// </summary>
  static public implicit operator NegativeInt32 ( int value ) => new ( value );

  /// <summary>
  /// Implicit operator for conversion from <see cref="NegativeInt32"/> to <see langword="int"/>.
  /// </summary>
  static public implicit operator int ( NegativeInt32 value ) => value.Value;

  /// <summary>
  /// Conversion method.
  /// </summary>
  static public NegativeInt32 ToNegativeInt32 ( int value ) => new ( value );

  /// <summary>
  /// Conversion method.
  /// </summary>
  static public int ToInt32 ( NegativeInt32 value ) => value.Value;

  /// <summary>
  /// This struct hash code.
  /// </summary>
  override public int GetHashCode () => value.GetHashCode ();

  /// <summary>
  /// Equals operator.
  /// </summary>
  static public bool operator == ( NegativeInt32 left, NegativeInt32 right ) => left.Value == right.Value;

  /// <summary>
  /// Not equals operator.
  /// </summary>
  static public bool operator != ( NegativeInt32 left, NegativeInt32 right ) => left.Value != right.Value;

  /// <summary>
  /// Equals method.
  /// </summary>
  override public bool Equals ( object? obj ) => obj is NegativeInt32 other && Value == other.Value;

  /// <summary>
  /// Equals method.
  /// </summary>
  public bool Equals ( NegativeInt32 other ) => Value == other.Value;

  /// <summary>
  /// Int32 string representation of this instance.
  /// </summary>
  override public string ToString () => value.ToString ();
}
