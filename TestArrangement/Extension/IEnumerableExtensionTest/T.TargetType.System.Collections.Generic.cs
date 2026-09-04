using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_generic_test
{
  [TestMethod]
  [DataRow ( 100, 107 )]
  [DataRow ( null, 11 )]
  public void Dictionary_KeySelectorOnly ( int? capacityRequested, int capacityGotten )
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;

    AsOrToTargetType<Dictionary<int, int>> targetType = system_collections_generic.Dictionary ( keySelector, keyComparer );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Dictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Dictionary<int, int> target = targetType.Ctor(source, capacityRequested);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );
    Assert.AreEqual ( capacityGotten, target.Capacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", false, true )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", true, false )]
  public void Dictionary_KeySelectorOnly_NullParameter ( string errMsg, bool nullComparer, bool nullSelector )
  {
    TestComparer<int> keyComparer = nullComparer ? null! : new ();
    Func<int, int> keySelector = nullSelector ? null! : x => x;

    Action test = () => system_collections_generic.Dictionary ( keySelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 100, 107 )]
  [DataRow ( null, 11 )]
  public void Dictionary ( int? capacityRequested, int capacityGotten )
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;

    AsOrToTargetType<Dictionary<int, int>> targetType = system_collections_generic.Dictionary
    (
      keySelector,
      valueSelector,
      keyComparer
    );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Dictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Dictionary<int, int> target = targetType.Ctor(source, capacityRequested);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );
    Assert.AreEqual ( capacityGotten, target.Capacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", 'k' )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", 'v' )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", 'c' )]
  public void DictionaryNullParameter ( string errMsg, char whosNull )
  {
    TestComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector    = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector  = whosNull == 'v' ? null! : x => x;

    Action test = () => system_collections_generic.Dictionary ( keySelector, valueSelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 100, 107 )]
  [DataRow ( null, 17 )]
  public void HashSet ( int? capacityRequested, int capacityGotten )
  {
    TestComparer<int> itemComparer = new ();
    AsOrToTargetType<HashSet<int>> targetType = system_collections_generic.HashSet ( itemComparer );

    HashSet<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( itemComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    HashSet<int> target = targetType.Ctor(source, capacityRequested);

    Assert.IsTrue ( ReferenceEquals ( itemComparer, target.Comparer ) );
    Assert.AreEqual ( capacityGotten, target.Capacity );

    Assert.IsTrue ( targetType.CanCast ( new HashSet<int> ( [], itemComparer ) ) );
    Assert.IsFalse ( targetType.CanCast ( new HashSet<int> ( [], new TestComparer<int> () ) ) );
    Assert.IsFalse ( targetType.CanCast ( new HashSet<object> ( [] ) ) );

    Assert.IsTrue ( source.SequenceEqual ( target.OrderBy ( x => x ) ) );
  }

  [TestMethod]
  public void HashSet_NullComparer ()
  {
    TestComparer<int> itemComparer = null!;
    Action test = () => system_collections_generic.HashSet ( itemComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Item comparer not provided. (Parameter 'itemComparer')", e.Message );
  }

  [TestMethod]
  public void LinkedList ()
  {
    TestComparer<int> itemComparer = new ();
    AsOrToTargetType<LinkedList<int>> targetType = system_collections_generic.LinkedList<int> ();

    LinkedList<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    LinkedList<int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void List ( int? capacity )
  {
    AsOrToTargetType<List<int>> targetType = system_collections_generic.List<int> ( );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    List<int> target = targetType.Ctor(source, capacity);

    Assert.AreEqual ( capacity ?? 16, target.Capacity );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( 100, 107 )]
  [DataRow ( null, 17 )]
  public void OrderedDictionary_KeySelectorOnly ( int? capacityRequested, int capacityGotten )
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;

    AsOrToTargetType<OrderedDictionary<int, int>> targetType = system_collections_generic.OrderedDictionary ( keySelector, keyComparer );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    OrderedDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    OrderedDictionary<int, int> target = targetType.Ctor(source, capacityRequested);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );
    Assert.AreEqual ( capacityGotten, target.Capacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", false, true )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", true, false )]
  public void OrderedDictionary_KeySelectorOnly_NullParameter ( string errMsg, bool nullComparer, bool nullSelector )
  {
    TestComparer<int> keyComparer = nullComparer ? null! : new ();
    Func<int, int> keySelector = nullSelector ? null! : x => x;

    Action test = () => system_collections_generic.OrderedDictionary ( keySelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 100, 107 )]
  [DataRow ( null, 17 )]
  public void OrderedDictionary ( int? capacityRequested, int capacityGotten )
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;

    AsOrToTargetType<OrderedDictionary<int, int>> targetType = system_collections_generic.OrderedDictionary
    (
      keySelector,
      valueSelector,
      keyComparer
    );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    OrderedDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    OrderedDictionary<int, int> target = targetType.Ctor(source, capacityRequested);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );
    Assert.AreEqual ( capacityGotten, target.Capacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", 'k' )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", 'v' )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", 'c' )]
  public void OrderedDictionaryNullParameter ( string errMsg, char whosNull )
  {
    TestComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector    = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector  = whosNull == 'v' ? null! : x => x;

    Action test = () => system_collections_generic.OrderedDictionary ( keySelector, valueSelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void PriorityQueue ( int? capacity )
  {
    ReverseOrderComparer<int> priorityComparer = new ();
    AsOrToTargetType<PriorityQueue<int, int>> targetType = system_collections_generic.PriorityQueue<int, int>( priorityComparer );

    PriorityQueue<int, int> empty = targetType.Empty ();
    Assert.AreEqual ( 0, empty.Count );
    Assert.IsTrue ( ReferenceEquals ( priorityComparer, empty.Comparer ) );

    IEnumerable<(int Item, int Priority)> source = XEnumerable.RangeEnumerable(1, 10)
      .Select ( x => new ValueTuple<int, int>(x, x *2));
    PriorityQueue<int, int> target = targetType.Ctor(source, capacity);

    Assert.IsTrue ( ReferenceEquals ( priorityComparer, target.Comparer ) );
    Assert.AreEqual ( capacity ?? 16, target.Capacity );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    List<int> list = [];
    while (target.Count > 0)
      list.Add ( target.Dequeue () );

    Assert.IsTrue ( source.Select ( x => x.Item ).Reverse ().SequenceEqual ( list ) );
  }

  [TestMethod]
  public void PriorityQueue_NullComparer ()
  {
    ReverseOrderComparer<int> priorityComparer = null!;
    Action test = () => system_collections_generic.PriorityQueue<int, int> ( priorityComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Priority comparer not provided. (Parameter 'priorityComparer')", e.Message );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Queue ( int? capacity )
  {
    AsOrToTargetType<Queue<int>> targetType = system_collections_generic.Queue<int> ( );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Queue<int> target = targetType.Ctor(source, capacity);

    Assert.AreEqual ( capacity ?? 16, target.Capacity );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  public void SortedDictionary_KeySelectorOnly ()
  {
    ReverseOrderComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;

    AsOrToTargetType<SortedDictionary<int, int>> targetType = system_collections_generic.SortedDictionary ( keySelector, keyComparer );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    SortedDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    SortedDictionary<int, int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), x))
      .OrderByDescending(x => x.Key);
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", false, true )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", true, false )]
  public void SortedDictionary_KeySelectorOnly_NullParameter ( string errMsg, bool nullComparer, bool nullSelector )
  {
    ReverseOrderComparer<int> keyComparer = nullComparer ? null! : new ();
    Func<int, int> keySelector = nullSelector ? null! : x => x;

    Action test = () => system_collections_generic.SortedDictionary ( keySelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  public void SortedDictionary ()
  {
    ReverseOrderComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;

    AsOrToTargetType<SortedDictionary<int, int>> targetType = system_collections_generic.SortedDictionary
    (
      keySelector,
      valueSelector,
      keyComparer
    );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    SortedDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    SortedDictionary<int, int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)))
      .OrderByDescending(x => x.Key);
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", 'k' )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", 'v' )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", 'c' )]
  public void SortedDictionaryNullParameter ( string errMsg, char whosNull )
  {
    ReverseOrderComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector            = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector          = whosNull == 'v' ? null! : x => x;

    Action test = () => system_collections_generic.SortedDictionary ( keySelector, valueSelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }
}
