using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.Segmentation.Exceptionality;
using Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class IListOfTRefEnumeratorTest
{
  [TestMethod]
  public void PubCtor_NegativeOffset ()
  {
    try
    {
      RefList<int> list = new ([]);
      using (new IListRefEnumerator<RefList<int>, int> ( -1, 0, list )) { }
    }
    catch (ImpossibleSegmentationException e)
    {
      const string expMessage = "Offset must be a non-negative integer, but it is -1.";
      Assert.AreEqual ( expMessage, e.Message );
    }


  }

  [TestMethod]
  public void PubCtor_NegativeCount ()
  {
    try
    {
      RefList<int> list = new ([]);
      using (new IListRefEnumerator<RefList<int>, int> ( 0, -1, list )) { }
    }
    catch (ImpossibleSegmentationException e)
    {
      const string expMessage = "Count must be a non-negative integer, but it is -1.";
      Assert.AreEqual ( expMessage, e.Message );
    }
  }

  [TestMethod]
  [DataRow ( 3, 3, "List has length 5, given offset 3 and count 3 produces out-of indexing in range 5–5." )]
  [DataRow ( 3, 4, "List has length 5, given offset 3 and count 4 produces out-of indexing in range 5–6." )]
  [DataRow ( 8, 3, "List has length 5, given offset 8 and count 3 produces out-of indexing in range 5–10." )]
  public void PubCtor_InvalidSegmentation ( int offset, int count, string errMsg )
  {
    try
    {
      RefList<int> list = new ([ 1, 2, 3, 4, 5 ]);
      using (new IListRefEnumerator<RefList<int>, int> ( offset, count,  list)) { }
    }
    catch (ImpossibleSegmentationException e)
    { 
      Assert.AreEqual ( errMsg, e.Message );
    }
  }

  [TestMethod]
  public void Current ()
  {
    RefList<int> list = new ([ 1, 2, 3, 4, 5 ]);
    using IListRefEnumerator<RefList<int>,int> enumerator = new (0,5, list);
    Assert.AreEqual ( 0, enumerator.Current );    
    _ = enumerator.MoveNext ();
    Assert.AreEqual ( 1, enumerator.Current );
  }

  [TestMethod]
  [DataRow ( 0, 1, 1 )]
  [DataRow ( 4, 1, 5 )]
  [DataRow ( 3, 1, 4 )]
  public void MoveNextA ( int offset, int count, int current )
  {
    RefList<int> list = new ([ 1, 2, 3, 4, 5 ]);
    using IListRefEnumerator<RefList<int>,int> enumerator = new (offset,count, list);
    Assert.AreEqual ( 0, enumerator.Current );

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
    RefList<int> list = new ([ 1, 2, 3, 4, 5 ]);
    using IListRefEnumerator<RefList<int>,int> enumerator = new (offset,count, list);
    Assert.AreEqual ( 0, enumerator.Current );

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
    RefList<int> list = new ([ 1, ]);
    using IListRefEnumerator<RefList<int>,int> enumerator = new (0,1, list);
    Assert.IsTrue ( enumerator.MoveNext () );

    enumerator.Reset ();
    Assert.AreEqual ( 0, enumerator.Current );
    Assert.IsTrue ( enumerator.MoveNext () );
  }
}
