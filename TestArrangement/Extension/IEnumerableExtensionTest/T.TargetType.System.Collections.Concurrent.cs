using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using c_concurrent = Software9119.Collection.Superb.Extension.system_collections_concurrent;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_c_concurrent_tests
{
  [TestMethod]
  public void ConcurrentBag ()
  {
    AsOrToTargetType<ConcurrentBag <int>> targetType = c_concurrent.ConcurrentBag<int>();

    ConcurrentBag <int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    ConcurrentBag <int> target = targetType.Ctor(source);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target.Order () ) );
  }

  [TestMethod]
  [DataRow ( 100, 107, 2, true )]
  [DataRow ( 100, 107, 2, false )]
  [DataRow ( null, 37, null, true )]
  [DataRow ( null, 37, null, false )]
  public void ConcurrentDictionary
  (
    int? cap,
    int expCap,
    int? concurrency,
    bool keySelectorOnly
  )
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = keySelectorOnly
      ? x => x
      : x => x * 3;

    AsOrToTargetType<ConcurrentDictionary<int, int>> targetType = keySelectorOnly
      ? c_concurrent.ConcurrentDictionary
        (keySelector, keyComparer, capacity: cap, concurrency)
      : c_concurrent.ConcurrentDictionary
        (keySelector, valueSelector, keyComparer, capacity: cap, concurrency);

    ConcurrentDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    ConcurrentDictionary<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );

    object _tables = Reflection.GetNonPublicFieldValue(target, "_tables");
    object[] _locks = (object[])Reflection.GetNonPublicFieldValue(_tables, "_locks");

    Assert.AreNotEqual ( Environment.ProcessorCount, concurrency ?? 0 );
    Assert.HasCount ( concurrency ?? Environment.ProcessorCount, _locks );

    int _initialCapacity = (int)Reflection.GetNonPublicFieldValue(target, "_initialCapacity");
    Assert.AreEqual ( expCap, _initialCapacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));

    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", 'k' )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", 'c' )]
  public void ConcurrentDictionary_KeySelectorOnly_NullParameter ( string errMsg, char whosNull )
  {
    TestComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector    = whosNull == 'k' ? null! : x => x;

    Action test = () => c_concurrent.ConcurrentDictionary(keySelector, keyComparer, null, null);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", 'k' )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", 'v' )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", 'c' )]
  public void ConcurrentDictionary_NullParameter ( string errMsg, char whosNull )
  {
    TestComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector    = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector  = whosNull == 'v' ? null! : x => x;

    Action test = () => c_concurrent.ConcurrentDictionary
      (keySelector, valueSelector, keyComparer, null, null);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  public void ConcurrentQueue ()
  {
    AsOrToTargetType<ConcurrentQueue <int>> targetType = c_concurrent.ConcurrentQueue<int>();

    ConcurrentQueue <int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    ConcurrentQueue <int> target = targetType.Ctor(source);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  public void ConcurrentStack ()
  {
    AsOrToTargetType<ConcurrentStack <int>> targetType = c_concurrent.ConcurrentStack<int>();

    ConcurrentStack <int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    ConcurrentStack <int> target = targetType.Ctor(source);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.Reverse ().SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void OrderablePartitioner_Array ( bool loadBalance )
  {
    AsOrToTargetType<OrderablePartitioner <int>> targetType = c_concurrent.OrderablePartitioner<int>(default, loadBalance);

    OrderablePartitioner <int> empty = targetType.Empty ();
    Assert.IsFalse ( empty.GetPartitions ( 1 ).Single ().MoveNext () );

    int[] source = [ .. XEnumerable.RangeEnumerable(1, 10) ];
    OrderablePartitioner <int> target = targetType.Ctor(source);

    string typeName = target.GetType().Name;
    Assert.StartsWith ( loadBalance ? "DynamicPartitionerForArray" : "StaticIndexRangePartitionerForArray", typeName );

    Assert.IsFalse ( targetType.CanCast ( source ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    IEnumerable<EnumerableEnumerator<int>> partions = target.GetPartitions(2).Select(x => new EnumerableEnumerator<int>(x));
    Assert.IsTrue ( partions.SelectMany ( x => x ).SequenceEqual ( source ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void OrderablePartitioner_IList ( bool loadBalance )
  {
    AsOrToTargetType<OrderablePartitioner <int>> targetType = c_concurrent.OrderablePartitioner<int>(default, loadBalance);

    OrderablePartitioner <int> empty = targetType.Empty ();
    Assert.IsFalse ( empty.GetPartitions ( 1 ).Single ().MoveNext () );

    List<int> source = [ .. XEnumerable.RangeEnumerable(1, 10) ];
    OrderablePartitioner <int> target = targetType.Ctor(source);

    string typeName = target.GetType().Name;
    Assert.StartsWith ( loadBalance ? "DynamicPartitionerForIList" : "StaticIndexRangePartitionerForIList", typeName );

    Assert.IsFalse ( targetType.CanCast ( source ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    IEnumerable<EnumerableEnumerator<int>> partions = target.GetPartitions(2).Select(x => new EnumerableEnumerator<int>(x));
    Assert.IsTrue ( partions.SelectMany ( x => x ).SequenceEqual ( source ) );
  }

  [TestMethod]
  [DataRow ( EnumerablePartitionerOptions.None )]
  [DataRow ( EnumerablePartitionerOptions.NoBuffering )]
  public void OrderablePartitioner_Enumerable ( EnumerablePartitionerOptions opts )
  {
    AsOrToTargetType<OrderablePartitioner <int>> targetType = c_concurrent.OrderablePartitioner<int>(opts, default);

    OrderablePartitioner <int> empty = targetType.Empty ();
    Assert.IsFalse ( empty.GetPartitions ( 1 ).Single ().MoveNext () );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    OrderablePartitioner <int> target = targetType.Ctor(source);

    string typeName = target.GetType().Name;
    Assert.StartsWith ( "DynamicPartitionerForIEnumerable", typeName );

    Assert.IsFalse ( targetType.CanCast ( source ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    bool _useSingleChunking = (bool)Reflection.GetNonPublicFieldValue(target, "_useSingleChunking");
    Assert.AreEqual ( opts == EnumerablePartitionerOptions.NoBuffering, _useSingleChunking );

    IEnumerable<EnumerableEnumerator<int>> partions = target.GetPartitions(2).Select(x => new EnumerableEnumerator<int>(x));
    Assert.IsTrue ( partions.SelectMany ( x => x ).SequenceEqual ( source ) );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void BlockingCollection ( int? capacityLimit )
  {
    AsOrToTargetType<BlockingCollection <int>> targetType = c_concurrent.BlockingCollection<int>(capacityLimit);

    BlockingCollection <int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    ConcurrentQueue<int>? source = XEnumerable.RangeEnumerable(1, 10).AsOrToConcurrentQueue()!;
    BlockingCollection <int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( source ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.AreEqual ( capacityLimit ?? -1, target.BoundedCapacity );

    Assert.IsTrue ( target.SequenceEqual ( source ) );
  }
}