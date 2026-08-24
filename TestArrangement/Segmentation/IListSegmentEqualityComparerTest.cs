using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;

using System;

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

  [TestMethod]
  public void ObjectGetHashCode_Segment ()
  {
    IListSegmentEqualityComparer comparer = new ();
    IListSegment segment = new(new int [0]);

    Assert.AreEqual ( segment.GetHashCode (), comparer.GetHashCode ( (object) segment ) );
  }

  [TestMethod]
  public void ObjectGetHashCode_Null ()
  {
    IListSegmentEqualityComparer comparer = new ();
    Assert.AreEqual ( HashCode.Combine ( (object) null! ), comparer.GetHashCode ( null! ) );
  }

  [TestMethod]
  public void ObjectGetHashCode_Object ()
  {
    object obj = new();
    IListSegmentEqualityComparer comparer = new ();
    Assert.AreEqual ( obj.GetHashCode (), comparer.GetHashCode ( obj ) );
  }

  [TestMethod]
  public void ObjectEquals_Segment_Equals ()
  {
    IListSegmentEqualityComparer comparer = new ();
    object segment = new IListSegment (new int [0]);
    Assert.IsTrue ( comparer.Equals ( segment, segment ) );
  }

  [TestMethod]
  public void ObjectEquals_Segment_NotEqual ()
  {
    IListSegmentEqualityComparer comparer = new ();
    object i = new IListSegment (new int [0]);
    object you = new IListSegment (new int [0]);
    Assert.IsFalse ( comparer.Equals ( i, you ) );
  }

  [TestMethod]
  public void ObjectEquals_Object_Equals ()
  {
    IListSegmentEqualityComparer comparer = new ();
    object obj = new ();
    Assert.IsTrue ( comparer.Equals ( obj, obj ) );
  }

  [TestMethod]
  public void ObjectEquals_Object_NotEqual ()
  {
    IListSegmentEqualityComparer comparer = new ();
    object obj1 = new ();
    object obj2 = new ();
    Assert.IsFalse ( comparer.Equals ( obj1, obj2 ) );
  }

  [TestMethod]
  public void ObjectEquals_Null ()
  {
    IListSegmentEqualityComparer comparer = new ();
    object? @null = null;
    Assert.IsTrue ( comparer.Equals ( @null, @null ) );
  }
}
