using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Linq;

using TestFrozenDict = System.Collections.Frozen.FrozenDictionary<int, int>;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;


#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  public void ToFrozenDictionary_KeySelectorOnly ()
  {
    Func<int, int> keySelector = x => x * 2;
    TestKeyComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    TestFrozenDict? test = source.ToFrozenDictionary(keySelector, keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void ToFrozenDictionary_KeySelectorOnly_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    TestFrozenDict? test = explicitNull
      ? source.ToFrozenDictionary(x => x, null)!
      : source.ToFrozenDictionary(x => x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void ToFrozenDictionary_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    TestFrozenDict? test = returnsDefault
      ? source.ToFrozenDictionary(x => x, behavior: behavior!.Value)
      : source.ToFrozenDictionary(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void ToFrozenDictionary ()
  {
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = x => x * 3;
    TestKeyComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    TestFrozenDict? test = source.ToFrozenDictionary(keySelector, valueSelector, keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void ToFrozenDictionary_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    TestFrozenDict? test = explicitNull
      ? source.ToFrozenDictionary(x => x, x => x, null)!
      : source.ToFrozenDictionary(x => x, x => x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void ToFrozenDictionary_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    TestFrozenDict? test = returnsDefault
      ? source.ToFrozenDictionary(x => x, x => x, behavior: behavior!.Value)
      : source.ToFrozenDictionary(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }
}
