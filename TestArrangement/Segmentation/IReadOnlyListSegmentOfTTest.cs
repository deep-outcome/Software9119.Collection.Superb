using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.Segmentation.Exceptionality;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class IReadOnlyListSegmentOfTTest
{
  static public IEnumerable<object []> NullListCtors ()
  {
    yield return new Func<object> [] { () => new IReadOnlyListSegment<string> ( null! ) };
    yield return new Func<object> [] { () => new IReadOnlyListSegment<string> ( null!, 0, 0 ) };
  }

  [TestMethod]
  [DynamicData ( nameof ( NullListCtors ), DynamicDataSourceType.Method )]
  public void NullList ( Func<object> test )
  {
    ArgumentNullException ae = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Null list provided. (Parameter 'list')", ae.Message );
  }

  [TestMethod]
  public void DefaultCtor ()
  {
    List<string?> list = ["a", "b", "c", "d", "e",];

    IReadOnlyListSegment<string> segment = new ( list );
    Assert.HasCount ( list.Count, segment );
    Assert.AreEqual ( 0, segment.Offset );
    Assert.AreSame ( list, segment.List );

    Assert.AreSame ( EqualityComparer<string>.Default, segment.EqualityComparer );
  }

  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void DefaultCtor_NullEqualityComparer ( bool passNullExplicitly )
  {
    List<string?> list = ["a", "b", "c", "d", "e",];

    IReadOnlyListSegment<string> segment = passNullExplicitly ? new ( list, null ) : new ( list );
    Assert.AreSame ( EqualityComparer<string>.Default, segment.EqualityComparer );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 4, 1 )]
  [DataRow ( 1, 3 )]
  [DataRow ( 0, 0 )]
  [DataRow ( 4, 0 )]
  public void OffsetCtor ( int offset, int count )
  {
    List<string?> list = ["a", "b", "c", "d", "e",];

    IReadOnlyListSegment<string> segment = new ( list, offset: offset, count );
    Assert.HasCount ( count, segment );
    Assert.AreEqual ( offset, segment.Offset );
    Assert.AreSame ( list, segment.List );

    Assert.AreSame ( EqualityComparer<string>.Default, segment.EqualityComparer );
  }

  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void OffsetCtor_NullEqualityComparer ( bool passNullExplicitly )
  {
    List<string?> list = ["a", "b", "c", "d", "e",];
    const int offset = 0;
    int count = list.Count;

    IReadOnlyListSegment<string>  segment = passNullExplicitly ? new ( list, offset: offset, count, null ) : new ( list, offset: offset, count );
    Assert.AreSame ( EqualityComparer<string>.Default, segment.EqualityComparer );
  }

  [TestMethod]
  [DataRow ( 3, 3, "List has length 5, given offset 3 and count 3 produces out-of indexing in range 5–5.", DisplayName = "Impossible segmentation, offsetting." )]
  [DataRow ( -1, 0, "Offset must be a non-negative integer, but it is -1.", DisplayName = "Negative offset." )]
  [DataRow ( 0, -1, "Count must be a non-negative integer, but it is -1.", DisplayName = "Negative count." )]
  public void OffsetCtor_InvalidSegmentation ( int offset, int count, string errMsg )
  {
    List<string?> list = ["a", "b", "c", "d", "e",];

    Func<object> test =() => new IReadOnlyListSegment<string> ( list, offset: offset, count );
    ImpossibleSegmentationException ise = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );

    Assert.AreEqual ( errMsg, ise.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0017:Simplify object initialization", Justification = "Not particularly useful." )]
  public void EqualityComparer ()
  {
    IReadOnlyListSegmentEqualityComparer<int> comparer = new();
    IReadOnlyListSegment<IReadOnlyListSegment<int>> segment = new( [] );
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
    IReadOnlyListSegment<string> segment = new ( ["a", "b", "c", "d", "e"], offset, count: count);
    string? test = segment[index];
    Assert.AreEqual ( exp, test );
  }

  [TestMethod]
  [DataRow ( 0, 5, -1, "Index must be non-negative, but it is -1." )]
  [DataRow ( 0, 5, 5, "Segment length is 5, index 5 is out of its range." )]
  [DataRow ( 2, 2, 2, "Segment length is 2, index 2 is out of its range." )]
  [DataRow ( 0, 0, 0, "Segment length is 0, index 0 is out of its range." )]
  public void Indexer_Get_NegativeScenarios ( int offset, int count, int index, string expMsg )
  {
    IReadOnlyListSegment<string> segment = new ( ["a", "b", "c", "d", "e"], offset, count: count);
    Func<object> test = () => segment[index]!;
    IndexOutOfSegmentException e = Assert.ThrowsExactly<IndexOutOfSegmentException> ( test );
    Assert.AreEqual ( expMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5, 5, DisplayName = "Full coverage by segment." )]
  [DataRow ( 3, 2, 5, DisplayName = "Segmentation." )]
  [DataRow ( 0, 0, 0, DisplayName = "Empty segment." )]
  [DataRow ( 3, 0, 3, DisplayName = "Empty segment, offsetting" )]
  public void ValidateSetup_PositiveScenario ( int offset, int count, int expLimit )
  {
    IReadOnlyListSegment<int> segment = new ( [1,2,3,4,5], offset, count: count);
    bool result = segment.ValidateSetup ( count, out int limit, out ImpossibleSegmentationException? e );
    Assert.IsFalse ( result );
    Assert.AreEqual ( expLimit, limit );
    Assert.IsNull ( e );
  }

  [TestMethod]
  public void ValidateList_PositiveScenario ()
  {
    IReadOnlyListSegment<int> segment = new ( new int[0], 0, 0);

    bool result = segment.ValidateList ( out ArgumentNullException? e );
    Assert.IsFalse ( result );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 2, 2, 3, DisplayName = "Segmentation lower." )]
  [DataRow ( 2, 4, 2, 3, DisplayName = "Segmentation upper." )]
  [DataRow ( 0, 0, 0, 5, DisplayName = "Lower." )]
  [DataRow ( 4, 4, 0, 5, DisplayName = "Upper." )]
  public void ValidateIndex_PositiveScenarios ( int index, int computedIndex, int offset, int count )
  {
    IReadOnlyListSegment<string> segment = new ( ["a", "b", "c", "d", "e"], offset, count: count);
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
    IReadOnlyListSegment<string> segment = new ( ["a", "b", "c", "d", "e"], offset, count: count);
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
  [DataRow ( 0, 5, 5, true, DisplayName = "No segmentation, contains." )]
  [DataRow ( 0, 5, 0, false, DisplayName = "No segmentation, does not contain." )]
  [DataRow ( 2, 2, 4, true, DisplayName = "Segmentation, contains." )]
  [DataRow ( 2, 2, 5, false, DisplayName = "Segmentation, does not contain." )]
  [DataRow ( 0, 0, 5, false, DisplayName = "Empty segment." )]
  public void Contains_DefaultEqualityComparer ( int offset, int count, int value, bool contains )
  {
    IReadOnlyListSegment<int> segment = new ( [1,2,3,4,5], offset, count: count);
    bool result = segment.Contains(value);
    Assert.AreEqual ( contains, result );
  }

  sealed class SixEqualsFiveEqualityComparer : IEqualityComparer<int>
  {
    bool IEqualityComparer<int>.Equals ( int x, int y )
    {
      if ((x, y) == (6, 5) || (x, y) == (5, 6))
        return true;

      return x == y;
    }
    [SuppressMessage ( "Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "!" )]
    int IEqualityComparer<int>.GetHashCode ( int obj ) => throw new NotImplementedException ();
  }

  [TestMethod]
  public void Contains_CustomEqualityComparer ()
  {
    IReadOnlyListSegment<int> segment = new ( [1,2,3,4,5], new SixEqualsFiveEqualityComparer());
    bool result = segment.Contains(6);
    Assert.IsTrue ( result );
  }

  [TestMethod]
  public void CopyTo_NullArray ()
  {
    IReadOnlyListSegment<int> segment = new ( [] );
    Action test = () => segment.CopyTo(null!, 0);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Value cannot be null. (Parameter 'array')", e.Message );
  }

  [TestMethod]
  public void CopyTo_NegativeIndex ()
  {
    IReadOnlyListSegment<int> segment = new ( [] );
    Action test = () => segment.CopyTo([], -1);
    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    Assert.AreEqual ( "Index must be non-negative. (Parameter 'arrayIndex')\r\nActual value was -1.", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5, 0, 4 )]
  [DataRow ( 0, 5, 1, 5 )]
  [DataRow ( 1, 3, 0, 2 )]
  [DataRow ( 1, 3, 1, 3 )]
  [DataRow ( 0, 1, 0, 0 )]
  public void CopyTo_InsufficientArrayLenght ( int offset, int count, int startingIndex, int arrayLength )
  {
    IReadOnlyListSegment<int> segment = new ( [1,2,3,4,5], offset, count: count);
    Action test = () => segment.CopyTo(new int[arrayLength], startingIndex);
    ArgumentException e = Assert.ThrowsExactly<ArgumentException>(test);
    string expMsg = $"Array length of {arrayLength} is insufficient, starting index {startingIndex}, segement length {segment.Count}. (Parameter 'array')";
    Assert.AreEqual ( expMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5, 0, 6, new [] { 1, 2, 3, 4, 5, 0 } )]
  [DataRow ( 0, 5, 1, 6, new [] { 0, 1, 2, 3, 4, 5 } )]
  [DataRow ( 1, 3, 0, 4, new [] { 2, 3, 4, 0 } )]
  [DataRow ( 1, 3, 1, 4, new [] { 0, 2, 3, 4 } )]
  [DataRow ( 0, 0, 0, 2, new [] { 0, 0 } )]
  [DataRow ( 0, 0, 1, 2, new [] { 0, 0 } )]
  public void CopyTo_SufficientArrayLenght_Array ( int offset, int count, int startingIndex, int arrayLength, int [] expResult )
  {

    IReadOnlyListSegment<int> segment = new ( new int []  { 1,2,3,4,5 }, offset, count: count);
    int [] test = new int [ arrayLength ];
    segment.CopyTo ( test, startingIndex );

    Assert.IsTrue ( expResult.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 0, 5, 0, 6, new [] { 1, 2, 3, 4, 5, 0 } )]
  [DataRow ( 0, 5, 1, 6, new [] { 0, 1, 2, 3, 4, 5 } )]
  [DataRow ( 1, 3, 0, 4, new [] { 2, 3, 4, 0 } )]
  [DataRow ( 1, 3, 1, 4, new [] { 0, 2, 3, 4 } )]
  [DataRow ( 0, 0, 0, 2, new [] { 0, 0 } )]
  [DataRow ( 0, 0, 1, 2, new [] { 0, 0 } )]
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  public void CopyTo_SufficientArrayLenght_List ( int offset, int count, int startingIndex, int arrayLength, int [] expResult )
  {

    IReadOnlyListSegment<int> segment = new ( new List<int> { 1,2,3,4,5 }, offset, count: count);
    int [] test = new int [ arrayLength ];
    segment.CopyTo ( test, startingIndex );

    Assert.IsTrue ( expResult.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 0, 5, 0, 6, new [] { 1, 2, 3, 4, 5, 0 } )]
  [DataRow ( 0, 5, 1, 6, new [] { 0, 1, 2, 3, 4, 5 } )]
  [DataRow ( 1, 3, 0, 4, new [] { 2, 3, 4, 0 } )]
  [DataRow ( 1, 3, 1, 4, new [] { 0, 2, 3, 4 } )]
  [DataRow ( 0, 0, 0, 2, new [] { 0, 0 } )]
  [DataRow ( 0, 0, 1, 2, new [] { 0, 0 } )]
  public void CopyTo_SufficientArrayLenght_OtherCollection ( int offset, int count, int startingIndex, int arrayLength, int [] expResult )
  {
    ImmutableArray<int> source = ImmutableArray.Create([1,2,3,4,5]);

    IReadOnlyListSegment<int> segment = new (source, offset, count: count);
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
    IReadOnlyListSegment<int> segment = new ( [1,2,3,4,5], offset, count: count);
    int result = segment.IndexOf(value);
    Assert.AreEqual ( index, result );
  }

  [TestMethod]
  public void IndexOf_CustomEqualityComparer ()
  {
    IReadOnlyListSegment<int> segment = new ( [1,2,3,4,5], new SixEqualsFiveEqualityComparer());
    int result = segment.IndexOf(6);
    Assert.AreEqual ( 4, result );
  }

  [TestMethod]
  [DataRow ( 0, 5, new [] { 1, 2, 3, 4, 5, } )]
  [DataRow ( 1, 3, new [] { 2, 3, 4, } )]
  [DataRow ( 0, 0, new int [] { } )]
  [DataRow ( 3, 0, new int [] { } )]
  public void IEnumerableGetEnumerator ( int offset, int count, int [] expResult )
  {
    IReadOnlyListSegment<int> segment = new ( [1,2,3,4,5], offset, count: count);
    IEnumerator enumerator = ((IEnumerable)segment).GetEnumerator();
    List<int> test = [];
    while (enumerator.MoveNext ()) { test.Add ( (int) enumerator.Current ); }
    Assert.IsTrue ( expResult.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 0, 5, new [] { 1, 2, 3, 4, 5, } )]
  [DataRow ( 1, 3, new [] { 2, 3, 4, } )]
  [DataRow ( 0, 0, new int [] { } )]
  [DataRow ( 3, 0, new int [] { } )]
  public void IEnumerableOfTGetEnumerator ( int offset, int count, int [] expResult )
  {
    IReadOnlyListSegment<int> segment = new ( [1,2,3,4,5], offset, count: count);
    IEnumerator<int> enumerator = segment.GetEnumerator();
    List<int> test = [];
    while (enumerator.MoveNext ()) { test.Add ( enumerator.Current ); }
    Assert.IsTrue ( expResult.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void EqualsObject ()
  {
    IReadOnlyListSegment<int> segment = new ( []);
    Assert.IsTrue ( segment.Equals ( (object)segment ) );
    Assert.IsFalse ( segment.Equals ( null ) );
    Assert.IsFalse ( segment.Equals ( new object () ) );
  }

  [TestMethod]
  public void Equals_Equals ()
  {
    IReadOnlyListSegment<int> segment = new ( []);
    Assert.IsTrue ( segment.Equals ( segment ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Reference ()
  {
    IReadOnlyListSegment<int> i = new ( [1]);
    IReadOnlyListSegment<int> you = new ([1]);
    Assert.IsFalse ( i.Equals ( you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Offset ()
  {
    int[] source = [1,2, 3];
    IReadOnlyListSegment<int> i = new ( source, 0, 2);
    IReadOnlyListSegment<int> you = new (source, 1, 2);
    Assert.IsFalse ( i.Equals ( you ) );
  }

  [TestMethod]
  public void Equals_NotEqual_Count ()
  {
    int[] source = [1,2, 3];
    IReadOnlyListSegment<int> i = new ( source, 1, 1);
    IReadOnlyListSegment<int> you = new (source, 1, 2);
    Assert.IsFalse ( i.Equals ( you ) );
  }

  [TestMethod]
  public void EqualOperator ()
  {
    IReadOnlyListSegment<int> segment = new ( new int [0]);
#pragma warning disable CS1718 // Comparison made to same variable
    Assert.IsTrue ( segment == segment );
#pragma warning restore CS1718 // Comparison made to same variable

    IReadOnlyListSegment<int> other = new ( new int [0]);
    Assert.IsFalse ( segment == other );
  }

  [TestMethod]
  public void NotEqualOperator ()
  {
    IReadOnlyListSegment<int> segment = new ( new int [0]);
#pragma warning disable CS1718 // Comparison made to same variable
    Assert.IsFalse ( segment != segment );
#pragma warning restore CS1718 // Comparison made to same variable

    IReadOnlyListSegment<int> other = new (  new int [0]);
    Assert.IsTrue ( segment != other );
  }
}
