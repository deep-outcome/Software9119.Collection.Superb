using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class UnsupportedBlockingCollectionTypeExceptionTest
{
  [TestMethod]
  public void Message ()
  {
    UnsupportedBlockingCollectionTypeException e = new((BlockingCollectionType)333, "typeX");
    Assert.AreEqual ( "Unsupported blocking collection type, '333'. (Parameter 'typeX')", e.Message );
  }

  [TestMethod]
  public void ParamExpression ()
  {
    UnsupportedBlockingCollectionTypeException e = new((BlockingCollectionType)333);
    Assert.AreEqual ( "Unsupported blocking collection type, '333'. (Parameter '(BlockingCollectionType)333')", e.Message );
  }
}
