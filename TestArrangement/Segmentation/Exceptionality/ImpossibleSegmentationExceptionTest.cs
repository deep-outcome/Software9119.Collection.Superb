using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.Segmentation.Exceptionality;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation.Exceptionality;

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
  [DataRow ( 3, 5, 5 )]
  [DataRow ( 5, 5, 7 )]
  public void OufRangeMsg ( int count, int r1, int r2 )
  {
    const int offset = 3;
    int limit = SegmentationValidator.LimitOutOf(offset,count);
    ImpossibleSegmentationException e = ImpossibleSegmentationException.OufRangeMsg ( 5, offset: offset, count: count, limit: limit);
    string expMsg = $"List has length 5, given offset 3 and count {count} produces out-of indexing in range {r1}–{r2}.";
    Assert.AreEqual ( expMsg, e.Message );
  }
}
