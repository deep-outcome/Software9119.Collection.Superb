using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class TestComparer<T> : IEqualityComparer<T>
{
  public bool Equals ( T? x, T? y ) => EqualityComparer<T>.Default.Equals ( x, y );
  public int GetHashCode ( [DisallowNull] T obj ) => obj.GetHashCode ();
}
