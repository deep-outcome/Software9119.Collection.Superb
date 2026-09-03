using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_frozen_test
{
  [TestMethod]
  public void FrozenDictionary_KeySelectorOnly ()
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;

    AsOrToTargetType<FrozenDictionary<int, int>> targetType = system_collections_frozen.FrozenDictionary ( keySelector, keyComparer );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    FrozenDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    FrozenDictionary<int, int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", false, true )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", true, false )]
  public void FrozenDictionary_KeySelectorOnly_NullParameter ( string errMsg, bool nullComparer, bool nullSelector )
  {
    TestComparer<int> keyComparer = nullComparer ? null! : new ();
    Func<int, int> keySelector = nullSelector ? null! : x => x *2;

    Action test = () => system_collections_frozen.FrozenDictionary ( keySelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  public void FrozenDictionary ()
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;

    AsOrToTargetType<FrozenDictionary<int, int>> targetType = system_collections_frozen.FrozenDictionary
    (
      keySelector,
      valueSelector,
      keyComparer
    );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    FrozenDictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    FrozenDictionary<int, int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", 'k' )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", 'v' )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", 'c' )]
  public void FrozenDictionaryNullParameter ( string errMsg, char whosNull )
  {
    TestComparer<int> keyComparer  = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector        = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector      = whosNull == 'v' ? null! : x => x;

    Action test = () => system_collections_frozen.FrozenDictionary ( keySelector, valueSelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  public void FrozenSet ()
  {
    TestComparer<int> itemComparer = new ();
    AsOrToTargetType<FrozenSet<int>> targetType = system_collections_frozen.FrozenSet ( itemComparer );

    FrozenSet<int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( itemComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    FrozenSet<int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( ReferenceEquals ( itemComparer, target.Comparer ) );

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( System.Collections.Frozen.FrozenSet.ToFrozenSet ( [], new TestComparer<int> () ) ) );
    Assert.IsFalse ( targetType.CanCast ( System.Collections.Frozen.FrozenSet.ToFrozenSet<object> ( [] ) ) );
    Assert.IsTrue ( source.SequenceEqual ( target.OrderBy ( x => x ) ) );
  }


  [TestMethod]
  public void FrozenSet_NullComparer ()
  {
    TestComparer<int> itemComparer = null!;
    Action test = () => system_collections_frozen.FrozenSet ( itemComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Item comparer not provided. (Parameter 'itemComparer')", e.Message );
  }
}
