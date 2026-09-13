using System;
using System.Runtime.Serialization;

namespace Software9119.Collection.Superb.Segmentation;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Exception thrown on bad segmentation settings.
/// </summary>
public class ImpossibleSegmentationException : ArgumentOutOfRangeException
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

  static public ImpossibleSegmentationException OufRangeMsg ( int length, int offset, int count, int limit )
  {
    string msg = "With available length {0}, given offset {1} and count {2} produce out-of indexing in range {3}–{4}.";

    int topIndexOver = limit -1;
    msg = string.Format ( msg, length, offset, count, length, topIndexOver );
    return new ImpossibleSegmentationException ( msg );
  }

  public ImpossibleSegmentationException ( SerializationInfo info, StreamingContext context ) : base ( info, context ) { }
  public ImpossibleSegmentationException () { }
  public ImpossibleSegmentationException ( string message ) : base ( paramName: null, message: SegmentationExceptionHelper.DebugValMsg ( message ) ) { }
  public ImpossibleSegmentationException ( string message, Exception innerException ) : base ( message: message, innerException ) { }
}