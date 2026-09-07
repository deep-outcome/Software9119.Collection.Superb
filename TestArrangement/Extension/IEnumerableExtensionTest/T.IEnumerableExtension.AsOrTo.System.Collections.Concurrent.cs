using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{

  [TestMethod]
  public void AsOrToConcurrentBag ()
  {
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ConcurrentBag<int> test = source.AsOrToConcurrentBag()!;

    Assert.IsTrue ( source.SequenceEqual ( test.Order () ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToConcurrentBag_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ConcurrentBag<int>? test = returnsDefault
      ? source.AsOrToConcurrentBag(behavior: behavior!.Value)
      : source.AsOrToConcurrentBag();

    Assert.AreEqual ( test?.IsEmpty ?? false, returnsDefault == false );
  }
}
