using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class IListRefSegmentEqualityComparerTest
{

  [TestMethod]
  public void Equals_Equals ()
  {
    IListRefSegmentEqualityComparer<int> comparer = new ();
    IListRefSegment<int> segment = new([]);
    Assert.IsTrue ( comparer.Equals ( segment, segment ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Reference ()
  {
    IListRefSegment<int> i = new ( []);
    IListRefSegment<int> you = new ([]);
    IListRefSegmentEqualityComparer<int> comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Offset ()
  {
    int[] source = [1,2, 3];
    IListRefSegment<int> i = new ( source, 0, 2);
    IListRefSegment<int> you = new (source, 1, 2);
    IListRefSegmentEqualityComparer<int> comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Count ()
  {
    int[] source = [1,2, 3];
    IListRefSegment<int> i = new ( source, 1, 1);
    IListRefSegment<int> you = new (source, 1, 2);
    IListRefSegmentEqualityComparer<int> comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void _GetHashCode ()
  {
    IListRefSegmentEqualityComparer<int> comparer = new ();
    IListRefSegment<int> segment = new([]);

    Assert.AreEqual ( segment.GetHashCode (), comparer.GetHashCode ( segment ) );
  }
}
