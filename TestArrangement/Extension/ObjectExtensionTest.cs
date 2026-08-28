using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

namespace Software9119.Collection.Superb.TestArrangement.Extension;

[TestClass]
public class ObjectExtensionTest
{
  [TestMethod]
  public void IsNull_Object ()
  {
    Assert.IsFalse ( new object ().IsNull () );
    Assert.IsTrue ( ((object?) null).IsNull () );
  }

  [TestMethod]
  public void IsNull_Struct ()
  {
    Assert.IsFalse ( ((int?) 0).IsNull () );
    Assert.IsTrue ( ((int?) null).IsNull () );
  }

  [TestMethod]
  public void IsNotNull_Object ()
  {
    Assert.IsTrue ( new object ().IsNotNull () );
    Assert.IsFalse ( ((object?) null).IsNotNull () );
  }

  [TestMethod]
  public void IsNotNull_Struct ()
  {
    Assert.IsTrue ( ((int?) 0).IsNotNull () );
    Assert.IsFalse ( ((int?) null).IsNotNull () );
  }

  [TestMethod]
  public void IsDefault ()
  {
    Assert.IsFalse ( 1.IsDefault () );
    Assert.IsTrue ( 0.IsDefault () );
  }

  [TestMethod]
  public void IsNotDefault ()
  {
    Assert.IsFalse ( 0.IsNotDefault () );
    Assert.IsTrue ( 1.IsNotDefault () );
  }

  [TestMethod]
  [DataRow ( 0, 0, true )]
  [DataRow ( null, null, true )]
  [DataRow ( null, 0, false )]
  [DataRow ( 0, null, false )]
  public void Matches_Object ( object? one, object? another, bool result )
  {
    Assert.AreEqual ( result, one.Matches ( another ) );
  }

  [TestMethod]
  [DataRow ( 0, 0, true )]
  [DataRow ( null, null, true )]
  [DataRow ( null, 0, false )]
  [DataRow ( 0, null, false )]
  public void Matches_Struct ( int? one, int? another, bool result )
  {
    Assert.AreEqual ( result, one.Matches ( another ) );
  }
}
