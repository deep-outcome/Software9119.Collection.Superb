using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IListSegment" /> equality comparer.
/// </summary>
public class IListSegmentEqualityComparer : IEqualityComparer<IListSegment>, IEqualityComparer
{
  /// <summary>
  /// It calls <see cref="IListSegment.Equals(IListSegment)"/>.
  /// </summary>
  public bool Equals ( IListSegment x, IListSegment y ) => x.Equals ( y );

  /// <summary>
  /// It calls <see cref="IListSegment.GetHashCode"/>.
  /// </summary>
  public int GetHashCode ( [DisallowNull] IListSegment obj ) => obj.GetHashCode ();

  /// <summary>
  /// Calls to <see cref="GetHashCode(IListSegment)"/> or <see cref="object.GetHashCode()"/> with
  /// <paramref name="obj"/> based on type match.
  /// </summary>
  /// <remarks><see cref="HashCode.Combine{Object}"/> for <see langword="null"/>.</remarks>
  public int GetHashCode ( object obj )
  {
    if (obj is null)
      return HashCode.Combine ( obj );

    if (obj is IListSegment segment)
      return GetHashCode ( segment );

    return obj.GetHashCode ();
  }

  readonly EqualityComparer<object> comparer = EqualityComparer<object>.Default;

  /// <summary>
  /// Calls to <see cref="Equals(IListSegment, IListSegment)"/> 
  /// or <c>EqualityComparer&lt;object&gt;.Equals(object?, object?)</c>
  /// with <paramref name="x"/> and <paramref name="y"/> based on type match.
  /// </summary>
  new public bool Equals ( object? x, object? y )
  {
    if (x is IListSegment x_segment && y is IListSegment y_segment)
      return Equals ( x_segment, y_segment );


    return comparer.Equals ( x, y );
  }
}
