using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class ReverseOrderComparer : IComparer
{
  readonly Comparer comparer = Comparer.Default;
  public int Compare ( object? x, object? y )
  {
    return comparer.Compare ( x, y ) switch
    {
      1 => -1,
      -1 => 1,
      _ => 0,
    };
  }
}

class ReverseOrderComparer<T> : IComparer<T>
{
  readonly Comparer<T> comparer = Comparer<T>.Default;
  public int Compare ( T? x, T? y )
  {
    return comparer.Compare ( x, y ) switch
    {
      1 => -1,
      -1 => 1,
      _ => 0,
    };
  }
}

sealed class MyOrderingComparer<T> : ReverseOrderComparer<T>;