using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void AsOrToCollection ( int? capacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    Collection<int> test = capacity is int
      ? source.AsOrToCollection(capacity)!
      : source.AsOrToCollection()!;

    List<int> items = (List<int>) Reflection.GetNonPublicFieldValue ( test, "items" );
    Assert.AreEqual ( capacity ?? 16, items.Capacity );

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToCollection_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Collection<int>? test = returnsDefault
      ? source.AsOrToCollection(behavior: behavior!.Value)
      : source.AsOrToCollection();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void AsOrToObservableCollection ()
  {
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ObservableCollection<int> test = source.AsOrToObservableCollection()!;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToObservableCollection_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ObservableCollection<int>? test = returnsDefault
      ? source.AsOrToObservableCollection(behavior!.Value)
      : source.AsOrToObservableCollection();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void AsOrToReadOnlyCollection ( int? capacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    ReadOnlyCollection<int> test = capacity is int
      ? source.AsOrToReadOnlyCollection(capacity)!
      : source.AsOrToReadOnlyCollection()!;

    List<int> list = (List<int>) Reflection.GetNonPublicFieldValue ( test, "list" );
    Assert.AreEqual ( capacity ?? 16, list.Capacity );

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToReadOnlyCollection_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ReadOnlyCollection<int>? test = returnsDefault
      ? source.AsOrToReadOnlyCollection(behavior: behavior!.Value)
      : source.AsOrToReadOnlyCollection();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 100, 107, true )]
  [DataRow ( 100, 107, false )]
  [DataRow ( null, 11, true )]
  [DataRow ( null, 11, false )]
  public void IntoReadOnlyDictionary ( int? cap, int expCap, bool keySelectorOnly )
  {
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = keySelectorOnly ? x => x: x => x * 3;
    TestComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    ReadOnlyDictionary<int, int>? test = cap is int
      ? keySelectorOnly
        ? source.IntoReadOnlyDictionary(keySelector, cap, keyComparer: keyComparer)!
        : source.IntoReadOnlyDictionary(keySelector, valueSelector, cap, keyComparer: keyComparer)!
      : keySelectorOnly
        ? source.IntoReadOnlyDictionary ( keySelector, keyComparer: keyComparer )!
        : source.IntoReadOnlyDictionary ( keySelector, valueSelector, keyComparer: keyComparer )!;

    Dictionary<int, int> innerDict = (Dictionary<int, int>)Reflection
      .GetNonPublicFieldValue ( test, "m_dictionary" );

    Assert.IsTrue ( ReferenceEquals ( keyComparer, innerDict.Comparer ) );
    Assert.AreEqual ( expCap, innerDict.Capacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source
    .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoReadOnlyDictionary_KeySelectorOnly_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    ReadOnlyDictionary<int, int>? test = explicitNull
    ? source.IntoReadOnlyDictionary(x => x, keyComparer: null)!
    : source.IntoReadOnlyDictionary(x => x)!;

    Dictionary<int, int> innerDict = (Dictionary<int, int>)Reflection
      .GetNonPublicFieldValue ( test, "m_dictionary" );

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, innerDict.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoReadOnlyDictionary_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ReadOnlyDictionary<int, int>? test = returnsDefault
    ? source.IntoReadOnlyDictionary(x => x, behavior: behavior!.Value)
    : source.IntoReadOnlyDictionary(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoReadOnlyDictionary_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    ReadOnlyDictionary<int, int>? test = explicitNull
    ? source.IntoReadOnlyDictionary(x => x, x => x, keyComparer: null)!
    : source.IntoReadOnlyDictionary(x => x, x => x)!;

    Dictionary<int, int> innerDict = (Dictionary<int, int>)Reflection
      .GetNonPublicFieldValue ( test, "m_dictionary" );

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, innerDict.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoReadOnlyDictionary_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ReadOnlyDictionary<int, int>? test = returnsDefault
    ? source.IntoReadOnlyDictionary(x => x, x => x, behavior: behavior!.Value)
    : source.IntoReadOnlyDictionary(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void AsOrToReadOnlyObservableCollection ()
  {
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ReadOnlyObservableCollection<int> test = source.AsOrToReadOnlyObservableCollection()!;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToReadOnlyObservableCollection_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ReadOnlyObservableCollection<int>? test = returnsDefault
      ? source.AsOrToReadOnlyObservableCollection(behavior!.Value)
      : source.AsOrToReadOnlyObservableCollection();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( false, 1000 )]
  [DataRow ( true, null )]
  public void AsOrToReadOnlySet_HashSet ( bool defaultComparer, int? capacity )
  {
    IEqualityComparer<int> comparer = defaultComparer
      ? EqualityComparer<int>.Default
      : new TestComparer<int> ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    ReadOnlySet<int>? test = defaultComparer
      ? source.AsOrToReadOnlySet()!
      : source.AsOrToReadOnlySet(capacity, equalityComparer: comparer)!;

    HashSet<int> set = (HashSet<int>)Reflection.GetNonPublicFieldValue ( test, "_set" );
    Assert.IsTrue ( ReferenceEquals ( comparer, set.Comparer ) );
    Assert.AreEqual ( capacity is int ? 1103 : 11, set.Capacity );

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void AsOrToReadOnlySet_FrozenSet ( bool defaultComparer )
  {
    IEqualityComparer<int> comparer = defaultComparer
      ? EqualityComparer<int>.Default
      : new TestComparer<int> ();

    ReadOnlySetType setType = ReadOnlySetType.FrozenSet;
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ReadOnlySet<int>? test = defaultComparer
      ? source.AsOrToReadOnlySet(setType: setType)!
      : source.AsOrToReadOnlySet(equalityComparer: comparer, setType: setType)!;

    FrozenSet<int> set = (FrozenSet<int>)Reflection.GetNonPublicFieldValue ( test, "_set" );
    Assert.IsTrue ( ReferenceEquals ( comparer, set.Comparer ) ); ;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void AsOrToReadOnlySet_ImmutableHashSet ( bool defaultComparer )
  {
    IEqualityComparer<int> comparer = defaultComparer
      ? EqualityComparer<int>.Default
      : new TestComparer<int> ();

    ReadOnlySetType setType = ReadOnlySetType.ImmutableHashSet;
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ReadOnlySet<int>? test = defaultComparer
      ? source.AsOrToReadOnlySet(setType: setType)!
      : source.AsOrToReadOnlySet(equalityComparer: comparer, setType: setType)!;

    ImmutableHashSet<int> set = (ImmutableHashSet<int>)Reflection.GetNonPublicFieldValue ( test, "_set" );
    Assert.IsTrue ( ReferenceEquals ( comparer, set.KeyComparer ) ); ;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void AsOrToReadOnlySet_ImmutableSortedSet ( bool defaultComparer )
  {
    IComparer<int> comparer = defaultComparer
      ? Comparer<int>.Default
      : new ReverseOrderComparer<int> ();

    ReadOnlySetType setType = ReadOnlySetType.ImmutableSortedSet;
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ReadOnlySet<int>? test = defaultComparer
      ? source.AsOrToReadOnlySet(setType: setType)!
      : source.AsOrToReadOnlySet(sortingComparer: comparer, setType: setType)!;

    ImmutableSortedSet<int> set = (ImmutableSortedSet<int>)Reflection.GetNonPublicFieldValue ( test, "_set" );
    Assert.IsTrue ( ReferenceEquals ( comparer, set.KeyComparer ) ); ;

    IEnumerable<int> expectation = defaultComparer ? source : source.Reverse();
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( false )]
  [DataRow ( true )]
  public void AsOrToReadOnlySet_SortedSet ( bool defaultComparer )
  {
    IComparer<int> comparer = defaultComparer
      ? Comparer<int>.Default
      : new ReverseOrderComparer<int> ();

    ReadOnlySetType setType = ReadOnlySetType.SortedSet;
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ReadOnlySet<int>? test = defaultComparer
      ? source.AsOrToReadOnlySet(setType: setType)!
      : source.AsOrToReadOnlySet(sortingComparer: comparer, setType: setType)!;

    SortedSet<int> set = (SortedSet<int>)Reflection.GetNonPublicFieldValue ( test, "_set" );
    Assert.IsTrue ( ReferenceEquals ( comparer, set.Comparer ) ); ;

    IEnumerable<int> expectation = defaultComparer ? source : source.Reverse();
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToReadOnlySet_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ReadOnlySet<int>? test = returnsDefault
      ? source.AsOrToReadOnlySet(behavior: behavior!.Value)
      : source.AsOrToReadOnlySet();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void AsOrToReadOnlySet_UnsupportedReadOnlySetType ()
  {
    Action test = () => _ = ((int []?) null).AsOrToReadOnlySet ( setType: (ReadOnlySetType)999);
    UnsupportedReadOnlySetTypeException e = Assert.ThrowsExactly<UnsupportedReadOnlySetTypeException> ( test );
    Assert.AreEqual ( "Unsupported set type, '999'. (Parameter 'setType')", e.Message );
  }

  // readme
  [TestMethod]
  public void Sample_ObjectModel ()
  {
    // read-only set sample
    IComparer<int> comparer = new MyOrderingComparer<int>();
    ReadOnlySetType setType = ReadOnlySetType.SortedSet;
    ReadOnlySet<int> readOnlySet = Enumerable.Range(0, 10)
      .AsOrToReadOnlySet(setType: setType, sortingComparer: comparer)!;

    IEnumerable<int> expectation = Enumerable.Range(0, 10).Reverse();
    Assert.IsTrue ( expectation.SequenceEqual ( readOnlySet ) );
  }
}
