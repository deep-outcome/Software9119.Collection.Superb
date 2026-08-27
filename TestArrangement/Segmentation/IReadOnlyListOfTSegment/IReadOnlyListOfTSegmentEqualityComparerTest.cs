using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;

using System;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class IReadOnlyListOfTSegmentEqualityComparerTest
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

  [TestMethod]
  public void ObjectGetHashCode_Segment ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    IReadOnlyListSegment<int> segment = new(new int [0]);

    Assert.AreEqual ( segment.GetHashCode (), comparer.GetHashCode ( (object) segment ) );
  }

  [TestMethod]
  public void ObjectGetHashCode_Null ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    Assert.AreEqual ( HashCode.Combine ( (object) null! ), comparer.GetHashCode ( null! ) );
  }

  [TestMethod]
  public void ObjectGetHashCode_Object ()
  {
    object obj = new();
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    Assert.AreEqual ( obj.GetHashCode (), comparer.GetHashCode ( obj ) );
  }

  [TestMethod]
  public void ObjectEquals_Segment_Equals ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    object segment = new IReadOnlyListSegment<int> (new int [0]);
    Assert.IsTrue ( comparer.Equals ( segment, segment ) );
  }

  [TestMethod]
  public void ObjectEquals_Segment_NotEqual ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    object i = new IReadOnlyListSegment<int> (new int [0]);
    object you = new IReadOnlyListSegment<int> (new int [0]);
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void ObjectEquals_Object_Equals ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    object obj = new ();
    Assert.IsTrue ( comparer.Equals ( obj, obj ) );
  }

  [TestMethod]
  public void ObjectEquals_Object_NotEqual ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    object obj1 = new ();
    object obj2 = new ();
    Assert.IsFalse ( comparer.Equals ( obj1, obj2 ) );
  }

  [TestMethod]
  public void ObjectEquals_Null ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new ();
    object? @null = null;
    Assert.IsTrue ( comparer.Equals ( @null, @null ) );
  }
}
