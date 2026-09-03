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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToArrayList_IEnumerableOfT_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ArrayList? test = returnsDefault
      ? source.AsOrToArrayList(behavior: behavior!.Value)
      : source.AsOrToArrayList();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToArrayList_IEnumerable_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ArrayList? test = returnsDefault
      ? source.AsOrToArrayList(behavior: behavior!.Value)
      : source.AsOrToArrayList();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoHashtable_IEnumerableOfT_KeySelectorOnly ()
  {
    const int cap = 1000;
    Func<int, object> keySelector = x => x * 2;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    Hashtable test = source.IntoHashtable(keySelector, cap)!;

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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoHashtable_IEnumerableOfT_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Hashtable? test = returnsDefault
      ? source.IntoHashtable(x => x, behavior: behavior!.Value)
      : source.IntoHashtable(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoHashtable_IEnumerable_KeySelectorOnly ()
  {
    const int cap = 1000;
    Func<object, object> keySelector = x => x.GetHashCode();

    IEnumerable source = Enumerable.Range(0, 10);
    Hashtable test = source.IntoHashtable(keySelector, cap)!;

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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoHashtable_IEnumerable_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Hashtable? test = returnsDefault
      ? source.IntoHashtable(x => x, behavior: behavior!.Value)
      : source.IntoHashtable(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoHashtable_IEnumerableOfT ()
  {
    const int cap = 1000;
    Func<int, object> keySelector = x => x * 2;
    Func<int, object> valueSelector = x => x * 3;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    Hashtable test = source.IntoHashtable(keySelector, valueSelector, cap)!;

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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoHashtable_IEnumerableOfT_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Hashtable? test = returnsDefault
      ? source.IntoHashtable(x => x, x => x, behavior: behavior!.Value)
      : source.IntoHashtable(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoHashtable_IEnumerable ()
  {
    const int cap = 1000;
    Func<object, object> keySelector = x => x.GetHashCode();
    Func<object, object> valueSelector = x => x.GetHashCode() * 2;

    IEnumerable source = Enumerable.Range(0, 10);
    Hashtable test = source.IntoHashtable(keySelector,valueSelector, cap)!;

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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoHashtable_IEnumerable_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Hashtable? test = returnsDefault
      ? source.IntoHashtable(x => x, x => x, behavior: behavior!.Value)
      : source.IntoHashtable(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToQueue_IEnumerableOfT_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Queue? test = returnsDefault
      ? source.AsOrToQueue(behavior: behavior!.Value)
      : source.AsOrToQueue();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToQueue_IEnumerable_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Queue? test = returnsDefault
      ? source.AsOrToQueue(behavior: behavior!.Value)
      : source.AsOrToQueue();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoSortedList_IEnumerableOfT_KeySelectorOnly ()
  {
    const int cap = 1000;
    Func<int, object> keySelector = x => x * 2;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    SortedList test = source.IntoSortedList(keySelector, cap)!;

    IEnumerable<(int, int)> expectation = source.Select(x => ((int)keySelector(x), x)).OrderBy(x => x.Item1);
    IEnumerable<(int, int)> actual = test
      .Cast<DictionaryEntry>()
      .Select(x => ((int)x.Key, (int)x.Value!))
      .OrderBy(x => x.Item1);

    Assert.IsTrue ( expectation.SequenceEqual ( actual ) );
    Assert.AreEqual ( cap, test.Capacity );
  }


  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoSortedList_IEnumerableOfT_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {   
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    SortedList? test = returnsDefault
      ? source.IntoSortedList(x => x, behavior: behavior!.Value)
      : source.IntoSortedList(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoSortedList_IEnumerable_KeySelectorOnly ()
  {
    const int cap = 1000;
    Func<object, object> keySelector = x => x.GetHashCode();

    IEnumerable source = Enumerable.Range(0, 10);
    SortedList test = source.IntoSortedList(keySelector, cap)!;

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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoSortedList_IEnumerable_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {    
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    SortedList? test = returnsDefault
      ? source.IntoSortedList(x => x, behavior: behavior!.Value)
      : source.IntoSortedList(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoSortedList_IEnumerableOfT ()
  {
    const int cap = 1000;
    Func<int, object> keySelector = x => x * 2;
    Func<int, object> valueSelector = x => x * 3;

    IEnumerable<int> source = Enumerable.Range(0, 10);
    SortedList test = source.IntoSortedList(keySelector, valueSelector, cap)!;

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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoSortedList_IEnumerableOfT_NullBehavior ( NullBehavior? behavior )
  {    
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    SortedList? test = returnsDefault
      ? source.IntoSortedList(x => x, x => x, behavior: behavior!.Value)
      : source.IntoSortedList(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoSortedList_IEnumerable ()
  {
    const int cap = 1000;
    Func<object, object> keySelector = x => x.GetHashCode();
    Func<object, object> valueSelector = x => x.GetHashCode() * 2;

    IEnumerable source = Enumerable.Range(0, 10);
    SortedList test = source.IntoSortedList(keySelector,valueSelector, cap)!;

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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoSortedList_IEnumerable_NullBehavior ( NullBehavior? behavior )
  {    
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    SortedList? test = returnsDefault
      ? source.IntoSortedList(x => x, x => x, behavior: behavior!.Value)
      : source.IntoSortedList(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToStack_IEnumerableOfT_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Stack? test = returnsDefault
      ? source.AsOrToStack(behavior: behavior!.Value)
      : source.AsOrToStack();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
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
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToStack_IEnumerable_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Stack? test = returnsDefault
      ? source.AsOrToStack(behavior: behavior!.Value)
      : source.AsOrToStack();

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }
}
