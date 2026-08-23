using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class IListSegmentEqualityComparerTest
{
  [TestMethod]
  public void Equals_Equals ()
  {
    IListSegmentEqualityComparer comparer = new ();
    IListSegment segment = new(new int [0]);
    Assert.IsTrue ( comparer.Equals ( segment, segment ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Reference ()
  {
    IListSegment i = new (new int [0]);
    IListSegment you = new (new int [0]);
    IListSegmentEqualityComparer comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Offset ()
  {
    int[] source = [1,2, 3];
    IListSegment i = new ( source, 0, 2);
    IListSegment you = new (source, 1, 2);
    IListSegmentEqualityComparer comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Count ()
  {
    int[] source = [1,2, 3];
    IListSegment i = new ( source, 1, 1);
    IListSegment you = new (source, 1, 2);
    IListSegmentEqualityComparer comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void _GetHashCode ()
  {
    IListSegmentEqualityComparer comparer = new ();
    IListSegment segment = new(new int [0]);

    Assert.AreEqual ( segment.GetHashCode (), comparer.GetHashCode ( segment ) );
  }
}
