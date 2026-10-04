using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;

namespace Software9119.Collection.Superb.TestArrangement.Indexing.Auxiliaries;

[TestClass]
public class DelegateExtensionsTest
{
  [TestMethod]
  public void ParamNames_SafeGet ()
  {
    ParamNames<int> test;

    test = () => -1;
    Assert.AreEqual ( -1, test.SafeGet () );

    test = null!;
    Assert.AreEqual ( 0, test.SafeGet () );

    test = () => 0;
    Assert.AreEqual ( 0, test.SafeGet () );
  }
}
