using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Numerics;

using System;

namespace Software9119.Collection.Superb.TestArrangement.Numerics;

[TestClass]
public class NonNegativeInt32Test
{
  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( int.MaxValue )]
  public void Constructor ( int value )
  {
    NonNegativeInt32 num = new (value);
    Assert.AreEqual ( value, num.value );
  }

  [TestMethod]
  [DataRow ( -1 )]
  [DataRow ( int.MinValue )]
  public void Constructor_NegativeValue ( int value )
  {
    Action test = () => _ = new NonNegativeInt32 ( value );
    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    Assert.AreEqual ( "Value must be non-negative. (Parameter 'value')", e.Message );
  }

  [TestMethod]
  public void ImplicitCastOperator_FromInt ()
  {
    int value = 999;
    NonNegativeInt32 num = value;
    Assert.AreEqual ( value, num.value );

    num = NonNegativeInt32.ToNonNegativeInt32 ( value );
    Assert.AreEqual ( value, num.value );
  }

  [TestMethod]
  public void ImplicitCastOperator_FromNonNegativeInt32 ()
  {
    NonNegativeInt32 num = new (999);
    int value = num;
    Assert.AreEqual ( num.value, value );

    value = NonNegativeInt32.ToInt32 ( num );
    Assert.AreEqual ( num.value, value );
  }

  [TestMethod]
  public void GetHashCodeTest ()
  {
    int value = 999;
    NonNegativeInt32 num = value;
    Assert.AreEqual ( value.GetHashCode (), num.GetHashCode () );
  }

  [TestMethod]
  public void EqualsOperator ()
  {
    NonNegativeInt32 a = new (999);
    NonNegativeInt32 b = new (999);
    Assert.IsTrue ( a == b );
    Assert.IsFalse ( a == new NonNegativeInt32 ( 998 ) );
  }

  [TestMethod]
  public void NotEqualOperator ()
  {
    NonNegativeInt32 a = new (999);
    NonNegativeInt32 b = new (999);
    Assert.IsFalse ( a != b );
    Assert.IsTrue ( a != new NonNegativeInt32 ( 998 ) );
  }

  [TestMethod]
  public void Equals_Object ()
  {
    NonNegativeInt32 a = new (999);
    NonNegativeInt32 b = new (998);
    Assert.IsTrue ( a.Equals ( (object) a ) );
    Assert.IsFalse ( a.Equals ( (object) b ) );
    Assert.IsFalse ( a.Equals ( null ) );
  }

  [TestMethod]
  public void Equals ()
  {
    NonNegativeInt32 a = new (999);
    NonNegativeInt32 b = new (998);
    Assert.IsTrue ( a.Equals ( a ) );
    Assert.IsFalse ( a.Equals ( b ) );
  }
}
