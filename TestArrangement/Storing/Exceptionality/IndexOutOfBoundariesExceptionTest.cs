using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;

namespace Software9119.Collection.Superb.TestArrangement.Storing.Exceptionality;

[TestClass]
public class IndexOutOfBoundariesExceptionTest
{
  [TestMethod]
  public void OutOfBoundsMsg ()
  {
    const string msg = "For available '0' is index '1' out of bounds.";
    IndexOutOfBoundariesException test = IndexOutOfBoundariesException.OutOfBoundsMsg(1, available: 0);
    Assert.AreEqual ( msg, test.Message );
  }

  [TestMethod]
  public void NegativeIndexMsg ()
  {
    const string msg = "Index must be non-negative, but it is '-1'.";
    IndexOutOfBoundariesException test = IndexOutOfBoundariesException.NegativeIndexMsg(-1);
    Assert.AreEqual ( msg, test.Message );
  }
}
