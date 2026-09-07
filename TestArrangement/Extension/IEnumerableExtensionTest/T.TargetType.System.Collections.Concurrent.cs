using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using collections_concurrent = Software9119.Collection.Superb.Extension.system_collections_concurrent;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_concurrent_tests
{
  [TestMethod]
  public void ConcurrentBag ()
  {
    AsOrToTargetType<ConcurrentBag <int>> targetType = collections_concurrent.ConcurrentBag<int>();

    ConcurrentBag <int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    ConcurrentBag <int> target = targetType.Ctor(source);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target.Order () ) );
  }
}