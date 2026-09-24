using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;

namespace Software9119.Collection.Superb.TestArrangement.Indexing.Exceptionality;

[TestClass]
public class IndexOutOfBoundariesExceptionTest
{
  [TestMethod]
  [DataRow ( "implicit" )]
  [DataRow ( null )]
  [DataRow ( "" )]
  [DataRow ( "yourParam" )]
  public void OutOfBoundsForInsertionMsg ( string paramName )
  {
    string msg = "Cannot insert at index '1' when available is '0'.";
    string paramString = paramName == "yourParam" ? $" (Parameter '{paramName}')" : "";
    msg += paramString;

    IndexOutOfBoundariesException test = paramName == "implicit"
      ? IndexOutOfBoundariesException.OutOfBoundsForInsertionMsg(1, available: 0)
      : IndexOutOfBoundariesException.OutOfBoundsForInsertionMsg(1, available: 0, paramName);

    Assert.AreEqual ( msg, test.Message );
  }

  [TestMethod]
  [DataRow ( "implicit" )]
  [DataRow ( null )]
  [DataRow ( "" )]
  [DataRow ( "yourParam" )]
  public void OutOfBoundsMsg ( string paramName )
  {
    string msg = "For available '0' is index '1' out of bounds.";
    string paramString = paramName == "yourParam" ? $" (Parameter '{paramName}')" : "";
    msg += paramString;

    IndexOutOfBoundariesException test = paramName == "implicit"
      ? IndexOutOfBoundariesException.OutOfBoundsMsg(1, available: 0)
      : IndexOutOfBoundariesException.OutOfBoundsMsg(1, available: 0, paramName);

    Assert.AreEqual ( msg, test.Message );
  }

  [TestMethod]
  [DataRow ( "implicit" )]
  [DataRow ( null )]
  [DataRow ( "" )]
  [DataRow ( "yourParam" )]
  public void NegativeIndexMsg ( string paramName )
  {
    string msg = "Index must be non-negative, but it is '-1'.";
    string paramString = paramName == "yourParam" ? $" (Parameter '{paramName}')" : "";
    msg += paramString;

    IndexOutOfBoundariesException test = paramName == "implicit"
      ? IndexOutOfBoundariesException.NegativeIndexMsg(-1)
      : IndexOutOfBoundariesException.NegativeIndexMsg(-1, paramName);

    Assert.AreEqual ( msg, test.Message );
  }
}
