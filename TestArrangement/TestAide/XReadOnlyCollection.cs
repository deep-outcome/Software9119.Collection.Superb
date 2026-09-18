using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class XReadOnlyCollection<T> ( List<T> items ) : IReadOnlyCollection<T>
{
  public int Count => items.Count;

  public IEnumerator<T> GetEnumerator () => items.GetEnumerator ();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
}
