using Software9119.Collection.Superb.Numerics;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Indexing;

/// <summary>
/// Indexing validator contains useful methods for working with indexes.
/// </summary>
static public class IndexingValidator
{
  /// <summary>
  /// Computes exclusive upper bound for <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public int LimitOutOf ( NonNegativeInt32 offset, NonNegativeInt32 count ) => offset + count;

  /// <summary>
  /// Validates <paramref name="index"/> is valid for target <paramref name="count"/>.
  /// </summary>
  /// <returns><see langword="true"/> if index is invalid.</returns>
  static public bool ValidateIndex
  (
    NonNegativeInt32 index,
    NonNegativeInt32 count,
   [NotNullWhen ( true )] out IndexOutOfBoundariesException? e
  )
  {
    if (index >= count)
    {
      e = IndexOutOfBoundariesException.OutOfBoundsMsg ( index: index, count );
      return true;
    }

    e = null;
    return false;
  }

  /// <summary>
  /// Validates <paramref name="index"/> is not negative and valid for target <paramref name="count"/>.
  /// </summary>
  /// <returns><see langword="true"/> if index is invalid.</returns>
  static public bool ValidateIndex
  (
    int index,
    NonNegativeInt32 count,
    [NotNullWhen ( true )] out IndexOutOfBoundariesException? e
  )
  {
    if (index < 0)
    {
      e = IndexOutOfBoundariesException.NegativeIndexMsg ( index );
      return true;
    }

    return ValidateIndex ( ((NonNegativeInt32) index), count, out e );
  }

  /// <summary>
  /// Validates whether <paramref name="count"/> and <paramref name="offset"/> create
  /// valid segmentation over source of length/count <paramref name="available"/>.
  /// </summary>
  /// <returns><see langword="true"/> if segmentation is invalid.</returns>
  /// <remarks>Empty segment is considered to be always valid.</remarks>
  static public bool ValidateSegmentation
  (
    NonNegativeInt32 available,
    NonNegativeInt32 offset,
    NonNegativeInt32 count,
    out int limit,
    [NotNullWhen ( true )] out ImpossibleSegmentationException? e
  )
  {
    limit = LimitOutOf ( offset, count );
    if (count != 0)
    {
      if (limit > available)
      {
        e = ImpossibleSegmentationException.OufRangeMsg ( available: available, offset: offset, count );
        return true;
      }
    }

    e = null;
    return false;
  }
}
