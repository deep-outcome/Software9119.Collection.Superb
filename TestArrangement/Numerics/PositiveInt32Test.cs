using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Numerics;

using System;
using System.Globalization;

namespace Software9119.Collection.Superb.TestArrangement.Numerics;

[TestClass]
public class PositiveInt32Test
{
  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( int.MaxValue )]
  public void Constructor ( int value )
  {
    PositiveInt32 num = new (value);
    Assert.AreEqual ( value, num.value );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( -1 )]
  [DataRow ( int.MinValue )]
  public void Constructor_NegativeValue ( int value )
  {
    Action test = () => _ = new PositiveInt32 ( value );
    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    Assert.AreEqual ( $"Value must be positive, but it is '{value}'.", e.Message );
  }

  [TestMethod]
  public void ImplicitCastOperator_FromInt ()
  {
    int value = 999;
    PositiveInt32 num = value;
    Assert.AreEqual ( value, num.value );

    num = PositiveInt32.ToPositiveInt32 ( value );
    Assert.AreEqual ( value, num.value );
  }

  [TestMethod]
  public void ImplicitCastOperator_FromPositiveInt32 ()
  {
    PositiveInt32 num = new (999);
    int value = num;
    Assert.AreEqual ( num.value, value );

    value = PositiveInt32.ToInt32 ( num );
    Assert.AreEqual ( num.value, value );
  }

  [TestMethod]
  public void GetHashCodeTest ()
  {
    int value = 999;
    PositiveInt32 num = value;
    Assert.AreEqual ( value.GetHashCode (), num.GetHashCode () );
  }

  [TestMethod]
  public void EqualsOperator ()
  {
    PositiveInt32 a = new (999);
    PositiveInt32 b = new (999);
    Assert.IsTrue ( a == b );
    Assert.IsFalse ( a == new PositiveInt32 ( 998 ) );
  }

  [TestMethod]
  public void NotEqualOperator ()
  {
    PositiveInt32 a = new (999);
    PositiveInt32 b = new (999);
    Assert.IsFalse ( a != b );
    Assert.IsTrue ( a != new PositiveInt32 ( 998 ) );
  }

  [TestMethod]
  public void Equals_Object ()
  {
    PositiveInt32 a = new (999);
    PositiveInt32 b = new (998);
    Assert.IsTrue ( a.Equals ( (object) a ) );
    Assert.IsFalse ( a.Equals ( (object) b ) );
    Assert.IsFalse ( a.Equals ( null ) );
  }

  [TestMethod]
  public void Equals ()
  {
    PositiveInt32 a = new (999);
    PositiveInt32 b = new (998);
    Assert.IsTrue ( a.Equals ( a ) );
    Assert.IsFalse ( a.Equals ( b ) );
  }

  [TestMethod]
  public void ToStringTest ()
  {
    int num = 999;
    PositiveInt32 a = num;
    Assert.AreEqual ( num.ToString ( CultureInfo.InvariantCulture ), a.ToString () );
  }
}
