using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IListSegment" /> equality comparer.
/// </summary>
public class IListSegmentEqualityComparer : IEqualityComparer<IListSegment>
{
  /// <summary>
  /// It calls <see cref="IListSegment.Equals(IListSegment)"/>.
  /// </summary>
  public bool Equals ( IListSegment x, IListSegment y ) => x.Equals ( y );

  /// <summary>
  /// It calls <see cref="IListSegment.GetHashCode"/>.
  /// </summary>
  public int GetHashCode ( [DisallowNull] IListSegment obj ) => obj.GetHashCode ();
}
