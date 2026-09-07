using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class EnumerableEnumerator<T> ( IEnumerator<T> enumerator ) : IEnumerable<T>
{
  public IEnumerator<T> GetEnumerator () => enumerator;
  IEnumerator IEnumerable.GetEnumerator () => enumerator;
}
