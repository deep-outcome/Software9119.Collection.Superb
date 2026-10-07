using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.Storing;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class collection_superb_storing_test
{
  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Capacitor ( int? capacity )
  {
    AsOrToTargetType<Capacitor<int>> targetType = collection_superb_storing.Capacitor<int> ( capacity );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Capacitor<int> target = targetType.Ctor(source);

    Assert.AreEqual ( capacity ?? 16, target.Capacity );
    Assert.IsTrue ( source.SequenceEqual ( target ) );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Accumulator ( int? capacity )
  {
    AsOrToTargetType<Accumulator<int>> targetType = collection_superb_storing.Accumulator<int> ( capacity );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Accumulator<int> target = targetType.Ctor(source);

    Assert.AreEqual ( capacity ?? 16, target.Capacity );
    Assert.IsTrue ( source.SequenceEqual ( target ) );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
  }
}
