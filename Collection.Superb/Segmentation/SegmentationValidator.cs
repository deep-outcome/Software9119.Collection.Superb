using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.Numerics;

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Segmentation;

class SegmentationValidator
{
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public int LimitOutOf ( int offset, int count ) => offset + count;


  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public int CorrelateIndex ( int index, int offset ) => index + offset;

  static public bool ValidateSegmentation ( int length, int offset, int count, out int limit,
   [NotNullWhen ( true )] out ImpossibleSegmentationException? e )
  {
    limit = LimitOutOf ( offset, count );

    if (offset < 0)
      e = ImpossibleSegmentationException.NegativeOffsetMsg ( offset );
    else if (count < 0)
      e = ImpossibleSegmentationException.NegativeCountMsg ( count );
    else if (limit > length)
      e = ImpossibleSegmentationException.OufRangeMsg ( length: length, offset: offset, count: count, limit );
    else
    {
      e = null;
      return false;
    }

    return true;
  }

  static public bool ValidateList
  (
    [NotNullWhen ( false )] IEnumerable? list,
    [NotNullWhen ( true )] out ArgumentNullException? e,
    [CallerArgumentExpression ( nameof ( list ) )] string? listParamName = null
  )
  {
    if (list.IsNull ())
    {
      e = new ArgumentNullException ( paramName: listParamName, message: "Null list provided." );
      return true;
    }

    e = null;
    return false;
  }

  static public bool ValidateIndex (
    ref int index,
    int offset,
    int count,
    [NotNullWhen ( true )] out IndexOutOfSegmentException? e )
  {

    if (index < 0)
    {
      e = IndexOutOfSegmentException.NegativeIndexMsg ( index );
      return true;
    }

    if (index >= count)
    {
      e = IndexOutOfSegmentException.OutOfRangeMsg ( index, length: count );
      return true;
    }

    index = CorrelateIndex ( index, offset );

    e = null;
    return false;
  }

  static public bool ValidateIndex
  (
    NonNegativeInt32 index,
    NonNegativeInt32 count,
   [NotNullWhen ( true )] out IndexOutOfSegmentException? e
  )
  {
    if (index >= count)
    {
      e = IndexOutOfSegmentException.OutOfRangeMsg ( index: index, length: count );
      return true;
    }

    e = null;
    return false;
  }
}
