using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IListRefSegment{T}" /> equality comparer.
/// </summary>
public class IListRefSegmentEqualityComparer<T> : IEqualityComparer<IListRefSegment<T>>
{
  /// <summary>
  /// It calls <see cref="IListRefSegment{T}.Equals(IListRefSegment{T})"/>.
  /// </summary>
  public bool Equals ( IListRefSegment<T> x, IListRefSegment<T> y ) => x.Equals ( y );

  /// <summary>
  /// It calls <see cref="IListRefSegment{T}.GetHashCode"/>.
  /// </summary>
  public int GetHashCode ( [DisallowNull] IListRefSegment<T> obj ) => obj.GetHashCode ();
}
