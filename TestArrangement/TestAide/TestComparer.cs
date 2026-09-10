using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class TestComparer : IEqualityComparer
{
  readonly IEqualityComparer equalityComparer = EqualityComparer<object>.Default;
  new public bool Equals ( object? x, object? y ) => equalityComparer.Equals ( x, y );
  public int GetHashCode ( [DisallowNull] object obj ) => obj.GetHashCode ();
}

class TestComparer<T> : IEqualityComparer<T>
{
  readonly EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
  public bool Equals ( T? x, T? y ) => equalityComparer.Equals ( x, y );
  public int GetHashCode ( [DisallowNull] T obj ) => obj.GetHashCode ();
}

sealed class MyKeyComparer<T> : TestComparer<T>;