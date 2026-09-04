using System;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class ReverseOrderComparer<T> : IComparer<T>
{
  readonly Comparer<T> comparer = Comparer<T>.Default;
  public int Compare ( T? x, T? y )
  {
    return comparer.Compare ( x, y ) switch
    {
      1 => -1,
      0 => 0,
      -1 => 1,
      _ => throw new InvalidOperationException ( "Impossible happened." )
    };
  }
}
