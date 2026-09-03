using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class system_collections_generic_test
{
  [TestMethod]
  public void Dictionary_KeySelectorOnly ()
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;

    AsOrToTargetType<Dictionary<int, int>> targetType = system_collections_generic.Dictionary ( keySelector, keyComparer );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Dictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Dictionary<int, int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", false, true )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", true, false )]
  public void Dictionary_KeySelectorOnly_NullParameter ( string errMsg, bool nullComparer, bool nullSelector )
  {
    TestComparer<int> keyComparer = nullComparer ? null! : new ();
    Func<int, int> keySelector = nullSelector ? null! : x => x;

    Action test = () => system_collections_generic.Dictionary ( keySelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  public void Dictionary ()
  {
    TestComparer<int> keyComparer = new ();
    Func<int, int> keySelector = x => x *2;
    Func<int, int> valueSelector = x => x *3;

    AsOrToTargetType<Dictionary<int, int>> targetType = system_collections_generic.Dictionary
    (
      keySelector,
      valueSelector,
      keyComparer
    );
    Assert.IsFalse ( targetType.CanCast ( null! ) );
    Dictionary<int, int> empty = targetType.Empty ();
    Assert.HasCount ( 0, empty );
    Assert.IsTrue ( ReferenceEquals ( keyComparer, empty.Comparer ) );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(1, 10);
    Dictionary<int, int> target = targetType.Ctor(source, null);

    Assert.IsTrue ( ReferenceEquals ( keyComparer, target.Comparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
      .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( target ) );
  }

  [TestMethod]
  [DataRow ( "Key selector not provided. (Parameter 'keySelector')", 'k' )]
  [DataRow ( "Value selector not provided. (Parameter 'valueSelector')", 'v' )]
  [DataRow ( "Key comparer not provided. (Parameter 'keyComparer')", 'c' )]
  public void DictionaryNullParameter ( string errMsg, char whosNull )
  {
    TestComparer<int> keyComparer = whosNull is 'c' ? null! : new ();
    Func<int, int> keySelector    = whosNull == 'k' ? null! : x => x;
    Func<int, int> valueSelector  = whosNull == 'v' ? null! : x => x;

    Action test = () => system_collections_generic.Dictionary ( keySelector, valueSelector, keyComparer );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }
}

