using Software9119.Collection.Superb.Numerics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Software9119.Collection.Superb.Indexing;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Exception thrown on bad segmentation settings.
/// </summary>
public class ImpossibleSegmentationException : ArgumentOutOfRangeException
{
  static public ImpossibleSegmentationException NegativeCountMsg ( int count, SegmentationParamNames paramNames = default )
  {
    string msg = "Count must be a non-negative integer, but it is '{0}'.";
    msg = string.Format ( msg, count );
    return new ImpossibleSegmentationException ( msg, paramName: paramNames.Count );
  }

  static public ImpossibleSegmentationException NegativeOffsetMsg ( int offset, SegmentationParamNames paramNames = default )
  {
    string msg = "Offset must be a non-negative integer, but it is '{0}'.";
    msg = string.Format ( msg, offset );
    return new ImpossibleSegmentationException ( msg, paramName: paramNames.Offset );
  }

  static public ImpossibleSegmentationException ForwardSegmentationMsg
  (
    NonNegativeInt32 available,
    NonNegativeInt32 offset,
    NonNegativeInt32 count,
    SegmentationParamNames paramNames = default
  )
  {
    const string msg = "With available '{0}', given offset '{1}' and count '{2}' produce out-of indexing.{3}";
    string paramsString = ParamsString(paramNames);

    string errMsg = string.Format ( msg, available, offset, count, paramsString );
    return new ImpossibleSegmentationException ( errMsg );
  }

  static public ImpossibleSegmentationException BackwardSegmentationMsg
  (
    NonNegativeInt32 available,
    NonNegativeInt32 rearSet,
    NonNegativeInt32 count,
    string []? parameters = null
  )
  {
    string msg = "With available '{0}', given rearSet '{1}' and count '{2}' produce out-of indexing.{3}";
    string paramsString = ParamsString(parameters);

    string errMsg = string.Format ( msg, available, rearSet, count, paramsString );
    return new ImpossibleSegmentationException ( errMsg );
  }

  static internal string ParamsString ( IEnumerable<string?>? parameters )
  {
    parameters = parameters?.Where ( x => !string.IsNullOrWhiteSpace ( x ) ) ?? Enumerable.Empty<string> ();
    string join = string.Join("','", parameters );

    if (join.Length == 0)
      return join;

    string intro = parameters.AtLeast(2) ? "Parameters" : "Parameter";

    return $" ({intro} '{join}')";
  }

  public ImpossibleSegmentationException ( SerializationInfo info, StreamingContext context ) : base ( info, context ) { }
  public ImpossibleSegmentationException () { }
  public ImpossibleSegmentationException ( string? message ) : base ( paramName: null, message: message ) { }
  public ImpossibleSegmentationException ( string? message, string? paramName ) : base ( paramName: paramName, message: message ) { }
  public ImpossibleSegmentationException ( string? message, Exception? innerException ) : base ( message: message, innerException ) { }
}