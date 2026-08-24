using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;

using System;

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

  [TestMethod]
  public void ObjectGetHashCode_Segment ()
  {
    IListSegmentEqualityComparer<int> comparer = new ();
    IListSegment<int> segment = new([]);

    Assert.AreEqual ( segment.GetHashCode (), comparer.GetHashCode ( (object) segment ) );
  }

  [TestMethod]
  public void ObjectGetHashCode_Null ()
  {
    IListSegmentEqualityComparer<int> comparer = new ();
    Assert.AreEqual ( HashCode.Combine ( (object) null! ), comparer.GetHashCode ( null! ) );
  }

  [TestMethod]
  public void ObjectGetHashCode_Object ()
  {
    object obj = new();
    IListSegmentEqualityComparer<int> comparer = new ();
    Assert.AreEqual ( obj.GetHashCode (), comparer.GetHashCode ( obj ) );
  }

  [TestMethod]
  public void ObjectEquals_Segment_Equals ()
  {
    IListSegmentEqualityComparer<int> comparer = new ();
    object segment = new IListSegment<int> ( []);
    Assert.IsTrue ( comparer.Equals ( segment, segment ) );
  }

  [TestMethod]
  public void ObjectEquals_Segment_NotEqual ()
  {
    IListSegmentEqualityComparer<int> comparer = new ();
    object i = new IListSegment<int> ([]);
    object you = new IListSegment<int> ([]);
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void ObjectEquals_Object_Equals ()
  {
    IListSegmentEqualityComparer<int> comparer = new ();
    object obj = new ();
    Assert.IsTrue ( comparer.Equals ( obj, obj ) );
  }

  [TestMethod]
  public void ObjectEquals_Object_NotEqual ()
  {
    IListSegmentEqualityComparer<int> comparer = new ();
    object obj1 = new ();
    object obj2 = new ();
    Assert.IsFalse ( comparer.Equals ( obj1, obj2 ) );
  }

  [TestMethod]
  public void ObjectEquals_Null ()
  {
    IListSegmentEqualityComparer<int> comparer = new ();
    object? @null = null;
    Assert.IsTrue ( comparer.Equals ( @null, @null ) );
  }
}
