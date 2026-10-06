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

[TestClass]
[SuppressMessage ( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = "Not truly beneficial." )]
public class segment_type_test
{
  [TestMethod]
  [DataRow ( 100, true )]
  [DataRow ( 100, false )]
  [DataRow ( null, false )]
  [DataRow ( null, true )]
  public void IListSegment ( int? capacity, bool trimCapacity )
  {
    TestComparer<object?> comparer = new ();

    AsOrToTargetType<IListSegment> targetType = segment_type.IListSegment(capacity, trimCapacity, comparer);

    Assert.HasCount ( 0, targetType.Empty () );
    Assert.AreEqual ( Array.Empty<object> (), targetType.Empty ().List );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    IListSegment target = targetType.Ctor(source);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    int expectedCapacity = trimCapacity ? 12 : capacity ?? 16;

    Assert.HasCount ( 12, target );
    Assert.AreEqual ( 0, target.Offset );
    Assert.IsTrue ( source.SequenceEqual ( target.Cast<int> () ) );
    Assert.IsTrue ( ReferenceEquals ( comparer, target.EqualityComparer ) );
    Assert.AreEqual ( expectedCapacity, ((ArrayList) target.List).Capacity );
  }

  [TestMethod]
  [DataRow ( 100, true )]
  [DataRow ( 100, false )]
  [DataRow ( null, false )]
  public void IListSegment_IListAlready ( int? capacity, bool trimCapacity )
  {
    TestComparer<object?> comparer = new ();

    AsOrToTargetType<IListSegment> targetType = segment_type.IListSegment(capacity, trimCapacity, comparer);

    IEnumerable<object> source = XEnumerable.ObjectsEnumerable(3).AsOrToList(3)!;
    IListSegment target = targetType.Ctor(source);

    Assert.HasCount ( 3, target );
    Assert.AreEqual ( 0, target.Offset );
    Assert.IsTrue ( ReferenceEquals ( comparer, target.EqualityComparer ) );
    Assert.AreEqual ( 3, ((List<object>) target.List).Capacity );
    Assert.IsTrue ( ReferenceEquals ( source, target.List ) );
  }

  [TestMethod]
  public void IListSegment_NullItemComparer ()
  {
    Action test = () => segment_type.IListSegment(0, default, itemComparer: null!);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );

