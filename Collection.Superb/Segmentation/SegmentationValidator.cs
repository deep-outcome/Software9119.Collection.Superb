using Software9119.Collection.Superb.Segmentation.Exceptionality;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Segmentation;

class SegmentationValidator
{
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public int LimitOutOf ( int offset, int count ) => offset + count;

  static public bool ValidateSetup ( int listLength, int offset, int count, out int limit,
   [NotNullWhen(true)]
    out ImpossibleSegmentationException? e )
  {
    limit = LimitOutOf ( offset, count );

    if (offset < 0)
      e = ImpossibleSegmentationException.NegativeOffsetMsg ( offset );
    else if (count < 0)
      e = ImpossibleSegmentationException.NegativeCountMsg ( count );
    else if (limit > listLength)
      e = ImpossibleSegmentationException.OufRangeMsg ( listLength: listLength, offset: offset, count: count, limit );
    else
    {
      e = null;
      return false;
    }

    return true;

  }

  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public bool ValidateList<T>
  (
    [NotNullWhen ( false )] IList<T?>? list,
    [NotNullWhen ( true )] out ArgumentNullException? e,
    [CallerArgumentExpression ( nameof ( list ) )] string? listParamName = null
  )
  {
    if (list == null)
    {
      e = new ArgumentNullException ( paramName: listParamName, message: "Null list provided." );
      return true;
    }

    e = null;
    return false;
  }

  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public bool ValidateIndex<T> (
    ref int index,
    in IListSegment<T> segment,
    [NotNullWhen ( true )] out IndexOutOfSegmentException? e )
  {

    if (index < 0)
    {
      e = IndexOutOfSegmentException.NegativeIndexMsg ( index );
      return true;
    }

    int offset = segment.offset;
    index += offset;
    if (index >= segment.limit)
    {
      e = IndexOutOfSegmentException.OutOfRangeMsg ( index: index - offset, length: segment.Count );
      return true;
    }

    e = null;
    return false;
  }
}
