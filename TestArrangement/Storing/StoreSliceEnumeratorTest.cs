using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Storing;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Storing;

[TestClass]
public class StoreSliceEnumeratorTest
{
  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 0, 8 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 6 )]
  [DataRow ( 2, 4 )]
  public void Constructor ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int> slice = new(source,offset, count);
    StoreSliceEnumerator<int> enumerator = new (slice);

    Assert.AreEqual ( offset - 1, enumerator.resetIndex );
    Assert.AreEqual ( offset - 1, enumerator.index );
    Assert.AreEqual ( count + offset - 1, enumerator.limit );
    Assert.IsTrue ( ReferenceEquals ( slice.store, enumerator.store ) );
  }

  [TestMethod]
  public void Constructor_DefaultStoreSlice ()
  {
    StoreSlice<int> slice = default;
    StoreSliceEnumerator<int> enumerator = new( slice );

    Assert.AreEqual ( -1, enumerator.resetIndex );
    Assert.AreEqual ( -1, enumerator.index );
    Assert.AreEqual ( -1, enumerator.limit );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), enumerator.store ) );

    Assert.AreEqual ( 0, enumerator.Current );
    Assert.IsFalse ( enumerator.MoveNext () );
  }

  [TestMethod]
  public void Default_IsSafeForUse ()
  {
    StoreSliceEnumerator<object> enumerator = default;

    Assert.IsNull ( enumerator.Current );
    Assert.AreEqual ( 0, enumerator.index );
    Assert.IsFalse ( enumerator.MoveNext () );
    Assert.AreEqual ( 0, enumerator.index );
    enumerator.Reset ();
    enumerator.Dispose ();
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 0, 8 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 6 )]
  [DataRow ( 2, 4 )]
  public void Enumeration ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];

    StoreSlice<int> slice = new(source,offset, count);
    ArraySegment<int> expectation = new(source, offset, count);

    StoreSliceEnumerator<int> enumerator = new (slice);
    EnumerableEnumerator<int> enumerable = new (enumerator);

    Assert.IsTrue ( expectation.SequenceEqual ( enumerable ) );
  }

  [TestMethod]
  public void Empty ()
  {
    StoreSlice<int> slice = new([], 0, 0);

    StoreSliceEnumerator<int> enumerator = new (slice);
    Assert.AreEqual ( 0, enumerator.Current );
    Assert.IsFalse ( enumerator.MoveNext () );

    enumerator.Reset ();
    Assert.IsFalse ( enumerator.MoveNext () );
    Assert.AreEqual ( 0, enumerator.Current );
  }

  static (int, int, (bool, int) []) [] MoveNextData ()
  {
    return [
      (0, 3, [(true, 1), (true, 2), (true, 3), (false, 3)]),
      (0, 2, [(true, 1), (true, 2), (false, 2)]),
      (1, 2, [(true, 2), (true, 3), (false, 3)]),
    ];
  }

  [TestMethod]
  [DynamicData ( nameof ( MoveNextData ) )]
  public void MoveNext ( int offset, int count, (bool, int) [] steps )
  {
    StoreSlice<int> slice = new ([1,2, 3], offset, count);
    StoreSliceEnumerator<int> enumerator = new (slice);

    foreach ((bool moves, int value) in steps)
    {
      Assert.AreEqual ( moves, enumerator.MoveNext () );
      Assert.AreEqual ( value - 1, enumerator.index );
      Assert.AreEqual ( value, enumerator.Current );
    }
  }

  [TestMethod]
  public void Current ()
  {
    StoreSlice<int>  capacitor = new([3], 0, 1);
    StoreSliceEnumerator<int> enumerator = new (capacitor);

    Assert.AreEqual ( 0, enumerator.Current );
    Assert.AreEqual ( 0, ((IEnumerator) enumerator).Current );

    _ = enumerator.MoveNext ();

    Assert.AreEqual ( 3, enumerator.Current );
    Assert.AreEqual ( 3, ((IEnumerator) enumerator).Current );
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 0, 8 )]
  [DataRow ( 2, 4 )]
  public void Reset ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int>  capacitor = new(source, offset, count);
    StoreSliceEnumerator<int> enumerator = new (capacitor);

    _ = enumerator.MoveNext ();
    enumerator.Reset ();

    Assert.AreEqual ( enumerator.resetIndex, enumerator.index );
    Assert.AreEqual ( 0, enumerator.Current );
  }

  // veryfing struct equality works as it should, not truly implemented
  [TestMethod]
  public void Equals_Object ()
  {
    int[] source = [1, 2, 3, 4, 5];

    StoreSlice<int> s1 = new (source, 0, 4);
    StoreSlice<int> s2 = new (source, 0 ,4);

    StoreSliceEnumerator<int> e1 = new (s1);
    StoreSliceEnumerator<int> e2 = new (s2);

    Assert.IsTrue ( e1.Equals ( (object) e2 ) );

    _ = e2.MoveNext ();
    Assert.IsFalse ( e1.Equals ( (object) e2 ) );

    StoreSlice<int> s3 = new (source, 0, 5);
    StoreSlice<int> s4 = new (source, 1, 4);
    StoreSlice<int> s5 = new ([], 0, 4);

    Assert.IsFalse ( e1.Equals ( (object) new StoreSliceEnumerator<int> ( s3 ) ) );
    Assert.IsFalse ( e1.Equals ( (object) new StoreSliceEnumerator<int> ( s4 ) ) );
    Assert.IsFalse ( e1.Equals ( (object) new StoreSliceEnumerator<int> ( s5 ) ) );
  }
}
