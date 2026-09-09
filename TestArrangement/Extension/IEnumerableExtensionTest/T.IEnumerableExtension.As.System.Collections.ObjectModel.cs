using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  public void AsReadOnlyDictionary ()
  {
    Dictionary<object, int> source = new ()
    {
      [ new object () ] = 1,
      [ new object () ] = 2,
      [ new object () ] = 3,
    };

    ReadOnlyDictionary<object, int> test = source.AsReadOnlyDictionary()!;
    object innerDict = Reflection.GetNonPublicFieldValue ( test, "m_dictionary" );
    Assert.IsTrue ( ReferenceEquals ( source, innerDict ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsReadOnlyDictionary_NullBehavior ( NullBehavior? behavior )
  {
    Dictionary<object, int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ReadOnlyDictionary<object, int>? test = returnsDefault
      ? source.AsReadOnlyDictionary(behavior: behavior!.Value)
      : source.AsReadOnlyDictionary();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void AsReadOnlySet ()
  {
    HashSet<int> source = [];
    ReadOnlySet<int> test = source.AsReadOnlySet()!;

    object set = Reflection.GetNonPublicFieldValue ( test, "_set" );
    Assert.IsTrue ( ReferenceEquals ( source, set ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsReadOnlySet_NullBehavior ( NullBehavior? behavior )
  {
    HashSet<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ReadOnlySet<int> test = returnsDefault
      ? source.AsReadOnlySet(behavior: behavior!.Value)!
      : source.AsReadOnlySet()!;

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }
}
