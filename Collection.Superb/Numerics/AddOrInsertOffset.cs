using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Numerics;

/// <summary>
/// Offset carrying information about operation type.
/// </summary>
/// <remarks>
/// Internal type opened only for implementors.
/// </remarks>
[DebuggerDisplay ( "({value}, inserting={inserting})" )]
readonly public struct AddOrInsertOffset ( int value, bool inserting ) : IEquatable<AddOrInsertOffset>
{
  /// <summary>
  /// When <paramref name="value"/> equals <paramref name="count"/>, it's addition operation, otherwise insertion operation.
  /// </summary>  
  /// <remarks>
  /// Relations of and guarantees about parameters are not validated and are reponsibility of implementors.
  /// </remarks>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public AddOrInsertOffset CreateUsingCount ( int value, int count ) => new ( value, value != count );

  readonly internal int value = value;
  readonly internal bool inserting = inserting;

  /// <summary>
  /// Implicit operator for conversion from <see cref="AddOrInsertOffset"/> to <see langword="int"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public implicit operator int ( AddOrInsertOffset value ) => value.value;

  /// <summary>
  /// Conversion method.
  /// </summary>
  static public int ToInt32 ( AddOrInsertOffset value ) => value;

  /// <summary>
  /// This struct hash code.
  /// </summary>
  override public int GetHashCode () => HashCode.Combine ( value, inserting );

  /// <summary>
  /// Equals operator.
  /// </summary>
  static public bool operator == ( AddOrInsertOffset left, AddOrInsertOffset right ) => left.value == right.value && left.inserting == right.inserting;

  /// <summary>
  /// Not equals operator.
  /// </summary>
  static public bool operator != ( AddOrInsertOffset left, AddOrInsertOffset right ) => !(left == right);

  /// <summary>
  /// Equals method.
  /// </summary>
  override public bool Equals ( object? obj ) => obj is AddOrInsertOffset offset && Equals ( offset );

  /// <summary>
  /// Equals method.
  /// </summary>
  public bool Equals ( AddOrInsertOffset other ) => other == this;
}

