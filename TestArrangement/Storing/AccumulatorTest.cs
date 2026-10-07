using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Storing;
using Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Storing;

[TestClass]
[SuppressMessage ( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = @"¯\_x_x_/¯" )]
public class AccumulatorTest
{
  [TestMethod]
  public void Constructor_Parameterless ()
  {
    Accumulator<int> accumulator = new ();

    Assert.AreEqual ( Array.Empty<int> (), accumulator.store );
    Assert.AreEqual ( 0, accumulator.storeIndex );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_Capacity ( int capacity )
  {
    Accumulator<int> accumulator = new (capacity);
    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );

    Assert.AreEqual ( capacity, accumulator.store.Length );
    Assert.AreEqual ( 0, accumulator.storeIndex );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  public void Constructor_Enumerable_NullItems ()
  {
    Accumulator<int> accumulator = new ((IEnumerable<int>?)null);

    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );
    Assert.AreEqual ( 0, accumulator.storeIndex );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  public void Constructor_Enumerable_Enumerable ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 9);
    Accumulator<int> accumulator = new (source);

    Assert.AreEqual ( 16, accumulator.store.Length );
    Assert.AreEqual ( 9, accumulator.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( accumulator ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  public void Constructor_Enumerable_EmptyEnumerable ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 0);
    Accumulator<int> accumulator = new (source);

    Assert.AreEqual ( 0, accumulator.storeIndex );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  public void Constructor_Enumerable_Array ()
  {
    int[] source = Enumerable.Range(0, 9).ToArray();
    Accumulator<int> accumulator = new (source);

    Assert.AreEqual ( 16, accumulator.store.Length );
    Assert.AreEqual ( 9, accumulator.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( accumulator ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  public void Constructor_Enumerable_EmptyArray ()
  {
    int[] source = new int[0];
    Accumulator<int> accumulator = new (source);

    Assert.AreEqual ( 0, accumulator.storeIndex );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );
  }

  [TestMethod]
  public void Constructor_Enumerable_Collection ()
  {
    XCollection<int> source = new(Enumerable.Range(0, 9).ToList());
    Accumulator<int> accumulator = new (source);

    Assert.AreEqual ( 16, accumulator.store.Length );
    Assert.AreEqual ( 9, accumulator.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( accumulator ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  public void Constructor_Enumerable_EmptyCollection ()
  {
    XCollection<int> source = new ([]);
    Accumulator<int> accumulator = new (source);

    Assert.AreEqual ( 0, accumulator.storeIndex );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );
  }

  [TestMethod]
  public void Constructor_Enumerable_ReadOnlyCollection ()
  {
    XReadOnlyCollection<int> source = new(Enumerable.Range(0, 9).ToList());
    Accumulator<int> accumulator = new (source);

    Assert.AreEqual ( 16, accumulator.store.Length );
    Assert.AreEqual ( 9, accumulator.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( accumulator ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  public void Constructor_Enumerable_EmptyReadOnlyCollection ()
  {
    XReadOnlyCollection<int> source = new ([]);
    Accumulator<int> accumulator = new (source);

    Assert.AreEqual ( 0, accumulator.storeIndex );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_NullItems ( int capacity )
  {
    Accumulator<int> accumulator = new (null, capacity);

    Assert.AreEqual ( capacity, accumulator.store.Length );
    Assert.AreEqual ( 0, accumulator.storeIndex );
    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  [DataRow ( 0, 16 )]
  [DataRow ( 1000, 1000 )]
  public void Constructor_CapacityAndEnumerable_Enumerable ( int capacity, int expCapacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 9);
    Accumulator<int> accumulator = new (source, capacity);

    Assert.AreEqual ( expCapacity, accumulator.store.Length );
    Assert.AreEqual ( 9, accumulator.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( accumulator ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_EmptyEnumerable ( int capacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 0);
    Accumulator<int> accumulator = new (source, capacity);

    Assert.AreEqual ( capacity, accumulator.store.Length );
    Assert.AreEqual ( 0, accumulator.storeIndex );

    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 16 )]
  [DataRow ( 1000, 1000 )]
  public void Constructor_CapacityAndEnumerable_Array ( int capacity, int expCapacity )
  {
    int[] source = XEnumerable.RangeEnumerable(0, 9).ToArray();
    Accumulator<int> accumulator = new (source, capacity);

    Assert.AreEqual ( expCapacity, accumulator.store.Length );
    Assert.AreEqual ( 9, accumulator.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( accumulator ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_EmptyArray ( int capacity )
  {
    int[] source = new int[0];
    Accumulator<int> accumulator = new (source, capacity);

    Assert.AreEqual ( capacity, accumulator.store.Length );
    Assert.AreEqual ( 0, accumulator.storeIndex );

    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 16 )]
  [DataRow ( 1000, 1000 )]
  public void Constructor_CapacityAndEnumerable_Collection ( int capacity, int expCapacity )
  {
    XCollection<int> source = new (XEnumerable.RangeEnumerable(0, 9).ToList());
    Accumulator<int> accumulator = new (source, capacity);

    Assert.AreEqual ( expCapacity, accumulator.store.Length );
    Assert.AreEqual ( 9, accumulator.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( accumulator ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_EmptyCollection ( int capacity )
  {
    XCollection<int> source = new ([]);
    Accumulator<int> accumulator = new (source, capacity);

    Assert.AreEqual ( capacity, accumulator.store.Length );
    Assert.AreEqual ( 0, accumulator.storeIndex );

    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 16 )]
  [DataRow ( 1000, 1000 )]
  public void Constructor_CapacityAndEnumerable_ReadOnlyCollection ( int capacity, int expCapacity )
  {
    XReadOnlyCollection<int> source = new (XEnumerable.RangeEnumerable(0, 9).ToList());
    Accumulator<int> accumulator = new (source, capacity);

    Assert.AreEqual ( expCapacity, accumulator.store.Length );
    Assert.AreEqual ( 9, accumulator.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( accumulator ) );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_EmptyReadOnlyCollection ( int capacity )
  {
    XReadOnlyCollection<int> source = new([]);
    Accumulator<int> accumulator = new (source, capacity);

    Assert.AreEqual ( capacity, accumulator.store.Length );
    Assert.AreEqual ( 0, accumulator.storeIndex );

    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), accumulator.store ) );
  }

  [TestMethod]
  [DataRow ( null, 0 )]
  [DataRow ( 0, 0 )]
  [DataRow ( 10, 0 )]
  [DataRow ( 10, 8 )]
  [DataRow ( 10, 10 )]
  public void Constructor_Starter ( int? size, int count )
  {
    int []? store = size is int ? new int[size.Value] : null;
    Accumulator<int> accumulator = new (count, store);

    Assert.IsTrue ( ReferenceEquals ( store ?? Array.Empty<int> (), accumulator.store ) );
    Assert.AreEqual ( count, accumulator.storeIndex );

    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );
    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  [DataRow ( null, 1 )]
  [DataRow ( null, 2 )]
  [DataRow ( 0, 1 )]
  [DataRow ( 0, 2 )]
  [DataRow ( 1, 2 )]
  [DataRow ( 1, 3 )]
  public void Constructor_Starter_InvalidCount ( int? size, int count )
  {
    int []? store = size is int s ? new int[s] : null;
    Action test = () => _ = new Accumulator<int> ( count, store);

    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    string errMsg = $"Count ({count}) must be less than or equal to store length ({size ?? 0}). (Parameter 'count')";
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Design", "MSTEST0032:Assertion condition is always true", Justification = "Intentional." )]
  public void Defaults ()
  {
    Accumulator<int> accumulator = [];

    Assert.AreEqual ( 4, Accumulator<int>.defaultCapacity );
    Assert.AreEqual ( GrowFactor.Two, accumulator.growFactor );
    Assert.AreEqual ( CapacitationPolicy.GeometricJump, accumulator.capacitationPolicy );

    Assert.IsFalse ( accumulator.itemShouldDefault );
    Assert.IsTrue ( new Accumulator<object> ().itemShouldDefault );
    Assert.IsTrue ( new Accumulator<NoRefList> ().itemShouldDefault );

    Assert.IsFalse ( accumulator.LockGrowFactor );
    Assert.IsFalse ( accumulator.LockCapacitationPolicy );
  }

  [TestMethod]
  public void CapacitationPolicyTest ()
  {
    const CapacitationPolicy policy = CapacitationPolicy.StaticJump;

    Accumulator<int> accumulator = new();
    Assert.AreNotEqual ( policy, accumulator.CapacitationPolicy );
    Assert.AreNotEqual ( policy, accumulator.capacitationPolicy );

    accumulator.CapacitationPolicy = policy;
    Assert.AreEqual ( policy, accumulator.CapacitationPolicy );
    Assert.AreEqual ( policy, accumulator.capacitationPolicy );

    foreach (CapacitationPolicy one in Enum.GetValues ( typeof ( CapacitationPolicy ) ))
      accumulator.CapacitationPolicy = one;
  }

  [TestMethod]
  public void CapacitationPolicyTest_LockedPolicy ()
  {
    const CapacitationPolicy policy = CapacitationPolicy.StaticJump;

    Action test;
    Accumulator<int> accumulator;
    InvalidOperationException e;

    test = () => _ = new Accumulator<int> () { LockCapacitationPolicy = true, CapacitationPolicy = policy };
    e = Assert.ThrowsExactly<InvalidOperationException> ( test );
    Assert.AreEqual ( "Capacitation policy is locked.", e.Message );

    accumulator = new () { LockCapacitationPolicy = true };
    test = () => accumulator.CapacitationPolicy = policy;
    e = Assert.ThrowsExactly<InvalidOperationException> ( test );
    Assert.AreEqual ( "Capacitation policy is locked.", e.Message );

    accumulator = new () { CapacitationPolicy = policy, LockCapacitationPolicy = true };
    Assert.AreEqual ( policy, accumulator.CapacitationPolicy );
  }

  [TestMethod]
  [DataRow ( int.MinValue )]
  [DataRow ( -1 )]
  [DataRow ( 3 )]
  [DataRow ( int.MaxValue )]
  public void CapacitationPolicyTest_InvalidPolicy ( int value )
  {
    CapacitationPolicy policy = (CapacitationPolicy)value;

    Accumulator<int> accumulator = new();
    Action test = () => accumulator.CapacitationPolicy = policy;

    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    Assert.AreEqual ( $"Unsupported capacitation policy, '{value}'. (Parameter 'value')", e.Message );
  }

  [TestMethod]
  public void Store ()
  {
    int[] store = new int[3];
    Accumulator<int> accumulator = new ( 0, store );
    Assert.IsTrue ( ReferenceEquals ( store, accumulator.Store ) );
    Assert.IsTrue ( ReferenceEquals ( accumulator.store, accumulator.Store ) );
  }

  [TestMethod]
  public void Slice_NegativeOffset ()
  {
    Accumulator<int> accumulator = new (6, [1,2,3,4,5,6, 7,8]);
    Action test = () => _ = accumulator.Slice(-1, 0);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    Assert.AreEqual ( "Offset must be non-negative integer, but it is '-1'. (Parameter 'offset')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void Slice_InvalidSegment ( int index, int count )
  {
    Accumulator<int> accumulator = new (new int[5], 10);
    Action test = () => _ = accumulator.Slice(index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available '5', given offset '{0}' and count '{1}' produce out-of indexing. {2}";
    const string paramsString = "(Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count, paramsString );

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
  public void Slice_ValidSegment ( int size, int index, int count )
  {
    int [] source = Enumerable.Range(0, size).ToArray();

    Accumulator<int> accumulator = new (source, 10);

    StoreSlice<int> test = accumulator.Slice ( index, count );
    ArraySegment<int> expectation = new (source, index, count);

    Assert.AreEqual ( index, test.Offset );
    Assert.AreEqual ( count, test.Count );
    Assert.IsTrue ( ReferenceEquals ( accumulator.Store, test.Store ) );

    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }
}
