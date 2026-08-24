using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class IListOfTSegmentEqualityComparerTest
{

  [TestMethod]
  public void Equals_Equals ()
  {
    IListSegmentEqualityComparer<int> comparer = new ();
    IListSegment<int> segment = new([]);
    Assert.IsTrue ( comparer.Equals ( segment, segment ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Reference ()
  {
    IListSegment<int> i = new ([]);
    IListSegment<int> you = new ([]);
    IListSegmentEqualityComparer<int> comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Offset ()
  {
    int[] source = [1,2, 3];
    IListSegment<int> i = new ( source, 0, 2);
    IListSegment<int> you = new (source, 1, 2);
    IListSegmentEqualityComparer<int> comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Count ()
  {
    int[] source = [1,2, 3];
    IListSegment<int> i = new ( source, 1, 1);
    IListSegment<int> you = new (source, 1, 2);
    IListSegmentEqualityComparer<int> comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void _GetHashCode ()
  {
    IListSegmentEqualityComparer<int> comparer = new ();
    IListSegment<int> segment = new([]);

    Assert.AreEqual ( segment.GetHashCode (), comparer.GetHashCode ( segment ) );
  }
}
