using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
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

  [TestMethod]
  [DataRow ( true, 1000 )]
  [DataRow ( false, 1000 )]
  [DataRow ( true, null )]
  [DataRow ( false, null )]
  [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Ok." )]
  public void IntoNameValueCollection_IEnumerableOfT ( bool defaultComparer, int? capacity )
  {
    IEqualityComparer keyComparer = new TestComparer ();
    Func<int, string> keySelector = x => (x * 2).ToString();
    Func<int, string> valueSelector = x => (x *3).ToString();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    NameValueCollection test = capacity is int
      ? defaultComparer
        ? source.IntoNameValueCollection(keySelector,valueSelector, capacity)!
        : source.IntoNameValueCollection(keySelector,valueSelector, capacity, keyComparer)!
      : defaultComparer
        ? source.IntoNameValueCollection ( keySelector, valueSelector)!
        : source.IntoNameValueCollection ( keySelector, valueSelector, keyComparer: keyComparer)!;

    object _comparer = Reflection.GetNonPublicFieldValue<NameObjectCollectionBase>  ( test, "_keyComparer" );

    if (defaultComparer)
      Assert.AreEqual ( "CultureAwareComparer", _comparer.GetType ().Name );
    else
      Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    ArrayList _entriesArray = (ArrayList)Reflection.GetNonPublicFieldValue<NameObjectCollectionBase> ( test, "_entriesArray" );
    Assert.AreEqual ( capacity ?? 16, _entriesArray.Capacity );

    IEnumerable<(string, string)> expectation = source
      .Select(x => (keySelector(x), valueSelector(x)));

    IEnumerable<(string, string?)> actual = test.Cast<string> ().Select(x => (x, test[x]));

    Assert.IsTrue ( expectation.SequenceEqual ( actual! ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoNameValueCollection_IEnumerableOfT_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    NameValueCollection? test = returnsDefault
        ? source.IntoNameValueCollection(x => "", x => "", behavior: behavior!.Value)
        : source.IntoNameValueCollection(x => "", x => "");

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( true, 1000 )]
  [DataRow ( false, 1000 )]
  [DataRow ( true, null )]
  [DataRow ( false, null )]
  [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Ok." )]
  public void IntoNameValueCollection_IEnumerable ( bool defaultComparer, int? capacity )
  {
    IEqualityComparer keyComparer = new TestComparer ();
    Func<object, string> keySelector = x => ((int)x * 2).ToString();
    Func<object, string> valueSelector = x => ((int)x *3).ToString();

    IEnumerable source = Enumerable.Range(0, 10).Cast<object>();
    NameValueCollection test = capacity is int
      ? defaultComparer
        ? source.IntoNameValueCollection(keySelector,valueSelector, capacity)!
        : source.IntoNameValueCollection(keySelector,valueSelector, capacity, keyComparer)!
      : defaultComparer
        ? source.IntoNameValueCollection ( keySelector, valueSelector)!
        : source.IntoNameValueCollection ( keySelector, valueSelector, keyComparer: keyComparer)!;

    object _comparer = Reflection.GetNonPublicFieldValue<NameObjectCollectionBase>  ( test, "_keyComparer" );

    if (defaultComparer)
      Assert.AreEqual ( "CultureAwareComparer", _comparer.GetType ().Name );
    else
      Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    ArrayList _entriesArray = (ArrayList)Reflection.GetNonPublicFieldValue<NameObjectCollectionBase> ( test, "_entriesArray" );
    Assert.AreEqual ( capacity ?? 16, _entriesArray.Capacity );

    IEnumerable<(string, string)> expectation = source
      .Cast<object>()
      .Select(x => (keySelector(x), valueSelector(x)));

    IEnumerable<(string, string?)> actual = test.Cast<string> ().Select(x => (x, test[x]));

    Assert.IsTrue ( expectation.SequenceEqual ( actual! ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoNameValueCollection_IEnumerable_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    NameValueCollection? test = returnsDefault
        ? source.IntoNameValueCollection(x => "", x => "", behavior: behavior!.Value)
        : source.IntoNameValueCollection(x => "", x => "");

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( true, 1000 )]
  [DataRow ( false, 1000 )]
  [DataRow ( true, null )]
  [DataRow ( false, null )]
  public void IntoOrderedDictionary_IEnumerableOfT ( bool keySelectorOnly, int? capacity )
  {
    IEqualityComparer keyComparer = new TestComparer ();
    Func<int, object> keySelector = x => x * 2;
    Func<int, object> valueSelector = keySelectorOnly ? x => x : x => x *3;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    OrderedDictionary test = capacity is int
      ? keySelectorOnly
        ? source.IntoOrderedDictionary (keySelector, capacity, keyComparer)!
        : source.IntoOrderedDictionary (keySelector,valueSelector, capacity, keyComparer)!
      : keySelectorOnly
        ? source.IntoOrderedDictionary (keySelector, keyComparer: keyComparer)!
        : source.IntoOrderedDictionary (keySelector,valueSelector, keyComparer: keyComparer)!;

    object _comparer = Reflection.GetNonPublicFieldValue ( test, "_comparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    int _initialCapacity = (int)Reflection.GetNonPublicFieldValue ( test, "_initialCapacity" );
    Assert.AreEqual ( capacity ?? 0, _initialCapacity );

    IEnumerable<(int, object)> expectation = source
      .Select(x => ((int)keySelector(x), valueSelector(x)));

    IEnumerable<(int, object?)> actual = test
      .Cast<DictionaryEntry> ()
      .Select(x => ((int)x.Key, x.Value));

    Assert.IsTrue ( expectation.SequenceEqual ( actual! ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoOrderedDictionary_IEnumerableOfT_DefaultComparer ( bool keySelectorOnly )
  {
    Func<int, object> selector = x => x;

    IEnumerable<int> source = [];
    OrderedDictionary  test = keySelectorOnly
        ? source.IntoOrderedDictionary (selector, capacity: null)!
        : source.IntoOrderedDictionary (selector, selector, capacity: null)!;

    object _comparer = Reflection.GetNonPublicFieldValue ( test, "_comparer" );
    Assert.IsNull ( _comparer );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault, true )]
  [DataRow ( NullBehavior.ReturnDefault, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void IntoOrderedDictionary_IEnumerableOfT_NullBehavior ( NullBehavior? behavior, bool keySelectorOnly )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    OrderedDictionary ? test = returnsDefault
      ? keySelectorOnly
        ? source.IntoOrderedDictionary (x => null!, behavior: behavior!.Value)
        : source.IntoOrderedDictionary (x => null!, x=> null!, behavior: behavior!.Value)
      : keySelectorOnly
        ? source.IntoOrderedDictionary (x => null!)
        : source.IntoOrderedDictionary (x => null!, x=> null!);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( true, 1000 )]
  [DataRow ( false, 1000 )]
  [DataRow ( true, null )]
  [DataRow ( false, null )]
  public void IntoOrderedDictionary_IEnumerable ( bool keySelectorOnly, int? capacity )
  {
    IEqualityComparer keyComparer = new TestComparer ();
    Func<object, object> keySelector = x => (int)x * 2;
    Func<object, object> valueSelector = keySelectorOnly ? x => x : x => (int)x *3;

    IEnumerable source = Enumerable.Range(0, 10);
    OrderedDictionary  test = capacity is int
      ? keySelectorOnly
        ? source.IntoOrderedDictionary (keySelector, capacity, keyComparer)!
        : source.IntoOrderedDictionary (keySelector,valueSelector, capacity, keyComparer)!
      : keySelectorOnly
        ? source.IntoOrderedDictionary (keySelector, keyComparer: keyComparer)!
        : source.IntoOrderedDictionary (keySelector,valueSelector, keyComparer: keyComparer)!;

    object _comparer = Reflection.GetNonPublicFieldValue ( test, "_comparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    int _initialCapacity = (int)Reflection.GetNonPublicFieldValue ( test, "_initialCapacity" );
    Assert.AreEqual ( capacity ?? 0, _initialCapacity );

    IEnumerable<(int, object)> expectation = source
      .Cast<int>()
      .Select(x => ((int)keySelector(x), valueSelector(x)));

    IEnumerable<(int, object?)> actual = test
      .Cast<DictionaryEntry> ()
      .Select(x => ((int)x.Key, x.Value));

    Assert.IsTrue ( expectation.SequenceEqual ( actual! ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoOrderedDictionary_IEnumerable_DefaultComparer ( bool keySelectorOnly )
  {
    Func<object, object> selector = x => x;

    IEnumerable source = Enumerable.Range(0, 10);
    OrderedDictionary  test = keySelectorOnly
        ? source.IntoOrderedDictionary (selector, capacity: null)!
        : source.IntoOrderedDictionary (selector, selector, capacity: null)!;

    object _comparer = Reflection.GetNonPublicFieldValue ( test, "_comparer" );
    Assert.IsNull ( _comparer );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault, true )]
  [DataRow ( NullBehavior.ReturnDefault, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void IntoOrderedDictionary_IEnumerable_NullBehavior ( NullBehavior? behavior, bool keySelectorOnly )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    OrderedDictionary? test = returnsDefault
      ? keySelectorOnly
        ? source.IntoOrderedDictionary(x => x, behavior: behavior!.Value)
        : source.IntoOrderedDictionary(x => x, x=> x, behavior: behavior!.Value)
      : keySelectorOnly
        ? source.IntoOrderedDictionary(x => x)
        : source.IntoOrderedDictionary(x => x, x=> x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }
}
