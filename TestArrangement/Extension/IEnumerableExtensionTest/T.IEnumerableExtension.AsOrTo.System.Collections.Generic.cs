using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  [DataRow ( 1000, 1103 )]
  [DataRow ( null, 11 )]
  public void IntoDictionary_KeySelectorOnly ( int? cap, int expCap )
  {
    Func<int, int> keySelector = x => x * 2;
    TestComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    Dictionary<int, int> test = cap is int
      ? source.IntoDictionary(keySelector, keyComparer, cap)!
      : source.IntoDictionary(keySelector, keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
    Assert.AreEqual ( expCap, test.Capacity );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoDictionary_KeySelectorOnly_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    Dictionary<int, int>? test = explicitNull
    ? source.IntoDictionary(x => x, keyComparer: null)!
    : source.IntoDictionary(x => x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoDictionary_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Dictionary<int, int>? test = returnsDefault
    ? source.IntoDictionary(x => x, behavior: behavior!.Value)
    : source.IntoDictionary(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 1000, 1103 )]
  [DataRow ( null, 11 )]
  public void IntoDictionary ( int? cap, int expCap )
  {
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = x => x * 3;
    TestComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    Dictionary<int, int>? test = cap is int
      ? source.IntoDictionary(keySelector, valueSelector, cap, keyComparer: keyComparer)!
      : source.IntoDictionary(keySelector, valueSelector, keyComparer: keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
    .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
    Assert.AreEqual ( expCap, test.Capacity );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoDictionary_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    Dictionary<int, int>? test = explicitNull
    ? source.IntoDictionary(x => x, x => x, keyComparer: null)!
    : source.IntoDictionary(x => x, x => x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoDictionary_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Dictionary<int, int>? test = returnsDefault
    ? source.IntoDictionary(x => x, x => x, behavior: behavior!.Value)
    : source.IntoDictionary(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 100, 107 )]
  [DataRow ( null, 11 )]
  public void AsOrToHashSet ( int? capacityRequested, int capacityGotten )
  {
    TestComparer<int> itemComparer = new ();
    IEnumerable<int> source = Enumerable.Range(0, 10);
    HashSet<int> test = capacityRequested is int
      ? source.AsOrToHashSet(capacityRequested, itemComparer: itemComparer)!
      : source.AsOrToHashSet(itemComparer: itemComparer)!;

    Assert.IsTrue ( ReferenceEquals ( itemComparer, test.Comparer ) );
    Assert.IsTrue ( source.SequenceEqual ( test.OrderBy ( x => x ) ) );
    Assert.AreEqual ( capacityGotten, test.Capacity );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void AsOrToHashSet_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    HashSet<int>? test = explicitNull
      ? source.AsOrToHashSet(itemComparer: null)!
      : source.AsOrToHashSet()!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToHashSet_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    HashSet<int>? test = returnsDefault
      ? source.AsOrToHashSet(behavior: behavior!.Value)
      : source.AsOrToHashSet();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void AsOrToLinkedList ()
  {
    IEnumerable<int> source = Enumerable.Range(0, 10);
    LinkedList<int> test = source.AsOrToLinkedList()!;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToLinkedList_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    LinkedList<int>? test = returnsDefault
      ? source.AsOrToLinkedList(behavior!.Value)
      : source.AsOrToLinkedList();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void AsOrToList ( int? capacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    List<int> test = capacity is int
      ? source.AsOrToList(capacity)!
      : source.AsOrToList()!;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.AreEqual ( capacity ?? 16, test.Capacity );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToList_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    List<int>? test = returnsDefault
      ? source.AsOrToList(behavior: behavior!.Value)
      : source.AsOrToList();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 1000, 1103 )]
  [DataRow ( null, 17 )]
  public void IntoOrderedDictionary_KeySelectorOnly ( int? cap, int expCap )
  {
    Func<int, int> keySelector = x => x * 2;
    TestComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    OrderedDictionary<int, int> test = cap is int
      ? source.IntoOrderedDictionary(keySelector, keyComparer, cap)!
      : source.IntoOrderedDictionary(keySelector, keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
    Assert.AreEqual ( expCap, test.Capacity );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoOrderedDictionary_KeySelectorOnly_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    OrderedDictionary<int, int>? test = explicitNull
    ? source.IntoOrderedDictionary(x => x, keyComparer: null)!
    : source.IntoOrderedDictionary(x => x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoOrderedDictionary_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    OrderedDictionary<int, int>? test = returnsDefault
    ? source.IntoOrderedDictionary(x => x, behavior: behavior!.Value)
    : source.IntoOrderedDictionary(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 1000, 1103 )]
  [DataRow ( null, 17 )]
  public void IntoOrderedDictionary ( int? cap, int expCap )
  {
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = x => x * 3;
    TestComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    OrderedDictionary<int, int>? test = cap is int
      ? source.IntoOrderedDictionary(keySelector, valueSelector, cap, keyComparer: keyComparer)!
      : source.IntoOrderedDictionary(keySelector, valueSelector, keyComparer: keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
    .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
    Assert.AreEqual ( expCap, test.Capacity );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoOrderedDictionary_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    OrderedDictionary<int, int>? test = explicitNull
    ? source.IntoOrderedDictionary(x => x, x => x, keyComparer: null)!
    : source.IntoOrderedDictionary(x => x, x => x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoOrderedDictionary_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    OrderedDictionary<int, int>? test = returnsDefault
    ? source.IntoOrderedDictionary(x => x, x => x, behavior: behavior!.Value)
    : source.IntoOrderedDictionary(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  // readme

  [TestMethod]
  public void Generic_Sample ()
  {
    MyKeyComparer<int> comparer = new ();
    Func<int, int> keySelector = x => x * 10;
    Func<int, int> valueSelector = x => x * 20;
    IEnumerable<int> source = Enumerable.Range(0, 10);

    OrderedDictionary<int, int> list = source.IntoOrderedDictionary(keySelector, valueSelector, keyComparer: comparer)!;
    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( list ) );
  }
}
