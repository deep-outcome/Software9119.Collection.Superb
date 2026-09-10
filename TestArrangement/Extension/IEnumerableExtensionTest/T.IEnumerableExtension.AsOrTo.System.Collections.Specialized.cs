using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{

  [TestMethod]
  [DataRow ( 1000, true )]
  [DataRow ( 1000, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void IntoHybridDictionary_IEnumerableOfT ( int? cap, bool keySelectorOnly )
  {
    Func<int, object> keySelector = x => x * 2;
    Func<int, object> valueSelector = keySelectorOnly ? x => x : x => x * 3;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    HybridDictionary test = cap is int
      ? keySelectorOnly
        ? source.IntoHybridDictionary(keySelector, cap)!
        : source.IntoHybridDictionary ( keySelector,valueSelector, cap )!
      : keySelectorOnly
        ? source.IntoHybridDictionary(keySelector)!
        : source.IntoHybridDictionary ( keySelector,valueSelector)!;

    IEnumerable<(int, int)> expectation = source
      .Select(x => ((int)keySelector(x), (int)valueSelector(x)));

    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );

    if (cap is int)
    {
      object hashtable = Reflection.GetNonPublicFieldValue(test, "hashtable");
      int _loadsize = (int)Reflection.GetNonPublicFieldValue(hashtable, "_loadsize");
      Assert.AreEqual ( 1149, _loadsize );
    }
  }

  [TestMethod]
  [DataRow ( true, true )]
  [DataRow ( false, true )]
  [DataRow ( true, false )]
  [DataRow ( false, false )]
  public void IntoHybridDictionary_IEnumerableOfT_CaseSensitivity ( bool caseSensitive, bool keySelectorOnly )
  {
    Func<int, object> keySelector = x => x * 2;
    Func<int, object> valueSelector = keySelectorOnly ? x => x : x => x * 3;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    HybridDictionary test = caseSensitive
      ? keySelectorOnly
        ? source.IntoHybridDictionary(keySelector)!
        : source.IntoHybridDictionary ( keySelector,valueSelector)!
      : keySelectorOnly
        ? source.IntoHybridDictionary(keySelector, caseSensitive: false)!
        : source.IntoHybridDictionary ( keySelector,valueSelector, caseSensitive: false)!;

    bool caseInsensitive = (bool)Reflection.GetNonPublicFieldValue(test, "caseInsensitive");
    Assert.AreEqual ( !caseSensitive, caseInsensitive );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault, true )]
  [DataRow ( NullBehavior.ReturnDefault, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void IntoHybridDictionary_IEnumerableOfT_NullBehavior ( NullBehavior? behavior, bool keySelectorOnly )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    HybridDictionary? test = returnsDefault
      ? keySelectorOnly
        ? source.IntoHybridDictionary(x => x, behavior: behavior!.Value)
        : source.IntoHybridDictionary(x => x, x=> x, behavior: behavior!.Value)
      : keySelectorOnly
        ? source.IntoHybridDictionary(x => x)
        : source.IntoHybridDictionary(x => x, x=> x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 1000, true )]
  [DataRow ( 1000, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void IntoHybridDictionary_IEnumerable ( int? cap, bool keySelectorOnly )
  {
    Func<object, object> keySelector = x => (int)x * 2;
    Func<object, object> valueSelector = keySelectorOnly ? x => x : x => (int)x * 3;

    IEnumerable source = Enumerable.Range(0, 10);
    HybridDictionary test = cap is int
      ? keySelectorOnly
        ? source.IntoHybridDictionary(keySelector, cap)!
        : source.IntoHybridDictionary ( keySelector,valueSelector, cap )!
      : keySelectorOnly
        ? source.IntoHybridDictionary(keySelector)!
        : source.IntoHybridDictionary ( keySelector,valueSelector)!;

    IEnumerable<(int, int)> expectation = source
      .Cast<object>()
      .Select(x => ((int)keySelector(x), (int)valueSelector(x)));

    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );

    if (cap is int)
    {
      object hashtable = Reflection.GetNonPublicFieldValue(test, "hashtable");
      int _loadsize = (int)Reflection.GetNonPublicFieldValue(hashtable, "_loadsize");
      Assert.AreEqual ( 1149, _loadsize );
    }
  }

  [TestMethod]
  [DataRow ( true, true )]
  [DataRow ( false, true )]
  [DataRow ( true, false )]
  [DataRow ( false, false )]
  public void IntoHybridDictionary_IEnumerable_CaseSensitivity ( bool caseSensitive, bool keySelectorOnly )
  {
    Func<object, object> keySelector = x => (int)x * 2;
    Func<object, object> valueSelector = keySelectorOnly ? x => x : x => (int)x * 3;

    IEnumerable source = Enumerable.Range(0, 10);
    HybridDictionary test = caseSensitive
      ? keySelectorOnly
        ? source.IntoHybridDictionary(keySelector)!
        : source.IntoHybridDictionary ( keySelector,valueSelector)!
      : keySelectorOnly
        ? source.IntoHybridDictionary ( keySelector, caseSensitive: false )!
        : source.IntoHybridDictionary ( keySelector, valueSelector, caseSensitive: false )!;

    bool caseInsensitive = (bool)Reflection.GetNonPublicFieldValue(test, "caseInsensitive");
    Assert.AreEqual ( !caseSensitive, caseInsensitive );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault, true )]
  [DataRow ( NullBehavior.ReturnDefault, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void IntoHybridDictionary_IEnumerable_NullBehavior ( NullBehavior? behavior, bool keySelectorOnly )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    HybridDictionary? test = returnsDefault
      ? keySelectorOnly
        ? source.IntoHybridDictionary(x => x, behavior: behavior!.Value)
        : source.IntoHybridDictionary(x => x, x=> x, behavior: behavior!.Value)
      : keySelectorOnly
        ? source.IntoHybridDictionary(x => x)
        : source.IntoHybridDictionary(x => x, x=> x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( true, true )]
  [DataRow ( true, false )]
  [DataRow ( false, true )]
  [DataRow ( false, false )]
  public void IntoListDictionary_IEnumerableOfT ( bool keySelectorOnly, bool defaultComparer )
  {
    IComparer keyComparer = defaultComparer ? Comparer.Default : new ReverseOrderComparer ();
    Func<int, object> keySelector = x => x * 2;
    Func<int, object> valueSelector = keySelectorOnly ? x => x : x => x * 3;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    ListDictionary test = defaultComparer
      ? keySelectorOnly
        ? source.IntoListDictionary(keySelector)!
        : source.IntoListDictionary(keySelector,valueSelector)!
      : keySelectorOnly
        ? source.IntoListDictionary(keySelector,keyComparer)!
        : source.IntoListDictionary(keySelector,valueSelector,keyComparer)!;

    object _comparer = Reflection.GetNonPublicFieldValue ( test, "comparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    IEnumerable<(int, int)> expectation = source
      .Select(x => ((int)keySelector(x), (int)valueSelector(x)));

    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!));

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault, true )]
  [DataRow ( NullBehavior.ReturnDefault, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void IntoListDictionary_IEnumerableOfT_NullBehavior ( NullBehavior? behavior, bool keySelectorOnly )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ListDictionary? test = returnsDefault
      ? keySelectorOnly
        ? source.IntoListDictionary(x => x, behavior: behavior!.Value)
        : source.IntoListDictionary(x => x, x=> x, behavior: behavior!.Value)
      : keySelectorOnly
        ? source.IntoListDictionary(x => x)
        : source.IntoListDictionary(x => x, x=> x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( true, true )]
  [DataRow ( true, false )]
  [DataRow ( false, true )]
  [DataRow ( false, false )]
  public void IntoListDictionary_IEnumerable ( bool keySelectorOnly, bool defaultComparer )
  {
    IComparer keyComparer = defaultComparer ? Comparer.Default : new ReverseOrderComparer ();
    Func<object, object> keySelector = x => (int) x * 2;
    Func<object, object> valueSelector = keySelectorOnly ? x => x : x => (int)x * 3;

    IEnumerable source = Enumerable.Range(0, 10).Cast<object>();
    ListDictionary test = defaultComparer
      ? keySelectorOnly
        ? source.IntoListDictionary(keySelector)!
        : source.IntoListDictionary(keySelector,valueSelector)!
      : keySelectorOnly
        ? source.IntoListDictionary(keySelector,keyComparer)!
        : source.IntoListDictionary(keySelector,valueSelector,keyComparer)!;

    object _comparer = Reflection.GetNonPublicFieldValue ( test, "comparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    IEnumerable<(int, int)> expectation = source
      .Cast<object>()
      .Select(x => ((int)keySelector(x), (int)valueSelector(x)));

    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!));

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault, true )]
  [DataRow ( NullBehavior.ReturnDefault, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void IntoListDictionary_IEnumerable_NullBehavior ( NullBehavior? behavior, bool keySelectorOnly )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ListDictionary? test = returnsDefault
      ? keySelectorOnly
        ? source.IntoListDictionary(x => x, behavior: behavior!.Value)
        : source.IntoListDictionary(x => x, x=> x, behavior: behavior!.Value)
      : keySelectorOnly
        ? source.IntoListDictionary(x => x)
        : source.IntoListDictionary(x => x, x=> x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }
}
