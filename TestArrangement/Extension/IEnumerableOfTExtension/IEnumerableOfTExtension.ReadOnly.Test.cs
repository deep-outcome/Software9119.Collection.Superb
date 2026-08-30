using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableOfTExtension;

[TestClass]
public partial class IEnumerableExtensionTest
{
  [TestMethod]
  [SuppressMessage ( "Design", "MSTEST0032:Assertion condition is always true", Justification = "Intentional" )]
  public void DefaultCapacities ()
  {
    Assert.AreEqual ( 8, IEnumerableExtension.DefaultListCapacity );
    Assert.AreEqual ( 8, IEnumerableExtension.DefaultDictCapacity );
  }

  [TestMethod]
  public void EnumerableNull ()
  {
    const string expectation = "Null source enumerable encounter. (Parameter 'test')";
    string test = IEnumerableExtension.EnumerableNull ( "test" ).Message;
    Assert.AreEqual ( expectation, test );
  }

  [TestMethod]
  public void DictionaryNull ()
  {
    const string expectation = "Null source dictionary encounter. (Parameter 'test')";
    string test = IEnumerableExtension.DictionaryNull ( "test" ).Message;
    Assert.AreEqual ( expectation, test );
  }

  [TestMethod]
  public void AsOrToIList_NullSource_ReturnEmpty ()
  {
    IList<int>? test = ((IEnumerable<int>?) null).AsOrToIList ( EnumerableNullBehavior.ReturnEmpty );
    Assert.IsTrue ( test is int [] );
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void AsOrToIList_NullSource_ReturnDefault ()
  {
    IList<int>? test = ((IEnumerable<int>?) null).AsOrToIList ( EnumerableNullBehavior.ReturnDefault );
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void AsOrToIList_NullSource_ThrowException ()
  {
    const string expectation = "Null source enumerable encounter. (Parameter 'enumerable')";
    Action test = () => ((IEnumerable<int>?) null).AsOrToIList ( EnumerableNullBehavior.ThrowException );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void AsOrToIList_NullSource_UknownBehavior ()
  {
    const string expectation = "Unsupported behavior, '793'. (Parameter 'behavior')";
    Action test = () => ((IEnumerable<int>?) null).AsOrToIList ( (EnumerableNullBehavior) 793 );
    UnsupportedNullBehaviorException e = Assert.ThrowsExactly<UnsupportedNullBehaviorException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void AsOrToIList_EnumerableIsIListOfT ()
  {
    int [] ilist = [];
    IList<int>? test = ilist.AsOrToIList();
    Assert.IsTrue ( ReferenceEquals ( ilist, test ) );
  }

  [TestMethod]
  public void AsOrToIList_EnumerableIsICollectionOfT ()
  {
    HashSet<int> set = [1 ,2, 3];
    IList<int> test = set.AsOrToIList()!;
    Assert.AreEqual ( typeof ( int [] ), test.GetType () );
    Assert.IsTrue ( set.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void AsOrToIList_EnumerableIsEnumerable ()
  {
    IEnumerable<int> enumerable = Enumerable.Range(1, 9).Select(x => x);
    IList<int> test = enumerable.AsOrToIList()!;
    Assert.AreEqual ( typeof ( List<int> ), test.GetType () );
    Assert.AreEqual ( 16, ((List<int>) test).Capacity );
    Assert.IsTrue ( enumerable.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void AsOrToIList_ExactCapacity ()
  {
    const int count = 3;
    IEnumerable<int> enumerable = Enumerable.Range(1, count).Select(x => x);
    IList<int> test = enumerable.AsOrToIList(capacity: count)!;
    Assert.AreEqual ( typeof ( List<int> ), test.GetType () );
    Assert.AreEqual ( count, ((List<int>) test).Capacity );
    Assert.IsTrue ( enumerable.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void AsOrToReadOnlyCollection_NullSource_ReturnEmpty ()
  {
    ReadOnlyCollection<int> test = ((IEnumerable<int>?) null).AsOrToReadOnlyCollection ( EnumerableNullBehavior.ReturnEmpty )!;
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void AsOrToReadOnlyCollection_NullSource_ReturnDefault ()
  {
    ReadOnlyCollection<int> test = ((IEnumerable<int>?) null).AsOrToReadOnlyCollection ( EnumerableNullBehavior.ReturnDefault )!;
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void AsOrToReadOnlyCollection_NullSource_ThrowException ()
  {
    const string expectation = "Null source enumerable encounter. (Parameter 'enumerable')";
    Action test = () => ((IEnumerable<int>?) null).AsOrToReadOnlyCollection ( EnumerableNullBehavior.ThrowException );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void AsOrToReadOnlyCollection_UknownBehavior ()
  {
    const string expectation = "Unsupported behavior, '793'. (Parameter 'behavior')";
    Action test = () => ((IEnumerable<int>?) null).AsOrToReadOnlyCollection ( (EnumerableNullBehavior) 793 );
    UnsupportedNullBehaviorException e = Assert.ThrowsExactly<UnsupportedNullBehaviorException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void AsOrToReadOnlyCollection ()
  {
    IEnumerable<int> enumerable = Enumerable.Range(1, 9).Select(x => x);
    ReadOnlyCollection<int> coll = enumerable.AsOrToReadOnlyCollection()!;
    IList<int> test = Items(coll);
    Assert.AreEqual ( typeof ( List<int> ), test.GetType () );
    Assert.AreEqual ( 16, ((List<int>) test).Capacity );
    Assert.IsTrue ( enumerable.SequenceEqual ( coll ) );
  }

  [TestMethod]
  public void AsOrToReadOnlyCollection_ReadOnlyCollectionAlready ()
  {
    IEnumerable<int> enumerable = new ReadOnlyCollection<int>(new int[0]);
    ReadOnlyCollection<int> test = enumerable.AsOrToReadOnlyCollection()!;
    Assert.IsTrue ( ReferenceEquals ( enumerable, test ) );
  }

  [TestMethod]
  public void AsOrToReadOnlyCollection_ExactCapacity ()
  {
    const int count = 3;
    IEnumerable<int> enumerable = Enumerable.Range(1, count).Select(x => x);
    ReadOnlyCollection<int> coll = enumerable.AsOrToReadOnlyCollection(capacity: count)!;
    IList<int> test = Items(coll);
    Assert.AreEqual ( typeof ( List<int> ), test.GetType () );
    Assert.AreEqual ( count, ((List<int>) test).Capacity );
    Assert.IsTrue ( enumerable.SequenceEqual ( coll ) );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_KeySelectorOnly_NullSource_ReturnEmpty ()
  {
    Func<int, int> keySelector = x => x *2;
    ReadOnlyDictionary<int, int> test = ((IEnumerable<int>?) null).ToReadOnlyDictionary
    (
      keySelector,
      EnumerableNullBehavior.ReturnEmpty
    )!;
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_KeySelectorOnly_NullSource_ReturnDefault ()
  {
    Func<int, int> keySelector = x => x *2;
    ReadOnlyDictionary<int, int> test = ((IEnumerable<int>?) null).ToReadOnlyDictionary
    (
      keySelector,
      EnumerableNullBehavior.ReturnDefault
    )!;
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_KeySelectorOnly_NullSource_ThrowException ()
  {
    const string expectation = "Null source enumerable encounter. (Parameter 'enumerable')";
    Func<int, int> keySelector = x => x *2;
    Action test = () => _ = ((IEnumerable<int>?) null).ToReadOnlyDictionary
    (
      keySelector,
      EnumerableNullBehavior.ThrowException
    );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_KeySelectorOnly_NullSource_UknownBehavior ()
  {
    const string expectation = "Unsupported behavior, '793'. (Parameter 'behavior')";
    Func<int, int> keySelector = x => x *2;
    Action test = () => _ = ((IEnumerable<int>?) null).ToReadOnlyDictionary
    (
      keySelector,
      (EnumerableNullBehavior) 793
    );

    UnsupportedNullBehaviorException e = Assert.ThrowsExactly<UnsupportedNullBehaviorException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_KeySelectorOnly_NullKeySelector ()
  {
    const string expectation = "Key selector not provided. (Parameter 'keySelector')";
    Func<int, int> keySelector = null!;
    Action test = () => _ = new int[0].ToReadOnlyDictionary(keySelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Unneeded." )]
  public void ToReadOnlyDictionary_KeySelectorOnly ()
  {
    IEnumerable<int> enumerable = Enumerable.Range(1, 12);
    Func<int, int> keySelector = x => x *2;
    ReadOnlyDictionary<int, int> test = enumerable.ToReadOnlyDictionary(keySelector)!;

    Assert.IsTrue ( test.Keys.SequenceEqual ( enumerable.Select ( keySelector ) ) );
    Assert.IsTrue ( test.Values.SequenceEqual ( enumerable ) );

    Dictionary<int, int> dict = Dictionary(test);
    Assert.AreEqual ( 23, dict.Capacity );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Unneeded." )]
  public void ToReadOnlyDictionary_KeySelectorOnly_ExactCapacity ()
  {
    const int count = 3;
    IEnumerable<int> enumerable = Enumerable.Range(1, count);
    Func<int, int> keySelector = x => x *2;
    ReadOnlyDictionary<int, int> test = enumerable.ToReadOnlyDictionary
    (
      keySelector,
      capacity: count
    )!;

    Assert.IsTrue ( test.Keys.SequenceEqual ( enumerable.Select ( keySelector ) ) );
    Assert.IsTrue ( test.Values.SequenceEqual ( enumerable ) );

    Dictionary<int, int> dict = Dictionary(test);
    Assert.AreEqual ( count, dict.Capacity );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_NullSource_ReturnEmpty ()
  {
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    ReadOnlyDictionary<int, int> test = ((IEnumerable<int>?) null).ToReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      EnumerableNullBehavior.ReturnEmpty
    )!;
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_NullSource_ReturnDefault ()
  {
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    ReadOnlyDictionary<int, int> test = ((IEnumerable<int>?) null).ToReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      EnumerableNullBehavior.ReturnDefault
    )!;
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_NullSource_ThrowException ()
  {
    const string expectation = "Null source enumerable encounter. (Parameter 'enumerable')";
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    Action test = () => _ = ((IEnumerable<int>?) null).ToReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      EnumerableNullBehavior.ThrowException
    );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_NullSource_UknownBehavior ()
  {
    const string expectation = "Unsupported behavior, '793'. (Parameter 'behavior')";
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    Action test = () => _ = ((IEnumerable<int>?) null).ToReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      (EnumerableNullBehavior) 793
    );

    UnsupportedNullBehaviorException e = Assert.ThrowsExactly<UnsupportedNullBehaviorException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_NullKeySelector ()
  {
    const string expectation = "Key selector not provided. (Parameter 'keySelector')";
    Func<int, int> keySelector = null!;
    Func<int, int> valueSelector = x => x *3;
    Action test = () => _ = new int[0].ToReadOnlyDictionary(keySelector, valueSelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void ToReadOnlyDictionary_NullValueSelector ()
  {
    const string expectation = "Value selector not provided. (Parameter 'valueSelector')";
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = null!;
    Action test = () => _ = new int[0].ToReadOnlyDictionary(keySelector, valueSelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Unneeded." )]
  public void ToReadOnlyDictionary ()
  {
    IEnumerable<int> enumerable = Enumerable.Range(1, 12);
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    ReadOnlyDictionary<int, int> test = enumerable.ToReadOnlyDictionary(keySelector,valueSelector)!;

    Assert.IsTrue ( test.Keys.SequenceEqual ( enumerable.Select ( keySelector ) ) );
    Assert.IsTrue ( test.Values.SequenceEqual ( enumerable.Select ( valueSelector ) ) );

    Dictionary<int, int> dict = Dictionary(test);
    Assert.AreEqual ( 23, dict.Capacity );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Unneeded." )]
  public void ToReadOnlyDictionary_ExactCapacity ()
  {
    const int count = 3;
    IEnumerable<int> enumerable = Enumerable.Range(1, count);
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    ReadOnlyDictionary<int, int> test = enumerable.ToReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      capacity: count
    )!;

    Assert.IsTrue ( test.Keys.SequenceEqual ( enumerable.Select ( keySelector ) ) );
    Assert.IsTrue ( test.Values.SequenceEqual ( enumerable.Select ( valueSelector ) ) );

    Dictionary<int, int> dict = Dictionary(test);
    Assert.AreEqual ( count, dict.Capacity );
  }

  static IList<int> Items ( ReadOnlyCollection<int> coll )
  {
    PropertyInfo property = NonPublicInstanceProperty(coll.GetType(), "Items");
    IList<int> items = (IList<int>)property.GetValue(coll)!;
    return items;
  }

  static Dictionary<int, int> Dictionary ( ReadOnlyDictionary<int, int> dict )
  {
    PropertyInfo property = NonPublicInstanceProperty(dict.GetType(), "Dictionary");
    IDictionary<int, int> dictionary = (IDictionary<int, int>)property.GetValue(dict)!;
    return (Dictionary<int, int>) dictionary;
  }

  static PropertyInfo NonPublicInstanceProperty ( Type from, string ofName )
  {
    const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
    PropertyInfo info = from.GetProperty ( ofName, flags )!;
    return info;
  }

  [TestMethod]
  public void AsReadOnlyDictionary_NullSource_ReturnEmpty ()
  {
    ReadOnlyDictionary<int, int> test = ((IDictionary<int,int>?) null).AsReadOnlyDictionary(EnumerableNullBehavior.ReturnEmpty)!;
    Assert.HasCount ( 0, test );
  }


  [TestMethod]
  public void AsReadOnlyDictionary_NullSource_ReturnDefault ()
  {
    ReadOnlyDictionary<int, int> test = ((IDictionary<int,int>?) null).AsReadOnlyDictionary(EnumerableNullBehavior.ReturnDefault)!;
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void AsReadOnlyDictionary_NullSource_ThrowException ()
  {
    const string expectation = "Null source dictionary encounter. (Parameter 'dict')";
    Action test = () => _ = ((IDictionary<int,int>?) null).AsReadOnlyDictionary(EnumerableNullBehavior.ThrowException);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void AsReadOnlyDictionary_NullSource_UknownBehavior ()
  {
    const string expectation = "Unsupported behavior, '793'. (Parameter 'behavior')";
    Action test = () => _ = ((IDictionary<int,int>?) null).AsReadOnlyDictionary((EnumerableNullBehavior) 793);

    UnsupportedNullBehaviorException e = Assert.ThrowsExactly<UnsupportedNullBehaviorException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void AsReadOnlyDictionary ()
  {
    IDictionary<int, int> source = new [] { 1,2,3 }.ToDictionary(x => x);
    ReadOnlyDictionary<int, int> test = source.AsReadOnlyDictionary()!;
    Assert.IsTrue ( ReferenceEquals ( source, Dictionary ( test ) ) );
  }
}
