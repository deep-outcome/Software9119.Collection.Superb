using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void AsOrToCollection ( int? capacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    Collection<int> test = capacity is int
      ? source.AsOrToCollection(capacity)!
      : source.AsOrToCollection()!;

    List<int> items = (List<int>) Reflection.GetNonPublicFieldValue ( test, "items" );
    Assert.AreEqual ( capacity ?? 16, items.Capacity );

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToCollection_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Collection<int>? test = returnsDefault
      ? source.AsOrToCollection(behavior: behavior!.Value)
      : source.AsOrToCollection();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }
}
