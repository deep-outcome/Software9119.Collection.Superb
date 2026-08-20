using System;
using System.Runtime.Serialization;

namespace Software9119.Collection.Superb.Segmentation.Exceptionality;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Exception thrown on bad segmentation settings.
/// </summary>
public class ImpossibleSegmentationException : Exception
{
  static public ImpossibleSegmentationException NegativeCountMsg ( int count )
  {
    string msg = "Count must be a non-negative integer, but it is {0}.";
    msg = string.Format ( msg, count );
    return new ImpossibleSegmentationException ( msg );
  }

  static public ImpossibleSegmentationException NegativeOffsetMsg ( int offset )
  {
    string msg = "Offset must be a non-negative integer, but it is {0}.";
    msg = string.Format ( msg, offset );
    return new ImpossibleSegmentationException ( msg );
  }

  static public ImpossibleSegmentationException OufRangeMsg ( int listLength, int offset, int count, int limit )
  {
    string msg = "List has length {0}, given offset {1} and count {2} produces out-of indexing in range {3}–{4}.";

    int topIndexOver = limit -1;
    msg = string.Format ( msg, listLength, offset, count, listLength, topIndexOver );
    return new ImpossibleSegmentationException ( msg );
  }

  public ImpossibleSegmentationException ( SerializationInfo info, StreamingContext context ) : base ( info, context ) { }
  public ImpossibleSegmentationException () { }
  public ImpossibleSegmentationException ( string message ) : base ( SegmentationExceptionHelper.DebugValMsg ( message ) ) { }
  public ImpossibleSegmentationException ( string message, Exception innerException ) : base ( message, innerException ) { }
}