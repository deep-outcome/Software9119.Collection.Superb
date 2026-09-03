using System;
using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// Allows for segmented enumeration of arbitraty <see cref="IList{U}"/>.
/// </summary>
public ref struct IListRefEnumerator<T, U> : IEnumerator<U?>
  where T : struct, IList<U?>, allows ref struct
{
  readonly T list;
  readonly int offset;
  readonly int limit;

  int index;

  /// <summary>
  /// Public constructor.
  /// </summary>
  /// <exception cref="ArgumentNullException">when <paramref name="list"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">For negative <paramref name="offset"/> or negative <paramref name="count"/> or
  /// when combination of <paramref name="offset"/> and <paramref name="count"/> is invalid.</exception>
  public IListRefEnumerator ( int offset, int count, T list ) : this ( list, offset, SegmentationValidator.LimitOutOf ( offset, count ) )
  {
    int listLength = list.Count;
    if (SegmentationValidator.ValidateSetup ( listLength, offset: offset, count: count, out _, out ImpossibleSegmentationException? ise ))
      throw ise;
  }

  internal IListRefEnumerator ( T list, int offset, int limit )
  {
    this.list = list;
    this.offset = offset;
    this.limit = limit;
    Reset ();
  }

  U? current;

  /// <summary>
  /// Current enumeration item.
  /// </summary>
  readonly public U? Current => current;

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
