using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using c_objectmodel = Software9119.Collection.Superb.Extension.system_collections_objectmodel;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_objectmodel_test
{

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void Collection ( int? capacity )
  {
    AsOrToTargetType<Collection<int>> targetType = c_objectmodel.Collection<int> (capacity );

    Collection<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    const int count = 10;
    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, count);
    Collection<int> target = targetType.Ctor(source);

    Assert.HasCount ( count, target );

    List<int> items = (List<int>) Reflection.GetNonPublicFieldValue ( target, "items" );
    Assert.AreEqual ( capacity ?? 16, items.Capacity );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void ObservableCollection ( bool useList )
  {
    AsOrToTargetType<ObservableCollection<int>> targetType = c_objectmodel.ObservableCollection<int> ( );

    ObservableCollection<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    const int count = 10;
    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, count);
    if (useList)
      source = source.ToList ();

    ObservableCollection<int> target = targetType.Ctor(source);

    Assert.HasCount ( count, target );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void ReadOnlyCollection ( int? capacity )
  {
    AsOrToTargetType<ReadOnlyCollection<int>> targetType = c_objectmodel.ReadOnlyCollection<int> (capacity );

    ReadOnlyCollection<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    int[] emptyList = (int[]) Reflection.GetNonPublicFieldValue ( empty, "list" );
    Assert.IsTrue ( ReferenceEquals ( emptyList, Array.Empty<int> () ) );

    const int count = 10;
    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, count);
    ReadOnlyCollection<int> target = targetType.Ctor(source);

    Assert.HasCount ( count, target );

    List<int> targetList = (List<int>) Reflection.GetNonPublicFieldValue ( target, "list" );
    Assert.AreEqual ( capacity ?? 16, targetList.Capacity );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  public void ReadOnlyDictionary_AsOnly ()
  {
    AsOrToTargetType<ReadOnlyDictionary<object, int>> targetType = c_objectmodel.ReadOnlyDictionary<object, int> ( );

    ReadOnlyDictionary<object, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    Dictionary<object, int> source = new ()
    {
      [new object()] = 1,
      [new object()] = 2,
      [new object()] = 3,
    };

    ReadOnlyDictionary<object, int> target = targetType.Ctor(source);
    object innerDict = Reflection.GetNonPublicFieldValue ( target, "m_dictionary" );
    Assert.IsTrue ( ReferenceEquals ( source, innerDict ) );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.HasCount ( source.Count, target );
    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( 100, 107, true )]
  [DataRow ( 100, 107, false )]
  [DataRow ( null, 11, true )]
  [DataRow ( null, 11, false )]
  public void ReadOnlyDictionary ( int? capacityRequested, int capacityGotten, bool keySelectorOnly )
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = keySelectorOnly ? x => x : x => x *3;

    AsOrToTargetType<ReadOnlyDictionary<int, int>> targetType = keySelectorOnly
      ? c_objectmodel.ReadOnlyDictionary
        (keySelector, keyComparer, capacityRequested)
      : c_objectmodel.ReadOnlyDictionary
        (keySelector, valueSelector, keyComparer, capacityRequested);

    ReadOnlyDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    ReadOnlyDictionary<int, int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    Dictionary<int, int> innerDict = (Dictionary<int, int>)Reflection
      .GetNonPublicFieldValue ( target, "m_dictionary" );

    Assert.IsTrue ( ReferenceEquals ( keyComparer, innerDict.Comparer ) );
    Assert.AreEqual ( capacityGotten, innerDict.Capacity );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }
}
