using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;

using System;
using System.Collections;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// Allows for segmented enumeration of arbitraty <see cref="IList"/>.
/// </summary>
public struct IListEnumerator : IEnumerator
{
  readonly IList list;
  readonly int offset;
  readonly int limit;

  int index;

  /// <summary>
  /// Public constructor.
  /// </summary>
  /// <exception cref="ArgumentNullException">when <paramref name="list"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// <list type="bullet">
  /// <item>When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over <paramref name="list"/>.</item>
  /// <item>When <paramref name="offset"/> is less than <c>0</c>.</item>  
  /// </list>
  /// </exception>
  public IListEnumerator ( int offset, NonNegativeInt32 count, IList list ) : this ( list, offset, IndexingValidator.LimitOutOf ( offset, count ) )
  {
    if (SegmentingValidator.ValidateList ( list, out ArgumentNullException? ane ))
      throw ane;

#pragma warning disable CA1062 // Validate arguments of public methods
    int listLength = list.Count;
#pragma warning restore CA1062 // Validate arguments of public methods
    if (IxValidator.ValidateSegmentation ( listLength, offset: offset, count: count, out _, out ImpSegExc? ise ) == 1)
      throw ise!;
  }

  internal IListEnumerator ( IList list, int offset, int limit )
  {
    this.list = list;
    this.offset = offset;
    this.limit = limit;
    Reset ();
  }

  object? current;

  /// <summary>
  /// Current enumeration item.
  /// </summary>
  readonly public object? Current => current;

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
