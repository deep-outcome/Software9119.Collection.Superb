using System;
using System.Collections;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;

readonly struct NoRefList ( IList values ) : IList
{
  public object? this [ int index ] { get => values [ index ]; set => values [ index ] = value; }

  public bool IsFixedSize => values.IsFixedSize;
  public int Count => values.Count;
  public bool IsReadOnly => values.IsReadOnly;
  public bool IsSynchronized => values.IsSynchronized;
  public object SyncRoot => values.SyncRoot;

  public int Add ( object? value ) => values.Add ( value );
  public void Clear () => values.Clear ();
  public bool Contains ( object? value ) => values.Contains ( value );
  public void CopyTo ( Array array, int index ) => values.CopyTo ( array, index );
  public IEnumerator GetEnumerator () => values.GetEnumerator ();
  public int IndexOf ( object? value ) => values.IndexOf ( value );
  public void Insert ( int index, object? value ) => values.Insert ( index, value );
  public void Remove ( object? value ) => values.Remove ( value );
  public void RemoveAt ( int index ) => values.RemoveAt ( index );
}
