using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.Segmentation.Exceptionality;

using System;
using System.Collections;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class IListEnumeratorTest
{
  [TestMethod]
  public void PubCtor_NullList ()
  {
    int[]? list = null;
    Func<object> test = () => new IListEnumerator(0,0, list!);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> (test);
    Assert.AreEqual ( "Null list provided. (Parameter 'list')", e.Message );
  }

  [TestMethod]
  public void PubCtor_NegativeOffset ()
  {
    int[]? list = [];
    Func<object> test = () => new IListEnumerator(-1,0, list);
    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> (test);
    const string expMessage = "Offset must be a non-negative integer, but it is -1.";
    Assert.AreEqual ( expMessage, e.Message );
  }

  [TestMethod]
  public void PubCtor_NegativeCount ()
  {
    int[]? list = [];
    Func<object> test = () => new IListEnumerator(0,-1, list);
    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> (test);
    const string expMessage = "Count must be a non-negative integer, but it is -1.";
    Assert.AreEqual ( expMessage, e.Message );
  }

  [TestMethod]
  [DataRow ( 3, 3, "List has length 5, given offset 3 and count 3 produces out-of indexing in range 5–5." )]
  [DataRow ( 3, 4, "List has length 5, given offset 3 and count 4 produces out-of indexing in range 5–6." )]
  [DataRow ( 8, 3, "List has length 5, given offset 8 and count 3 produces out-of indexing in range 5–10." )]
  public void PubCtor_InvalidSegmentation ( int offset, int count, string errMsg )
  {
    Func<object> test = () => new IListEnumerator(offset,count, new int [] { 1,2,3,4, 5 } );
    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> (test);
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  public void Current ()
  {
    IListEnumerator enumerator = new (0,5, new int [] {1,2,3,4, 5 } );
    Assert.IsNull ( enumerator.Current );
    Assert.AreEqual ( enumerator.Current, ((IEnumerator) enumerator).Current );
    _ = enumerator.MoveNext ();
    Assert.AreEqual ( 1, enumerator.Current );
    Assert.AreEqual ( enumerator.Current, ((IEnumerator) enumerator).Current );
  }

  [TestMethod]
  [DataRow ( 0, 1, 1 )]
  [DataRow ( 4, 1, 5 )]
  [DataRow ( 3, 1, 4 )]
  public void MoveNextA ( int offset, int count, int current )
  {
    IListEnumerator enumerator = new (offset,count, new int [] { 1,2,3,4,5 } );
    Assert.IsNull ( enumerator.Current );

    Assert.IsTrue ( enumerator.MoveNext () );
    Assert.AreEqual ( current, enumerator.Current );

    Assert.IsFalse ( enumerator.MoveNext () );
    Assert.AreEqual ( current, enumerator.Current );
  }

  [TestMethod]
  [DataRow ( 0, 2, new int [] { 1, 2 } )]
  [DataRow ( 3, 2, new int [] { 4, 5 } )]
  [DataRow ( 2, 2, new int [] { 3, 4 } )]
  public void MoveNextB ( int offset, int count, int [] current )
  {
    IListEnumerator enumerator = new (offset,count, new int [] { 1,2,3,4,5 } );
    Assert.IsNull ( enumerator.Current );

    Assert.IsTrue ( enumerator.MoveNext () );
    Assert.AreEqual ( current [ 0 ], enumerator.Current );

    Assert.IsTrue ( enumerator.MoveNext () );
    Assert.AreEqual ( current [ 1 ], enumerator.Current );

    Assert.IsFalse ( enumerator.MoveNext () );
    Assert.AreEqual ( current [ 1 ], enumerator.Current );
  }

  [TestMethod]
  public void Reset ()
  {
    IListEnumerator enumerator = new (0,1, new int [] { 1 } );
    Assert.IsTrue ( enumerator.MoveNext () );

    enumerator.Reset ();
    Assert.IsNull ( enumerator.Current );
    Assert.IsTrue ( enumerator.MoveNext () );
  }
}
