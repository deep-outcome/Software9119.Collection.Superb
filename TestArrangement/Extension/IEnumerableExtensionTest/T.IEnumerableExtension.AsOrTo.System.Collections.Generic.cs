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
      ? source.IntoDictionary(keySelector, cap, keyComparer)!
      : source.IntoDictionary(keySelector, keyComparer: keyComparer)!;

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
      ? source.IntoOrderedDictionary(keySelector, cap, keyComparer)!
      : source.IntoOrderedDictionary(keySelector, keyComparer: keyComparer)!;

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

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void IntoPriorityQueue ( int? capacity )
  {
    ReverseOrderComparer<int> priorityComparer = new ();
    IEnumerable<(int, int)> source = Enumerable.Range(0, 10).Select(x => (x, x*2 ));
    PriorityQueue<int, int> test = capacity is int
      ? source.IntoPriorityQueue(capacity, priorityComparer: priorityComparer)!
      : source.IntoPriorityQueue(priorityComparer: priorityComparer)!;

    Assert.IsTrue ( ReferenceEquals ( priorityComparer, test.Comparer ) );

    List<int> list = [];
    while (test.Count > 0)
      list.Add ( test.Dequeue () );

    Assert.IsTrue ( source.Select ( x => x.Item1 ).Reverse ().SequenceEqual ( list ) );
    Assert.AreEqual ( capacity ?? 16, test.Capacity );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoPriorityQueue_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<(int, int)> source = [];
    PriorityQueue<int, int>? test = explicitNull
      ? source.IntoPriorityQueue(priorityComparer: null)!
      : source.IntoPriorityQueue()!;

    Assert.IsTrue ( ReferenceEquals ( Comparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoPriorityQueue_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<(int, int)> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    PriorityQueue<int, int>? test = returnsDefault
      ? source.IntoPriorityQueue(behavior: behavior!.Value)
      : source.IntoPriorityQueue();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void AsOrToTypedQueue ( int? capacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    Queue<int> test = capacity is int
      ? source.AsOrToTypedQueue(capacity)!
      : source.AsOrToTypedQueue()!;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.AreEqual ( capacity ?? 16, test.Capacity );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToTypedQueue_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Queue<int>? test = returnsDefault
      ? source.AsOrToTypedQueue(behavior: behavior!.Value)
      : source.AsOrToTypedQueue();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoSortedDictionary_KeySelectorOnly ()
  {
    Func<int, int> keySelector = x => x * 2;
    ReverseOrderComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    SortedDictionary<int, int> test = source.IntoSortedDictionary(keySelector, keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );
    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), x))
      .OrderByDescending(x => x.Key);
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoSortedDictionary_KeySelectorOnly_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    SortedDictionary<int, int>? test = explicitNull
    ? source.IntoSortedDictionary(x => x, keyComparer: null)!
    : source.IntoSortedDictionary(x => x)!;

    Assert.IsTrue ( ReferenceEquals ( Comparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoSortedDictionary_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    SortedDictionary<int, int>? test = returnsDefault
    ? source.IntoSortedDictionary(x => x, behavior: behavior!.Value)
    : source.IntoSortedDictionary(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoSortedDictionary ()
  {
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = x => x * 3;
    ReverseOrderComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    SortedDictionary<int, int>? test = source.IntoSortedDictionary(keySelector, valueSelector, keyComparer: keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
    .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)))
    .OrderByDescending(x => x.Key);
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoSortedDictionary_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    SortedDictionary<int, int>? test = explicitNull
    ? source.IntoSortedDictionary(x => x, x => x, keyComparer: null)!
    : source.IntoSortedDictionary(x => x, x => x)!;

    Assert.IsTrue ( ReferenceEquals ( Comparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoSortedDictionary_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    SortedDictionary<int, int>? test = returnsDefault
    ? source.IntoSortedDictionary(x => x, x => x, behavior: behavior!.Value)
    : source.IntoSortedDictionary(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 1000 )]
  [DataRow ( null )]
  public void IntoTypedSortedList_KeySelectorOnly ( int? cap )
  {
    Func<int, int> keySelector = x => x * 2;
    ReverseOrderComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    SortedList<int, int> test = cap is int
      ? source.IntoTypedSortedList(keySelector, cap, keyComparer)!
      : source.IntoTypedSortedList(keySelector, keyComparer: keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), x))
      .OrderByDescending(x => x.Key);
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
    Assert.AreEqual ( cap ?? 16, test.Capacity );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoTypedSortedList_KeySelectorOnly_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    SortedList<int, int>? test = explicitNull
    ? source.IntoTypedSortedList(x => x, keyComparer: null)!
    : source.IntoTypedSortedList(x => x)!;

    Assert.IsTrue ( ReferenceEquals ( Comparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoTypedSortedList_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    SortedList<int, int>? test = returnsDefault
    ? source.IntoTypedSortedList(x => x, behavior: behavior!.Value)
    : source.IntoTypedSortedList(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  [DataRow ( 1000 )]
  [DataRow ( null )]
  public void IntoTypedSortedList ( int? cap )
  {
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = x => x * 3;
    ReverseOrderComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    SortedList<int, int>? test = cap is int
      ? source.IntoTypedSortedList(keySelector, valueSelector, cap, keyComparer: keyComparer)!
      : source.IntoTypedSortedList(keySelector, valueSelector, keyComparer: keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
    .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)))
    .OrderByDescending(x => x.Key);
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
    Assert.AreEqual ( cap ?? 16, test.Capacity );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoTypedSortedList_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    SortedList<int, int>? test = explicitNull
    ? source.IntoTypedSortedList(x => x, x => x, keyComparer: null)!
    : source.IntoTypedSortedList(x => x, x => x)!;

    Assert.IsTrue ( ReferenceEquals ( Comparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoTypedSortedList_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    SortedList<int, int>? test = returnsDefault
    ? source.IntoTypedSortedList(x => x, x => x, behavior: behavior!.Value)
    : source.IntoTypedSortedList(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void AsOrToSortedSet ()
  {
    ReverseOrderComparer<int> itemComparer = new ();
    IEnumerable<int> source = Enumerable.Range(0, 10);
    SortedSet<int> test = source.AsOrToSortedSet(itemComparer: itemComparer)!;

    Assert.IsTrue ( ReferenceEquals ( itemComparer, test.Comparer ) );
    Assert.IsTrue ( source.Reverse ().SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void AsOrToSortedSet_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    SortedSet<int>? test = explicitNull
      ? source.AsOrToSortedSet(itemComparer: null)!
      : source.AsOrToSortedSet()!;

    Assert.IsTrue ( ReferenceEquals ( Comparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToSortedSet_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    SortedSet<int>? test = returnsDefault
      ? source.AsOrToSortedSet(behavior: behavior!.Value)
      : source.AsOrToSortedSet();

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
