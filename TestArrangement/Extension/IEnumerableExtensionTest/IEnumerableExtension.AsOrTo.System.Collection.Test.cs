using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  public void AsOrToArrayList_IEnumerableOfT ()
  {
    const int cap = 1000;
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ArrayList test = source.AsOrToArrayList(cap)!;
    Assert.IsTrue ( source.SequenceEqual ( test.Cast<int> () ) );
    Assert.AreEqual ( cap, test.Capacity );
  }

  [TestMethod]
  public void AsOrToArrayList_IEnumerableOfT_NullBehavior ()
  {
    IEnumerable<int> source = null!;
    ArrayList? test = source.AsOrToArrayList(behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void AsOrToArrayList_IEnumerable ()
  {
    const int cap = 1000;
    IEnumerable source = Enumerable.Range(0, 10);
    ArrayList test = source.AsOrToArrayList(cap)!;
    Assert.IsTrue ( source.Cast<int> ().SequenceEqual ( test.Cast<int> () ) );
    Assert.AreEqual ( cap, test.Capacity );
  }

  [TestMethod]
  public void AsOrToArrayList_IEnumerable_NullBehavior ()
  {
    IEnumerable source = null!;
    ArrayList? test = source.AsOrToArrayList(behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToHashtable_IEnumerableOfT_KeySelectorOnly ()
  {
    const int cap = 1000;
    Func<int, object> keySelector = x => x * 2;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    Hashtable test = source.ToHashtable(keySelector, cap)!;

    IEnumerable<(int, int)> expectation = source.Select(x => ((int)keySelector(x), x)).OrderBy(x => x.Item1);
    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );

    int _loadsize = (int)Reflection.GetNonPublicFieldValue(test, "_loadsize");
    Assert.AreEqual ( 1149, _loadsize );
  }

  [TestMethod]
  public void ToHashtable_IEnumerableOfT_KeySelectorOnly_NullBehavior ()
  {
    Func<int, object> keySelector = x => x;
    IEnumerable<int> source = null!;
    Hashtable? test = source.ToHashtable(keySelector, behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToHashtable_IEnumerable_KeySelectorOnly ()
  {
    const int cap = 1000;
    Func<object, object> keySelector = x => x.GetHashCode();

    IEnumerable source = Enumerable.Range(0, 10);
    Hashtable test = source.ToHashtable(keySelector, cap)!;

    IEnumerable<(int, int)> expectation = source
      .Cast<int>()
      .Select(x => ((int)keySelector(x), x))
      .OrderBy(x => x.Item1);
    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );

    int _loadsize = (int)Reflection.GetNonPublicFieldValue(test, "_loadsize");
    Assert.AreEqual ( 1149, _loadsize );
  }

  [TestMethod]
  public void ToHashtable_IEnumerable_KeySelectorOnly_NullBehavior ()
  {
    Func<object, object> keySelector = x => x;
    IEnumerable source = null!;
    Hashtable? test = source.ToHashtable(keySelector, behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToHashtable_IEnumerableOfT ()
  {
    const int cap = 1000;
    Func<int, object> keySelector = x => x * 2;
    Func<int, object> valueSelector = x => x * 3;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    Hashtable test = source.ToHashtable(keySelector, valueSelector, cap)!;

    IEnumerable<(int, int)> expectation = source
      .Select(x => ((int)keySelector(x), (int)valueSelector(x)))
      .OrderBy(x => x.Item1);
    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );

    int _loadsize = (int)Reflection.GetNonPublicFieldValue(test, "_loadsize");
    Assert.AreEqual ( 1149, _loadsize );
  }

  [TestMethod]
  public void ToHashtable_IEnumerableOfT_NullBehavior ()
  {
    Func<int, object> keySelector = x => x;
    Func<int, object> valueSelector = x => x;
    IEnumerable<int> source = null!;
    Hashtable? test = source.ToHashtable(keySelector, valueSelector, behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToHashtable_IEnumerable ()
  {
    const int cap = 1000;
    Func<object, object> keySelector = x => x.GetHashCode();
    Func<object, object> valueSelector = x => x.GetHashCode() * 2;

    IEnumerable source = Enumerable.Range(0, 10);
    Hashtable test = source.ToHashtable(keySelector,valueSelector, cap)!;

    IEnumerable<(int, int)> expectation = source
      .Cast<int>()
      .Select(x => ((int)keySelector(x), (int)valueSelector(x)))
      .OrderBy(x => x.Item1);
    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );

    int _loadsize = (int)Reflection.GetNonPublicFieldValue(test, "_loadsize");
    Assert.AreEqual ( 1149, _loadsize );
  }

  [TestMethod]
  public void ToHashtable_IEnumerable_NullBehavior ()
  {
    Func<object, object> keySelector = x => x;
    Func<object, object> valueSelector = x => x;
    IEnumerable source = null!;
    Hashtable? test = source.ToHashtable(keySelector,valueSelector, behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void AsOrToQueue_IEnumerableOfT ()
  {
    const int cap = 1000;
    IEnumerable<int> source = Enumerable.Range(0, 10);
    Queue test = source.AsOrToQueue(cap)!;
    Assert.IsTrue ( source.SequenceEqual ( test.Cast<int> () ) );

    Array storage = (Array)Reflection.GetNonPublicFieldValue(test, "_array");
    Assert.HasCount ( cap, storage );
  }

  [TestMethod]
  public void AsOrToQueue_IEnumerableOfT_NullBehavior ()
  {
    IEnumerable<int> source = null!;
    Queue? test = source.AsOrToQueue(behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void AsOrToQueue_IEnumerable ()
  {
    const int cap = 1000;
    IEnumerable source = Enumerable.Range(0, 10);
    Queue test = source.AsOrToQueue(cap)!;
    Assert.IsTrue ( source.Cast<int> ().SequenceEqual ( test.Cast<int> () ) );

    Array storage = (Array)Reflection.GetNonPublicFieldValue(test, "_array");
    Assert.HasCount ( cap, storage );
  }

  [TestMethod]
  public void AsOrToQueue_IEnumerable_NullBehavior ()
  {
    IEnumerable source = null!;
    Queue? test = source.AsOrToQueue(behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToSortedList_IEnumerableOfT_KeySelectorOnly ()
  {
    const int cap = 1000;
    Func<int, object> keySelector = x => x * 2;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    SortedList test = source.ToSortedList(keySelector, cap)!;

    IEnumerable<(int, int)> expectation = source.Select(x => ((int)keySelector(x), x)).OrderBy(x => x.Item1);
    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
    Assert.AreEqual ( cap, test.Capacity );
  }

  [TestMethod]
  public void ToSortedList_IEnumerableOfT_KeySelectorOnly_NullBehavior ()
  {
    Func<int, object> keySelector = x => x;
    IEnumerable<int> source = null!;
    SortedList? test = source.ToSortedList(keySelector, behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToSortedList_IEnumerable_KeySelectorOnly ()
  {
    const int cap = 1000;
    Func<object, object> keySelector = x => x.GetHashCode();

    IEnumerable source = Enumerable.Range(0, 10);
    SortedList test = source.ToSortedList(keySelector, cap)!;

    IEnumerable<(int, int)> expectation = source
      .Cast<int>()
      .Select(x => ((int)keySelector(x), x))
      .OrderBy(x => x.Item1);
    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
    Assert.AreEqual ( cap, test.Capacity );
  }

  [TestMethod]
  public void ToSortedList_IEnumerable_KeySelectorOnly_NullBehavior ()
  {
    Func<object, object> keySelector = x => x;
    IEnumerable source = null!;
    SortedList? test = source.ToSortedList(keySelector, behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToSortedList_IEnumerableOfT ()
  {
    const int cap = 1000;
    Func<int, object> keySelector = x => x * 2;
    Func<int, object> valueSelector = x => x * 3;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    SortedList test = source.ToSortedList(keySelector, valueSelector, cap)!;

    IEnumerable<(int, int)> expectation = source
      .Select(x => ((int)keySelector(x), (int)valueSelector(x)))
      .OrderBy(x => x.Item1);
    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
    Assert.AreEqual ( cap, test.Capacity );
  }

  [TestMethod]
  public void ToSortedList_IEnumerableOfT_NullBehavior ()
  {
    Func<int, object> keySelector = x => x;
    Func<int, object> valueSelector = x => x;
    IEnumerable<int> source = null!;
    SortedList? test = source.ToSortedList(keySelector, valueSelector, behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToSortedList_IEnumerable ()
  {
    const int cap = 1000;
    Func<object, object> keySelector = x => x.GetHashCode();
    Func<object, object> valueSelector = x => x.GetHashCode() * 2;

    IEnumerable source = Enumerable.Range(0, 10);
    SortedList test = source.ToSortedList(keySelector,valueSelector, cap)!;

    IEnumerable<(int, int)> expectation = source
      .Cast<int>()
      .Select(x => ((int)keySelector(x), (int)valueSelector(x)))
      .OrderBy(x => x.Item1);
    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
    Assert.AreEqual ( cap, test.Capacity );
  }

  [TestMethod]
  public void ToSortedList_IEnumerable_NullBehavior ()
  {
    Func<object, object> keySelector = x => x;
    Func<object, object> valueSelector = x => x;
    IEnumerable source = null!;
    SortedList? test = source.ToSortedList(keySelector,valueSelector, behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void AsOrToStack_IEnumerableOfT ()
  {
    const int cap = 1000;
    IEnumerable<int> source = Enumerable.Range(0, 10);
    Stack test = source.AsOrToStack(cap)!;
    Assert.IsTrue ( source.SequenceEqual ( test.Cast<int> ().Reverse () ) );

    Array storage = (Array)Reflection.GetNonPublicFieldValue(test, "_array");
    Assert.HasCount ( cap, storage );
  }

  [TestMethod]
  public void AsOrToStack_IEnumerableOfT_NullBehavior ()
  {
    IEnumerable<int> source = null!;
    Stack? test = source.AsOrToStack(behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void AsOrToStack_IEnumerable ()
  {
    const int cap = 1000;
    IEnumerable source = Enumerable.Range(0, 10);
    Stack test = source.AsOrToStack(cap)!;
    Assert.IsTrue ( source.Cast<int> ().SequenceEqual ( test.Cast<int> ().Reverse () ) );

    Array storage = (Array)Reflection.GetNonPublicFieldValue(test, "_array");
    Assert.HasCount ( cap, storage );
  }

  [TestMethod]
  public void AsOrToStack_IEnumerable_NullBehavior ()
  {
    IEnumerable source = null!;
    Stack? test = source.AsOrToStack(behavior: EnumerableNullBehavior.ReturnDefault);
    Assert.IsNull ( test );
  }
}
