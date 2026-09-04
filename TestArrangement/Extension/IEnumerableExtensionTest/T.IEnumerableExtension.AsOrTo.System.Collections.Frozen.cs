using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

using fzDictionary = System.Collections.Frozen.FrozenDictionary<int, int>;
using fzSet = System.Collections.Frozen.FrozenSet<int>;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;


#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  public void IntoFrozenDictionary_KeySelectorOnly ()
  {
    Func<int, int> keySelector = x => x * 2;
    TestComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    fzDictionary test = source.IntoFrozenDictionary(keySelector, keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoFrozenDictionary_KeySelectorOnly_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    fzDictionary test = explicitNull
      ? source.IntoFrozenDictionary(x => x, keyComparer: null)!
      : source.IntoFrozenDictionary(x => x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoFrozenDictionary_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    fzDictionary? test = returnsDefault
      ? source.IntoFrozenDictionary(x => x, behavior: behavior!.Value)
      : source.IntoFrozenDictionary(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoFrozenDictionary ()
  {
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = x => x * 3;
    TestComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    fzDictionary test = source.IntoFrozenDictionary(keySelector, valueSelector, keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoFrozenDictionary_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    fzDictionary test = explicitNull
      ? source.IntoFrozenDictionary(x => x, x => x, keyComparer: null)!
      : source.IntoFrozenDictionary(x => x, x => x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoFrozenDictionary_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    fzDictionary? test = returnsDefault
      ? source.IntoFrozenDictionary(x => x, x => x, behavior: behavior!.Value)
      : source.IntoFrozenDictionary(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void AsOrToFrozenSet ()
  {
    TestComparer<int> itemComparer = new ();
    IEnumerable<int> source = Enumerable.Range(0, 10);
    fzSet test = source.AsOrToFrozenSet(itemComparer)!;

    Assert.IsTrue ( ReferenceEquals ( itemComparer, test.Comparer ) );
    Assert.IsTrue ( source.SequenceEqual ( test.OrderBy ( x => x ) ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void AsOrToFrozenSet_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    fzSet test = explicitNull
      ? source.AsOrToFrozenSet(itemComparer: null)!
      : source.AsOrToFrozenSet()!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToFrozenSet_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    fzSet? test = returnsDefault
      ? source.AsOrToFrozenSet(behavior: behavior!.Value)
      : source.AsOrToFrozenSet();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  // readme

#pragma warning disable IDE0001
  [TestMethod]
  public void Frozen_Sample ()
  {
    Func<int, int> keySelector = x => x * 10;
    Func<int, int> valueSelector = x => x * 20;
    IEnumerable<int> source = Enumerable.Range(0, 10);

    FrozenDictionary<int, int> list = source.IntoFrozenDictionary(keySelector, valueSelector, behavior: EnumerableNullBehavior.ReturnDefault)!;
    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( list ) );
  }
#pragma warning restore IDE0001
}
