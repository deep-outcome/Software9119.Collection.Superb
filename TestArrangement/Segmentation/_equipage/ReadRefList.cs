using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;

readonly ref struct ReadRefList<T> ( IReadOnlyList<T> values ) : IReadOnlyList<T>
{
  public T this [ int index ] => values [ index ];

  public int Count => values.Count;

  public IEnumerator<T> GetEnumerator () => values.GetEnumerator ();
  IEnumerator IEnumerable.GetEnumerator () => ((IEnumerable) values).GetEnumerator ();
}
