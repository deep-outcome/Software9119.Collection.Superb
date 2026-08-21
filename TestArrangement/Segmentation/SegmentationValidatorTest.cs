using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.Segmentation.Exceptionality;

using System;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class SegmentationValidatorTest
{
  [TestMethod]
  public void LimitOutOf ()
  {
    Assert.AreEqual ( 8, SegmentationValidator.LimitOutOf ( 3, 5 ) );
  }

  [TestMethod]
  [DataRow ( 3, 3, 6, "List has length 5, given offset 3 and count 3 produces out-of indexing in range 5–5.", DisplayName = "Impossible segmentation, offsetting." )]
  [DataRow ( 3, 5, 8, "List has length 5, given offset 3 and count 5 produces out-of indexing in range 5–7.", DisplayName = "Impossible segmentation, offsetting, range." )]
  [DataRow ( 0, 6, 6, "List has length 5, given offset 0 and count 6 produces out-of indexing in range 5–5.", DisplayName = "Impossible segmentation." )]
  [DataRow ( 0, 7, 7, "List has length 5, given offset 0 and count 7 produces out-of indexing in range 5–6.", DisplayName = "Impossible segmentation, range." )]
  [DataRow ( -1, 0, -1, "Offset must be a non-negative integer, but it is -1.", DisplayName = "Negative offset." )]
  [DataRow ( 0, -1, -1, "Count must be a non-negative integer, but it is -1.", DisplayName = "Negative count." )]
  [DataRow ( -1, -1, -2, "Offset must be a non-negative integer, but it is -1.", DisplayName = "Negative count and negative offset." )]
  public void ValidateSetup_NegativeScenarios ( int offset, int count, int expLimit, string errMsg )
  {
    bool result = SegmentationValidator.ValidateSetup ( 5, offset, count: count, out int limit, out ImpossibleSegmentationException? e);
    Assert.IsTrue ( result );
    Assert.AreEqual ( expLimit, limit );
    Assert.AreEqual ( errMsg, e!.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5, 5, DisplayName = "Full coverage by segment." )]
  [DataRow ( 0, 2, 2, DisplayName = "Segmentation, start." )]
  [DataRow ( 3, 2, 5, DisplayName = "Segmentation, end." )]
  [DataRow ( 2, 2, 4, DisplayName = "Segmentation, middle." )]
  [DataRow ( 0, 0, 0, DisplayName = "Empty segment." )]
  [DataRow ( 3, 0, 3, DisplayName = "Empty segment, offsetting" )]
  public void ValidateSetup_PositiveScenarios ( int offset, int count, int expLimit )
  {
    bool result = SegmentationValidator.ValidateSetup ( 5, offset, count: count, out int limit, out ImpossibleSegmentationException? e);
    Assert.IsFalse ( result );
    Assert.AreEqual ( expLimit, limit );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void ValidateList ( bool nullList )
  {
    int[]? list = nullList ? null : [];

    bool result = SegmentationValidator.ValidateList ( list, out ArgumentNullException? e);
    Assert.AreEqual ( nullList, result );
    Assert.AreEqual ( nullList, e is not null );

    string expMsg = nullList ? "Null list provided. (Parameter 'list')" : "";
    Assert.AreEqual ( expMsg, e?.Message ?? "" );
  }

  [TestMethod]
  [DataRow ( 2, 3, 1, 2, DisplayName = "Offsetting, index out of bounds." )]
  [DataRow ( 5, 5, 0, 5, DisplayName = "No offset, index out of bounds" )]
  [DataRow ( 0, 0, 0, 0, DisplayName = "Empty segment, 0 index." )]
  [DataRow ( 1, 2, 1, 0, DisplayName = "Empty segment, other index." )]
  [DataRow ( -3, -3, 1, 4, DisplayName = "Negative index." )]
  [DataRow ( -1, -1, 0, 0, DisplayName = "Empty segment, negative index." )]
  public void ValidateIndex_NegativeScenarios ( int index, int computedIndex, int offset, int count )
  {
    int origIndex = index;
    IListSegment<int> segment = new ([1,2,3,4,5], offset, count);

    bool result = SegmentationValidator.ValidateIndex ( ref index, in segment, out IndexOutOfSegmentException? e);

    Assert.IsTrue ( result );
    Assert.AreEqual ( computedIndex, index );
    Assert.IsNotNull ( e );

    string expMsg = index < 0
      ? $"Index must be non-negative, but it is {index}."
      : $"Segment length is {count}, index {origIndex} is out of its range.";
    Assert.AreEqual ( expMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1, 1, 2 )]
  [DataRow ( 1, 2, 1, 2 )]
  [DataRow ( 0, 0, 0, 5 )]  
  public void ValidateIndex_PositiveScenarios ( int index, int computedIndex, int offset, int count )
  {
    IListSegment<int> segment = new ([1,2,3,4,5], offset, count);
    bool result = SegmentationValidator.ValidateIndex ( ref index, in segment, out IndexOutOfSegmentException? e);
    Assert.IsFalse ( result );
    Assert.AreEqual ( computedIndex, index );
    Assert.IsNull ( e );
  }
}
