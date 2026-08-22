using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class IReadOnlyListSegmentEqualityComparerTest
{

  [TestMethod]
  public void Equals_Equals ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    IReadOnlyListSegment<int> segment = new([]);
    Assert.IsTrue ( comparer.Equals ( segment, segment ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Reference ()
  {
    IReadOnlyListSegment<int> i = new ( new int [0]);
    IReadOnlyListSegment<int> you = new (new int [0]);
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Offset ()
  {
    int[] source = [1,2, 3];
    IReadOnlyListSegment<int> i = new ( source, 0, 2);
    IReadOnlyListSegment<int> you = new (source, 1, 2);
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Count ()
  {
    int[] source = [1,2, 3];
    IReadOnlyListSegment<int> i = new ( source, 1, 1);
    IReadOnlyListSegment<int> you = new (source, 1, 2);
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void _GetHashCode ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    IReadOnlyListSegment<int> segment = new([]);

    Assert.AreEqual ( segment.GetHashCode (), comparer.GetHashCode ( segment ) );
  }
}
