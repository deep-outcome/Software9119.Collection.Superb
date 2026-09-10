using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using c_specialized = Software9119.Collection.Superb.Extension.system_collections_specialized;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_specialized_test
{
  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, false )]
  [DataRow ( null, true, false )]
  [DataRow ( null, false, true )]
  public void HybridDictionary ( int? capacity, bool keySelectorOnly, bool caseInsensitive )
  {
    Func<object, object> keySelector = x => x.GetHashCode();
    Func<object, object> valueSelector = keySelectorOnly
      ? x => x
      : x => x.GetHashCode() * 2;

    AsOrToTargetType<HybridDictionary> targetType = keySelectorOnly
      ? c_specialized.HybridDictionary(keySelector, capacity, caseInsensitive)
      : c_specialized.HybridDictionary(keySelector, valueSelector, capacity, caseInsensitive);

    Assert.HasCount ( 0, targetType.Empty () );

    IEnumerable<object> source = XEnumerable.ObjectsEnumerable(2);
    HybridDictionary target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    if (capacity is int)
    {
      object hashtable = Reflection.GetNonPublicFieldValue(target, "hashtable");
      int _loadsize = (int)Reflection.GetNonPublicFieldValue(hashtable, "_loadsize");
      Assert.AreEqual ( 117, _loadsize );
    }

    bool _caseInsensitive = (bool)Reflection.GetNonPublicFieldValue(target, "caseInsensitive");
    Assert.AreEqual ( caseInsensitive, _caseInsensitive );

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
  public void HybridDictionary_KeySelectorOnly_NullKeySelector ()
  {
    Func<int, object> keySelector = null!;
    Action test = () => c_specialized.HybridDictionary(keySelector, default, default);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( "Key selector not provided. (Parameter 'keySelector')", e.Message );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", 'k' )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", 'v' )]
  public void HybridDictionary_NullSelector ( string errMsg, char whosNull )
  {
    Func<int, object> keySelector    = whosNull == 'k' ? null! : x => x;
    Func<int, object> valueSelector  = whosNull == 'v' ? null! : x => x; ;

    Action test = () => c_specialized.HybridDictionary(keySelector, valueSelector, default, default);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void ListDictionary ( bool keySelectorOnly )
  {
    ReverseOrderComparer keyComparer = new ();

    Func<object, object> keySelector = x => x.GetHashCode();
    Func<object, object> valueSelector = keySelectorOnly
      ? x => x
      : x => x.GetHashCode() * 2;

    AsOrToTargetType<ListDictionary> targetType = keySelectorOnly
      ? c_specialized.ListDictionary(keySelector, keyComparer)
      : c_specialized.ListDictionary(keySelector, valueSelector, keyComparer);

    ListDictionary empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    object _comparer = Reflection.GetNonPublicFieldValue ( empty, "comparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    IEnumerable<object> source = XEnumerable.ObjectsEnumerable(2);
    ListDictionary target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    _comparer = Reflection.GetNonPublicFieldValue ( target, "comparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    IEnumerable<(int, object)> expectation = source
      .Select(x => ((int) keySelector(x), valueSelector(x)));

    IEnumerable<(int, object)> actual = target
      .Cast<DictionaryEntry> ()
      .Select(x => ((int)x.Key, x.Value!));

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", "k" )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", "c" )]
  public void ListDictionary_KeySelectorOnly_NullParameter ( string errMsg, string whosNull )
  {
    ReverseOrderComparer keyComparer = whosNull == "c" ? null! : new();
    Func<int, object> keySelector    = whosNull == "k" ? null! : x => x;
    Action test = () => c_specialized.ListDictionary(keySelector, keyComparer);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", "k" )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", "v" )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", "c" )]
  public void ListDictionary_NullParameter ( string errMsg, string whosNull )
  {
    ReverseOrderComparer keyComparer = whosNull == "c" ? null! : new();
    Func<int, object> keySelector    = whosNull == "k" ? null! : x => x;
    Func<int, object> valueSelector  = whosNull == "v" ? null! : x => x;

    Action test = () => c_specialized.ListDictionary(keySelector, valueSelector, keyComparer);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 1000 )]
  [DataRow ( null )]
  [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Ok." )]
  public void NameValueCollection ( int? capacity )
  {
    TestComparer keyComparer = new ();

    Func<int, string> keySelector = x => (x * 2).ToString();
    Func<int, string> valueSelector = x => (x *3).ToString();

    AsOrToTargetType<NameValueCollection > targetType = c_specialized.NameValueCollection
    (
      keySelector,
      valueSelector,
      capacity,
      keyComparer
    );

    NameValueCollection  empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    object _comparer = Reflection.GetNonPublicFieldValue<NameObjectCollectionBase>  ( empty, "_keyComparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    NameValueCollection  target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    _comparer = Reflection.GetNonPublicFieldValue<NameObjectCollectionBase> ( target, "_keyComparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    ArrayList _entriesArray = (ArrayList)Reflection.GetNonPublicFieldValue<NameObjectCollectionBase> ( target, "_entriesArray" );
    Assert.AreEqual ( capacity ?? 16, _entriesArray.Capacity );

    IEnumerable<(string, string)> expectation = source
      .Select(x => (keySelector(x), valueSelector(x)));

    IEnumerable<(string, string?)> actual = target.Cast<string> ().Select(x => (x, target[x]));

    Assert.IsTrue ( expectation.SequenceEqual ( actual! ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", "k" )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", "v" )]
  public void NameValueCollection_NullParameter ( string errMsg, string whosNull )
  {
    Func<int, string> keySelector   = whosNull == "k" ? null! : x => "";
    Func<int, string> valueSelector = whosNull == "v" ? null! : x => "";

    Action test = () => c_specialized.NameValueCollection (keySelector, valueSelector, null, null);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 1000, true )]
  [DataRow ( 1000, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void OrderedDictionary ( int? capacity, bool keySelectorOnly )
  {
    TestComparer keyComparer = new ();

    Func<object, object> keySelector = x => x.GetHashCode();
    Func<object, object> valueSelector = keySelectorOnly
      ? x => x
      : x => x.GetHashCode() * 2;

    AsOrToTargetType<OrderedDictionary > targetType = keySelectorOnly
      ? c_specialized.OrderedDictionary (keySelector, capacity, keyComparer)
      : c_specialized.OrderedDictionary (keySelector, valueSelector, capacity, keyComparer);

    OrderedDictionary  empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    object _comparer = Reflection.GetNonPublicFieldValue ( empty, "_comparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    IEnumerable<object> source = XEnumerable.ObjectsEnumerable(200);
    OrderedDictionary  target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    _comparer = Reflection.GetNonPublicFieldValue ( target, "_comparer" );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, _comparer ) );

    int _initialCapacity = (int)Reflection.GetNonPublicFieldValue ( target, "_initialCapacity" );
    Assert.AreEqual ( capacity ?? 0, _initialCapacity );

    IEnumerable<(int, object)> expectation = source
      .Select(x => ((int) keySelector(x), valueSelector(x)));

    IEnumerable<(int, object)> actual = target
      .Cast<DictionaryEntry> ()
      .Select(x => ((int)x.Key, x.Value!));

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", "k" )]
  public void OrderedDictionary_KeySelectorOnly_NullParameter ( string errMsg, string whosNull )
  {
    TestComparer keyComparer = new();
    Func<int, object> keySelector = whosNull == "k" ? null! : x => x;
    Action test = () => c_specialized.OrderedDictionary (keySelector, null, keyComparer);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", "k" )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", "v" )]
  public void OrderedDictionary_NullParameter ( string errMsg, string whosNull )
  {
    TestComparer keyComparer = new();
    Func<int, object> keySelector   = whosNull == "k" ? null! : x => x;
    Func<int, object> valueSelector = whosNull == "v" ? null! : x => x;

    Action test = () => c_specialized.OrderedDictionary (keySelector, valueSelector, null, keyComparer);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>( test );
    Assert.AreEqual ( errMsg, e.Message );
  }
}
