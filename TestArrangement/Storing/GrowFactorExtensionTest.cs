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
    Assert.IsTrue ( factor.ToFloat ( out float number ) );
    Assert.AreEqual ( expectation, number );

    // no error, all values are handled
    _ = Enum.GetValues ( typeof ( GrowFactor ) ).Cast<GrowFactor> ().All ( x => GrowFactorExtension.ToFloat ( x, out _ ) );
  }

  [TestMethod]
  public void ToFloat_Unknown ()
  {
    Assert.IsFalse ( ((GrowFactor) (-1)).ToFloat ( out float number ) );
    Assert.AreEqual ( default, number );
  }
}
