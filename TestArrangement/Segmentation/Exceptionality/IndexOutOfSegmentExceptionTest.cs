using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation.Exceptionality;

[TestClass]
public class IndexOutOfSegmentExceptionTest
{
  [TestMethod]
  public void OutOfRangeMsg ()
  {
    IndexOutOfSegmentException e = IndexOutOfSegmentException.OutOfRangeMsg ( 3, 3 );
    Assert.AreEqual ( "Segment length is 3, index 3 is out of its range.", e.Message );
  }

  [TestMethod]
  public void NegativeIndexMsg ()
  {
    IndexOutOfSegmentException e = IndexOutOfSegmentException.NegativeIndexMsg ( -1 );
    Assert.AreEqual ( "Index must be non-negative, but it is -1.", e.Message );
  }
}
