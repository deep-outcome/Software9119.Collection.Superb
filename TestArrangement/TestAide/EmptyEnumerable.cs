using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class EmptyEnumerable<T> : IEnumerable<T>
{
  public IEnumerator<T> GetEnumerator () => new EmptyEnumerator<T> ();
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
}

sealed class EmptyEnumerator<T> : IEnumerator<T>
{
  public T Current { get; } = default ( T )!;
  object? IEnumerator.Current => Current;

  public void Dispose () { }
  public bool MoveNext () => false;
  public void Reset () { }
}