using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void AsOrToImmutableArray ( int? capacity )
  {
    const int count = 10;
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, count);
    ImmutableArray<int>? test = source.AsOrToImmutableArray(capacity)!;

    Assert.HasCount ( count, test );
    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToImmutableArray_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ImmutableArray<int> test = returnsDefault
      ? source.AsOrToImmutableArray(behavior: behavior!.Value)
      : source.AsOrToImmutableArray();

    Assert.AreEqual ( returnsDefault, test.IsDefault );
    Assert.HasCount ( 0, test.IsDefault ? [] : test );
  }

  [TestMethod]
  public void AsOrToImmutableArray_ExactLengthEnforcement ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    Action test= () => _ = source.AsOrToImmutableArray ( 1000, enforceLengthCountMatch: true )!;

    Exception e = Assert.ThrowsExactly<InvalidOperationException> ( test );
    Assert.AreEqual ( "MoveToImmutable can only be performed when Count equals Capacity.", e.Message );
  }
}
