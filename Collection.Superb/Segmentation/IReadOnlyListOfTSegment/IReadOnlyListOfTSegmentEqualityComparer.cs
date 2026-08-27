using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IReadOnlyListSegment{T}" /> equality comparer.
/// </summary>
public class IReadOnlyListSegmentEqualityComparer<T> : IEqualityComparer<IReadOnlyListSegment<T>>, IEqualityComparer
{
  /// <summary>
  /// It calls <see cref="IReadOnlyListSegment{T}.Equals(IReadOnlyListSegment{T})"/>.
  /// </summary>
  public bool Equals ( IReadOnlyListSegment<T> x, IReadOnlyListSegment<T> y ) => x.Equals ( y );

  /// <summary>
  /// It calls <see cref="IReadOnlyListSegment{T}.GetHashCode"/>.
  /// </summary>
  public int GetHashCode ( [DisallowNull] IReadOnlyListSegment<T> obj ) => obj.GetHashCode ();

  /// <summary>
  /// Calls to <see cref="GetHashCode(IReadOnlyListSegment{T})"/> or <see cref="object.GetHashCode()"/> with
  /// <paramref name="obj"/> based on type match.
  /// </summary>
  /// <remarks><see cref="HashCode.Combine{Object}"/> for <see langword="null"/>.</remarks>
  public int GetHashCode ( object obj )
  {
    if (obj is null)
      return HashCode.Combine ( obj );

    if (obj is IReadOnlyListSegment<T> segment)
      return GetHashCode ( segment );

    return obj.GetHashCode ();
  }

  readonly EqualityComparer<object> comparer = EqualityComparer<object>.Default;

  /// <summary>
  /// Calls to <see cref="Equals(IReadOnlyListSegment{T}, IReadOnlyListSegment{T})"/> 
  /// or <c>EqualityComparer&lt;object&gt;.Equals(object?, object?)</c>
  /// with <paramref name="x"/> and <paramref name="y"/> based on type match.
  /// </summary>
  new public bool Equals ( object? x, object? y )
  {
    if (x is IReadOnlyListSegment<T> x_segment && y is IReadOnlyListSegment<T> y_segment)
      return Equals ( x_segment, y_segment );


    return comparer.Equals ( x, y );
  }
}
