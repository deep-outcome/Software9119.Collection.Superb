using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Numerics;
using Software9119.Collection.Superb.Storing;
using Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Software9119.Collection.Superb.TestArrangement.Storing;

[TestClass]
[SuppressMessage ( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = @"¯\_x_x_/¯" )]
public class CapacitorTest
{

  [TestMethod]
  public void NullMatchPredicate ()
  {
    ArgumentNullException e = Capacitor.NullMatchPredicate("XXX");
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'XXX')", e.Message );
  }

  [TestMethod]
  public void NullTargetArray ()
  {
    ArgumentNullException e = Capacitor.NullTargetArray("XXX");
    Assert.AreEqual ( "Target array must be provided. (Parameter 'XXX')", e.Message );
  }

  [TestMethod]
  public void NullComparisonDelegate ()
  {
    ArgumentNullException e = Capacitor.NullComparisonDelegate("XXX");
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'XXX')", e.Message );
  }

  [TestMethod]
  public void InsufficientTargetArray ()
  {
    ArgumentOutOfRangeException e = Capacitor.InsufficientTargetArray("XXX", 2, 3);
    const string msg
      = "Insufficient target array size, available length 2 cannot accomodate 3 items. (Parameter 'XXX')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( null, 10, 0 )]
  [DataRow ( 0, 10, 0 )]
  [DataRow ( 10, 0, 0 )]
  [DataRow ( 10, 8, 8 )]
  [DataRow ( 10, 10, 10 )]
  [DataRow ( 10, 11, 10 )]
  public void Constructor_Protected ( int? size, int count, int index )
  {
    int []? store = size is int ? new int[size.Value] : null;
    Capacitor<int> capacitor = new (count, store);

    Assert.IsTrue ( ReferenceEquals ( store ?? capacitor.store, capacitor.store ) );
    Assert.AreEqual ( index, capacitor.index );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void Constructor_Parameterless ()
  {
    Capacitor<int> capacitor = new ();
    Assert.AreEqual ( Array.Empty<int> (), capacitor.store );
    Assert.AreEqual ( 0, capacitor.index );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_Capacity ( int capacity )
  {
    Capacitor<int> capacitor = new (capacity);
    Assert.AreEqual ( capacity, capacitor.store.Length );
    Assert.AreEqual ( 0, capacitor.index );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void Constructor_Enumerable_Enumerable ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 9);
    Capacitor<int> capacitor = new (source);
    Assert.AreEqual ( 16, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.index );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );

    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Constructor_Enumerable_NotEnumerable ()
  {
    IEnumerable<int> source = Enumerable.Range(0, 9);
    Capacitor<int> capacitor = new (source);
    Assert.AreEqual ( 9, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.index );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );

    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Constructor_CapacityAndEnumerable_Enumerable ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 9);
    Capacitor<int> capacitor = new (source, 1000);
    Assert.AreEqual ( 1000, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.index );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );

    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Constructor_CapacityAndEnumerable_NotEnumerable ()
  {
    IList<int> source = (IList<int>)Enumerable.Range(0, 9);
    Capacitor<int> capacitor = new (source, 1000);
    Assert.AreEqual ( 1000, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.index );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );

    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Empty ()
  {
    Capacitor<int> empty = Capacitor<int>.Empty;
    Assert.IsFalse ( ReferenceEquals ( Capacitor<int>.Empty, empty ) );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), empty.store ) );
    Assert.AreEqual ( 0, empty.index );
    Assert.AreEqual ( GrowFactor.Two, empty.growFactor );
    Assert.IsFalse ( empty.LockGrowFactor );
  }

  [TestMethod]
  [SuppressMessage ( "Design", "MSTEST0032:Assertion condition is always true", Justification = "Intentional." )]
  public void Defaults ()
  {
    Capacitor<int> capacitor = [];

    Assert.AreEqual ( 4, Capacitor<int>.defaultCapacity );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );

    Assert.IsFalse ( capacitor.itemShouldDefault );
    Assert.IsTrue ( new Capacitor<object> ().itemShouldDefault );
    Assert.IsTrue ( new Capacitor<NoRefList> ().itemShouldDefault );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void AddOrInsert_OffsetEnumerableExtraCapacity_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.AddOrInsert ( default, null, 1000 ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void AddOrInsert_OffsetEnumerableExtraCapacity_ArrayItems ( int index )
  {
    int[] insertion = [4,5];
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void AddOrInsert_OffsetEnumerableExtraCapacity_CollectionItems ( int index )
  {
    XCollection<int> insertion = new([4,5]);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void AddOrInsert_OffsetEnumerableExtraCapacity_ReadOnlyCollectionItems ( int index )
  {
    XReadOnlyCollection<int> insertion = new([4,5]);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3, 0, 3, 0, DisplayName = "add,empty items" )]
  [DataRow ( 3, 0, 6, 3, DisplayName = "add,auto-grow" )]
  [DataRow ( 3, 2, 5, 2, DisplayName = "add,extra cap" )]
  [DataRow ( 3, 2, 10, 3, DisplayName = "add,extra cap,auto-grow" )]
  [DataRow ( 1, 0, 3, 0, DisplayName = "insert,empty items" )]
  [DataRow ( 1, 0, 6, 3, DisplayName = "insert,auto-grow" )]
  [DataRow ( 1, 0, 5, 2, DisplayName = "insert,exact-grow" )]
  [DataRow ( 1, 2, 5, 2, DisplayName = "insert,extra cap" )]
  [DataRow ( 1, 2, 10, 7, DisplayName = "insert,extra cap,auto-grow" )]
  [DataRow ( 1, 2, 12, 9, DisplayName = "insert,extra cap,auto-grow,exact-grow" )]
  [DataRow ( 0, 0, 3, 0, DisplayName = "insert start,empty items" )]
  [DataRow ( 0, 0, 6, 3, DisplayName = "insert start,auto-grow" )]
  [DataRow ( 0, 0, 5, 2, DisplayName = "insert start,exact-grow" )]
  [DataRow ( 0, 2, 5, 2, DisplayName = "insert start,extra cap" )]
  [DataRow ( 0, 2, 10, 7, DisplayName = "insert start,extra cap,auto-grow" )]
  [DataRow ( 0, 2, 13, 10, DisplayName = "insert start,extra cap,auto-grow,exact-grow" )]
  public void AddOrInsert_OffsetEnumerableExtraCapacity_Enumerable ( int index, int extraCap, int cap, int count )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, count);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, extraCap ) );

    Assert.AreEqual ( cap, capacitor.Capacity );
    Assert.AreEqual ( source.Length + count, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void AddOrInsert_OffsetArray_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.AddOrInsert ( default, (int []?) null ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, default )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, default )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, default )]
  public void AddOrInsert_OffsetArray ( int index, int count, int capacity )
  {
    int[] insertion = Enumerable.Range(4, count).ToArray();
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source, capacity);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion ) );

    int capCount = source.Length + count;
    Assert.AreEqual ( capCount, capacitor.Capacity );
    Assert.AreEqual ( capCount, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void AddOrInsert_OffsetCollection_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.AddOrInsert ( default, (ICollection<int>?) null ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, default )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, default )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, default )]
  public void AddOrInsert_OffsetCollection ( int index, int count, int capacity )
  {
    ICollection<int> insertion = new XCollection<int>(Enumerable.Range(4, count).ToList());
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source, capacity);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion ) );

    int capCount = source.Length + count;
    Assert.AreEqual ( capCount, capacitor.Capacity );
    Assert.AreEqual ( capCount, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void AddOrInsert_OffsetReadOnlyCollection_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.AddOrInsert ( default, (IReadOnlyCollection<int>?) null ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, default )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, default )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, default )]
  public void AddOrInsert_OffsetReadOnlyCollection_Array ( int index, int count, int capacity )
  {
    int[] data = Enumerable.Range(4, count).ToArray();
    AddOrInsert_OffsetReadOnlyCollection ( index, capacity, data );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, default )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, default )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, default )]
  public void AddOrInsert_OffsetReadOnlyCollection_Collection ( int index, int count, int capacity )
  {
    List<int> data = Enumerable.Range(4, count).ToList();
    AddOrInsert_OffsetReadOnlyCollection ( index, capacity, data );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, default )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, default )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, default )]
  public void AddOrInsert_OffsetReadOnlyCollection_ReadOnlyCollection ( int index, int count, int capacity )
  {
    List<int> data = Enumerable.Range(4, count).ToList();
    IReadOnlyCollection<int>  roCollection = new XReadOnlyCollection<int> ( data );
    AddOrInsert_OffsetReadOnlyCollection ( index, capacity, roCollection );
  }

  static void AddOrInsert_OffsetReadOnlyCollection ( int index, int capacity, IReadOnlyCollection<int> insertion )
  {
    int[] source = [1,2,3];
    Capacitor<int> capacitor = new (source, capacity);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion ) );

    int capCount = source.Length + insertion.Count;
    Assert.AreEqual ( capCount, capacitor.Capacity );
    Assert.AreEqual ( capCount, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );

  }

  [TestMethod]
  public void AutoGrow_Empty ()
  {
    Capacitor<int> capacitor = [];
    Assert.AreEqual ( 0, capacitor.store.Length );

    Assert.IsTrue ( capacitor.AutoGrow () );
    Assert.AreEqual ( Capacitor<int>.defaultCapacity, capacitor.store.Length );
  }

  [TestMethod]
  public void AutoGrow_NotFull ()
  {
    Capacitor<int> capacitor = new (1);
    Assert.IsFalse ( capacitor.AutoGrow () );
    Assert.AreEqual ( 1, capacitor.store.Length );
  }

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0017:Simplify object initialization", Justification = "" )]
  public void AutoGrow_Growing ()
  {
    Capacitor<int> capacitor = new ( 11 );
    capacitor.index = 11;
    capacitor.growFactor = GrowFactor.OneAndHalf;

    Assert.IsTrue ( capacitor.AutoGrow () );
    Assert.AreEqual ( 16, capacitor.store.Length );

    capacitor.index = 16;
    capacitor.growFactor = GrowFactor.Five;

    Assert.IsTrue ( capacitor.AutoGrow () );
    Assert.AreEqual ( 80, capacitor.store.Length );
  }

  [TestMethod]
  public void ItemsCountToEndInclusive ()
  {
    Capacitor<int> capacitor = [1,2,3];
    Assert.AreEqual ( 1, capacitor.ItemsCountToEndInclusive ( 2 ) );
    Assert.AreEqual ( 0, capacitor.ItemsCountToEndInclusive ( 3 ) );
    Assert.AreEqual ( -1, capacitor.ItemsCountToEndInclusive ( 4 ) );
  }

  // readme

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0058:Expression value is never used", Justification = "Readme style." )]
  [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Readme style" )]
  public void Capacitor_NoVersion_Sample ()
  {
    StringBuilder builder = new();
    Capacitor<int> capacitor = [ 1, 2, 3, 4, 5 ];

    foreach (int item in capacitor)
    {
      capacitor.Add ( item );
      capacitor.Reverse ();
      builder.Append ( $"{item}, " );
    }

    Assert.AreEqual ( "1, 5, 2, 4, 3", GetString () );

    string GetString ()
    {
      builder.Length -= 2;
      return builder.ToString ();
    }
  }

  [TestMethod]
  public void Capacitor_GrowFactor_Sample ()
  {
    Capacitor<int> capacitor;

    capacitor = new () { GrowFactor = GrowFactor.OneAndHalf, LockGrowFactor = true, };
    Assert.AreEqual ( GrowFactor.OneAndHalf, capacitor.GrowFactor );

    capacitor = new () { LockGrowFactor = true, GrowFactor = GrowFactor.Five, };
    Assert.AreEqual ( GrowFactor.Two, capacitor.GrowFactor );
  }
}
