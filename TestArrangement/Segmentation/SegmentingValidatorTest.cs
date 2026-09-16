using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Segmentation;

using System;

namespace Software9119.Collection.Superb.TestArrangement.Segmentation;

[TestClass]
public class SegmentingValidatorTest
{
  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void ValidateList ( bool nullList )
  {
    int[]? list = nullList ? null : [];

    bool result = SegmentingValidator.ValidateList ( list, out ArgumentNullException? e);
    Assert.AreEqual ( nullList, result );
    Assert.AreEqual ( nullList, e is not null );

    string expMsg = nullList ? "Null list provided. (Parameter 'list')" : "";
    Assert.AreEqual ( expMsg, e?.Message ?? "" );
  }
}
