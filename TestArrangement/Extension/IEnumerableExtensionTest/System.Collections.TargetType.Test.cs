using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_test
{
  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void ArrayList ( int? capacity )
  {
    AsOrToTargetType<ArrayList> targetType = system_collections.ArrayList();

    Assert.IsTrue ( targetType.TryCast );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<object> source = new object [] {new(), new() }.Select(x => x);

    ArrayList target = targetType.Ctor(source, capacity);
    Assert.IsTrue ( source.SequenceEqual ( target.Cast<object> () ) );
    Assert.AreEqual ( capacity ?? 4, target.Capacity );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void ArrayList_ICollection ( int? capacity )
  {
    AsOrToTargetType<ArrayList> targetType = system_collections.ArrayList();

    ICollection source = new object [] { new(), new() };

    ArrayList target = targetType.Ctor(source, capacity);
    Assert.IsTrue ( source.Cast<object> ().SequenceEqual ( target.Cast<object> () ) );

    Assert.AreEqual ( capacity ?? target.Count, target.Capacity );
  }

  [TestMethod]
  [DataRow ( 100, true, 117 )]
  [DataRow ( 100, false, 117 )]
  [DataRow ( null, true, 2 )]
  [DataRow ( null, false, 2 )]
  public void Hashtable ( int? capacity, bool keySelectorOnly, int loadSize )
  {
    Func<object, object> keySelector = x => x.GetHashCode();
    Func<object, object> valueSelector = keySelectorOnly
      ? x => x
      : x => x.GetHashCode() * 2;

    AsOrToTargetType<Hashtable> targetType = keySelectorOnly
      ? system_collections.Hashtable ( keySelector)
      : system_collections.Hashtable(keySelector, valueSelector);

    Assert.IsFalse ( targetType.TryCast );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<object> source = new object[] {new(), new() }.Select(x => x);
    Hashtable target = targetType.Ctor(source, capacity);

    int _loadsize = (int)Reflection.GetNonPublicFieldValue(target, "_loadsize");
    Assert.AreEqual ( loadSize, _loadsize );

    IEnumerable<(int, object)> expectation = source
      .Select(x => ((int) keySelector(x), valueSelector(x)))
      .OrderBy(x => x.Item1);
    IEnumerable<(int, object)> actual = target
      .Cast<DictionaryEntry> ()
      .Select(x => ((int)x.Key, x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void Hashtable_NullKeySelector ( bool keySelectorOnly )
  {
    Func<object, object> keySelector = null!;
    Func<object, object> valueSelector = x => x;

    Action test = keySelectorOnly
      ? () => system_collections.Hashtable ( keySelector)
      : () => system_collections.Hashtable(keySelector, valueSelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( "Key selector not provided. (Parameter 'keySelector')", e.Message );
  }

  [TestMethod]
  public void Hashtable_NullValueSelector ()
  {
    Func<object, object> keySelector = x => x;
    Func<object, object> valueSelector = null!;

    Action test = () => system_collections.Hashtable(keySelector, valueSelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( "Value selector not provided. (Parameter 'valueSelector')", e.Message );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Queue ( int? capacity )
  {
    AsOrToTargetType<Queue> targetType = system_collections.Queue();

    Assert.IsTrue ( targetType.TryCast );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<object> source = new object [] {new(), new() }.Select(x => x);

    Queue target = targetType.Ctor(source, capacity);
    Assert.IsTrue ( source.SequenceEqual ( target.Cast<object> () ) );

    Array storage = (Array)Reflection.GetNonPublicFieldValue(target, "_array");
    Assert.HasCount ( capacity ?? 32, storage );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Queue_ICollection ( int? capacity )
  {
    AsOrToTargetType<Queue> targetType = system_collections.Queue();

    ICollection source = new object [] { new(), new() };

    Queue target = targetType.Ctor(source, capacity);
    Assert.IsTrue ( source.Cast<object> ().SequenceEqual ( target.Cast<object> () ) );

    Array storage = (Array)Reflection.GetNonPublicFieldValue(target, "_array");
    Assert.HasCount ( capacity ?? target.Count, storage );
  }

  [TestMethod]
  [DataRow ( 100, true )]
  [DataRow ( 100, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void SortedList ( int? capacity, bool keySelectorOnly )
  {
    Func<object, object> keySelector = x => x.GetHashCode();
    Func<object, object> valueSelector = keySelectorOnly
      ? x => x
      : x => x.GetHashCode() * 2;

    AsOrToTargetType<SortedList> targetType = keySelectorOnly
      ? system_collections.SortedList ( keySelector)
      : system_collections.SortedList(keySelector, valueSelector);

    Assert.IsFalse ( targetType.TryCast );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<object> source = new object[] {new(), new() }.Select(x => x);
    SortedList target = targetType.Ctor(source, capacity);

    Assert.AreEqual ( capacity ?? 16, target.Capacity );

    IEnumerable<(int, object)> expectation = source
      .Select(x => ((int) keySelector(x), valueSelector(x)))
      .OrderBy(x => x.Item1);
    IEnumerable<(int, object)> actual = target
      .Cast<DictionaryEntry> ()
      .Select(x => ((int)x.Key, x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void SortedList_NullKeySelector ( bool keySelectorOnly )
  {
    Func<object, object> keySelector = null!;
    Func<object, object> valueSelector = x => x;

    Action test = keySelectorOnly
      ? () => system_collections.SortedList ( keySelector)
      : () => system_collections.SortedList(keySelector, valueSelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( "Key selector not provided. (Parameter 'keySelector')", e.Message );
  }

  [TestMethod]
  public void SortedList_NullValueSelector ()
  {
    Func<object, object> keySelector = x => x;
    Func<object, object> valueSelector = null!;

    Action test = () => system_collections.SortedList(keySelector, valueSelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( "Value selector not provided. (Parameter 'valueSelector')", e.Message );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Stack ( int? capacity )
  {
    AsOrToTargetType<Stack> targetType = system_collections.Stack();

    Assert.IsTrue ( targetType.TryCast );
    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<object> source = new object [] {new(), new(), new(), new() }.Select(x => x);

    Stack target = targetType.Ctor(source, capacity);
    Assert.IsTrue ( source.SequenceEqual ( target.Cast<object> ().Reverse () ) );

    Array storage = (Array)Reflection.GetNonPublicFieldValue(target, "_array");
    Assert.HasCount ( capacity ?? 10, storage );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Stack_ICollection ( int? capacity )
  {
    AsOrToTargetType<Stack> targetType = system_collections.Stack();

    ICollection source = new object [] 
    { 
      new(), new(), new(), new(),
      new(), new(), new(), new(),
      new(), new(), new(), new(),
    };

    Stack target = targetType.Ctor(source, capacity);
    Assert.IsTrue ( source.Cast<object> ().SequenceEqual ( target.Cast<object> ().Reverse () ) );

    Array storage = (Array)Reflection.GetNonPublicFieldValue(target, "_array");
    Assert.HasCount ( capacity ?? source.Count, storage );
  }
}
