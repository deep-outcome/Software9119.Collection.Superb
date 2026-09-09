using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class UnsupportedReadOnlySetTypeExceptionTest
{
  [TestMethod]
  public void Message ()
  {
    UnsupportedReadOnlySetTypeException e = new((ReadOnlySetType)333, "typeOfSet");
    Assert.AreEqual ( "Unsupported set type, '333'. (Parameter 'typeOfSet')", e.Message );
  }

  [TestMethod]
  public void ParamExpression ()
  {
    UnsupportedReadOnlySetTypeException e = new((ReadOnlySetType)333);
    Assert.AreEqual ( "Unsupported set type, '333'. (Parameter '(ReadOnlySetType)333')", e.Message );
  }
}
