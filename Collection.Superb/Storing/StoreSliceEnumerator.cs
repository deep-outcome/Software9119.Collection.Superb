using System;
using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Storing;

/// <summary>
/// <see cref="StoreSlice{T}"/> enumerator.
/// </summary>
public struct StoreSliceEnumerator<T> : IEnumerator<T?>
{
  readonly internal T?[] store;
  readonly internal int limit;

  internal int index;
  readonly internal int resetIndex;

  /// <summary>
  /// Constructor.
  /// </summary>
  public StoreSliceEnumerator ( in StoreSlice<T?> slice )
  {
    int index = slice.offset - 1;

    limit = index + slice.count;
    resetIndex = index;

    store = slice.SafeStore;

    Reset ();
  }

  /// <summary>
  /// Nothing to dispose.
  /// </summary>
  readonly public void Dispose () { }

  /// <summary>
  /// Advances enumerator by one item.
  /// </summary>
  /// <returns><see langword="true"/> when enumeration advances, <see langword="false"/> when enumeration reached its end already.</returns>
  public bool MoveNext ()
  {
    if (index < limit)
    {
      ++index;
      return true;
    }

    return false;
  }

  /// <summary>
  /// Current element or <c>default(<typeparamref name="T"/>)</c>, if enumeration is not yet started.
  /// </summary>
  /// <remarks>Keeps last enumeration item after enumeration end.</remarks>
  /// <exception cref="NullReferenceException">For <c>default(<see cref="CapacitorStoreEnumerator{T}"/>)</c>.</exception>
  readonly public T? Current => index == resetIndex ? default ( T? ) : store [ index ];

  /// <summary>
  /// Current element or <c>default(<typeparamref name="T"/>)</c>, if enumeration is not yet started.
  /// </summary>
  /// <remarks>Keeps last enumeration item after enumeration end.</remarks>
  /// <exception cref="NullReferenceException">For <c>default(<see cref="CapacitorStoreEnumerator{T}"/>)</c>.</exception>
  readonly object? IEnumerator.Current => Current;

  /// <summary>
  /// Resets enumeration to its start.
  /// </summary>
  public void Reset () => index = resetIndex;
}
