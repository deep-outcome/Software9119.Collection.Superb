using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;

using System.Globalization;

namespace Software9119.Collection.Superb.TestArrangement.Indexing.Exceptionality;

[TestClass]
public class ImpossibleSegmentationExceptionTest
{

  [TestMethod]
  public void NegativeCountMsg ()
  {
    ImpossibleSegmentationException e = ImpossibleSegmentationException.NegativeCountMsg ( -3 );
    Assert.AreEqual ( "Count must be a non-negative integer, but it is -3.", e.Message );
  }

  [TestMethod]
  public void NegativeOffsetMsg ()
  {
    ImpossibleSegmentationException e = ImpossibleSegmentationException.NegativeOffsetMsg ( -2 );
    Assert.AreEqual ( "Offset must be a non-negative integer, but it is -2.", e.Message );
  }

  [TestMethod]
  public void ForwardSegmentationMsg ()
  {
    ImpossibleSegmentationException e = ImpossibleSegmentationException.ForwardSegmentationMsg (1, 2, 3);
    string expMsg = "With available 1, given offset 2 and count 3 produce out-of indexing.";
    Assert.AreEqual ( expMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( null )]
  [DataRow ( false )]
  [DataRow ( true )]
  public void ForwardSegmentationMsg ( bool? withParamaters )
  {
    string[]? parameters = withParamaters == null
      ? null
      : withParamaters == true
        ? [ "AB", "C", "DE" ]
        : [];

    ImpossibleSegmentationException e = ImpossibleSegmentationException.ForwardSegmentationMsg (1, 2, 3,parameters);
    string msg = "With available 1, given offset 2 and count 3 produce out-of indexing.{0}";
    string paramsString = withParamaters == true ? " (Parameters 'AB','C','DE')" : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, paramsString );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void BackwardSegmentationMsg ()
  {
    ImpossibleSegmentationException e = ImpossibleSegmentationException.BackwardSegmentationMsg (1, 2, 3);
    string expMsg = "With available 1, given rearSet 2 and count 3 produce out-of indexing.";
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
    string msg = "With available 1, given rearSet 2 and count 3 produce out-of indexing.{0}";
    string paramsString = withParamaters == true ? " (Parameters 'AB','C','DE')" : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, paramsString );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( null )]
  [DataRow ( "" )]
  [DataRow ( "aaa,abc" )]
  public void ParamsString ( string paramsStr )
  {
    string[]? parameters = paramsStr?.Split(',');
    string test = ImpossibleSegmentationException.ParamsString(parameters);

    string expectation = paramsStr == null
      ? ""
      : paramsStr == ""
        ? " (Parameters '')"
        : " (Parameters 'aaa','abc')";
    Assert.AreEqual ( expectation, test );
  }
}
