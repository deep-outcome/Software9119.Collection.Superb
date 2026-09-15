using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;

using System;

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
  public void OufRangeMsg ()
  {
    ImpossibleSegmentationException e = ImpossibleSegmentationException.OufRangeMsg (1, 2, 3);
    string expMsg = "With available 1, given offset 2 and count 3 produce out-of indexing.";
    Assert.AreEqual ( expMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( -1, 0, 0, DisplayName = "Negative available." )]
  [DataRow ( 0, -1, 0, DisplayName = "Negative offset." )]
  [DataRow ( 0, 0, -1, DisplayName = "Negative count." )]
  public void OufRangeMsg ( int available, int offset, int count )
  {
    Action test = () => _ = ImpossibleSegmentationException.OufRangeMsg ( available, offset: offset, count: count);
    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException>( test );
    Assert.AreEqual ( "Value must be non-negative, but it is '-1'.", e?.Message );
  }
}
