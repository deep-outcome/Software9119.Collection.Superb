using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;
using Software9119.Collection.Superb.Segmentation;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Storing;

/// <summary>
/// General Slice type which implicitly converts to target segment types.
/// </summary>
[SuppressMessage
(
  "Performance",
  "CA1815:Override equals and operator equals on value types",
  Justification = "Perf-sensitive comparison need is unlikely."
)]
readonly public struct StoreSlice<T> : IEnumerable<T>
{
  readonly internal T?[] store;
  readonly internal int offset, count;

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="store"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// <list type="bullet">
  /// <item>When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.</item>
  /// <item>When <paramref name="offset"/> is less than <c>0</c>.</item>
  /// </list>
  /// </exception>
  public StoreSlice ( T? [] store, int offset, NonNegativeInt32 count )
  {
    if (store == null)
      throw new ArgumentNullException ( paramName: nameof ( store ), "Store must be provided." );

    if (IxValidator.ValidateSegmentation ( store.Length, offset, count, out _, out ImpSegExc? e ) == 1)
      throw e!;

    this.store = store;
    this.offset = offset;
    this.count = count;
  }

  internal StoreSlice ( T? [] store, int offset, int count )
  {
    this.store = store;
    this.offset = offset;
    this.count = count;
  }

  /// <summary>
  /// Slice offset.
  /// </summary>
  public int Offset => offset;
  
  /// <summary>
  /// Slice length.
  /// </summary>
  public int Count => count;

  /// <summary>
  /// Store of this slice.
  /// </summary>
  [SuppressMessage ( "Performance", "CA1819:Properties should not return arrays", Justification = "Intentionally open." )]
  public T? []? Store => store;

  internal T? [] SafeStore => store ?? Array.Empty<T?> ();

  /// <summary>
  /// <see cref="ArraySegment{T}"/> implicit conversion.
  /// </summary>
  static public implicit operator ArraySegment<T?> ( StoreSlice<T> s ) => s.ToArraySegment ();

  /// <summary>
  /// <see cref="IListSegment"/> implicit conversion.
  /// </summary>
  static public implicit operator IListSegment ( StoreSlice<T> s ) => s.ToIListSegment ();

  /// <summary>
  /// <see cref="IListSegment{T}"/> implicit conversion.
  /// </summary>
  static public implicit operator IListSegment<T?> ( StoreSlice<T> s ) => s.ToIListSegmentOfT ();

  /// <summary>
  /// <see cref="IReadOnlyListSegment{T}"/> implicit conversion.
  /// </summary>
  static public implicit operator IReadOnlyListSegment<T?> ( StoreSlice<T> s ) => s.ToIReadOnlyListSegment ();

  /// <summary>
  /// <see cref="Memory{T}"/> implicit conversion.
  /// </summary>
  static public implicit operator Memory<T?> ( StoreSlice<T> s ) => s.ToMemory ();

  /// <summary>
  /// <see cref="ReadOnlyMemory{T}"/> implicit conversion.
  /// </summary>
  static public implicit operator ReadOnlyMemory<T?> ( StoreSlice<T> s ) => s.ToReadOnlyMemory ();

  /// <summary>
  /// <see cref="Span{T}"/> implicit conversion.
  /// </summary>
  static public implicit operator Span<T?> ( StoreSlice<T> s ) => s.ToSpan ();

  /// <summary>
  /// <see cref="ReadOnlySpan{T}"/> implicit conversion.
  /// </summary>
  static public implicit operator ReadOnlySpan<T?> ( StoreSlice<T> s ) => s.ToReadOnlySpan ();

  /// <summary>
  /// <see cref="ArraySegment{T}"/> methodical conversion.
  /// </summary>
  public ArraySegment<T?> ToArraySegment () => new ( SafeStore, offset, count );

  /// <summary>
  /// <see cref="IListSegment"/> methodical conversion.
  /// </summary>
  public IListSegment ToIListSegment () => new ( SafeStore, offset, count );


  /// <summary>
  /// <see cref="IListSegment{T}"/> methodical conversion.
  /// </summary>
  public IListSegment<T?> ToIListSegmentOfT () => new ( SafeStore, offset, count );

  /// <summary>
  /// <see cref="IReadOnlyListSegment{T}"/> methodical conversion.
  /// </summary>
  public IReadOnlyListSegment<T?> ToIReadOnlyListSegment () => new ( SafeStore, offset, count );

  /// <summary>
  /// <see cref="Memory{T}"/> methodical conversion.
  /// </summary>
  public Memory<T?> ToMemory () => new ( SafeStore, offset, count );

  /// <summary>
  /// <see cref="ReadOnlyMemory{T}"/> methodical conversion.
  /// </summary>
  public ReadOnlyMemory<T?> ToReadOnlyMemory () => new ( SafeStore, offset, count );

  /// <summary>
  /// <see cref="Span{T}"/> methodical conversion.
  /// </summary>
  public Span<T?> ToSpan () => new ( SafeStore, offset, count );

  /// <summary>
  /// <see cref="ReadOnlySpan{T}"/> methodical conversion.
  /// </summary>
  public ReadOnlySpan<T?> ToReadOnlySpan () => new ( SafeStore, offset, count );

  /// <summary>
  /// Store slice enumerator.
  /// </summary>
  public StoreSliceEnumerator<T?> GetEnumerator () => new ( this );

  /// <summary>
  /// Store slice enumerator.
  /// </summary>
  /// <returns><see cref="StoreSliceEnumerator{T}"/>.</returns>
  IEnumerator<T> IEnumerable<T>.GetEnumerator () => new StoreSliceEnumerator<T?> ( this );

  /// <summary>
  /// Store slice enumerator.
  /// </summary>
  /// <returns><see cref="StoreSliceEnumerator{T}"/>.</returns>
  IEnumerator IEnumerable.GetEnumerator () => new StoreSliceEnumerator<T?> ( this );
}
