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

    AsOrToTargetType<Dictionary<int, int>> targetType = system_collections_generic.Dictionary ( keySelector, keyComparer, capacityRequested );

    Dictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Dictionary<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

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

    Action test = () => system_collections_generic.Dictionary ( keySelector, keyComparer, null );
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
      keyComparer,
      capacityRequested
    );

    Dictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Dictionary<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

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
  public void Dictionary_NullParameter ( string errMsg, char whosNull )
  {
    TestComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector    = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector  = whosNull == 'v' ? null! : x => x;

    Action test = () => system_collections_generic.Dictionary ( keySelector, valueSelector, keyComparer, null );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 100, 107 )]
  [DataRow ( null, 17 )]
  public void HashSet ( int? capacityRequested, int capacityGotten )
  {
    TestComparer<int> itemComparer = new ();
    AsOrToTargetType<HashSet<int>> targetType = system_collections_generic.HashSet ( itemComparer, capacityRequested );

    HashSet<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( itemComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    HashSet<int> target = targetType.Ctor(source);

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
    Action test = () => system_collections_generic.HashSet ( itemComparer, null );
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
    LinkedList<int> target = targetType.Ctor(source);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void List ( int? capacity )
  {
    AsOrToTargetType<List<int>> targetType = system_collections_generic.List<int> ( capacity );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    List<int> target = targetType.Ctor(source);

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

    AsOrToTargetType<OrderedDictionary<int, int>> targetType = system_collections_generic.OrderedDictionary
    (
      keySelector,
      keyComparer,
      capacityRequested
    );

    OrderedDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    OrderedDictionary<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

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

    Action test = () => system_collections_generic.OrderedDictionary ( keySelector, keyComparer, null );
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
      keyComparer,
      capacityRequested
    );

    OrderedDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    OrderedDictionary<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

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
  public void OrderedDictionary_NullParameter ( string errMsg, char whosNull )
  {
    TestComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector    = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector  = whosNull == 'v' ? null! : x => x;

    Action test = () => system_collections_generic.OrderedDictionary ( keySelector, valueSelector, keyComparer, null );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void PriorityQueue ( int? capacity )
  {
    ReverseOrderComparer<int> priorityComparer = new ();
    AsOrToTargetType<PriorityQueue<int, int>> targetType = system_collections_generic.PriorityQueue<int, int>( priorityComparer, capacity );

    PriorityQueue<int, int> empty = targetType.Empty ();
    Assert.AreEqual ( 0, empty.Count );
    Assert.IsTrue ( ReferenceEquals ( priorityComparer, empty.Comparer ) );

    IEnumerable<(int Item, int Priority)> source = XEnumerable.RangeEnumerable(1, 10)
      .Select ( x => new ValueTuple<int, int>(x, x *2));
    PriorityQueue<int, int> target = targetType.Ctor(source);

    Assert.IsTrue ( ReferenceEquals ( priorityComparer, target.Comparer ) );
    Assert.AreEqual ( capacity ?? 16, target.Capacity );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( new int [ 0 ] ) );

    List<int> list = [];
    while (target.Count > 0)
      list.Add ( target.Dequeue () );

    Assert.IsTrue ( source.Select ( x => x.Item ).Reverse ().SequenceEqual ( list ) );
  }

  [TestMethod]
  public void PriorityQueue_NullComparer ()
  {
    ReverseOrderComparer<int> priorityComparer = null!;
    Action test = () => system_collections_generic.PriorityQueue<int, int> ( priorityComparer, null );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Priority comparer not provided. (Parameter 'priorityComparer')", e.Message );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Queue ( int? capacity )
  {
    AsOrToTargetType<Queue<int>> targetType = system_collections_generic.Queue<int> ( capacity);
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Queue<int> target = targetType.Ctor(source);

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

    SortedDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    SortedDictionary<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

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

    SortedDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    SortedDictionary<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

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
  public void SortedDictionary_NullParameter ( string errMsg, char whosNull )
  {
    ReverseOrderComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector            = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector          = whosNull == 'v' ? null! : x => x;

    Action test = () => system_collections_generic.SortedDictionary ( keySelector, valueSelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void SortedList_KeySelectorOnly ( int? capacity )
  {
    ReverseOrderComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;

    AsOrToTargetType<SortedList<int, int>> targetType = system_collections_generic.SortedList ( keySelector, keyComparer, capacity );

    SortedList<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    SortedList<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );
    Assert.AreEqual ( capacity ?? 16, target.Capacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), x))
      .OrderByDescending(x => x.Key);
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", false, true )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", true, false )]
  public void SortedList_KeySelectorOnly_NullParameter ( string errMsg, bool nullComparer, bool nullSelector )
  {
    ReverseOrderComparer<int> keyComparer = nullComparer ? null! : new ();
    Func<int, int> keySelector = nullSelector ? null! : x => x;

    Action test = () => system_collections_generic.SortedList ( keySelector, keyComparer, null );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void SortedList ( int? capacity )
  {
    ReverseOrderComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;

    AsOrToTargetType<SortedList<int, int>> targetType = system_collections_generic.SortedList
    (
      keySelector,
      valueSelector,
      keyComparer,
      capacity
    );

    SortedList<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    SortedList<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );
    Assert.AreEqual ( capacity ?? 16, target.Capacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)))
      .OrderByDescending(x => x.Key);
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", 'k' )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", 'v' )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", 'c' )]
  public void SortedList_NullParameter ( string errMsg, char whosNull )
  {
    ReverseOrderComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector            = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector          = whosNull == 'v' ? null! : x => x;

    Action test = () => system_collections_generic.SortedList ( keySelector, valueSelector, keyComparer, null );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }


  [TestMethod]
  public void SortedSet ()
  {
    ReverseOrderComparer<int> itemComparer = new ();
    AsOrToTargetType<SortedSet<int>> targetType = system_collections_generic.SortedSet ( itemComparer );

    SortedSet<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( itemComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    SortedSet<int> target = targetType.Ctor(source);

    Assert.IsTrue ( ReferenceEquals ( itemComparer, target.Comparer ) );

    Assert.IsTrue ( targetType.CanCast ( new SortedSet<int> ( [], itemComparer ) ) );
    Assert.IsFalse ( targetType.CanCast ( new SortedSet<int> ( [], new ReverseOrderComparer<int> () ) ) );
    Assert.IsFalse ( targetType.CanCast ( new SortedSet<object> ( [] ) ) );

    Assert.IsTrue ( source.Reverse ().SequenceEqual ( target ) );
  }

  [TestMethod]
  public void SortedSet_NullComparer ()
  {
    ReverseOrderComparer<int> itemComparer = null!;
    Action test = () => system_collections_generic.SortedSet ( itemComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Item comparer not provided. (Parameter 'itemComparer')", e.Message );
  }


  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Stack ( int? capacity )
  {
    AsOrToTargetType<Stack<int>> targetType = system_collections_generic.Stack<int> ( capacity );

    Stack<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Stack<int> target = targetType.Ctor(source);

    Assert.AreEqual ( capacity ?? 16, target.Capacity );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.Reverse ().SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Array ( int? capacity )
  {
    AsOrToTargetType<object[]> targetType = system_collections_generic.Array<object> (capacity );

    object[] empty = targetType.Empty ();
    Assert.IsTrue ( ReferenceEquals ( System.Array.Empty<object> (), empty ) );

    const int count = 10;
    IEnumerable<object> source = XEnumerable.RangeEnumerable(1, count).Select(x => (object)x);
    object[] target = targetType.Ctor(source);

    Assert.HasCount ( capacity ?? count, target );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    ArraySegment<object> targetSegment = new (target, 0, count);
    Assert.IsTrue ( source.SequenceEqual ( targetSegment ) );

    int index = count;
    while (index < (capacity ?? count))
      Assert.IsNull ( target [ index++ ] );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void IList_Enumerable ( int? capacity )
  {
    AsOrToTargetType<IList<int>> targetType = system_collections_generic.IList<int> (capacity );

    IList<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( System.Array.Empty<int> (), empty ) );

    const int count = 10;
    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, count);
    IList<int> target = targetType.Ctor(source);

    Assert.HasCount ( count, target );
    Assert.AreEqual ( capacity ?? 16, ((List<int>) target).Capacity );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  public void IList_IList ()
  {
    AsOrToTargetType<IList<int>> targetType = system_collections_generic.IList<int> ( null );

    IEnumerable<int> source = Enumerable.Range(1, 10);
    IList<int> target = targetType.Ctor(source);

    Assert.IsTrue ( ReferenceEquals ( source, target ) );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
  }

  [TestMethod]
  public void IList_ICollection ()
  {
    AsOrToTargetType<IList<int>> targetType = system_collections_generic.IList<int> ( null );

    const int count = 11;
    HashSet<int> source = Enumerable.Range(1, count).AsOrToHashSet()!;
    IList<int> target = targetType.Ctor(source);

    Assert.HasCount ( count, target );
    Assert.HasCount ( count, (int []) target );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }
}