using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Numerics;

/// <summary>
/// Offset carrying information about operation type.
/// </summary>
/// <remarks>
/// Internal type opened only for implementors.
/// </remarks>
[DebuggerDisplay ( "({value}, inserting={inserting})" )]
readonly public struct AddInsertOffset : IEquatable<AddInsertOffset>
{
  /// <summary>
  /// Constructor.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  [SuppressMessage ( "Style", "IDE0290:Use primary constructor", Justification = "Cannot target method via class attribute." )]
  public AddInsertOffset ( int value, bool inserting )
  {
    this.value = value;
    this.inserting = inserting;
  }

  /// <summary>
  /// When <paramref name="value"/> equals <paramref name="count"/>, it's addition operation, otherwise insertion operation.
  /// </summary>  
  /// <remarks>
  /// Relations of and guarantees about parameters are not validated and are reponsibility of implementors.
  /// </remarks>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public AddInsertOffset CreateUsingCount ( int value, int count ) => new ( value, value != count );

  readonly internal int value;
  readonly internal bool inserting;

  /// <summary>
  /// Implicit operator for conversion from <see cref="AddInsertOffset"/> to <see langword="int"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public implicit operator int ( AddInsertOffset value ) => value.value;

  /// <summary>
  /// Implicit operator for conversion from <see cref="AddInsertOffset"/> to <see langword="bool"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public implicit operator bool ( AddInsertOffset value ) => value.inserting;

  /// <summary>
  /// Current offset.
  /// </summary>
  static public int ToInt32 ( AddInsertOffset value ) => value.value;

  /// <returns><see langword="true"/> if inserting offset.</returns>
  static public bool ToBoolean ( AddInsertOffset value ) => value.inserting;

  /// <summary>
  /// This struct hash code.
  /// </summary>
  override public int GetHashCode () => HashCode.Combine ( value, inserting );

  /// <summary>
  /// Equals operator.
  /// </summary>
  static public bool operator == ( AddInsertOffset left, AddInsertOffset right ) => left.value == right.value && left.inserting == right.inserting;

  /// <summary>
  /// Not equals operator.
  /// </summary>
  static public bool operator != ( AddInsertOffset left, AddInsertOffset right ) => !(left == right);

  /// <summary>
  /// Equals method.
  /// </summary>
  override public bool Equals ( object? obj ) => obj is AddInsertOffset offset && Equals ( offset );

  /// <summary>
  /// Equals method.
  /// </summary>
  public bool Equals ( AddInsertOffset other ) => other == this;
}

