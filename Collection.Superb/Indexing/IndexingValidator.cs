using Software9119.Collection.Superb.Numerics;

using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Indexing;

/// <summary>
/// Indexing validator contains useful methods for working with indexes.
/// </summary>
static public class IndexingValidator
{

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

}
