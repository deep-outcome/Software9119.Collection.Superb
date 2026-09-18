using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class XCollection<T> ( List<T> items ) : ICollection<T>
{
  public int Count => items.Count;

  public bool IsReadOnly => false;

  public void Add ( T item ) => items.Add ( item );
  public void Clear () => items.Clear ();
  public bool Contains ( T item ) => items.Contains ( item );
  public void CopyTo ( T [] array, int arrayIndex ) => items.CopyTo ( array, arrayIndex );
  public IEnumerator<T> GetEnumerator () => items.GetEnumerator ();
  public bool Remove ( T item ) => items.Remove ( item );
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
}
