using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class ReadOnlySetTypeExtensionTest
{
  [TestMethod]
  [DataRow ( ReadOnlySetType.FrozenSet, false )]
  [DataRow ( ReadOnlySetType.HashSet, false )]
  [DataRow ( ReadOnlySetType.ImmutableHashSet, false )]
  [DataRow ( ReadOnlySetType.ImmutableSortedSet, true )]
  [DataRow ( ReadOnlySetType.SortedSet, true )]
  public void IsSortedSet ( ReadOnlySetType type, bool itIs ) => Assert.AreEqual ( itIs, type.IsSortedSet () );
}
