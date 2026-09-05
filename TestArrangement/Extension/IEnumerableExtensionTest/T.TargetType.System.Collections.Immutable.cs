using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using collections_immutable = Software9119.Collection.Superb.Extension.system_collections_immutable;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_immutable_test
{
  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void ImmutableArray ( int? capacity )
  {
    AsOrToTargetType<ImmutableArray<int>> targetType = collections_immutable.ImmutableArray<int> ( false );

    ImmutableArray<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    const int count = 10;
    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, count);
    ImmutableArray<int> target = targetType.Ctor(source, capacity);

    Assert.HasCount ( count, target );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  public void ImmutableArray_ExactLengthEnforcement ()
  {
    AsOrToTargetType<ImmutableArray<int>> targetType = collections_immutable.ImmutableArray<int> ( true );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Action test = () => _ = targetType.Ctor(source, 1000);

    Exception e = Assert.ThrowsExactly<InvalidOperationException> ( test );
    Assert.AreEqual ( "MoveToImmutable can only be performed when Count equals Capacity.", e.Message );
  }
}