    Assert.AreEqual ( "Item comparer not provided. (Parameter 'itemComparer')", e.Message );
  }

  [TestMethod]
  [DataRow ( 100, true )]
  [DataRow ( 100, false )]
  [DataRow ( null, false )]
  [DataRow ( null, true )]
  public void IListSegmentOfT ( int? capacity, bool trimCapacity )
  {
    TestComparer<int> comparer = new ();

    AsOrToTargetType<IListSegment<int>> targetType = segment_type.IListSegment(capacity, trimCapacity, comparer);

    Assert.HasCount ( 0, targetType.Empty () );
    Assert.AreEqual ( Array.Empty<int> (), targetType.Empty ().List );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    IListSegment<int> target = targetType.Ctor(source);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    int expectedCapacity = trimCapacity ? 12 : capacity ?? 16;

    Assert.HasCount ( 12, target );
    Assert.AreEqual ( 0, target.Offset );
    Assert.IsTrue ( source.SequenceEqual ( target ) );
    Assert.IsTrue ( ReferenceEquals ( comparer, target.EqualityComparer ) );
    Assert.AreEqual ( expectedCapacity, ((List<int>) target.List).Capacity );
  }

  [TestMethod]
  [DataRow ( 100, true )]
  [DataRow ( 100, false )]
  [DataRow ( null, false )]
  [DataRow ( null, true )]
  public void IListSegmentOfT_IListAlready ( int? capacity, bool trimCapacity )
  {
    TestComparer<int> comparer = new ();

    AsOrToTargetType<IListSegment<int>> targetType = segment_type.IListSegment(capacity, trimCapacity, comparer);

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 3).AsOrToList(3)!;
    IListSegment<int> target = targetType.Ctor(source);

    Assert.HasCount ( 3, target );
    Assert.AreEqual ( 0, target.Offset );
    Assert.IsTrue ( ReferenceEquals ( comparer, target.EqualityComparer ) );
    Assert.AreEqual ( 3, ((List<int>) target.List).Capacity );
    Assert.IsTrue ( ReferenceEquals ( source, target.List ) );
  }

  [TestMethod]
  [DataRow ( 100, true )]
  [DataRow ( 100, false )]
  [DataRow ( null, false )]
  [DataRow ( null, true )]
  public void IListSegmentOfT_ICollection ( int? capacity, bool trimCapacity )
  {
    TestComparer<int> comparer = new ();

    AsOrToTargetType<IListSegment<int>> targetType = segment_type.IListSegment(capacity, trimCapacity, comparer);

    HashSet<int> source = XEnumerable.RangeEnumerable(0, 3).AsOrToHashSet(3)!;
    IListSegment<int> target = targetType.Ctor(source);

    Assert.HasCount ( 3, target );
    Assert.AreEqual ( 0, target.Offset );
    Assert.IsTrue ( source.SequenceEqual ( target ) );
    Assert.IsTrue ( ReferenceEquals ( comparer, target.EqualityComparer ) );
    Assert.IsTrue ( target.List is int [] );
    Assert.AreEqual ( 3, ((int []) target.List).Length );
  }

  [TestMethod]
  public void IListSegmentOfT_NullItemComparer ()
  {
    Action test = () => segment_type.IListSegment<int>(0, default, itemComparer: null!);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );

    Assert.AreEqual ( "Item comparer not provided. (Parameter 'itemComparer')", e.Message );
  }

  [TestMethod]
  [DataRow ( 100, true )]
  [DataRow ( 100, false )]
  [DataRow ( null, false )]
  [DataRow ( null, true )]
  public void IReadOnlyListSegment ( int? capacity, bool trimCapacity )
  {
    TestComparer<int> comparer = new ();

    AsOrToTargetType<IReadOnlyListSegment<int>> targetType = segment_type.IReadOnlyListSegment(capacity, trimCapacity, comparer);

    Assert.HasCount ( 0, targetType.Empty () );
    Assert.AreEqual ( Array.Empty<int> (), targetType.Empty ().List );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    IReadOnlyListSegment<int> target = targetType.Ctor(source);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    int expectedCapacity = trimCapacity ? 12 : capacity ?? 16;

    Assert.HasCount ( 12, target );
    Assert.AreEqual ( 0, target.Offset );
    Assert.IsTrue ( source.SequenceEqual ( target ) );
    Assert.IsTrue ( ReferenceEquals ( comparer, target.EqualityComparer ) );
    Assert.AreEqual ( expectedCapacity, ((List<int>) target.List).Capacity );
  }

  [TestMethod]
  [DataRow ( 100, true )]
  [DataRow ( 100, false )]
  [DataRow ( null, false )]
  [DataRow ( null, true )]
  public void IReadOnlyListSegment_IList ( int? capacity, bool trimCapacity )
  {
    TestComparer<int> comparer = new ();

    AsOrToTargetType<IReadOnlyListSegment<int>> targetType = segment_type.IReadOnlyListSegment(capacity, trimCapacity, comparer);

    IEnumerable<int> source = new XList<int>(XEnumerable.RangeEnumerable(0, 3).AsOrToList(3)!);
    IReadOnlyListSegment<int> target = targetType.Ctor(source);

    Assert.IsTrue ( ReferenceEquals ( comparer, target.EqualityComparer ) );
    Assert.HasCount ( 3, target );
    Assert.AreEqual ( 0, target.Offset );

    IReadOnlyList<int> list = target.List;
    Assert.IsTrue ( list is ReadOnlyCollection<int> );

    XList<int> innerList = (XList<int>) Reflection.GetNonPublicFieldValue ( list, "list" );
    Assert.AreEqual ( 3, innerList.List.Capacity );

    Assert.IsTrue ( ReferenceEquals ( source, innerList ) );
  }

  [TestMethod]
  [DataRow ( 100, true )]
  [DataRow ( 100, false )]
  [DataRow ( null, false )]
  [DataRow ( null, true )]
  public void IReadOnlyListSegment_ICollection ( int? capacity, bool trimCapacity )
  {
    TestComparer<int> comparer = new ();

    AsOrToTargetType<IReadOnlyListSegment<int>> targetType = segment_type.IReadOnlyListSegment(capacity, trimCapacity, comparer);

    HashSet<int> source = XEnumerable.RangeEnumerable(0, 3).AsOrToHashSet(3)!;
    IReadOnlyListSegment<int> target = targetType.Ctor(source);

    Assert.HasCount ( 3, target );
    Assert.AreEqual ( 0, target.Offset );
    Assert.IsTrue ( source.SequenceEqual ( target ) );
    Assert.IsTrue ( ReferenceEquals ( comparer, target.EqualityComparer ) );
    Assert.IsTrue ( target.List is int [] );
    Assert.AreEqual ( 3, ((int []) target.List).Length );
  }

  [TestMethod]
  public void IReadOnlyListSegment_NullItemComparer ()
  {
    Action test = () => segment_type.IReadOnlyListSegment<int>(0, default, itemComparer: null!);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );

    Assert.AreEqual ( "Item comparer not provided. (Parameter 'itemComparer')", e.Message );
  }

  [TestMethod]
  public void ArraySegment ()
  {
    AsOrToTargetType<ArraySegment<int>> targetType = segment_type.ArraySegment<int>();

    Assert.HasCount ( 0, targetType.Empty () );
    Assert.AreEqual ( Array.Empty<int> (), targetType.Empty ().Array );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    ArraySegment<int> target = targetType.Ctor(source);

    Assert.IsTrue ( targetType.CanCast ( target ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.HasCount ( 12, target );
    Assert.AreEqual ( 0, target.Offset );
    Assert.IsTrue ( source.SequenceEqual ( target ) );
    Assert.AreEqual ( 12, target.Array!.Length );
  }

  [TestMethod]
  public void ArraySegment_ArrayAlready ()
  {
    AsOrToTargetType<ArraySegment<int>> targetType = segment_type.ArraySegment<int>();
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12).ToArray();

    ArraySegment<int> target = targetType.Ctor(source);

    Assert.HasCount ( 12, target );
    Assert.AreEqual ( 0, target.Offset );
    Assert.IsTrue ( ReferenceEquals ( source, target.Array ) );
  }

  [TestMethod]
  public void Memory ()
  {
    AsOrToTargetType<Memory<int>> targetType = segment_type.Memory<int>();

    Memory<int> empty = targetType.Empty ();
    int[] emptyStore = (int[]) Reflection.GetNonPublicFieldValue ( empty, "_object" );

    Assert.AreEqual ( 0, empty.Length );
    Assert.AreEqual ( Array.Empty<int> (), emptyStore );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    Memory<int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( new int [ 0 ] ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.AreEqual ( 12, target.Length );
    Assert.IsTrue ( source.ToArray ().SequenceEqual ( target.Span ) );
  }

  [TestMethod]
  public void Memory_ArrayAlready ()
  {
    AsOrToTargetType<Memory<int>> targetType = segment_type.Memory<int>();
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12).ToArray();

    Memory<int> target = targetType.Ctor(source);
    int[] store = (int[]) Reflection.GetNonPublicFieldValue ( target, "_object" );

    Assert.AreEqual ( 12, target.Length );
    Assert.IsTrue ( ReferenceEquals ( source, store ) );
  }

  [TestMethod]
  public void ReadOnlyMemory ()
  {
    AsOrToTargetType<ReadOnlyMemory<int>> targetType = segment_type.ReadOnlyMemory<int>();

    ReadOnlyMemory<int> empty = targetType.Empty ();
    int[] emptyStore = (int[]) Reflection.GetNonPublicFieldValue ( empty, "_object" );

    Assert.AreEqual ( 0, empty.Length );
    Assert.AreEqual ( Array.Empty<int> (), emptyStore );

    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12);
    ReadOnlyMemory<int> target = targetType.Ctor(source);

    Assert.IsFalse ( targetType.CanCast ( new int [ 0 ] ) );
    Assert.IsFalse ( targetType.CanCast ( null! ) );

    Assert.AreEqual ( 12, target.Length );
    Assert.IsTrue ( source.ToArray ().SequenceEqual ( target.Span ) );
  }

  [TestMethod]
  public void ReadOnlyMemory_ArrayAlready ()
  {
    AsOrToTargetType<ReadOnlyMemory<int>> targetType = segment_type.ReadOnlyMemory<int>();
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 12).ToArray();

    ReadOnlyMemory<int> target = targetType.Ctor(source);
    int[] store = (int[]) Reflection.GetNonPublicFieldValue ( target, "_object" );

    Assert.AreEqual ( 12, target.Length );
    Assert.IsTrue ( ReferenceEquals ( source, store ) );
  }
}
