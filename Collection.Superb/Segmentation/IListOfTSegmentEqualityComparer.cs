using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IListSegment{T}" /> equality comparer.
/// </summary>
public class IListSegmentEqualityComparer<T> : IEqualityComparer<IListSegment<T>>
{
  /// <summary>
  /// It calls <see cref="IListSegment{T}.Equals(IListSegment{T})"/>.
  /// </summary>
  public bool Equals ( IListSegment<T> x, IListSegment<T> y ) => x.Equals ( y );

  /// <summary>
  /// It calls <see cref="IListSegment{T}.GetHashCode"/>.
  /// </summary>
  public int GetHashCode ( [DisallowNull] IListSegment<T> obj ) => obj.GetHashCode ();
}
