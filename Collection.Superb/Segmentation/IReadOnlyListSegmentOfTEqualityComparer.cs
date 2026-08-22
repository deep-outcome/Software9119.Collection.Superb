using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IReadOnlyListSegment{T}" /> equality comparer.
/// </summary>
public class IReadOnlyListSegmentEqualityComparer<T> : IEqualityComparer<IReadOnlyListSegment<T>>
{
  /// <summary>
  /// It calls <see cref="IReadOnlyListSegment{T}.Equals(IReadOnlyListSegment{T})"/>.
  /// </summary>
  public bool Equals ( IReadOnlyListSegment<T> x, IReadOnlyListSegment<T> y ) => x.Equals ( y );

  /// <summary>
  /// It calls <see cref="IReadOnlyListSegment{T}.GetHashCode"/>.
  /// </summary>
  public int GetHashCode ( [DisallowNull] IReadOnlyListSegment<T> obj ) => obj.GetHashCode ();
}
