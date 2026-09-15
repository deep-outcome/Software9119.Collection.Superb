using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;

namespace Software9119.Collection.Superb.TestArrangement.Indexing;

[TestClass]
public class IndexingValidatorTest
{
  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( int.MaxValue, int.MaxValue )]
  [DataRow ( 1, 0 )]
  [DataRow ( 5, 4 )]
  public void ValidateIndex_NegativeScenarios ( int i, int c )
  {
    NonNegativeInt32 index = i;
    NonNegativeInt32 count = c;

    bool result = IndexingValidator.ValidateIndex (index, count, out IndexOutOfBoundariesException? e);
    Assert.IsTrue ( result );
    Assert.IsNotNull ( e );

    string msg =  $"For available '{count}' is index '{index}' out of bounds.";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1 )]
  [DataRow ( 1, 2 )]
  [DataRow ( 5, 8 )]
  public void ValidateIndex_PositiveScenarios ( int i, int c )
  {
    NonNegativeInt32 index = i;
    NonNegativeInt32 count = c;

    bool result = IndexingValidator.ValidateIndex (index, count, out IndexOutOfBoundariesException? e);
    Assert.IsFalse ( result );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 0, "For available '0' is index '0' out of bounds." )]
  [DataRow ( int.MaxValue, int.MaxValue, "For available '2147483647' is index '2147483647' out of bounds." )]
  [DataRow ( 1, 0, "For available '0' is index '1' out of bounds." )]
  [DataRow ( 5, 4, "For available '4' is index '5' out of bounds." )]
  [DataRow ( -1, default, "Index must be non-negative, but it is '-1'." )]
  [DataRow ( int.MinValue, default, "Index must be non-negative, but it is '-2147483648'." )]
  public void ValidateIndex_Int32Index_NegativeScenarios ( int index, int count, string errMsg )
  {
    bool result = IndexingValidator.ValidateIndex (index, count, out IndexOutOfBoundariesException? e);
    Assert.IsTrue ( result );
    Assert.IsNotNull ( e );

    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1 )]
  [DataRow ( 1, 2 )]
  [DataRow ( 5, 8 )]
  public void ValidateIndex_Int32Index_PositiveScenarios ( int index, int count )
  {
    bool result = IndexingValidator.ValidateIndex (index, count, out IndexOutOfBoundariesException? e);
    Assert.IsFalse ( result );
    Assert.IsNull ( e );
  }
}
