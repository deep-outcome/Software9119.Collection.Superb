using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Indexing;

/// <summary>
/// Index type with homogenic step size.
/// </summary>
[DebuggerDisplay ( "(size {Size}, value {value})" )]
[SuppressMessage ( "Naming", "CA1716:Identifiers should not match keywords", Justification = "Let be brave for now." )]
public struct Step : IEquatable<Step>
{
  /// <summary>
  /// Size of this <see cref="Step"/>.
  /// </summary>
  /// <remarks>
  /// See also <see cref="Step(int)"/> and <see cref="Step(int, int)"/>.
  /// </remarks>
  [SuppressMessage ( "Design", "CA1051:Do not declare visible instance fields", Justification = "Fits scenario." )]
  readonly public int Size;
  internal int value;

  /// <summary>
  /// Constructor with initial value parameter.
  /// </summary>
  /// <param name="value">Value to start on.</param>
  /// <param name="size">Step size.</param>
  public Step ( int size, int value ) : this ( size ) => this.value = value;

  /// <summary>
  /// Default constructor.
  /// </summary>
  /// <param name="size">Step size.</param>
  public Step ( int size )
  {
    if (size == 0)
      throw new ArgumentOutOfRangeException ( nameof ( size ), "Cannot make zero-sized steps!" );

    Size = size;
    value = 0;
  }

  /// <summary>
  /// Provides <see cref="int"/> representation of <see cref="Step"/> instance.
  /// </summary>
  /// <returns>Current step value.</returns>
  readonly public int ToInt32 () => value;

  /// <param name="s">Step to cast.</param>
  /// <returns>Current step value.</returns>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public implicit operator int ( Step s ) => s.value;

  /// <summary>
  /// See <see cref="Increment()"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public Step operator ++ ( Step s )
  {
    s.Increment ();
    return s;
  }

  /// <summary>
  /// See <see cref="Decrement()"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public Step operator -- ( Step s )
  {
    s.Decrement ();
    return s;
  }

  /// <summary>
  /// See <see cref="Add(int)"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public Step operator + ( Step s, int i )
  {
    s.Add ( i );
    return s;
  }

  /// <summary>
  /// See <see cref="Subtract(int)"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public Step operator - ( Step s, int i )
  {
    s.Subtract ( i );
    return s;
  }

  /// <summary>
  /// Increments <see cref="Step"/> instance by its step size.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  public void Increment () => value += Size;

  /// <summary>
  /// Decrements <see cref="Step"/> instance by its step size.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  public void Decrement () => value -= Size;

  /// <summary>
  /// Provides consumer with ability to increment with arbitrary step size.
  /// </summary>
  /// <param name="heterogenicStep">Custom step size.</param>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  public void Add ( int heterogenicStep ) => value += heterogenicStep;

  /// <summary>
  /// Provides consumer with ability to decrement with arbitrary step size.
  /// </summary>
  /// <param name="heterogenicStep">Custom step size.</param>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  public void Subtract ( int heterogenicStep ) => value -= heterogenicStep;

  /// <summary>
  /// <paramref name="obj"/> equals if it is <see cref="Step"/> with same size and current value.
  /// </summary>
  override readonly public bool Equals ( object? obj ) => obj is Step step && Equals ( step );

  /// <summary>
  /// <paramref name="other"/> equals on same size and current value.
  /// </summary>
  readonly public bool Equals ( Step other ) => this == other;

  /// <summary>
  /// Equality operator.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public bool operator == ( Step one, Step another ) => one.Size == another.Size && one.value == another.value;

  /// <summary>
  /// Inequality operator.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public bool operator != ( Step one, Step another ) => !(one == another);

  /// <summary>
  /// Computes code using <see cref="HashCode.Combine{T1, T2}(T1, T2)"/> with size and current value.
  /// </summary>
  override readonly public int GetHashCode () => HashCode.Combine ( Size, value );
}

