using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Storing;

using System;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Storing;

[TestClass]
public class GrowFactorExtensionTest
{
  [TestMethod]
  [DataRow ( GrowFactor.OneAndHalf, 1.5f )]
  [DataRow ( GrowFactor.Two, 2.0f )]
  [DataRow ( GrowFactor.TwoAndHalf, 2.5f )]
  [DataRow ( GrowFactor.Three, 3.0f )]
  [DataRow ( GrowFactor.ThreeAndHalf, 3.5f )]
  [DataRow ( GrowFactor.Four, 4.0f )]
  [DataRow ( GrowFactor.FourAndHalf, 4.5f )]
  [DataRow ( GrowFactor.Five, 5.0f )]
  public void ToFloat ( GrowFactor factor, float expectation )
  {
    Assert.AreEqual ( expectation, factor.ToFloat () );

    // no error, all values are handled
    _ = Enum.GetValues ( typeof ( GrowFactor ) ).Cast<GrowFactor> ().Select ( GrowFactorExtension.ToFloat );
  }

  [TestMethod]
  public void ToFloat_Unknown ()
  {
    Action test = () => _ = ((GrowFactor) (-1)).ToFloat();
    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    Assert.AreEqual ( "Unsupported grow factor, '-1'. (Parameter 'factor')", e.Message );
  }
}
