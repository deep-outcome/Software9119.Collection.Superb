using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;

using System;

namespace Software9119.Collection.Superb.Ordering;

/// <summary>
/// Binary insertion sorting algorithm implementaiton.
/// </summary>
static public class BinaryInsertionOrder
{

  static ArgumentNullException NullArray ( string paramName ) => new ( paramName, message: "Array must be provided." );
  static ArgumentNullException NullComparer ( string paramName ) => new ( paramName, message: "Comparer must be provided." );

  /// <summary>
  /// Orders whole array using <paramref name="comparer"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> is <see langword="null"/>.</exception>
  static public void Order<T> ( T [] array, Comparison<T> comparer )
  {
    if (array == null)
      throw NullArray ( nameof ( array ) );

    Order<T> ( array, 0, array.Length, comparer );
  }

  /// <summary>
  /// Orders array segment defined by <paramref name="count"/> and <paramref name="offset"/> using <paramref name="comparer"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> or <paramref name="comparer"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segment over <paramref name="array"/>.
  /// </exception>
  static public void Order<T>
  (
    T [] array,
    NonNegativeInt32 offset,
    NonNegativeInt32 count,
    Comparison<T> comparer
  )
  {
    if (array == null)
      throw NullArray ( nameof ( array ) );

    if (comparer == null)
      throw NullComparer ( nameof ( comparer ) );

    if (array.Length == 0)
      return;

    int length = array.Length;
    int validation = IxValidator.ValidateSegmentation ( length, offset, count, out int limit, out ImpSegExc? e );
    switch (validation)
    {
      case -1: return;
      case 1: throw e!;
      case 0: break;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    for (int current = offset + 1 ; current < limit ; ++current)
    {
      T item = array[current];
      int left = offset;
      int right = current;

      while (left < right)
      {
        int probe = (left + right) >> 1;
        if (comparer ( item, array [ probe ] ) < 0)
          right = probe;
        else
          left = probe + 1;
      }

      if (left == current)
        continue;

      int shiftSize = current - left;
      Array.Copy ( array, left, array, left + 1, shiftSize );
      array [ left ] = item;
    }
  }

  /// <summary>
  /// Orders whole span using <paramref name="comparer"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  static public void Order<T> ( Span<T> span, Comparison<T> comparer )
  {
    if (comparer == null)
      throw NullComparer ( nameof ( comparer ) );

    int length = span.Length;
    for (int current = 1 ; current < length ; ++current)
    {
      T item = span[current];
      int left = 0;
      int right = current;

      while (left < right)
      {
        int probe = (left + right) >> 1;
        if (comparer ( item, span [ probe ] ) < 0)
          right = probe;
        else
          left = probe + 1;
      }

      if (left == current)
        continue;

      int count = current - left;
      span.Slice ( left, count ).CopyTo ( span.Slice ( left + 1, count ) );
      span [ left ] = item;
    }
  }
}
