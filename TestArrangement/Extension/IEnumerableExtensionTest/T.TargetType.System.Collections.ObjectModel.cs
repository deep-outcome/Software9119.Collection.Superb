using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_objectmodel_test
{

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Collection ( int? capacity )
  {
    AsOrToTargetType<Collection<int>> targetType = system_collections_objectmodel.Collection<int> (capacity );

    Collection<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    const int count = 10;
    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, count);
    Collection<int> target = targetType.Ctor(source);

    Assert.HasCount ( count, target );

    List<int> items = (List<int>) Reflection.GetNonPublicFieldValue ( target, "items" );
    Assert.AreEqual ( capacity ?? 16, items.Capacity );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }
}
