using Software9119.Collection.Superb.Numerics;

using System;
using System.Runtime.Serialization;

namespace Software9119.Collection.Superb.Indexing;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
/// <summary>
/// Thrown for index which is out of available boundaries.
/// </summary>
public class IndexOutOfBoundariesException : ArgumentOutOfRangeException
{
  static public IndexOutOfBoundariesException OutOfBoundsMsg ( NonNegativeInt32 index, NonNegativeInt32 available )
  {
    string msg = $"For available '{available}' is index '{index}' out of bounds.";
    return new IndexOutOfBoundariesException ( msg );
  }

  static public IndexOutOfBoundariesException NegativeIndexMsg ( NegativeInt32 index )
  {
    string msg = $"Index must be non-negative, but it is '{index}'.";
    return new IndexOutOfBoundariesException ( msg );
  }

  public IndexOutOfBoundariesException ( SerializationInfo info, StreamingContext context ) : base ( info, context ) { }
  public IndexOutOfBoundariesException () { }
  public IndexOutOfBoundariesException ( string message ) : base ( paramName: null!, message ) { }
  public IndexOutOfBoundariesException ( string message, Exception innerException ) : base ( message: message, innerException ) { }
}
