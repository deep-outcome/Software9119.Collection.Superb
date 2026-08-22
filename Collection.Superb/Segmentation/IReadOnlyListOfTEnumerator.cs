using Software9119.Collection.Superb.Segmentation.Exceptionality;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// Allows for segmented enumeration of arbitraty <see cref="IList{T}"/>.
/// </summary>
public struct IReadOnlyListEnumerator<T> : IEnumerator<T?>
{
  readonly IReadOnlyList<T?> list;
  readonly int offset;
  readonly int limit;

  int index;

  /// <summary>
  /// Public constructor.
  /// </summary>
  /// <exception cref="ArgumentNullException">when <paramref name="list"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">For negative <paramref name="offset"/> or negative <paramref name="count"/> or
  /// when combination of <paramref name="offset"/> and <paramref name="count"/> is invalid.</exception>
  public IReadOnlyListEnumerator ( int offset, int count, IReadOnlyList<T?> list ) : this ( list, offset, SegmentationValidator.LimitOutOf ( offset, count ) )
  {
    if (SegmentationValidator.ValidateList<T> ( list, out ArgumentNullException? ane ))
      throw ane;

#pragma warning disable CA1062 // Validate arguments of public methods
    int listLength = list.Count;
#pragma warning restore CA1062 // Validate arguments of public methods
    if (SegmentationValidator.ValidateSetup ( listLength, offset: offset, count: count, out _, out ImpossibleSegmentationException? ise ))
      throw ise;
  }

  internal IReadOnlyListEnumerator ( IReadOnlyList<T?> list, int offset, int limit )
  {
    this.list = list;
    this.offset = offset;
    this.limit = limit;
    Reset ();
  }

  T? current;

  /// <summary>
  /// Current enumeration item.
  /// </summary>
  readonly public T? Current => current;

  /// <summary>
  /// Current enumeration item.
  /// </summary>
  readonly object? IEnumerator.Current => current;

  /// <summary>
  /// Nothing to dispose.
  /// </summary>
  readonly public void Dispose () { }

  /// <summary>
  /// Returns <see langword="true"/> when enumerator can provide next enumeration item.
  /// </summary>
  public bool MoveNext ()
  {
    if (index < limit && ++index < limit)
    {
      current = list [ index ];
      return true;
    }

    return false;
  }

  /// <summary>
  /// Resets enumerator to initial state.
  /// </summary>
  public void Reset ()
  {
    index = offset - 1;
    current = default;
  }
}
