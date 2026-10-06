using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[SuppressMessage ( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = "Not truly beneficial." )]
#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{
  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToIListSegment_IEnumerableOfT ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<object?>? comparer = testComparer
      ? new TestComparer<object?>()
      : null;

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    IListSegment test = cap is int
      ? source.AsOrToIListSegment(cap, trimCapacity, comparer)
      : source.AsOrToIListSegment();

    int expectedCapacity = trimCapacity ? 12 : cap ?? 16;
    IEqualityComparer<object?> expectedComparer = testComparer  ? comparer! : EqualityComparer<object?>.Default;

    Assert.IsTrue ( source.SequenceEqual ( test.Cast<int> () ) );
    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.AreEqual ( expectedCapacity, ((ArrayList) test.List).Capacity );
    Assert.HasCount ( 12, test );
    Assert.AreEqual ( 0, test.Offset );
  }

  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToIListSegment_IListAlready_IEnumerableOfT ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<object?>? comparer = testComparer
      ? new TestComparer<object?>()
      : null;

    IEnumerable<object> source = XEnumerable.ObjectsEnumerable(3).AsOrToList(3)!;
    IListSegment test = source.AsOrToIListSegment(cap, trimCapacity, comparer);

    IEqualityComparer<object?> expectedComparer = testComparer  ? comparer! : EqualityComparer<object?>.Default;

    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.AreEqual ( 3, ((List<object>) test.List).Capacity );
    Assert.IsTrue ( ReferenceEquals ( source, test.List ) );
    Assert.HasCount ( 3, test );
    Assert.AreEqual ( 0, test.Offset );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToIListSegment_IEnumerableOfT_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    IListSegment test = returnsDefault
      ? source.AsOrToIListSegment(behavior: behavior!.Value)
      : source.AsOrToIListSegment();

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( returnsDefault, test == default );
  }

  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToIListSegment_IEnumerable ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<object?>? comparer = testComparer
      ? new TestComparer<object?>()
      : null;

    IEnumerable source = XEnumerable.RangeEnumerable(0, 12);
    IListSegment test = cap is int
      ? source.AsOrToIListSegment(cap, trimCapacity, comparer)
      : source.AsOrToIListSegment();

    int expectedCapacity = trimCapacity ? 12 : cap ?? 16;
    IEqualityComparer<object?> expectedComparer = testComparer  ? comparer! : EqualityComparer<object?>.Default;

    Assert.HasCount ( 12, test );
    Assert.AreEqual ( 0, test.Offset );
    Assert.IsTrue ( source.Cast<object> ().SequenceEqual ( test.Cast<object> () ) );
    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.AreEqual ( expectedCapacity, ((ArrayList) test.List).Capacity );
  }

  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToIListSegment_IListAlready_IEnumerable ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<object?>? comparer = testComparer
      ? new TestComparer<object?>()
      : null;

    IEnumerable source = XEnumerable.ObjectsEnumerable(3).AsOrToList(3)!;
    IListSegment test = source.AsOrToIListSegment(cap, trimCapacity, comparer);

    IEqualityComparer<object?> expectedComparer = testComparer  ? comparer! : EqualityComparer<object?>.Default;

    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.AreEqual ( 3, ((List<object>) test.List).Capacity );
    Assert.IsTrue ( ReferenceEquals ( source, test.List ) );
    Assert.HasCount ( 3, test );
    Assert.AreEqual ( 0, test.Offset );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToIListSegment_IEnumerable_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    IListSegment test = returnsDefault
      ? source.AsOrToIListSegment(behavior: behavior!.Value)
      : source.AsOrToIListSegment();

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( returnsDefault, test == default );
  }

  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToTypedIListSegment ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<int>? comparer = testComparer
      ? new TestComparer<int>()
      : null;

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    IListSegment<int> test = cap is int
      ? source.AsOrToTypedIListSegment(cap, trimCapacity, comparer)
      : source.AsOrToTypedIListSegment();

    int expectedCapacity = trimCapacity ? 12 : cap ?? 16;
    IEqualityComparer<int> expectedComparer = testComparer  ? comparer! : EqualityComparer<int>.Default;

    Assert.HasCount ( 12, test );
    Assert.AreEqual ( 0, test.Offset );
    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.IsTrue ( test.List is List<int> );
    Assert.AreEqual ( expectedCapacity, ((List<int>) test.List).Capacity );
  }

  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToTypedIListSegment_IListAlready ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<object?>? comparer = testComparer ? new TestComparer<object?>() : null;

    IEnumerable<object> source = XEnumerable.ObjectsEnumerable(3).AsOrToList(3)!;
    IListSegment<object> test = source.AsOrToTypedIListSegment(cap, trimCapacity, comparer)!;

    IEqualityComparer<object?> expectedComparer = testComparer  ? comparer! : EqualityComparer<object?>.Default;

    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.IsTrue ( ReferenceEquals ( source, test.List ) );
    Assert.AreEqual ( 3, ((List<object>) test.List).Capacity );
    Assert.HasCount ( 3, test );
    Assert.AreEqual ( 0, test.Offset );
  }

  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToTypedIListSegment_ICollection ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<int>? comparer = testComparer ? new TestComparer<int>() : null;

    HashSet<int> source = XEnumerable.RangeEnumerable(0, 3).AsOrToHashSet(3)!;
    IListSegment<int> test = source.AsOrToTypedIListSegment(cap, trimCapacity, comparer);

    IEqualityComparer<int> expectedComparer = testComparer  ? comparer! : EqualityComparer<int>.Default;

    Assert.HasCount ( 3, test );
    Assert.AreEqual ( 0, test.Offset );
    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.IsTrue ( test.List is int [] );
    Assert.AreEqual ( 3, ((int []) test.List).Length );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToTypedIListSegment_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    IListSegment<int> test = returnsDefault
      ? source.AsOrToTypedIListSegment(behavior: behavior!.Value)
      : source.AsOrToTypedIListSegment();

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( returnsDefault, test == default );
  }

  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToIReadOnlyListSegment ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<int>? comparer = testComparer
      ? new TestComparer<int>()
      : null;

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    IReadOnlyListSegment<int> test = cap is int
      ? source.AsOrToIReadOnlyListSegment(cap, trimCapacity, comparer)
      : source.AsOrToIReadOnlyListSegment();

    int expectedCapacity = trimCapacity ? 12 : cap ?? 16;
    IEqualityComparer<int> expectedComparer = testComparer  ? comparer! : EqualityComparer<int>.Default;

    Assert.HasCount ( 12, test );
    Assert.AreEqual ( 0, test.Offset );
    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.IsTrue ( test.List is List<int> );
    Assert.AreEqual ( expectedCapacity, ((List<int>) test.List).Capacity );
  }

  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToIReadOnlyListSegment_IList ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<object?>? comparer = testComparer ? new TestComparer<object?>() : null;

    IEnumerable<object> source = new XList<object>(XEnumerable.ObjectsEnumerable(3).AsOrToList(3)!);
    IReadOnlyListSegment<object> test = source.AsOrToIReadOnlyListSegment(cap, trimCapacity, comparer)!;

    IEqualityComparer<object?> expectedComparer = testComparer  ? comparer! : EqualityComparer<object?>.Default;

    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.HasCount ( 3, test );
    Assert.AreEqual ( 0, test.Offset );

    IReadOnlyList<object?> list = test.List;
    Assert.IsTrue ( list is ReadOnlyCollection<object?> );

    XList<object> innerList = (XList<object>) Reflection.GetNonPublicFieldValue ( list, "list" );
    Assert.AreEqual ( 3, innerList.List.Capacity );

    Assert.IsTrue ( ReferenceEquals ( source, innerList ) );
  }

  [TestMethod]
  [DataRow ( 100, true, true )]
  [DataRow ( 100, false, true )]
  [DataRow ( null, false, false )]
  public void AsOrToIReadOnlyListSegment_ICollection ( int? cap, bool trimCapacity, bool testComparer )
  {
    IEqualityComparer<int>? comparer = testComparer ? new TestComparer<int>() : null;

    HashSet<int> source = XEnumerable.RangeEnumerable(0, 3).AsOrToHashSet(3)!;
    IReadOnlyListSegment<int> test = source.AsOrToIReadOnlyListSegment(cap, trimCapacity, comparer);

    IEqualityComparer<int> expectedComparer = testComparer  ? comparer! : EqualityComparer<int>.Default;

    Assert.HasCount ( 3, test );
    Assert.AreEqual ( 0, test.Offset );
    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.IsTrue ( ReferenceEquals ( expectedComparer, test.EqualityComparer ) );
    Assert.IsTrue ( test.List is int [] );
    Assert.AreEqual ( 3, ((int []) test.List).Length );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToIReadOnlyListSegment_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    IReadOnlyListSegment<int> test = returnsDefault
      ? source.AsOrToIReadOnlyListSegment(behavior: behavior!.Value)
      : source.AsOrToIReadOnlyListSegment();

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( returnsDefault, test == default );
  }

  [TestMethod]
  public void AsOrToArraySegment ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    ArraySegment<int> test = source.AsOrToArraySegment();

    Assert.HasCount ( 12, test );
    Assert.AreEqual ( 0, test.Offset );
    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.AreEqual ( 12, test.Array!.Length );
  }

  [TestMethod]
  public void AsOrToArraySegment_ArrayAlready ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12).ToArray();
    ArraySegment<int> test = source.AsOrToArraySegment();

    Assert.HasCount ( 12, test );
    Assert.AreEqual ( 0, test.Offset );
    Assert.IsTrue ( ReferenceEquals ( source, test.Array ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrToArraySegment_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ArraySegment<int> test = returnsDefault
      ? source.AsOrToArraySegment(behavior: behavior!.Value)
      : source.AsOrToArraySegment();

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( returnsDefault, test == default );
  }

  [TestMethod]
  public void IntoMemory ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    Memory<int> test = source.IntoMemory();

    Assert.AreEqual ( 12, test.Length );
    Assert.IsTrue ( source.ToArray ().SequenceEqual ( test.Span ) );
  }

  [TestMethod]
  public void IntoMemory_ArrayAlready ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12).ToArray();
    Memory<int> test = source.IntoMemory();

    int[] store = (int[]) Reflection.GetNonPublicFieldValue ( test, "_object" );

    Assert.AreEqual ( 12, test.Length );
    Assert.IsTrue ( ReferenceEquals ( source, store ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoMemory_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    Memory<int> test = returnsDefault
      ? source.IntoMemory(behavior: behavior!.Value)
      : source.IntoMemory();

    Assert.AreEqual ( 0, test.Length );
    Assert.AreEqual ( returnsDefault, test.Equals ( default ( Memory<int> ) ) );
  }

  [TestMethod]
  public void IntoReadOnlyMemory ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    ReadOnlyMemory<int> test = source.IntoReadOnlyMemory();

    Assert.AreEqual ( 12, test.Length );
    Assert.IsTrue ( source.ToArray ().SequenceEqual ( test.Span ) );
  }

  [TestMethod]
  public void IntoReadOnlyMemory_ArrayAlready ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12).ToArray();
    ReadOnlyMemory<int> test = source.IntoReadOnlyMemory();

    int[] store = (int[]) Reflection.GetNonPublicFieldValue ( test, "_object" );

    Assert.AreEqual ( 12, test.Length );
    Assert.IsTrue ( ReferenceEquals ( source, store ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void IntoReadOnlyMemory_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    ReadOnlyMemory<int> test = returnsDefault
      ? source.IntoReadOnlyMemory(behavior: behavior!.Value)
      : source.IntoReadOnlyMemory();

    Assert.AreEqual ( 0, test.Length );
    Assert.AreEqual ( returnsDefault, test.Equals ( default ( ReadOnlyMemory<int> ) ) );
  }
}
