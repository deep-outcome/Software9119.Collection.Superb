using Software9119.Collection.Superb.Numerics;

using System;
using System.Runtime.Serialization;

namespace Software9119.Collection.Superb.Indexing;

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

  static public ImpossibleSegmentationException OufRangeMsg
  (
    NonNegativeInt32 available,
    NonNegativeInt32 offset,
    NonNegativeInt32 count,
    string []? parameters = null
  )
  {
    const string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing.{3}";

    int paramsLength = parameters?.Length ?? 0;
    string paramsString = paramsLength == 0 ? "" : $" (Parameters '{string.Join("','", parameters!)}')";

    string errMsg = string.Format ( msg, available, offset, count, paramsString );
    return new ImpossibleSegmentationException ( errMsg );
  }

  public ImpossibleSegmentationException ( SerializationInfo info, StreamingContext context ) : base ( info, context ) { }
  public ImpossibleSegmentationException () { }
  public ImpossibleSegmentationException ( string? message ) : base ( paramName: null, message: message ) { }
  public ImpossibleSegmentationException ( string? message, string? paramName ) : base ( paramName: paramName, message: message ) { }
  public ImpossibleSegmentationException ( string? message, Exception? innerException ) : base ( message: message, innerException ) { }
}