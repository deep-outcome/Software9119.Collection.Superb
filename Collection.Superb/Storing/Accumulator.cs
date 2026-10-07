using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;
using Software9119.Collection.Superb.Segmentation;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Storing;

/// <summary>
/// Flexible auto-expanding data structure, <see cref="Capacitor{T}"/> extension open to internal store exposition.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>Uses <see cref="CapacitationPolicy.GeometricJump"/> as default for <see cref="Capacitor{T}.CapacitationPolicy"/>.</item>
/// <item>See <see cref="Capacitor{T}"/> for details on capacitation.</item>
/// </list>
/// </remarks>
[SuppressMessage ( "Design", "CA1051:Do not declare visible instance fields", Justification = "Inheritance design." )]
public class Accumulator<T> : Capacitor<T>
{
  /// <summary>
  /// Default constructor.
  /// </summary>
  public Accumulator () : base () { }

  /// <summary>
  /// Constructor with initial capacity.
  /// </summary>
  public Accumulator ( NonNegativeInt32 capacity ) : base ( capacity ) { }

  /// <summary>
  /// Constructor with initial items.
  /// </summary>
  /// <remarks>
  /// Stores <paramref name="items"/> using Batch Capacitation or using Auto Capacitation,
  /// based on possibility to obtain <paramref name="items"/> count.
  /// </remarks>
  public Accumulator ( IEnumerable<T?>? items ) : base ( items ) { }

  /// <summary>
  /// Constructor with initial capacity and initial items.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Sets store capacity to <paramref name="capacity"/> and then stores <paramref name="items"/>.</item>
  /// <item>
  /// For eventual next capacitation uses Batch Capacitation or Auto Capacitation, based on possibility to obtain <paramref name="items"/> count.
  /// </item>
  /// </list>
  /// </remarks>
  public Accumulator ( IEnumerable<T?>? items, NonNegativeInt32 capacity ) : base ( items, capacity ) { }

  /// <summary>
  /// Constructor allowing to 'start' <see cref="Accumulator{T}"/> with pre-created internal store.
  /// </summary>
  /// <param name="count">How much of <paramref name="store"/> is considered populated.</param>  
  /// <exception cref="ArgumentOutOfRangeException">
  /// When <paramref name="count"/> is greater than <paramref name="store"/> length or not <c>0</c> when <paramref name="store"/>
  /// is <see langword="null"/>.
  /// </exception>
  public Accumulator ( NonNegativeInt32 count, T? []? store ) : base ( count, store )
  {
    int length = this.store.Length;
    if (length < count)
    {
      string msg = $"Count ({count}) must be less than or equal to store length ({length}).";
      throw new ArgumentOutOfRangeException ( paramName: nameof ( count ), msg );
    }
  }

  /// <summary>
  /// <see cref="CapacitationPolicy"/> backing field.
  /// </summary>
  new protected internal CapacitationPolicy capacitationPolicy = CapacitationPolicy.GeometricJump;

  /// <summary>
  /// Capacitation policy used by Batch Capacitation and Pre Capacitation.
  /// </summary>
  /// <remarks>
  /// Default is <see cref="CapacitationPolicy.GeometricJump"/>.
  /// </remarks>
  /// <exception cref="ArgumentOutOfRangeException">Upon try to set invalid <see cref="Storing.CapacitationPolicy"/>.</exception>
  /// <exception cref="InvalidOperationException">
  /// Upon try to set value when <see cref="Capacitor{T}.LockCapacitationPolicy"/> is <see langword="true"/>.
  /// </exception>
  override public CapacitationPolicy CapacitationPolicy
  {
    get => capacitationPolicy;
    set
    {
      if (LockCapacitationPolicy)
        throw new InvalidOperationException ( "Capacitation policy is locked." );

      if (Enum.IsDefined ( value ) == false)
        throw new ArgumentOutOfRangeException ( paramName: nameof ( value ), $"Unsupported capacitation policy, '{value}'." );

      capacitationPolicy = value;
    }
  }

  /// <summary>
  /// Internal store access.
  /// </summary>
  [SuppressMessage ( "Performance", "CA1819:Properties should not return arrays", Justification = "Intentionally open." )]
  public T? [] Store => store;

  /// <summary>
  /// Creates store slice.
  /// </summary>   
  /// <remarks>    
  /// <see cref="StoreSlice{T}"/> is capable of implicit conversion to many other slice types, e.g.:
  /// <list type="bullet">
  /// <item><see cref="Span{T}"/></item>
  /// <item><see cref="ReadOnlyMemory{T}"/></item>
  /// <item><see cref="IReadOnlyListSegment{T}"/></item>
  /// </list>
  /// </remarks>
  /// <exception cref="ImpossibleSegmentationException">
  /// <list type="bullet">
  /// <item>When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.</item>
  /// <item>When <paramref name="offset"/> is less than <c>0</c>.</item>
  /// </list>
  /// </exception>
  public StoreSlice<T?> Slice ( int offset, NonNegativeInt32 count )
  {
    if (ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter ) == 1)
      throw e!;

    return new ( store, offset, (int) count );
  }
}
