using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.Segmentation.Exceptionality;
using Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class IListRefSegmentTest
{
  [TestMethod]
  public void DefaultCtor ()
  {
    NoRefList list = new(new string[] { "a", "b", "c", "d", "e", } );

    IListRefSegment<NoRefList> segment = new ( list );
    Assert.AreEqual ( list.Count, segment.Count );
    Assert.AreEqual ( list.Count, segment.limit );
    Assert.AreEqual ( 0, segment.Offset );
    Assert.AreEqual ( 0, segment.offset );
    Assert.AreEqual ( list, segment.list );
    Assert.AreEqual ( list, segment.List );

    Assert.AreSame ( EqualityComparer<object>.Default, segment.EqualityComparer );
  }

  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void DefaultCtor_NullEqualityComparer ( bool passNullExplicitly )
  {
    RefList list = new(new string[] { "a", "b", "c", "d", "e", } );

    IListRefSegment<RefList> segment = passNullExplicitly ? new ( list, null ) : new ( list );
    Assert.AreSame ( EqualityComparer<object>.Default, segment.EqualityComparer );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 4, 1 )]
  [DataRow ( 1, 3 )]
  [DataRow ( 0, 0 )]
  [DataRow ( 4, 0 )]
  public void OffsetCtor ( int offset, int count )
  {
    NoRefList list = new(new string[] { "a", "b", "c", "d", "e", } );

    IListRefSegment<NoRefList> segment = new ( list, offset: offset, count );
    Assert.AreEqual ( count, segment.Count );
    Assert.AreEqual ( SegmentationValidator.LimitOutOf ( offset: offset, count ), segment.limit );
    Assert.AreEqual ( offset, segment.Offset );
    Assert.AreEqual ( offset, segment.offset );
    Assert.AreEqual ( list, segment.list );
    Assert.AreEqual ( list, segment.List );

    Assert.AreSame ( EqualityComparer<object>.Default, segment.EqualityComparer );
  }

  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void OffsetCtor_NullEqualityComparer ( bool passNullExplicitly )
  {
    RefList list = new(new string[] { "a", "b", "c", "d", "e", } );
    const int offset = 0;
    int count = list.Count;

    IListRefSegment<RefList>  segment = passNullExplicitly ? new ( list, offset: offset, count, null ) : new ( list, offset: offset, count );
    Assert.AreSame ( EqualityComparer<object>.Default, segment.EqualityComparer );
  }

  [TestMethod]
  [DataRow ( 3, 3, "List has length 5, given offset 3 and count 3 produces out-of indexing in range 5–5.", DisplayName = "Impossible segmentation, offsetting." )]
  [DataRow ( -1, 0, "Offset must be a non-negative integer, but it is -1.", DisplayName = "Negative offset." )]
  [DataRow ( 0, -1, "Count must be a non-negative integer, but it is -1.", DisplayName = "Negative count." )]
  public void OffsetCtor_InvalidSegmentation ( int offset, int count, string errMsg )
  {
    NoRefList list = new(new string[] { "a", "b", "c", "d", "e", } );
    Action test = () => _ = new IListRefSegment<NoRefList> ( list, offset: offset, count );
    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0017:Simplify object initialization", Justification = "Not particularly useful." )]
  public void EqualityComparer ()
  {
    SixEqualsFiveEqualityComparer comparer = new();
    RefList list = new (new int [0]);
    IListRefSegment<RefList> segment = new( list);
    segment.EqualityComparer = comparer;
    Assert.IsTrue ( ReferenceEquals ( comparer, segment.EqualityComparer ) );
  }

  [TestMethod]
  [DataRow ( 0, 5, 0, "a", DisplayName = "No segmentation, low." )]
  [DataRow ( 0, 5, 4, "e", DisplayName = "No segmentation, high." )]
  [DataRow ( 2, 2, 0, "c", DisplayName = "Segmentation, low." )]
  [DataRow ( 2, 2, 1, "d", DisplayName = "Segmentation, high." )]
  public void Indexer_Get_PositiveScenarios ( int offset, int count, int index, string exp )
  {
    RefList list = new (new string [] { "a", "b", "c", "d", "e" });
    IListRefSegment<RefList> segment = new (list , offset, count: count);
    object? test = segment[index];
    Assert.AreEqual ( exp, test );
  }

  [TestMethod]
  [DataRow ( 0, 5, -1, "Index must be non-negative, but it is -1." )]
  [DataRow ( 0, 5, 5, "Segment length is 5, index 5 is out of its range." )]
  [DataRow ( 2, 2, 2, "Segment length is 2, index 2 is out of its range." )]
  [DataRow ( 0, 0, 0, "Segment length is 0, index 0 is out of its range." )]
  public void Indexer_Get_NegativeScenarios ( int offset, int count, int index, string expMsg )
  {
    RefList list = new (new string [] { "a", "b", "c", "d", "e" });
    IListRefSegment<RefList> segment = new (list, offset, count: count);
    try
    {
      _ = segment [ index ];
    }
    catch (IndexOutOfSegmentException e)
    {
      Assert.AreEqual ( expMsg, e.Message );
    }
  }

  [TestMethod]
  [DataRow ( 0, 5, 0, DisplayName = "No segmentation, low." )]
  [DataRow ( 0, 5, 4, DisplayName = "No segmentation, high." )]
  [DataRow ( 2, 2, 0, DisplayName = "Segmentation, low." )]
  [DataRow ( 2, 2, 1, DisplayName = "Segmentation, high." )]
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Okay." )]
  public void Indexer_Set_PositiveScenarios ( int offset, int count, int index )
  {
    const string val = "z";
    RefList list = new (new string [] { "a", "b", "c", "d", "e" });
    IListRefSegment<RefList> segment = new ( list, offset, count: count );
    segment [ index ] = val;
    Assert.AreEqual ( val, segment [ index ]);
  }

  [TestMethod]
  [DataRow ( 0, 5, -1, "Index must be non-negative, but it is -1." )]
  [DataRow ( 0, 5, 5, "Segment length is 5, index 5 is out of its range." )]
  [DataRow ( 2, 2, 2, "Segment length is 2, index 2 is out of its range." )]
  [DataRow ( 0, 0, 0, "Segment length is 0, index 0 is out of its range." )]
  public void Indexer_Set_NegativeScenarios ( int offset, int count, int index, string expMsg )
  {
    RefList list = new (new string [] { "a", "b", "c", "d", "e" });
    IListRefSegment<RefList> segment = new ( list, offset, count: count);
    try
    {
      segment [ index ] = "";
    }
    catch (IndexOutOfSegmentException e)
    {
      Assert.AreEqual ( expMsg, e.Message );
    }
  }

  [TestMethod]
  [DataRow ( 0, 5, 5, DisplayName = "Full coverage by segment." )]
  [DataRow ( 3, 2, 5, DisplayName = "Segmentation." )]
  [DataRow ( 0, 0, 0, DisplayName = "Empty segment." )]
  [DataRow ( 3, 0, 3, DisplayName = "Empty segment, offsetting" )]
  public void ValidateSetup_PositiveScenario ( int offset, int count, int expLimit )
  {
    RefList list = new (new int [] { 1,2,3,4,5 });
    IListRefSegment<RefList> segment = new (list, offset, count: count);
    bool result = segment.ValidateSetup ( count, out int limit, out ImpossibleSegmentationException? e );
    Assert.IsFalse ( result );
    Assert.AreEqual ( expLimit, limit );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 2, 2, 3, DisplayName = "Segmentation lower." )]
  [DataRow ( 2, 4, 2, 3, DisplayName = "Segmentation upper." )]
  [DataRow ( 0, 0, 0, 5, DisplayName = "Lower." )]
  [DataRow ( 4, 4, 0, 5, DisplayName = "Upper." )]
  public void ValidateIndex_PositiveScenarios ( int index, int computedIndex, int offset, int count )
  {
    RefList list = new (new string [] { "a", "b", "c", "d", "e" });
    IListRefSegment<RefList> segment = new ( list, offset, count: count);
    bool result = segment.ValidateIndex (ref index, out IndexOutOfSegmentException? e );
    Assert.IsFalse ( result );
    Assert.AreEqual ( computedIndex, index );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 5, 5, 0, 5, DisplayName = "Index OOB." )]
  [DataRow ( 1, 3, 2, 1, DisplayName = "Index OOB, segementation." )]
  [DataRow ( 0, 0, 0, 0, DisplayName = "Empty segment." )]
  [DataRow ( -1, -1, 0, 5, DisplayName = "Negative Index." )]
  public void ValidateIndex_NegativeScenarios ( int index, int computedIndex, int offset, int count )
  {
    int origIndex = index;
    RefList list = new (new string [] { "a", "b", "c", "d", "e" });
    IListRefSegment<RefList> segment = new (list, offset, count: count);
    bool result = segment.ValidateIndex (ref index, out IndexOutOfSegmentException? e );
    Assert.IsTrue ( result );
    Assert.AreEqual ( computedIndex, index );
    Assert.IsNotNull ( e );
    string expMsg = index < 0
      ? $"Index must be non-negative, but it is {index}."
      : $"Segment length is {count}, index {origIndex} is out of its range.";
    Assert.AreEqual ( expMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5, new int [] { 0, 0, 0, 0, 0 }, new int [] { 0, 0, 0, 0, 0 }, DisplayName = "No segmentation." )]
  [DataRow ( 2, 2, new int [] { 1, 2, 0, 0, 5 }, new int [] { 0, 0, }, DisplayName = "Segmentation." )]
  [DataRow ( 0, 0, new int [] { 1, 2, 3, 4, 5 }, new int [] { }, DisplayName = "Empty segment." )]
  public void Clear ( int offset, int count, int [] sourceExp, int [] segmentExp )
  {
    RefList source = new(new int[] { 1,2,3,4,5 });
    IListRefSegment<RefList> segment = new ( source, offset, count: count);
    segment.Clear ();
    int index = 0;
    foreach (int exp in sourceExp)
    {
      Assert.AreEqual ( exp, source [ index++ ]);
    }
    index = 0;
    foreach (int exp in segmentExp)
    {
      Assert.AreEqual ( exp, segment [ index++ ]);
    }
  }

  [TestMethod]
  [DataRow ( 0, 5, 5, true, DisplayName = "No segmentation, contains." )]
  [DataRow ( 0, 5, 0, false, DisplayName = "No segmentation, does not contain." )]
  [DataRow ( 2, 2, 4, true, DisplayName = "Segmentation, contains." )]
  [DataRow ( 2, 2, 5, false, DisplayName = "Segmentation, does not contain." )]
  [DataRow ( 0, 0, 5, false, DisplayName = "Empty segment." )]
  public void Contains_DefaultEqualityComparer ( int offset, int count, int value, bool contains )
  {
    RefList list = new(new int[] { 1,2,3,4,5 });
    IListRefSegment<RefList> segment = new (list, offset, count: count);
    bool result = segment.Contains(value);
    Assert.AreEqual ( contains, result );
  }

  sealed class SixEqualsFiveEqualityComparer : IEqualityComparer<object>
  {
    bool IEqualityComparer<object>.Equals ( object? x, object? y )
    {
      if (x is int xi && y is int yi)
      {
        if ((xi, yi) == (6, 5) || (xi, yi) == (5, 6))
        {
          return true;
        }
      }

      return x == y;
    }
    [SuppressMessage ( "Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "!" )]
    int IEqualityComparer<object>.GetHashCode ( object? obj ) => throw new NotImplementedException ();
  }

  [TestMethod]
  public void Contains_CustomEqualityComparer ()
  {
    RefList list = new(new int[] { 1,2,3,4,5 });
    IListRefSegment<RefList> segment = new ( list, new SixEqualsFiveEqualityComparer());
    bool result = segment.Contains(6);
    Assert.IsTrue ( result );
  }

  [TestMethod]
  public void CopyTo_NullArray ()
  {
    RefList list = new(new int[0]);
    IListRefSegment<RefList> segment = new (list );
    try
    {
      segment.CopyTo ( null!, 0 );
    }
    catch (ArgumentNullException e)
    {
      Assert.AreEqual ( "Value cannot be null. (Parameter 'array')", e.Message );
    }
  }

  [TestMethod]
  public void CopyTo_NegativeIndex ()
  {
    RefList list = new(new int[0]);
    IListRefSegment<RefList> segment = new ( list );
    try
    {
      segment.CopyTo ( new int [ 0 ], -1 );
    }
    catch (ArgumentOutOfRangeException e)
    {
      Assert.AreEqual ( "Index must be non-negative. (Parameter 'index')\r\nActual value was -1.", e.Message );
    }
  }

  [TestMethod]
  [DataRow ( 0, 5, 0, 4 )]
  [DataRow ( 0, 5, 1, 5 )]
  [DataRow ( 1, 3, 0, 2 )]
  [DataRow ( 1, 3, 1, 3 )]
  [DataRow ( 0, 1, 0, 0 )]
  public void CopyTo_InsufficientArrayLenght ( int offset, int count, int startingIndex, int arrayLength )
  {
    RefList list = new(new int [] { 1,2,3,4,5 });
    IListRefSegment<RefList> segment = new (list , offset, count: count);

    try
    {
      segment.CopyTo ( new int [ arrayLength ], startingIndex );
    }
    catch (ArgumentException e)
    {
      string expMsg = $"Array length of {arrayLength} is insufficient, starting index {startingIndex}, segement length {segment.Count}. (Parameter 'array')";
      Assert.AreEqual ( expMsg, e.Message );
    }
  }

  [TestMethod]
  [DataRow ( 0, 5, 0, 6, new [] { 1, 2, 3, 4, 5, 0 } )]
  [DataRow ( 0, 5, 1, 6, new [] { 0, 1, 2, 3, 4, 5 } )]
  [DataRow ( 1, 3, 0, 4, new [] { 2, 3, 4, 0 } )]
  [DataRow ( 1, 3, 1, 4, new [] { 0, 2, 3, 4 } )]
  [DataRow ( 0, 0, 0, 2, new [] { 0, 0 } )]
  [DataRow ( 0, 0, 1, 2, new [] { 0, 0 } )]
  public void CopyTo_SufficientArrayLenght ( int offset, int count, int startingIndex, int arrayLength, int [] expResult )
  {
    RefList list = new(new int [] { 1,2,3,4,5 });
    IListRefSegment<RefList> segment = new ( list, offset, count: count);
    int [] test = new int [ arrayLength ];
    segment.CopyTo ( test, startingIndex );

    Assert.IsTrue ( expResult.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 0, 5, 5, 4 )]
  [DataRow ( 0, 5, 1, 0 )]
  [DataRow ( 0, 5, 0, -1 )]
  [DataRow ( 2, 2, 4, 1 )]
  [DataRow ( 2, 2, 3, 0 )]
  [DataRow ( 2, 2, 5, -1 )]
  [DataRow ( 0, 0, 5, -1 )]
  public void IndexOf_DefaultEqualityComparer ( int offset, int count, int value, int index )
  {
    RefList list = new(new int [] { 1,2,3,4,5 });
    IListRefSegment<RefList> segment = new ( list, offset, count: count);
    int result = segment.IndexOf(value);
    Assert.AreEqual ( index, result );
  }

  [TestMethod]
  public void IndexOf_CustomEqualityComparer ()
  {
    RefList list = new(new int [] { 1,2,3,4,5 });
    IListRefSegment<RefList> segment = new (list, new SixEqualsFiveEqualityComparer());
    int result = segment.IndexOf(6);
    Assert.AreEqual ( 4, result );
  }

  [TestMethod]
  [DataRow ( 0, 5, new [] { 1, 2, 3, 4, 5, } )]
  [DataRow ( 1, 3, new [] { 2, 3, 4, } )]
  [DataRow ( 0, 0, new int [] { } )]
  [DataRow ( 3, 0, new int [] { } )]
  public void GetRefEnumerator ( int offset, int count, int [] expResult )
  {
    RefList list = new(new int [] { 1,2,3,4,5 });
    IListRefSegment<RefList> segment = new ( list, offset, count: count);
    IListRefEnumerator<RefList> enumerator = segment.GetRefEnumerator();
    List<int> test = [];
    while (enumerator.MoveNext ()) { test.Add ( (int) enumerator.Current! ); }
    Assert.IsTrue ( expResult.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void EqualsObject ()
  {
    RefList list = new(new int [0]);
    IListRefSegment<RefList> segment = new (list );
    Assert.IsFalse ( segment.Equals ( null ) );
    Assert.IsFalse ( segment.Equals ( new object () ) );
  }
}
