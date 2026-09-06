using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Immutable = System.Collections.Immutable;

using collections_immutable = Software9119.Collection.Superb.Extension.system_collections_immutable;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_immutable_test
{
  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void ImmutableArray ( int? capacity )
  {
    AsOrToTargetType<ImmutableArray<int>> targetType = collections_immutable.ImmutableArray<int> ( false );

    ImmutableArray<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    const int count = 10;
    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, count);
    ImmutableArray<int> target = targetType.Ctor(source, capacity);

    Assert.HasCount ( count, target );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }

  [TestMethod]
  public void ImmutableArray_ExactLengthEnforcement ()
  {
    AsOrToTargetType<ImmutableArray<int>> targetType = collections_immutable.ImmutableArray<int> ( true );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Action test = () => _ = targetType.Ctor(source, 1000);

    Exception e = Assert.ThrowsExactly<InvalidOperationException> ( test );
    Assert.AreEqual ( "MoveToImmutable can only be performed when Count equals Capacity.", e.Message );
  }

  [TestMethod]
  public void ImmutableDictionary_KeySelectorOnly ()
  {
    TestComparer<int> keyComparer = new ();
    TestComparer<int> itemComparer = new ();
    Func<int, int> keySelector = x => x *2;

    AsOrToTargetType<ImmutableDictionary<int, int>> targetType = collections_immutable.ImmutableDictionary
    (
      keySelector,
      keyComparer,
      itemComparer
    );

    ImmutableDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.KeyComparer ) );
    Assert.IsTrue ( ReferenceEquals ( itemComparer, empty.ValueComparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    ImmutableDictionary<int, int> target = targetType.Ctor(source, null);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.KeyComparer ) );
    Assert.IsTrue ( ReferenceEquals ( itemComparer, target.ValueComparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", "sk" )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", "ck" )]
  [DataRow ( "Value comparer not provided. (Parameter 'valueComparer')", "ci" )]
  public void ImmutableDictionary_KeySelectorOnly_NullParameter ( string errMsg, string whosNull )
  {
    TestComparer<int> keyComparer   = whosNull == "ck" ? null! : new ();
    TestComparer<int> itemComparer  = whosNull == "ci" ? null! : new ();
    Func<int, int> keySelector      = whosNull == "sk" ? null! : x => x;

    Action test = () => collections_immutable.ImmutableDictionary
    (
      keySelector,
      keyComparer,
      itemComparer
    );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  public void ImmutableDictionary ()
  {
    TestComparer<int> keyComparer = new ();
    TestComparer<int> valueComparer = new ();
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;

    AsOrToTargetType<ImmutableDictionary<int, int>> targetType = collections_immutable.ImmutableDictionary
    (
      keySelector,
      valueSelector,
      keyComparer,
      valueComparer
    );

    ImmutableDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.KeyComparer ) );
    Assert.IsTrue ( ReferenceEquals ( valueComparer, empty.ValueComparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    ImmutableDictionary<int, int> target = targetType.Ctor(source, null);

    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Assert.IsFalse ( targetType.CanCast ( target ) );

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.KeyComparer ) );
    Assert.IsTrue ( ReferenceEquals ( valueComparer, target.ValueComparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", "sk" )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", "sv" )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", "ck" )]
  [DataRow ( "Value comparer not provided. (Parameter 'valueComparer')", "cv" )]
  public void ImmutableDictionaryNullParameter ( string errMsg, string whosNull )
  {
    TestComparer<int> keyComparer   = whosNull is "ck" ? null! : new ();
    TestComparer<int> valueComparer = whosNull is "cv" ? null! : new ();
    Func<int, int> keySelector      = whosNull == "sk" ? null! : x => x;
    Func<int, int> valueSelector    = whosNull == "sv" ? null! : x => x;

    Action test = () => collections_immutable.ImmutableDictionary
    (
      keySelector,
      valueSelector,
      keyComparer,
      valueComparer
    );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  public void ImmutableHashSet ()
  {
    TestComparer<int> itemComparer = new ();
    AsOrToTargetType<ImmutableHashSet<int>> targetType = collections_immutable.ImmutableHashSet ( itemComparer );

    ImmutableHashSet<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( itemComparer, empty.KeyComparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    ImmutableHashSet<int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( ReferenceEquals ( itemComparer, target.KeyComparer ) );

    Assert.IsTrue ( targetType.CanCast ( Immutable.ImmutableHashSet.Create ( equalityComparer: itemComparer ) ) );
    Assert.IsFalse ( targetType.CanCast ( Immutable.ImmutableHashSet.Create ( new TestComparer<int> () ) ) );
    Assert.IsFalse ( targetType.CanCast ( Immutable.ImmutableHashSet.Create<object> () ) );

    Assert.IsTrue ( source.SequenceEqual ( target.OrderBy ( x => x ) ) );
  }

  [TestMethod]
  public void ImmutableHashSet_NullComparer ()
  {
    TestComparer<int> itemComparer = null!;
    Action test = () => collections_immutable.ImmutableHashSet ( itemComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Item comparer not provided. (Parameter 'itemComparer')", e.Message );
  }

  [TestMethod]
  public void ImmutableList ()
  {
    AsOrToTargetType<ImmutableList <int>> targetType = collections_immutable.ImmutableList<int>();

    ImmutableList <int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    ImmutableList <int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.IsTrue ( source.SequenceEqual ( target ) );
  }
}
