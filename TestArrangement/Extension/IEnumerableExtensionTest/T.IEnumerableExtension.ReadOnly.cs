using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
[TestClass]
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  [SuppressMessage ( "Design", "MSTEST0032:Assertion condition is always true", Justification = "Intentional" )]
  public void DefaultCapacities ()
  {
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
  public void IntoReadOnlyDictionary_KeySelectorOnly_NullSource_ReturnEmpty ()
  {
    Func<int, int> keySelector = x => x *2;
    ReadOnlyDictionary<int, int> test = ((IEnumerable<int>?) null).IntoReadOnlyDictionary
    (
      keySelector,
      EnumerableNullBehavior.ReturnEmpty
    )!;
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void IntoReadOnlyDictionary_KeySelectorOnly_NullSource_ReturnDefault ()
  {
    Func<int, int> keySelector = x => x *2;
    ReadOnlyDictionary<int, int> test = ((IEnumerable<int>?) null).IntoReadOnlyDictionary
    (
      keySelector,
      EnumerableNullBehavior.ReturnDefault
    )!;
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void IntoReadOnlyDictionary_KeySelectorOnly_NullSource_ThrowException ()
  {
    const string expectation = "Null source enumerable encounter. (Parameter 'enumerable')";
    Func<int, int> keySelector = x => x *2;
    Action test = () => _ = ((IEnumerable<int>?) null).IntoReadOnlyDictionary
    (
      keySelector,
      EnumerableNullBehavior.ThrowException
    );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void IntoReadOnlyDictionary_KeySelectorOnly_NullSource_UknownBehavior ()
  {
    const string expectation = "Unsupported behavior, '793'. (Parameter 'behavior')";
    Func<int, int> keySelector = x => x *2;
    Action test = () => _ = ((IEnumerable<int>?) null).IntoReadOnlyDictionary
    (
      keySelector,
      (EnumerableNullBehavior) 793
    );

    UnsupportedNullBehaviorException e = Assert.ThrowsExactly<UnsupportedNullBehaviorException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void IntoReadOnlyDictionary_KeySelectorOnly_NullKeySelector ()
  {
    const string expectation = "Key selector not provided. (Parameter 'keySelector')";
    Func<int, int> keySelector = null!;
    Action test = () => _ = new int[0].IntoReadOnlyDictionary(keySelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Unneeded." )]
  public void IntoReadOnlyDictionary_KeySelectorOnly ()
  {
    IEnumerable<int> enumerable = Enumerable.Range(1, 12);
    Func<int, int> keySelector = x => x *2;
    ReadOnlyDictionary<int, int> test = enumerable.IntoReadOnlyDictionary(keySelector)!;

    Assert.IsTrue ( test.Keys.SequenceEqual ( enumerable.Select ( keySelector ) ) );
    Assert.IsTrue ( test.Values.SequenceEqual ( enumerable ) );

    Dictionary<int, int> dict = Dictionary(test);
    Assert.AreEqual ( 23, dict.Capacity );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Unneeded." )]
  public void IntoReadOnlyDictionary_KeySelectorOnly_ExactCapacity ()
  {
    const int count = 3;
    IEnumerable<int> enumerable = Enumerable.Range(1, count);
    Func<int, int> keySelector = x => x *2;
    ReadOnlyDictionary<int, int> test = enumerable.IntoReadOnlyDictionary
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
  public void IntoReadOnlyDictionary_NullSource_ReturnEmpty ()
  {
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    ReadOnlyDictionary<int, int> test = ((IEnumerable<int>?) null).IntoReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      EnumerableNullBehavior.ReturnEmpty
    )!;
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void IntoReadOnlyDictionary_NullSource_ReturnDefault ()
  {
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    ReadOnlyDictionary<int, int> test = ((IEnumerable<int>?) null).IntoReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      EnumerableNullBehavior.ReturnDefault
    )!;
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void IntoReadOnlyDictionary_NullSource_ThrowException ()
  {
    const string expectation = "Null source enumerable encounter. (Parameter 'enumerable')";
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    Action test = () => _ = ((IEnumerable<int>?) null).IntoReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      EnumerableNullBehavior.ThrowException
    );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void IntoReadOnlyDictionary_NullSource_UknownBehavior ()
  {
    const string expectation = "Unsupported behavior, '793'. (Parameter 'behavior')";
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    Action test = () => _ = ((IEnumerable<int>?) null).IntoReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      (EnumerableNullBehavior) 793
    );

    UnsupportedNullBehaviorException e = Assert.ThrowsExactly<UnsupportedNullBehaviorException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void IntoReadOnlyDictionary_NullKeySelector ()
  {
    const string expectation = "Key selector not provided. (Parameter 'keySelector')";
    Func<int, int> keySelector = null!;
    Func<int, int> valueSelector = x => x *3;
    Action test = () => _ = new int[0].IntoReadOnlyDictionary(keySelector, valueSelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void IntoReadOnlyDictionary_NullValueSelector ()
  {
    const string expectation = "Value selector not provided. (Parameter 'valueSelector')";
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = null!;
    Action test = () => _ = new int[0].IntoReadOnlyDictionary(keySelector, valueSelector);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Unneeded." )]
  public void IntoReadOnlyDictionary ()
  {
    IEnumerable<int> enumerable = Enumerable.Range(1, 12);
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    ReadOnlyDictionary<int, int> test = enumerable.IntoReadOnlyDictionary(keySelector,valueSelector)!;

    Assert.IsTrue ( test.Keys.SequenceEqual ( enumerable.Select ( keySelector ) ) );
    Assert.IsTrue ( test.Values.SequenceEqual ( enumerable.Select ( valueSelector ) ) );

    Dictionary<int, int> dict = Dictionary(test);
    Assert.AreEqual ( 23, dict.Capacity );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Unneeded." )]
  public void IntoReadOnlyDictionary_ExactCapacity ()
  {
    const int count = 3;
    IEnumerable<int> enumerable = Enumerable.Range(1, count);
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;
    ReadOnlyDictionary<int, int> test = enumerable.IntoReadOnlyDictionary
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
}
