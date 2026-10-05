using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Indexing;

[TestClass]
public class IndexingValidatorTest
{
  [TestMethod]
  public void LimitOutOf ()
  {
    Assert.AreEqual ( 8, IndexingValidator.LimitOutOf ( 3, (NonNegativeInt32) 5 ) );
    Assert.AreEqual ( 2, IndexingValidator.LimitOutOf ( -3, (NonNegativeInt32) 5 ) );
  }

  [TestMethod]
  public void CorrelateIndex ()
  {
    Assert.AreEqual ( 8, IndexingValidator.CorrelateIndex ( 3, 5 ) );
    Assert.AreEqual ( -8, IndexingValidator.CorrelateIndex ( -3, -5 ) );
    Assert.AreEqual ( 2, IndexingValidator.CorrelateIndex ( -3, 5 ) );
    Assert.AreEqual ( -2, IndexingValidator.CorrelateIndex ( 3, -5 ) );
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
  [DataRow ( -1, default, "Index must be non-negative integer, but it is '-1'." )]
  [DataRow ( int.MinValue, default, "Index must be non-negative integer, but it is '-2147483648'." )]
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
      ? "Index must be non-negative integer, but it is '-1'."
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
  [DataRow ( 1, 0, "Cannot insert at index '1' when available is '0'." )]
  [DataRow ( 5, 4, "Cannot insert at index '5' when available is '4'." )]
  [DataRow ( -1, default, "Index must be non-negative integer, but it is '-1'." )]
  [DataRow ( int.MinValue, default, "Index must be non-negative integer, but it is '-2147483648'." )]
  public void ValidateInsertionIndex_Int32Index_NegativeScenarios ( int index, int count, string errMsg )
  {
    bool result = IndexingValidator.ValidateInsertionIndex (index, count, out IndexOutOfBoundariesException? e);
    Assert.IsTrue ( result );
    Assert.IsNotNull ( e );

    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 1, "implicit" )]
  [DataRow ( 1, null )]
  [DataRow ( 1, "" )]
  [DataRow ( 1, "yourParam" )]
  [DataRow ( -1, "implicit" )]
  [DataRow ( -1, null )]
  [DataRow ( -1, "" )]
  [DataRow ( -1, "yourParam" )]
  [SuppressMessage ( "Style", "IDE0018:Inline variable declaration", Justification = "No." )]
  public void ValidateInsertionIndex_Int32Index_NegativeScenarios_ParameterName ( int index, string? paramName )
  {
    IndexOutOfBoundariesException? e;
    _ = paramName == "implicit"
      ? IndexingValidator.ValidateInsertionIndex ( index, 0, out e )
      : IndexingValidator.ValidateInsertionIndex ( index, 0, out e, paramName );

    string errMsg = index == -1
      ? "Index must be non-negative integer, but it is '-1'."
      : "Cannot insert at index '1' when available is '0'.";

    string paramString = paramName == "yourParam" ? " (Parameter 'yourParam')" : "";
    errMsg += paramString;

    Assert.AreEqual ( errMsg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1 )]
  [DataRow ( 1, 1 )]
  [DataRow ( 7, 8 )]
  [DataRow ( 8, 8 )]
  public void ValidateInsertionIndex_Int32Index_PositiveScenarios ( int index, int count )
  {
    bool result = IndexingValidator.ValidateInsertionIndex (index, count, out IndexOutOfBoundariesException? e);
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
      ? $"Index must be non-negative integer, but it is '{index}'."
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
  [DataRow ( -1, 5, 5, DisplayName = "Negative offset." )]
  public void ValidateSegmentation_NegativeScenarios ( int offset, int count, int available )
  {
    int result = IndexingValidator.ValidateSegmentation
    (
      available: available, offset, count: count, out int limit,
      out ImpossibleSegmentationException? e
    );

    Assert.AreEqual ( 1, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    string errMsg = offset < 0
      ? $"Offset must be non-negative integer, but it is '{offset}'."
      : $"With available '{available}', given offset '{offset}' and count '{count}' produce out-of indexing.";
    Assert.AreEqual ( errMsg, e!.Message );
  }

  static (ParamNames<SegmentationParamNames>?, bool) [] ValidateSegmentationData_Parameters ()
  {
    return [
      (null, false),
      (() => default, false),
      (() => new ("IndeX", "NumbeR", "ArraY"), true),
      (() => new ("IndeX", "", ""), true),
      (() => new ("", "", ""), false),
      (() => new (" ", " ", " "), false),
      (() => new (null!, null!, null), false),
    ];
  }

  [TestMethod]
  [DynamicData ( nameof ( ValidateSegmentationData_Parameters ) )]
  public void ValidateSegmentation_Parameters_InvalidSegment ( ParamNames<SegmentationParamNames>? parameters, bool validParams )
  {
    _ = IndexingValidator.ValidateSegmentation ( 5, 0, 6, out _, out ImpSegExc? e, parameters );

    string msg = "With available '5', given offset '0' and count '6' produce out-of indexing.{0}";
    string paramsStr = validParams
      ?  parameters.SafeGet().Count() == 1
        ? " (Parameter 'IndeX')"
        : " (Parameters 'IndeX','NumbeR','ArraY')"
      : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, paramsStr );

    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DynamicData ( nameof ( ValidateSegmentationData_Parameters ) )]
  public void ValidateSegmentation_Parameters_NegativeOffset ( ParamNames<SegmentationParamNames>? parameters, bool validParams )
  {
    _ = IndexingValidator.ValidateSegmentation ( 0, -1, 0, out _, out ImpSegExc? e, parameters );

    string msg = "Offset must be non-negative integer, but it is '-1'.{0}";
    string paramsStr = validParams ? " (Parameter 'IndeX')" : "";

    msg = string.Format ( CultureInfo.InvariantCulture, msg, paramsStr );

    Assert.AreEqual ( msg, e?.Message );
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
  public void ValidateSegmentationStrict_NegativeScenarios ( int offset, int count, int available )
  {
    int result = IndexingValidator.ValidateSegmentationStrict
    (
      available: available, offset, count: count, out int limit,
      out ImpossibleSegmentationException? e
    );

    Assert.AreEqual ( 1, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    string errMsg = $"With available '{available}', given offset '{offset}' and count '{count}' produce out-of indexing.";
    Assert.AreEqual ( errMsg, e!.Message );
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

  [TestMethod]
  // contains doc comment samples
  [DataRow ( 5, 6, 0 )] // sample
  [DataRow ( 5, 0, 2 )]
  [DataRow ( 5, 1, 3 )]
  [DataRow ( 5, 4, 6 )]
  [DataRow ( 5, 5, 1 )]
  [DataRow ( 5, 7, 9 )]
  [DataRow ( 1, 2, 0 )] // sample
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  [DataRow ( 0, 1, 1 )]
  [DataRow ( 0, -1, 0 )]
  public void ValidateBackwardSegmentation_NegativeScenarios ( int size, int index, int count )
  {
    Assert.AreEqual ( 1, IndexingValidator.ValidateBackwardSegmentation ( size, index, count, out ImpSegExc? e ) );

    string errMsg = index < 0
     ? $"Offset must be non-negative integer, but it is '-1'."
     : $"With available '{size}', given rearSet '{index}' and count '{count}' produce out-of indexing.";

    Assert.AreEqual ( errMsg, e!.Message );
  }

  [TestMethod]
  // contains doc comment samples
  [DataRow ( 5, 5, 0 )] // sample
  [DataRow ( 5, 4, 5 )] // sample
  [DataRow ( 5, 4, 1 )] // sample
  [DataRow ( 5, 0, 1 )]
  [DataRow ( 5, 1, 2 )]

  [DataRow ( 0, 0, 0 )] // sample
  [DataRow ( 1, 1, 0 )] // sample
  [DataRow ( 1, 0, 1 )] // sample
  public void ValidateBackwardSegmentation_PositiveScenarios ( int size, int index, int count )
  {
    int result = count == 0 ? -1 : 0;
    Assert.AreEqual ( result, IndexingValidator.ValidateBackwardSegmentation ( size, index, count, out ImpSegExc? e, () => [] ) );
    Assert.IsNull ( e );
  }

  static (ParamNames<SegmentationParamNames>?, bool) [] ValidateBackwardSegmentationData_Parameters ()
  {
    return [
      (null, false),
      (() => default, false),
      (() => new ("RearSeT", "NumbeR", "ArraY"), true),
      (() => new ("RearSeT", "", ""), true),
      (() => new ("", "", ""), false),
      (() => new (" ", " ", " "), false),
      (() => new (null!, null!, null), false),
    ];
  }

  [TestMethod]
  [DynamicData ( nameof ( ValidateBackwardSegmentationData_Parameters ) )]
  public void ValidateBackwardSegmentation_Parameters_InvalidSegment ( ParamNames<SegmentationParamNames>? parameters, bool validParams )
  {
    _ = IndexingValidator.ValidateBackwardSegmentation ( 5, 6, 0, out ImpSegExc? e, parameters );

    string msg = "With available '5', given rearSet '6' and count '0' produce out-of indexing.{0}";
    string paramsStr = validParams
      ?  parameters.SafeGet().ParamsCount == 1
        ? " (Parameter 'RearSeT')"
        : " (Parameters 'RearSeT','NumbeR','ArraY')"
      : "";

    msg = string.Format ( CultureInfo.InvariantCulture, msg, paramsStr );

    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DynamicData ( nameof ( ValidateBackwardSegmentationData_Parameters ) )]
  public void ValidateBackwardSegmentation_Parameters_NegativeOffset ( ParamNames<SegmentationParamNames>? parameters, bool validParams )
  {
    _ = IndexingValidator.ValidateBackwardSegmentation ( 0, -1, 0, out ImpSegExc? e, parameters );

    string msg = "Offset must be non-negative integer, but it is '-1'.{0}";
    string paramsStr = validParams ? " (Parameter 'RearSeT')" : "";

    msg = string.Format ( CultureInfo.InvariantCulture, msg, paramsStr );

    Assert.AreEqual ( msg, e?.Message );
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
