using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{

  [TestMethod]
  public void AsOrToConcurrentBag ()
  {
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ConcurrentBag<int> test = source.AsOrToConcurrentBag()!;

    Assert.IsTrue ( source.SequenceEqual ( test.Order () ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToConcurrentBag_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ConcurrentBag<int>? test = returnsDefault
      ? source.AsOrToConcurrentBag(behavior: behavior!.Value)
      : source.AsOrToConcurrentBag();

    Assert.AreEqual ( test?.IsEmpty ?? false, returnsDefault == false );
  }

  [TestMethod]
  [DataRow ( 100, 107, 2, true )]
  [DataRow ( 100, 107, 2, false )]
  [DataRow ( null, 37, null, true )]
  [DataRow ( null, 37, null, false )]
  public void IntoConcurrentDictionary ( int? cap, int expCap, int? concurrency, bool keySelectorOnly )
  {
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = keySelectorOnly ? x => x : x => x * 3;
    TestComparer<int> keyComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    ConcurrentDictionary<int, int>? test =
      cap is int
        ? keySelectorOnly
          ? source.IntoConcurrentDictionary(keySelector, capacity: cap, concurrency, keyComparer)!
          : source.IntoConcurrentDictionary(keySelector, valueSelector, capacity: cap, concurrency, keyComparer)!
        : keySelectorOnly
          ? source.IntoConcurrentDictionary(keySelector,  keyComparer: keyComparer)!
          : source.IntoConcurrentDictionary(keySelector, valueSelector, keyComparer: keyComparer)!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.Comparer ) );

    object _tables = Reflection.GetNonPublicFieldValue(test, "_tables");
    object[] _locks = (object[])Reflection.GetNonPublicFieldValue(_tables, "_locks");
    Assert.HasCount ( concurrency ?? Environment.ProcessorCount, _locks );
    Assert.AreNotEqual ( Environment.ProcessorCount, concurrency ?? 0 );

    int _initialCapacity = (int)Reflection.GetNonPublicFieldValue(test, "_initialCapacity");
    Assert.AreEqual ( expCap, _initialCapacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source
    .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true, true )]
  [DataRow ( true, false )]
  [DataRow ( false, false )]
  [DataRow ( false, true )]
  public void IntoConcurrentDictionary_DefaultComparer ( bool explicitNull, bool keySelectorOnly )
  {
    IEnumerable<int> source = [];
    ConcurrentDictionary<int, int>? test = explicitNull
      ? keySelectorOnly
        ? source.IntoConcurrentDictionary(x => x, keyComparer: null)!
        : source.IntoConcurrentDictionary(x => x, x => x, keyComparer: null)!
      : keySelectorOnly
        ? source.IntoConcurrentDictionary(x => x)!
        : source.IntoConcurrentDictionary(x => x, x => x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.Comparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault, true )]
  [DataRow ( NullBehavior.ReturnDefault, false )]
  [DataRow ( null, true )]
  [DataRow ( null, false )]
  public void IntoConcurrentDictionary_NullBehavior ( NullBehavior? behavior, bool keySelectorOnly )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ConcurrentDictionary<int, int>? test = returnsDefault
      ? keySelectorOnly
        ? source.IntoConcurrentDictionary(x => x, behavior: behavior!.Value)!
        : source.IntoConcurrentDictionary(x => x, x => x, behavior: behavior!.Value)!
      : keySelectorOnly
        ? source.IntoConcurrentDictionary(x => x)!
        : source.IntoConcurrentDictionary(x => x, x => x)!;

    Assert.AreEqual ( test?.IsEmpty ?? false, returnsDefault == false );
    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void AsOrToConcurrentQueue ()
  {
    IEnumerable<int> source = Enumerable.Range(0, 10);
    ConcurrentQueue<int> test = source.AsOrToConcurrentQueue()!;

    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToConcurrentQueue_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ConcurrentQueue<int>? test = returnsDefault
      ? source.AsOrToConcurrentQueue(behavior: behavior!.Value)
      : source.AsOrToConcurrentQueue();

    Assert.AreEqual ( test?.IsEmpty ?? false, returnsDefault == false );
    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }
}
