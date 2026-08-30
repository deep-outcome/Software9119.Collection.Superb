using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableOfTExtension;

[TestClass]
public class UnsupportedNullBehaviorExceptionTest
{
  [TestMethod]
  public void Message ()
  {    
    UnsupportedNullBehaviorException e = new((EnumerableNullBehavior)333, "behavior");
    Assert.AreEqual ( "Unsupported behavior, '333'. (Parameter 'behavior')", e.Message );
  }

  [TestMethod]
  public void ParamExpression ()
  {
    UnsupportedNullBehaviorException e = new((EnumerableNullBehavior)333);
    Assert.AreEqual ( "Unsupported behavior, '333'. (Parameter '(EnumerableNullBehavior)333')", e.Message );
  }
}
