using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Numerics;

using System;

namespace Software9119.Collection.Superb.TestArrangement.Numerics;

[TestClass]
public class AddOrInsertOffsetTest
{
  [TestMethod]
  [DataRow ( 222, false )]
  [DataRow ( 333, true )]
  public void Constructor ( int value, bool inserting )
  {
    AddInsertOffset test = new (value, inserting);
    Assert.AreEqual ( value, test.value );
    Assert.AreEqual ( inserting, test.inserting );
  }

  [TestMethod]
  [DataRow ( -1, -1, false )]
  [DataRow ( 0, 1, true )]
  [DataRow ( 1, 0, true )]
  [DataRow ( 1, 1, false )]
  public void CreateUsingCount ( int value, int count, bool inserting )
  {
    AddInsertOffset test = AddInsertOffset.CreateUsingCount(value, count);
    Assert.AreEqual ( value, test.value );
    Assert.AreEqual ( inserting, test.inserting );
  }

  [TestMethod]
  public void ImplicitCastOperator_FromOffsetToInt32 ()
  {
    AddInsertOffset offset = new (999, default);
    int test = offset;
    Assert.AreEqual ( offset.value, test );

    test = AddInsertOffset.ToInt32 ( offset );
    Assert.AreEqual ( offset.value, test );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void ImplicitCastOperator_FromOffsetToBoolean ( bool inserting )
  {
    AddInsertOffset offset = new (default, inserting);
    bool test = offset;
    Assert.AreEqual ( inserting, test );

    test = AddInsertOffset.ToBoolean ( offset );
    Assert.AreEqual ( inserting, test );
  }

  [TestMethod]
  [DataRow ( 10, true )]
  [DataRow ( 20, false )]
  public void GetHashCodeTest ( int value, bool inserting )
  {
    AddInsertOffset test = new (value, inserting);
    Assert.AreEqual ( HashCode.Combine ( value, inserting ), test.GetHashCode () );
  }

  [TestMethod]
  public void EqualsOperator ()
  {
    AddInsertOffset a = new (999, true);
    AddInsertOffset b = new (999, true);

    AddInsertOffset c = new (999, false);
    AddInsertOffset d = new (998, true);

    Assert.IsTrue ( a == b );
    Assert.IsFalse ( a == c );
    Assert.IsFalse ( a == d );
  }

  [TestMethod]
  public void NotEqualOperator ()
  {
    AddInsertOffset a = new (999, true);
    AddInsertOffset b = new (999, true);

    AddInsertOffset c = new (999, false);
    AddInsertOffset d = new (998, true);

    Assert.IsFalse ( a != b );
    Assert.IsTrue ( a != c );
    Assert.IsTrue ( a != d );
  }

  [TestMethod]
  public void Equals_Object ()
  {
    AddInsertOffset a = new (999, true);

    AddInsertOffset b = new (999, false);
    AddInsertOffset c = new (998, true);

    Assert.IsTrue ( a.Equals ( (object) a ) );
    Assert.IsFalse ( a.Equals ( (object) b ) );
    Assert.IsFalse ( a.Equals ( (object) c ) );
    Assert.IsFalse ( a.Equals ( null ) );
  }

  [TestMethod]
  public void Equals ()
  {
    AddInsertOffset a = new (999, true);

    AddInsertOffset b = new (999, false);
    AddInsertOffset c = new (998, true);

    Assert.IsTrue ( a.Equals ( a ) );
    Assert.IsFalse ( a.Equals ( b ) );
    Assert.IsFalse ( a.Equals ( c ) );
  }
}
