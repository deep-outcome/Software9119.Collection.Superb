using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;

readonly ref struct RefList<T> ( IList<T> values ) : IList<T>
{
  public T this [ int index ] { get => values [ index ]; set => values [ index ] = value; }
  public int Count => values.Count;
  public bool IsReadOnly => values.IsReadOnly;

  public void Add ( T item ) => values.Add ( item );
  public void Clear () => values.Clear ();
  public bool Contains ( T item ) => values.Contains ( item );
  public void CopyTo ( T [] array, int arrayIndex ) => values.CopyTo ( array, arrayIndex );
  public IEnumerator<T> GetEnumerator () => values.GetEnumerator ();
  public int IndexOf ( T item ) => values.IndexOf ( item );
  public void Insert ( int index, T item ) => values.Insert ( index, item );
  public bool Remove ( T item ) => values.Remove ( item );
  public void RemoveAt ( int index ) => values.RemoveAt ( index );
  IEnumerator IEnumerable.GetEnumerator () => ((IEnumerable) values).GetEnumerator ();
}
