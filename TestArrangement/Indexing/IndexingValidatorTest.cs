using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Software9119.Collection.Superb.TestArrangement.Indexing;

[TestClass]
public class IndexingValidatorTest
{
  [TestMethod]
  public void LimitOutOf ()
  {
    Assert.AreEqual ( 8, IndexingValidator.LimitOutOf ( (NonNegativeInt32) 3, (NonNegativeInt32) 5 ) );
  }

  [TestMethod]
  public void CorrelateIndex ()
  {
    Assert.AreEqual ( 8, IndexingValidator.CorrelateIndex ( (NonNegativeInt32) 3, (NonNegativeInt32) 5 ) );
  }

  [TestMethod]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 0, 1, 1 )]
  [DataRow ( 1, 3, 2 )]
  public void IndexToCountInclusiveDifference ( int from, int count, int diff )
  {
    Assert.AreEqual ( diff, IndexingValidator.IndexToCountInclusiveDifference ( from, count ) );
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( int.MaxValue, int.MaxValue )]
  [DataRow ( 1, 0 )]
  [DataRow ( 5, 4 )]
  public void ValidateIndex_NegativeScenarios ( int index, int count )
  {
    NonNegativeInt32 i = index;
    NonNegativeInt32 c = count;

    bool result = IndexingValidator.ValidateIndex (index: i, c, out IndexOutOfBoundariesException? e);
    Assert.IsTrue ( result );
    Assert.IsNotNull ( e );

    string msg =  $"For available '{count}' is index '{index}' out of bounds.";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( "implicit" )]
  [DataRow ( null )]
  [DataRow ( "" )]
  [DataRow ( "yourParam" )]
  [SuppressMessage ( "Style", "IDE0018:Inline variable declaration", Justification = "No." )]
  public void ValidateIndex_NegativeScenarios_Parameters ( string? paramName )
  {
    IndexOutOfBoundariesException? e;
    _ = paramName == "implicit"
      ? IndexingValidator.ValidateIndex ( index: (NonNegativeInt32) 0, (NonNegativeInt32) 0, out e )
      : IndexingValidator.ValidateIndex ( index: (NonNegativeInt32) 0, (NonNegativeInt32) 0, out e, paramName );

    string msg =  $"For available '0' is index '0' out of bounds.";
    string paramString = paramName == "yourParam" ? " (Parameter 'yourParam')" : "";
    msg += paramString;

    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1 )]
  [DataRow ( 1, 2 )]
  [DataRow ( 5, 8 )]
  public void ValidateIndex_PositiveScenarios ( int index, int count )
  {
    NonNegativeInt32 i = index;
    NonNegativeInt32 c = count;

    bool result = IndexingValidator.ValidateIndex (index: i, c, out IndexOutOfBoundariesException? e);
    Assert.IsFalse ( result );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 0, "For available '0' is index '0' out of bounds." )]
  [DataRow ( int.MaxValue, int.MaxValue, "For available '2147483647' is index '2147483647' out of bounds." )]
  [DataRow ( 1, 0, "For available '0' is index '1' out of bounds." )]
  [DataRow ( 5, 4, "For available '4' is index '5' out of bounds." )]
  [DataRow ( -1, default, "Index must be non-negative, but it is '-1'." )]
  [DataRow ( int.MinValue, default, "Index must be non-negative, but it is '-2147483648'." )]
  public void ValidateIndex_Int32Index_NegativeScenarios ( int index, int count, string errMsg )
  {
    bool result = IndexingValidator.ValidateIndex (index, count, out IndexOutOfBoundariesException? e);
    Assert.IsTrue ( result );
    Assert.IsNotNull ( e );

    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 0, "implicit" )]
  [DataRow ( 0, 0, null )]
  [DataRow ( 0, 0, "" )]
  [DataRow ( 0, 0, "yourParam" )]
  [DataRow ( -1, 0, "implicit" )]
  [DataRow ( -1, 0, null )]
  [DataRow ( -1, 0, "" )]
  [DataRow ( -1, 0, "yourParam" )]
  [SuppressMessage ( "Style", "IDE0018:Inline variable declaration", Justification = "No." )]
  public void ValidateIndex_Int32Index_NegativeScenarios_ParameterName ( int index, int count, string? paramName )
  {
    IndexOutOfBoundariesException? e;
    _ = paramName == "implicit"
      ? IndexingValidator.ValidateIndex ( index, count, out e )
      : IndexingValidator.ValidateIndex ( index, count, out e, paramName );

    string errMsg = index == -1
      ? "Index must be non-negative, but it is '-1'."
      : "For available '0' is index '0' out of bounds.";

    string paramString = paramName == "yourParam" ? " (Parameter 'yourParam')" : "";
    errMsg += paramString;

    Assert.AreEqual ( errMsg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1 )]
  [DataRow ( 1, 2 )]
  [DataRow ( 5, 8 )]
  public void ValidateIndex_Int32Index_PositiveScenarios ( int index, int count )
  {
    bool result = IndexingValidator.ValidateIndex (index, count, out IndexOutOfBoundariesException? e);
    Assert.IsFalse ( result );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 2, 1, 2, DisplayName = "Offsetting, index out of bounds." )]
  [DataRow ( 5, 0, 5, DisplayName = "No offset, index out of bounds" )]
  [DataRow ( 0, 0, 0, DisplayName = "Empty segment, 0 index." )]
  [DataRow ( 1, 1, 0, DisplayName = "Empty segment, other index." )]
  [DataRow ( -3, 1, 4, DisplayName = "Negative index." )]
  [DataRow ( -1, 0, 0, DisplayName = "Empty segment, negative index." )]
  public void ValidateIndexRef_NegativeScenarios ( int index, int offset, int count )
  {
    int origIndex = index;
    bool result = IndexingValidator.ValidateIndex
    (
      ref index, offset: offset, count,
      out IndexOutOfBoundariesException? e
    );

    Assert.IsTrue ( result );
    Assert.AreEqual ( origIndex, index );
    Assert.IsNotNull ( e );

    string expMsg = index < 0
      ? $"Index must be non-negative, but it is '{index}'."
      : $"For available '{count}' is index '{origIndex}' out of bounds.";
    Assert.AreEqual ( expMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1, 1, 2 )]
  [DataRow ( 1, 2, 1, 2 )]
  [DataRow ( 0, 0, 0, 1 )]
  [DataRow ( 4, 4, 0, 5 )]
  public void ValidateIndexRef_PositiveScenarios ( int index, int computedIndex, int offset, int count )
  {
    bool result = IndexingValidator.ValidateIndex
    (
      ref index, offset: offset, count,
      out IndexOutOfBoundariesException? e
    );
    Assert.IsFalse ( result );
    Assert.AreEqual ( computedIndex, index );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 3, 3, 5, DisplayName = "Impossible segmentation, offsetting." )]
  [DataRow ( 3, 5, 5, DisplayName = "Impossible segmentation, offsetting, range." )]
  [DataRow ( 0, 6, 5, DisplayName = "Impossible segmentation." )]
  [DataRow ( 0, 7, 5, DisplayName = "Impossible segmentation, range." )]
  [DataRow ( 0, 1, 0, DisplayName = "Impossible segmentation, nothing available." )]
  [DataRow ( 1, 1, 0, DisplayName = "Impossible segmentation, nothing available." )]
  [DataRow ( 1, 0, 0, DisplayName = "Empty segment, large offsetting" )]
  [DataRow ( 6, 0, 5, DisplayName = "Empty segment, large offsetting" )]
  public void ValidateSegmentation_ImpossibleSegment ( int offset, int count, int available )
  {
    int result = IndexingValidator.ValidateSegmentation
    (
      available: available, offset, count: count, out int limit,
      out ImpossibleSegmentationException? e
    );

    Assert.AreEqual ( 1, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    string errMsg = $"With available {available}, given offset {offset} and count {count} produce out-of indexing.";
    Assert.AreEqual ( errMsg, e!.Message );
  }

  [TestMethod]
  [DataRow ( null )]
  [DataRow ( false )]
  [DataRow ( true )]
  public void ValidateSegmentation_ImpossibleSegment_Parameters ( bool? withParamaters )
  {
    string[]? parameters = withParamaters == null
      ? null
      : withParamaters == true
        ? ["ABC", "xYz"]
        : [];

    _ = IndexingValidator.ValidateSegmentation ( 5, 0, 6, out _, out ImpSegExc? e, parameters );

    string msg = "With available 5, given offset 0 and count 6 produce out-of indexing.{0}";
    string parametersString = withParamaters == true ? " (Parameters 'ABC','xYz')" : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, parametersString );

    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DataRow ( -1, 0, 0, DisplayName = "Negative available." )]
  [DataRow ( 0, -1, 0, DisplayName = "Negative offset." )]
  [DataRow ( 0, 0, -1, DisplayName = "Negative count." )]
  public void ValidateSegmentation_NegativeValues ( int available, int offset, int count )
  {
    Action test = () => _ = IndexingValidator.ValidateSegmentation ( available, offset: offset, count: count, out _, out _);
    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException>( test );
    Assert.AreEqual ( "Value must be non-negative integer, but it is '-1'.", e?.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5, 5, DisplayName = "Full coverage by segment." )]
  [DataRow ( 0, 2, 5, DisplayName = "Segmentation, start." )]
  [DataRow ( 3, 2, 5, DisplayName = "Segmentation, end." )]
  [DataRow ( 1, 3, 5, DisplayName = "Segmentation, middle." )]
  [DataRow ( 0, 0, 0, DisplayName = "Empty segment, empty source." )]
  [DataRow ( 4, 0, 5, DisplayName = "Empty segment, offsetting" )]
  [DataRow ( 5, 0, 5, DisplayName = "Empty segment, false allowance" )]
  public void ValidateSegmentation_PositiveScenarios ( int offset, int count, int available )
  {
    int result = IndexingValidator.ValidateSegmentation
    (
      available, offset, count: count, out int limit,
      out ImpossibleSegmentationException? e
    );

    Assert.AreEqual ( count == 0 ? -1 : 0, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 3, 3, 5, DisplayName = "Impossible segmentation, offsetting." )]
  [DataRow ( 3, 5, 5, DisplayName = "Impossible segmentation, offsetting, range." )]
  [DataRow ( 0, 6, 5, DisplayName = "Impossible segmentation." )]
  [DataRow ( 0, 7, 5, DisplayName = "Impossible segmentation, range." )]
  [DataRow ( 0, 1, 0, DisplayName = "Impossible segmentation, nothing available." )]
  [DataRow ( 1, 1, 0, DisplayName = "Impossible segmentation, nothing available." )]
  [DataRow ( 1, 0, 0, DisplayName = "Empty segment, index not less" )]
  [DataRow ( 5, 0, 5, DisplayName = "Empty segment, index not less" )]
  [DataRow ( 6, 0, 5, DisplayName = "Empty segment, index not less" )]
  public void ValidateSegmentationStrict_ImpossibleSegment ( int offset, int count, int available )
  {
    int result = IndexingValidator.ValidateSegmentationStrict
    (
      available: available, offset, count: count, out int limit,
      out ImpossibleSegmentationException? e
    );

    Assert.AreEqual ( 1, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    string errMsg = $"With available {available}, given offset {offset} and count {count} produce out-of indexing.";
    Assert.AreEqual ( errMsg, e!.Message );
  }

  [TestMethod]
  [DataRow ( -1, 0, 0, DisplayName = "Negative available." )]
  [DataRow ( 0, -1, 0, DisplayName = "Negative offset." )]
  [DataRow ( 0, 0, -1, DisplayName = "Negative count." )]
  public void ValidateSegmentationStrict_NegativeValues ( int available, int offset, int count )
  {
    Action test = () => _ = IndexingValidator.ValidateSegmentationStrict ( available, offset: offset, count: count, out _, out _);
    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException>( test );
    Assert.AreEqual ( "Value must be non-negative integer, but it is '-1'.", e?.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5, 5, DisplayName = "Full coverage by segment." )]
  [DataRow ( 0, 2, 5, DisplayName = "Segmentation, start." )]
  [DataRow ( 3, 2, 5, DisplayName = "Segmentation, end." )]
  [DataRow ( 1, 3, 5, DisplayName = "Segmentation, middle." )]
  [DataRow ( 0, 0, 0, DisplayName = "Empty segment, empty source." )]
  [DataRow ( 4, 0, 5, DisplayName = "Empty segment, offsetting" )]
  public void ValidateSegmentationStrict_PositiveScenarios ( int offset, int count, int available )
  {
    int result = IndexingValidator.ValidateSegmentationStrict
    (
      available, offset, count: count, out int limit,
      out ImpossibleSegmentationException? e
    );

    Assert.AreEqual ( count == 0 ? -1 : 0, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.IsNull ( e );
  }

  // examples

  [TestMethod]
  public void IndexToCountInclusiveDifference_Example ()
  {
    int[] items = [1,2,3,4,5,6,7,8,9,10];
    int itemsToEndCount = IndexingValidator.IndexToCountInclusiveDifference ( 5, 10 );
    Array.Clear ( items, 5, itemsToEndCount );

    Assert.IsTrue ( new int [] { 1, 2, 3, 4, 5, 0, 0, 0, 0, 0 }.SequenceEqual ( items ) );
  }
}
