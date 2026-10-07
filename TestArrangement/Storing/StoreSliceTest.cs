using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;
using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.Storing;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Storing;

[TestClass]
[SuppressMessage ( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = "" )]
public class StoreSliceTest
{
  [TestMethod]
  public void PublicConstructor_NullStore ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => _ = new StoreSlice<int>(null!, 0, (NonNegativeInt32)0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Store must be provided. (Parameter 'store')", e.Message );
  }

  [TestMethod]
  public void PublicConstructor_NegativeOffset ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => _ = new StoreSlice<int>(new int[0], -1, (NonNegativeInt32)0);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    Assert.AreEqual ( "Offset must be non-negative integer, but it is '-1'.", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void PublicConstructor_InvalidSegment ( int index, int count )
  {
    Action test = () => _ = new StoreSlice<int>(new int[5], index, (NonNegativeInt32)count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available '5', given offset '{0}' and count '{1}' produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 0, 1 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 0, 4 )]
  [DataRow ( 5, 1, 3 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 0, 0 )]
  [DataRow ( 0, 0, 0 )]
  public void PublicConstructor_ValidSegment ( int size, int index, int count )
    => _ = new StoreSlice<int> ( new int [ size ], index, (NonNegativeInt32) count );

  [TestMethod]
  public void PublicConstructor ()
  {
    int[] store = new int [100];
    StoreSlice<int> test = new ( store, 25, (NonNegativeInt32) 33 );

    Assert.IsTrue ( ReferenceEquals ( store, test.store ) );
    Assert.IsTrue ( ReferenceEquals ( store, test.Store ) );
    Assert.IsTrue ( ReferenceEquals ( store, test.SafeStore ) );

    Assert.AreEqual ( 25, test.offset );
    Assert.AreEqual ( 25, test.Offset );
    Assert.AreEqual ( 33, test.count );
    Assert.AreEqual ( 33, test.Count );
  }

  [TestMethod]
  public void InternalConstructor ()
  {
    int[] store = new int [1];
    StoreSlice<int> test = new ( store, int.MinValue, int.MaxValue );

    Assert.IsTrue ( ReferenceEquals ( store, test.store ) );
    Assert.IsTrue ( ReferenceEquals ( store, test.Store ) );
    Assert.IsTrue ( ReferenceEquals ( store, test.SafeStore ) );

    Assert.AreEqual ( int.MinValue, test.offset );
    Assert.AreEqual ( int.MinValue, test.Offset );
    Assert.AreEqual ( int.MaxValue, test.count );
    Assert.AreEqual ( int.MaxValue, test.Count );
  }

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0059:Unnecessary assignment of a value", Justification = "Safety test." )]
  public void Default ()
  {
    StoreSlice<int> test = default;

    Assert.IsNull ( test.store );
    Assert.IsNull ( test.Store );

    Assert.AreEqual ( 0, test.offset );
    Assert.AreEqual ( 0, test.count );

    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), test.SafeStore ) );

    ArraySegment<int> arraySegment = test;
    arraySegment = test.ToArraySegment ();

    IListSegment listSegment = test;
    listSegment = test.ToIListSegment ();

    IListSegment<int> listSegmentOfT = test;
    listSegmentOfT = test.ToIListSegmentOfT ();

    IReadOnlyListSegment<int> readOnlyListSegment = test;
    readOnlyListSegment = test.ToIReadOnlyListSegment ();

    Memory<int> memory = test;
    memory = test.ToMemory ();

    ReadOnlyMemory<int> readOnlyMemory = test;
    readOnlyMemory = test.ToReadOnlyMemory ();

    Span<int> span = test;
    span = test.ToSpan ();

    ReadOnlySpan<int> readOnlySpan = test;
    readOnlySpan = test.ToReadOnlySpan ();
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 2, 4 )]
  public void ArraySegment ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int> slice = new (source, offset, count );

    Func<ArraySegment<int>>[] ctors = [
      () => slice,
      slice.ToArraySegment
    ];

    foreach (Func<ArraySegment<int>> c in ctors)
    {
      ArraySegment<int> test = c();

      Assert.AreEqual ( offset, test.Offset );
      Assert.AreEqual ( count, test.Count );
      Assert.IsTrue ( ReferenceEquals ( source, test.Array ) );
      Assert.IsTrue ( slice.SequenceEqual ( test ) );
    }
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 2, 4 )]
  public void IListSegment ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int> slice = new (source, offset, count );

    Func<IListSegment>[] ctors = [
      () => slice,
      slice.ToIListSegment
    ];

    foreach (Func<IListSegment> c in ctors)
    {
      IListSegment test = c();

      Assert.AreEqual ( offset, test.Offset );
      Assert.AreEqual ( count, test.Count );
      Assert.IsTrue ( ReferenceEquals ( source, test.List ) );
      Assert.IsTrue ( slice.SequenceEqual ( test.Cast<int> () ) );
    }
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 2, 4 )]
  public void IListSegmentOfT ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int> slice = new (source, offset, count );

    Func<IListSegment<int>>[] ctors = [
      () => slice,
      slice.ToIListSegmentOfT
    ];

    foreach (Func<IListSegment<int>> c in ctors)
    {
      IListSegment<int> test = c();

      Assert.AreEqual ( offset, test.Offset );
      Assert.AreEqual ( count, test.Count );
      Assert.IsTrue ( ReferenceEquals ( source, test.List ) );
      Assert.IsTrue ( slice.SequenceEqual ( test ) );
    }
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 2, 4 )]
  public void IReadOnlyListSegment ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int> slice = new (source, offset, count );

    Func<IReadOnlyListSegment<int>>[] ctors = [
      () => slice,
      slice.ToIReadOnlyListSegment
    ];

    foreach (Func<IReadOnlyListSegment<int>> c in ctors)
    {
      IReadOnlyListSegment<int> test = c();

      Assert.AreEqual ( offset, test.Offset );
      Assert.AreEqual ( count, test.Count );
      Assert.IsTrue ( ReferenceEquals ( source, test.List ) );
      Assert.IsTrue ( slice.SequenceEqual ( test ) );
    }
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 2, 4 )]
  public void Memory ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int> slice = new (source, offset, count );
    ArraySegment<int> expectation = new (source, offset, count );

    Func<Memory<int>>[] ctors = [
      () => slice,
      slice.ToMemory
    ];

    foreach (Func<Memory<int>> c in ctors)
    {
      Memory<int> test = c();

      Assert.AreEqual ( count, test.Length );
      Assert.IsTrue ( test.Span.SequenceEqual ( expectation ) );
    }
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 2, 4 )]
  public void ReadOnlyMemory ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int> slice = new (source, offset, count );
    ArraySegment<int> expectation = new (source, offset, count );

    Func<ReadOnlyMemory<int>>[] ctors = [
      () => slice,
      slice.ToReadOnlyMemory
    ];

    foreach (Func<ReadOnlyMemory<int>> c in ctors)
    {
      ReadOnlyMemory<int> test = c();

      Assert.AreEqual ( count, test.Length );
      Assert.IsTrue ( test.Span.SequenceEqual ( expectation ) );
    }
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 2, 4 )]
  public void Span ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int> slice = new (source, offset, count );
    ArraySegment<int> expectation = new (source, offset, count );

    Func<Span<int>>[] ctors = [
      () => slice,
      slice.ToSpan
    ];

    foreach (Func<Span<int>> c in ctors)
    {
      Span<int> test = c();

      Assert.AreEqual ( count, test.Length );
      Assert.IsTrue ( test.SequenceEqual ( expectation ) );
    }
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 0, 7 )]
  [DataRow ( 1, 7 )]
  [DataRow ( 2, 4 )]
  public void ReadOnlySpan ( int offset, int count )
  {
    int[] source = [ 1,2,3,4, 5,6,7,8 ];
    StoreSlice<int> slice = new (source, offset, count );
    ArraySegment<int> expectation = new (source, offset, count );

    Func<ReadOnlySpan<int>>[] ctors = [
      () => slice,
      slice.ToReadOnlySpan
    ];

    foreach (Func<ReadOnlySpan<int>> c in ctors)
    {
      ReadOnlySpan<int> test = c();

      Assert.AreEqual ( count, test.Length );
      Assert.IsTrue ( test.SequenceEqual ( expectation ) );
    }
  }

  [TestMethod]
  public void GetEnumerator ()
  {
    StoreSlice<int> slice = new ([ 1,2,3,4, 5,6,7,8 ], 2, 4 );

    StoreSliceEnumerator<int> e1 = slice.GetEnumerator();
    IEnumerator<int> e2 = ((IEnumerable<int>)slice).GetEnumerator();
    IEnumerator e3 = ((IEnumerable)slice).GetEnumerator();

    Assert.IsTrue ( e1.Equals ( e2 ) );
    Assert.IsTrue ( e1.Equals ( e3 ) );
  }


  // readme
  [TestMethod]
  [SuppressMessage ( "Style", "IDE0059:Unnecessary assignment of a value", Justification = "Readme sample." )]
  public void StoreSlice_Conversion_Sample ()
  {
    StoreSlice<int> slicer = default;

    ReadOnlyMemory<int> myMemory = slicer;
    myMemory = slicer.ToReadOnlyMemory ();
  }
}
