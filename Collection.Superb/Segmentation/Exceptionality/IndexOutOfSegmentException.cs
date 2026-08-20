using System;
using System.Runtime.Serialization;

namespace Software9119.Collection.Superb.Segmentation.Exceptionality;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
/// <summary>
/// Exception thrown why trying to index into segment out of its range.
/// </summary>
public class IndexOutOfSegmentException : Exception
{
  static public IndexOutOfSegmentException OutOfRangeMsg ( int index, int length )
  {
    string msg = "Segment length is {0}, index {1} is out of its range.";
    msg = string.Format ( msg, length, index );
    return new IndexOutOfSegmentException ( msg );
  }

  static public IndexOutOfSegmentException NegativeIndexMsg ( int index )
  {
    string msg = "Index must be non-negative, but it is {0}.";
    msg = string.Format ( msg, index );
    return new IndexOutOfSegmentException ( msg );
  }

  public IndexOutOfSegmentException ( SerializationInfo info, StreamingContext context ) : base ( info, context ) { }
  public IndexOutOfSegmentException () { }
  public IndexOutOfSegmentException ( string message ) : base ( SegmentationExceptionHelper.DebugValMsg ( message ) ) { }
  public IndexOutOfSegmentException ( string message, Exception innerException ) : base ( message, innerException ) { }
}
