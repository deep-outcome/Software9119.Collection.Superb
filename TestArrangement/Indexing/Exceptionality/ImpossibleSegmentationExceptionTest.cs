using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;

using System;
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
      ( new SegmentationParamNames("OutseT", "CounT", null), true),
      ( new SegmentationParamNames("OutseT", "CounT", "ArraY"), true),
      ( new SegmentationParamNames("OutseT", "", ""), true),
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
    if (valid == true && paramNames.Offset.Length > 0) msg += " (Parameter 'OutseT')";

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
        1 => " (Parameter 'OutseT')",
        2 => " (Parameters 'OutseT','CounT')",
        3 => " (Parameters 'OutseT','CounT','ArraY')",
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
  [DynamicData ( nameof ( ParamNamesData ) )]
  public void BackwardSegmentationMsg ( SegmentationParamNames paramNames, bool? valid )
  {
    ImpossibleSegmentationException e = ImpossibleSegmentationException.BackwardSegmentationMsg (1, 2, 3, paramNames);
    string msg = "With available '1', given rearSet '2' and count '3' produce out-of indexing.";
    if (valid == true)
    {
      msg += paramNames.Count () switch
      {
        1 => " (Parameter 'OutseT')",
        2 => " (Parameters 'OutseT','CounT')",
        3 => " (Parameters 'OutseT','CounT','ArraY')",
        _ => throw new InvalidOperationException ( "Unsupported parameters count." )
      };
    }

    Assert.AreEqual ( msg, e.Message );
  }

  static (SegmentationParamNames, bool) [] ParamsStringData ()
  {
    return [
      (default, false),
      (new ("IndeX", "NumbeR", "ArraY"), true),
      (new ("IndeX", "", "ArraY"), true),
      (new ("IndeX", "", ""), true),
      (new ("", "", ""), false),
      (new (" ", " ", " "), false),
      (new (null!, null!, null), false),
    ];
  }

  [TestMethod]
  [DynamicData ( nameof ( ParamsStringData ) )]
  public void ParamsString ( SegmentationParamNames parameters, bool validParams )
  {
    string expectation = validParams ?  parameters.ParamsCount switch
    {
      1 => " (Parameter 'IndeX')",
      2 => " (Parameters 'IndeX','ArraY')",
      3 => " (Parameters 'IndeX','NumbeR','ArraY')",
      _ => throw new InvalidOperationException("Unsupported parameters count.")
    }
    : "";

    string test = ImpossibleSegmentationException.ParamsString(parameters);
    Assert.AreEqual ( expectation, test );
  }
}
