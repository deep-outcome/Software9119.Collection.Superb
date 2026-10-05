using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;

using System;
using System.Globalization;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Indexing.Exceptionality;

[TestClass]
public class ImpossibleSegmentationExceptionTest
{

  static (SegmentationParamNames, bool?) [] ParamNamesData ()
  {
    return
    [
      ( default(SegmentationParamNames), false),
      ( new SegmentationParamNames("", "", ""), false),
      ( new SegmentationParamNames(" ", " ", " "), false),
      ( new SegmentationParamNames(null!, null!, null), false),
      ( new SegmentationParamNames("OffseT", "CounT", null), true),
      ( new SegmentationParamNames("OffseT", "CounT", "ArraY"), true),
      ( new SegmentationParamNames("OffseT", "", ""), true),
      ( default, null),
    ];
  }

  [TestMethod]
  [DynamicData ( nameof ( ParamNamesData ) )]
  public void NegativeCountMsg ( SegmentationParamNames paramNames, bool? valid )
  {
    ImpossibleSegmentationException e =
      valid.HasValue
      ? ImpossibleSegmentationException.NegativeCountMsg ( -3, paramNames )
      : ImpossibleSegmentationException.NegativeCountMsg ( -3 );

    string msg = "Count must be non-negative integer, but it is '-3'.";
    if (valid == true && paramNames.Count.Length > 0) msg += " (Parameter 'CounT')";

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DynamicData ( nameof ( ParamNamesData ) )]
  public void NegativeOffsetMsg ( SegmentationParamNames paramNames, bool? valid )
  {
    ImpossibleSegmentationException e =
      valid.HasValue
      ? ImpossibleSegmentationException.NegativeOffsetMsg ( -2, paramNames)
      : ImpossibleSegmentationException.NegativeOffsetMsg ( -2 );

    string msg = "Offset must be non-negative integer, but it is '-2'.";
    if (valid == true && paramNames.Offset.Length > 0) msg += " (Parameter 'OffseT')";

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DynamicData ( nameof ( ParamNamesData ) )]
  public void ForwardSegmentationMsg ( SegmentationParamNames paramNames, bool? valid )
  {
    ImpossibleSegmentationException e =
      valid.HasValue
      ? ImpossibleSegmentationException.ForwardSegmentationMsg (1, 2, 3, paramNames)
      : ImpossibleSegmentationException.ForwardSegmentationMsg (1, 2, 3);

    string msg = "With available '1', given offset '2' and count '3' produce out-of indexing.";
    if (valid == true)
    {
      msg += paramNames.Count () switch
      {
        1 => " (Parameter 'OffseT')",
        2 => " (Parameters 'OffseT','CounT')",
        3 => " (Parameters 'OffseT','CounT','ArraY')",
        _ => throw new InvalidOperationException ( "Unsupported parameters count." )
      };
    }

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void BackwardSegmentationMsg ()
  {
    ImpossibleSegmentationException e = ImpossibleSegmentationException.BackwardSegmentationMsg (1, 2, 3);
    string expMsg = "With available '1', given rearSet '2' and count '3' produce out-of indexing.";
    Assert.AreEqual ( expMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( null )]
  [DataRow ( false )]
  [DataRow ( true )]
  public void BackwardSegmentationMsg ( bool? withParamaters )
  {
    string[]? parameters = withParamaters == null
      ? null
      : withParamaters == true
        ? [ "AB", "C", "DE" ]
        : [];

    ImpossibleSegmentationException e = ImpossibleSegmentationException.BackwardSegmentationMsg (1, 2, 3,parameters);
    string msg = "With available '1', given rearSet '2' and count '3' produce out-of indexing.{0}";
    string paramsString = withParamaters == true ? " (Parameters 'AB','C','DE')" : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, paramsString );

    Assert.AreEqual ( msg, e.Message );
  }

  static (string? []?, string) [] ParamsStringData ()
  {
    return
    [
      (null, ""),
      ( [], ""),
      ( [ "", "", "" ], ""),
      ( [ " ", " ", " " ], ""),
      ( [ null, null, null ], "" ),
      ( [ "Abc", null, "XyZ"], " (Parameters 'Abc','XyZ')" ),
      ( [ "Abc", null], " (Parameter 'Abc')" )
    ];
  }

  [TestMethod]
  [DynamicData ( nameof ( ParamsStringData ) )]
  public void ParamsString ( string? []? parameters, string expectation )
  {
    string test = ImpossibleSegmentationException.ParamsString(parameters);
    Assert.AreEqual ( expectation, test );
  }
}
