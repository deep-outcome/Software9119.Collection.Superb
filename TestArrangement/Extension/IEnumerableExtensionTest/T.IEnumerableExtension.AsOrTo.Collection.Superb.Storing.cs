using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.Storing;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void AsOrToCapacitor ( int? capacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    Capacitor<int> test = capacity is int
      ? source.AsOrToCapacitor(capacity)!
      : source.AsOrToCapacitor()!;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.AreEqual ( capacity ?? 16, test.Capacity );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToCapacitor_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Capacitor<int>? test = returnsDefault
      ? source.AsOrToCapacitor(behavior: behavior!.Value)
      : source.AsOrToCapacitor();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void AsOrToAccumulator ( int? capacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    Accumulator<int> test = capacity is int
      ? source.AsOrToAccumulator(capacity)!
      : source.AsOrToAccumulator()!;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.AreEqual ( capacity ?? 16, test.Capacity );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToAccumulator_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Accumulator<int>? test = returnsDefault
      ? source.AsOrToAccumulator(behavior: behavior!.Value)
      : source.AsOrToAccumulator();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }
}
