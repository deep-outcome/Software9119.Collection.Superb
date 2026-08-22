using Software9119.Collection.Superb.Segmentation.Exceptionality;

using System;
using System.Collections;
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
  static public bool ValidateList
  (
    [NotNullWhen ( false )] IEnumerable? list,
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

  static public bool ValidateIndex (
    ref int index,
    int offset,
    int limit,
    int count,
    [NotNullWhen ( true )] out IndexOutOfSegmentException? e )
  {

    if (index < 0)
    {
      e = IndexOutOfSegmentException.NegativeIndexMsg ( index );
      return true;
    }

    index += offset;
    if (index >= limit)
    {
      e = IndexOutOfSegmentException.OutOfRangeMsg ( index: index - offset, length: count );
      return true;
    }

    e = null;
    return false;
  }
}
