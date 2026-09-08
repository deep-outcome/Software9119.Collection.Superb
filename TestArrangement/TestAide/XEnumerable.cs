using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

static class XEnumerable
{
  static readonly string [] enumerableInterfaceNames = ["IEnumerable", "IEnumerator", "IDisposable"];

  static public IEnumerable<int> RangeEnumerable ( int start, int count )
  {
    IEnumerable<int> e = count == 0
      ? new EmptyEnumerable<int>()
      : Enumerable.Range(start, count).Select(x => x);

    ValidateEnumerable ( e );
    return e;
  }

  static public IEnumerable<object> ObjectsEnumerable ( int objectCount )
  {
    IEnumerable<object> e = Enumerable
      .Range(0, objectCount)
      .Select(x => new object())
      .ToList()
      .Select(x => x);
    ValidateEnumerable ( e );
    return e;
  }

  static void ValidateEnumerable ( IEnumerable e )
  {
    Type [] interfaces = e.GetType().GetInterfaces();
    bool valid = interfaces.All(x =>
    {
      string name = x.Name;
      return enumerableInterfaceNames.Any(y => name.StartsWith(y, StringComparison.Ordinal));
    });

    if (!valid)
      throw new InvalidOperationException ( "Not pure enumerable." );
  }

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
}
