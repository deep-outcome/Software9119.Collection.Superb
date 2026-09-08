using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
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
}
