using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  [DataRow ( 100 )]
  [DataRow ( null )]
  public void AsOrToImmutableArray ( int? capacity )
  {
    const int count = 10;
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, count);
    ImmutableArray<int>? test = source.AsOrToImmutableArray(capacity)!;

    Assert.HasCount ( count, test );
    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToImmutableArray_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ImmutableArray<int> test = returnsDefault
      ? source.AsOrToImmutableArray(behavior: behavior!.Value)
      : source.AsOrToImmutableArray();

    Assert.AreEqual ( returnsDefault, test.IsDefault );
    Assert.HasCount ( 0, test.IsDefault ? [] : test );
  }

  [TestMethod]
  public void AsOrToImmutableArray_ExactLengthEnforcement ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    Action test= () => _ = source.AsOrToImmutableArray ( 1000, enforceLengthCountMatch: true )!;

    Exception e = Assert.ThrowsExactly<InvalidOperationException> ( test );
    Assert.AreEqual ( "MoveToImmutable can only be performed when Count equals Capacity.", e.Message );
  }

  [TestMethod]
  public void IntoImmutableDictionary_KeySelectorOnly ()
  {
    Func<int, int> keySelector = x => x * 2;
    TestComparer<int> keyComparer = new ();
    TestComparer<int> itemComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    ImmutableDictionary<int, int> test = source.IntoImmutableDictionary
    (
      keySelector,
      keyComparer,
      itemComparer
    )!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.KeyComparer ) );
    Assert.IsTrue ( ReferenceEquals ( itemComparer, test.ValueComparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source.Select(x => new KeyValuePair<int, int>(keySelector(x), x));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoImmutableDictionary_KeySelectorOnly_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    ImmutableDictionary<byte, int>? test = explicitNull
    ? source.IntoImmutableDictionary(x => (byte)x, keyComparer: null, itemComparer: null)!
    : source.IntoImmutableDictionary(x => (byte)x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<byte>.Default, test.KeyComparer ) );
    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<int>.Default, test.ValueComparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoImmutableDictionary_KeySelectorOnly_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ImmutableDictionary<int, int>? test = returnsDefault
    ? source.IntoImmutableDictionary(x => x, behavior: behavior!.Value)
    : source.IntoImmutableDictionary(x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }

  [TestMethod]
  public void IntoImmutableDictionary ()
  {
    Func<int, int> keySelector = x => x * 2;
    Func<int, int> valueSelector = x => x * 3;
    TestComparer<int> keyComparer = new ();
    TestComparer<int> valueComparer = new ();

    IEnumerable<int> source = Enumerable.Range(0, 10);
    ImmutableDictionary<int, int>? test = source.IntoImmutableDictionary
    (
      keySelector,
      valueSelector,
      keyComparer,
      valueComparer
    )!;

    Assert.IsTrue ( ReferenceEquals ( keyComparer, test.KeyComparer ) );
    Assert.IsTrue ( ReferenceEquals ( valueComparer, test.ValueComparer ) );

    IEnumerable<KeyValuePair<int, int>> expectation = source
    .Select(x => new KeyValuePair<int, int>(keySelector(x), valueSelector(x)));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void IntoImmutableDictionary_DefaultComparer ( bool explicitNull )
  {
    IEnumerable<int> source = [];
    ImmutableDictionary<byte, short>? test = explicitNull
    ? source.IntoImmutableDictionary(x => (byte)x, x => (short)x, keyComparer: null, valueComparer: null)!
    : source.IntoImmutableDictionary(x => (byte)x, x => (short)x)!;

    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<byte>.Default, test.KeyComparer ) );
    Assert.IsTrue ( ReferenceEquals ( EqualityComparer<short>.Default, test.ValueComparer ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoImmutableDictionary_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ImmutableDictionary<int, int>? test = returnsDefault
    ? source.IntoImmutableDictionary(x => x, x => x, behavior: behavior!.Value)
    : source.IntoImmutableDictionary(x => x, x => x);

    Assert.AreEqual ( test?.Count ?? -1, returnsDefault ? -1 : 0 );
  }
}
