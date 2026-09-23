using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;
using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.Storing;
using Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

using PVI = Software9119.Collection.Superb.TestArrangement.TestAide.PositionValueItem;

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
    Assert.AreEqual ( index, capacitor.storeIndex );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void Constructor_Parameterless ()
  {
    Capacitor<int> capacitor = new ();
    Assert.AreEqual ( Array.Empty<int> (), capacitor.store );
    Assert.AreEqual ( 0, capacitor.storeIndex );
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
    Assert.AreEqual ( 0, capacitor.storeIndex );
    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void Constructor_Enumerable_Enumerable ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 9);
    Capacitor<int> capacitor = new (source);
    Assert.AreEqual ( 16, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.storeIndex );
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
    Assert.AreEqual ( 9, capacitor.storeIndex );
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
    Assert.AreEqual ( 9, capacitor.storeIndex );
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
    Assert.AreEqual ( 9, capacitor.storeIndex );
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
    Assert.AreEqual ( 0, empty.storeIndex );
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
  [DataRow ( 3, 3, 6 )]
  [DataRow ( 3, 4, 4 )]
  [DataRow ( 0, 3, 6 )]
  [DataRow ( 0, 4, 4 )]
  [DataRow ( 1, 3, 6 )]
  [DataRow ( 1, 4, 4 )]
  public void AddOrInsert ( int index, int cap, int expCap )
  {
    int insertion = 9;
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source, cap);

    List<int> expectation = source.ToList();
    expectation.Insert ( index, insertion );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    capacitor.AddOrInsert ( offset, insertion );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 4, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 0, 4 )]
  [DataRow ( 0, 1, 1 )]
  public void AddOrInsert_EmptyCapacitor ( int index, int cap, int expCap )
  {
    int insertion = 9;
    Capacitor<int> capacitor = new ([], cap);

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    capacitor.AddOrInsert ( offset, insertion );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 1, capacitor.Count );
    Assert.AreEqual ( insertion, capacitor [ 0 ] );
  }

  [TestMethod]
  public void AddOrInsert_OffsetEnumerableRoomRequest_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.AddOrInsert ( default, null, 1000 ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void AddOrInsert_OffsetEnumerableRoomRequest_ArrayItems ( int index )
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
  [DataRow ( 0, 0, 2 )]
  [DataRow ( 0, 3, 3 )]
  public void AddOrInsert_OffsetEnumerableRoomRequest_ArrayItems_EmptyCapacitor ( int index, int cap, int expCap )
  {
    int[] insertion = [4,5];
    Capacitor<int> capacitor = new ([], cap);

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void AddOrInsert_OffsetEnumerableRoomRequest_CollectionItems ( int index )
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
  [DataRow ( 0, 0, 2 )]
  [DataRow ( 0, 3, 3 )]
  public void AddOrInsert_OffsetEnumerableRoomRequest_CollectionItems_EmptyCapacitor ( int index, int cap, int expCap )
  {
    XCollection<int> insertion = new([4,5]);
    Capacitor<int> capacitor = new ([], cap);

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void AddOrInsert_OffsetEnumerableRoomRequest_ReadOnlyCollectionItems ( int index )
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
  [DataRow ( 0, 0, 2 )]
  [DataRow ( 0, 3, 3 )]
  public void AddOrInsert_OffsetEnumerableRoomRequest_ReadOnlyCollectionItems_EmptyCapacitor ( int index, int cap, int expCap )
  {
    XReadOnlyCollection<int> insertion = new([4,5]);
    Capacitor<int> capacitor = new ([], cap);

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3, 0, 3, 0, DisplayName = "add,empty items" )]
  [DataRow ( 3, 0, 6, 3, DisplayName = "add,auto-grow" )]
  [DataRow ( 3, 2, 5, 2, DisplayName = "add,room req" )]
  [DataRow ( 3, 2, 10, 3, DisplayName = "add,room req,auto-grow" )]
  [DataRow ( 1, 0, 3, 0, DisplayName = "insert,empty items" )]
  [DataRow ( 1, 0, 6, 3, DisplayName = "insert,auto-grow" )]
  [DataRow ( 1, 0, 5, 2, DisplayName = "insert,exact-grow" )]
  [DataRow ( 1, 2, 5, 2, DisplayName = "insert,room req" )]
  [DataRow ( 1, 2, 10, 7, DisplayName = "insert,room req,auto-grow" )]
  [DataRow ( 1, 2, 12, 9, DisplayName = "insert,room req,auto-grow,exact-grow" )]
  [DataRow ( 0, 0, 3, 0, DisplayName = "insert start,empty items" )]
  [DataRow ( 0, 0, 6, 3, DisplayName = "insert start,auto-grow" )]
  [DataRow ( 0, 0, 5, 2, DisplayName = "insert start,exact-grow" )]
  [DataRow ( 0, 2, 5, 2, DisplayName = "insert start,room req" )]
  [DataRow ( 0, 2, 10, 7, DisplayName = "insert start,room req,auto-grow" )]
  [DataRow ( 0, 2, 13, 10, DisplayName = "insert start,room req,auto-grow,exact-grow" )]
  public void AddOrInsert_OffsetEnumerableRoomRequest_Enumerable ( int index, int roomReq, int cap, int count )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, count);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, roomReq ) );

    Assert.AreEqual ( cap, capacitor.Capacity );
    Assert.AreEqual ( source.Length + count, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 0, 4 )]
  [DataRow ( 0, 4, 4 )]
  [DataRow ( 0, 5, 4 )]
  [DataRow ( 0, 4, 5 )]
  [DataRow ( 3, 0, 4 )]
  [DataRow ( 3, 4, 4 )]
  [DataRow ( 3, 5, 4 )]
  [DataRow ( 3, 4, 5 )]
  public void AddOrInsert_OffsetEnumerableRoomRequest_Enumerable_NoOverCapacitation ( int index, int capacity, int roomRequest )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, 4);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);
    _ = capacitor.CapacitateForNext ( capacity );

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, roomRequest ) );

    int expectedCapacity = Math.Max(capacity,roomRequest) + 3;
    Assert.AreEqual ( expectedCapacity, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 0, 0, 4 )]
  [DataRow ( 0, 3, 3 )]
  [DataRow ( 0, 2, 4 )]
  public void AddOrInsert_OffsetEnumerableRoomRequest_Enumerable_EmptyCapacitor ( int index, int cap, int expCap )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, 3);
    Capacitor<int> capacitor = new ([]);

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion, cap ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
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
  [DataRow ( 0, 0, 2 )]
  [DataRow ( 0, 3, 3 )]
  public void AddOrInsert_OffsetArray_EmptyCapacitor ( int index, int cap, int expCap )
  {
    int[] insertion = [4,5];
    Capacitor<int> capacitor = new ([], cap);

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
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
  [DataRow ( 0, 0, 2 )]
  [DataRow ( 0, 3, 3 )]
  public void AddOrInsert_OffsetCollection_EmptyCapacitor ( int index, int cap, int expCap )
  {
    ICollection<int> insertion = new XCollection<int>([4,5]);
    Capacitor<int> capacitor = new ([], cap);

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
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
  [DataRow ( 0, 0, 2 )]
  [DataRow ( 0, 3, 3 )]
  public void AddOrInsert_OffsetReadOnlyCollection_EmptyCapacitor ( int index, int cap, int expCap )
  {
    IReadOnlyCollection<int> insertion = new XReadOnlyCollection<int>([4,5]);
    Capacitor<int> capacitor = new ([], cap);

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.IsTrue ( capacitor.AddOrInsert ( offset, insertion ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
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
    capacitor.storeIndex = 11;
    capacitor.growFactor = GrowFactor.OneAndHalf;

    Assert.IsTrue ( capacitor.AutoGrow () );
    Assert.AreEqual ( 16, capacitor.store.Length );

    capacitor.storeIndex = 16;
    capacitor.growFactor = GrowFactor.Five;

    Assert.IsTrue ( capacitor.AutoGrow () );
    Assert.AreEqual ( 80, capacitor.store.Length );
  }

  [TestMethod]
  [DataRow ( 0, 5, 5 )]
  [DataRow ( 4, 5, 1 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 6, 5, -1 )]
  public void AvailableCount ( int index, int count, int result )
  {
    int test = Capacitor<int>.AvailableCount(index, count);
    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 2 )]
  public void Capacitate ( int to )
  {
    Capacitor<int> capacitor = new([1,2,3]);
    capacitor.Capacitate ( to );
    Assert.AreEqual ( to, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void CreateAddInsOffset ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3]);
    bool inserting = index != capacitor.Count;

    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);
    Assert.AreEqual ( index, offset.value );
    Assert.AreEqual ( inserting, offset.inserting );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void CloneWithStoreAndCount ( int count )
  {
    Capacitor<int> capacitor = new()
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    int[] store = new int[8];
    Capacitor<int> test = capacitor.CloneWithStoreAndCount(store, count);

    Assert.AreEqual ( count, test.Count );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsTrue ( ReferenceEquals ( store, test.store ) );
  }

  [TestMethod]
  public void ItemsCountToEndInclusive ()
  {
    Capacitor<int> capacitor = new([1,2,3]);

    Assert.AreEqual ( 1, capacitor.ItemsCountToEndInclusive ( 2 ) );
    Assert.AreEqual ( 0, capacitor.ItemsCountToEndInclusive ( 3 ) );
    Assert.AreEqual ( -1, capacitor.ItemsCountToEndInclusive ( 4 ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 2 )]
  public void RemoveAndReturn ( int at )
  {
    object[] source = new int [] { 1,2,3 }.Cast<object>().ToArray();

    List<object> expectation = new (source);
    expectation.RemoveAt ( at );

    Capacitor<object> capacitor = new (source);
    object? removee = capacitor.RemoveAndReturn ( at );

    Assert.AreEqual ( removee, source [ at ] );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 2 )]
  public void RemoveAndReturn_StoreExpulsion ( int at )
  {
    List<int> source = new ([ 1,2,3 ]);
    object[] objectSource = source.Cast<object>().ToArray();
    int[] valueSource = source.ToArray();

    source.RemoveAt ( at );

    Capacitor<object> objectC = new (objectSource);
    Capacitor<int> valueC = new (valueSource);

    _ = objectC.RemoveAndReturn ( at );
    _ = valueC.RemoveAndReturn ( at );

    Assert.AreEqual ( 3, valueC.store [ 2 ] );
    Assert.AreEqual ( null, objectC.store [ 2 ] );
    Assert.IsTrue ( source.SequenceEqual ( valueC ) );
    Assert.IsTrue ( source.SequenceEqual ( objectC.Cast<int> () ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 3 )]
  public void PrepareStoreForAddIns_NoItems ( int index )
  {
    int[] source = [1,2,3];
    Capacitor<int> capacitor = new(source);
    AddOrInsertOffset offset = capacitor.CreateAddInsOffset(index);

    capacitor.PrepareStoreForAddIns ( offset, 0 );
    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 2 )]
  public void PrepareStoreForAddIns_Insertion ( int forCount )
  {
    int[] tail= [5,6,7];
    int [] source = [1,2,3,4,5,6,7];
    Capacitor<int> capacitor = new(source, 8);

    const int index = 4;
    AddOrInsertOffset offset = new (index, true);

    List<int> expectation = new (source);
    for (int c = forCount, ti = 0, ei = index ; c > 0 ; --c, ++ti, ++ei)
      expectation.Insert ( ei, tail [ ti ] );

    capacitor.PrepareStoreForAddIns ( offset, forCount );

    int capacity = source.Length + forCount;
    Assert.AreEqual ( capacity, capacitor.Capacity );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor.store! ) );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 2 )]
  public void PrepareStoreForAddIns_Addition ( int forCount )
  {
    int [] source = [1,2,3,4,5,6,7];
    Capacitor<int> capacitor = new(source, 8);

    AddOrInsertOffset offset = new (7, false);
    capacitor.PrepareStoreForAddIns ( offset, forCount );

    int capacity = source.Length + forCount;
    Assert.AreEqual ( capacity, capacitor.Capacity );
    Assert.IsTrue ( source.Concat ( new int [ forCount ] ).SequenceEqual ( capacitor.store! ) );
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 0, 5 )]
  [DataRow ( 2, 0 )]
  [DataRow ( 2, 2 )]
  [DataRow ( 4, 0 )]
  [DataRow ( 4, 1 )]
  public void ShiftItemsToRight ( int from, int by )
  {
    int [] source = [1,2,3,4,5];
    Capacitor<int> capacitor = new(source);
    _ = capacitor.CapacitateForNext ( by );

    List<int> expectation = new (source);
    for (int c = by, i = from ; c > 0 ; --c, ++i)
      expectation.Insert ( i, source [ i ] );

    capacitor.ShiftItemsToRight ( from, by );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor.store! ) );
  }

  [TestMethod]
  [DataRow ( 0, "1,2,3,4,5,0,1,2,3,4,5" )]
  [DataRow ( 2, "1,2,3,4,5,0,0,0,3,4,5" )]
  [DataRow ( 4, "1,2,3,4,5,0,0,0,0,0,5" )]
  public void ShiftItemsToRight_LargeShift ( int from, string stringExpectation )
  {
    const int shift = 6;
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    _ = capacitor.CapacitateForNext ( shift );

    List<int> expectation = stringExpectation.Split(',').Select(int.Parse).ToList();

    capacitor.ShiftItemsToRight ( from, shift );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor.store! ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 100 )]
  public void SetStoreWithCapacity ( int capacity )
  {
    Capacitor<int> capacitor = new();
    Assert.AreEqual ( 0, capacitor.Capacity );

    capacitor.SetStoreWithCapacity ( capacity );
    Assert.AreEqual ( capacity, capacitor.Capacity );
    Assert.AreEqual ( 0 == capacity, ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void ValidateIndex_Int32_PositiveScenarios ( int index )
  {
    Capacitor<int> capacitor = new([1,2]);

    Assert.IsFalse ( capacitor.ValidateIndex ( index, out IndexOutOfBoundariesException? e, "" ) );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( -1, "" )]
  [DataRow ( -1, null )]
  [DataRow ( -1, "yourParam" )]
  [DataRow ( 2, "" )]
  [DataRow ( 2, null )]
  [DataRow ( 2, "yourParam" )]
  public void ValidateIndex_Int32_NegativeScenarios ( int index, string paramName )
  {
    Capacitor<int> capacitor = new([1,2]);

    string errMsg = index == -1
      ? "Index must be non-negative, but it is '-1'."
      : "For available '2' is index '2' out of bounds.";

    string paramString = paramName == "yourParam" ? " (Parameter 'yourParam')" : "";
    errMsg += paramString;

    Assert.IsTrue ( capacitor.ValidateIndex ( index, out IndexOutOfBoundariesException? e, paramName ) );
    Assert.AreEqual ( errMsg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void ValidateIndex_NonNegativeInt32_PositiveScenarios ( int index )
  {
    Capacitor<int> capacitor = new([1,2]);

    Assert.IsFalse ( capacitor.ValidateIndex ( (NonNegativeInt32) index, out IndexOutOfBoundariesException? e, "" ) );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( "" )]
  [DataRow ( null )]
  [DataRow ( "yourParam" )]
  public void ValidateIndex_NonNegativeInt32_NegativeScenarios ( string paramName )
  {
    Capacitor<int> capacitor = new([1,2]);

    string errMsg = "For available '2' is index '2' out of bounds.";
    string paramString = paramName == "yourParam" ? " (Parameter 'yourParam')" : "";
    errMsg += paramString;

    Assert.IsTrue ( capacitor.ValidateIndex ( (NonNegativeInt32) 2, out IndexOutOfBoundariesException? e, paramName ) );
    Assert.AreEqual ( errMsg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 0, 2 )]
  [DataRow ( 5, 1, 3 )]
  [DataRow ( 5, 4, 6 )]
  [DataRow ( 5, 5, 1 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void ValidateRearSetConfiguration_NegativeScenarios ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Assert.IsTrue ( capacitor.ValidateRearSetConfiguration ( index, count, out ImpSegExc? e, "test and test" ) );

    string msg = "With available {0}, given rearSet {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );
    msg += " (Parameters test and test)";

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 4, 5 )]
  [DataRow ( 5, 0, 1 )]
  [DataRow ( 5, 1, 2 )]
  [DataRow ( 0, 0, 0 )]
  public void ValidateRearSetConfiguration_PositiveScenarios ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Assert.IsFalse ( capacitor.ValidateRearSetConfiguration ( index, count, out ImpSegExc? e, "" ) );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 0, 1 )]
  [DataRow ( 0, 0 )]
  [DataRow ( 4, 1 )]
  [DataRow ( 4, 0 )]
  [DataRow ( 5, 0 )]
  public void ValidateSegmentation_PositiveScenarios ( int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    int result = capacitor.ValidateSegmentation
    (
      (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e
    );

    Assert.AreEqual ( count == 0 ? -1 : 0, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 4, 2 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 10, 0 )]
  [DataRow ( 10, 1 )]
  public void ValidateSegmentation_NegativeScenarios ( int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    int result = capacitor.ValidateSegmentation
    (
      (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e
    );

    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, offset, count );

    Assert.AreEqual ( 1, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DataRow ( null )]
  [DataRow ( false )]
  [DataRow ( true )]
  public void ValidateSegmentation_Parameters ( bool? withParamaters )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    string[]? parameters = withParamaters == null
      ? null
      : withParamaters == true
        ? ["ABC", "xYz"]
        : [];

    _ = capacitor.ValidateSegmentation ( 0, 6, out _, out ImpSegExc? e, parameters );

    string msg = "With available 5, given offset 0 and count 6 produce out-of indexing.{0}";
    string parametersString = withParamaters == true ? " (Parameters 'ABC','xYz')" : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, parametersString );

    Assert.AreEqual ( msg, e?.Message );
  }


  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 0, 1 )]
  [DataRow ( 0, 0 )]
  [DataRow ( 4, 1 )]
  [DataRow ( 4, 0 )]
  [DataRow ( 5, 0 )]
  public void ValidateSegmentation_Static_PositiveScenarios ( int offset, int count )
  {
    int result = Capacitor<int>.ValidateSegmentation
    (
      5, (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e
    );

    Assert.AreEqual ( count == 0 ? -1 : 0, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 4, 2 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 10, 0 )]
  [DataRow ( 10, 1 )]
  public void ValidateSegmentation_Static_NegativeScenarios ( int offset, int count )
  {
    int result = Capacitor<int>.ValidateSegmentation
    (
      5, (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e
    );

    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, offset, count );

    Assert.AreEqual ( 1, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DataRow ( null )]
  [DataRow ( false )]
  [DataRow ( true )]
  public void ValidateSegmentation_Static_Parameters ( bool? withParamaters )
  {
    string[]? parameters = withParamaters == null
      ? null
      : withParamaters == true
        ? ["ABC", "xYz"]
        : [];

    _ = Capacitor<int>.ValidateSegmentation ( 5, 0, 6, out _, out ImpSegExc? e, parameters );

    string msg = "With available 5, given offset 0 and count 6 produce out-of indexing.{0}";
    string parametersString = withParamaters == true ? " (Parameters 'ABC','xYz')" : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, parametersString );

    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  public void Indexer ()
  {
    const int index = 2;
    const int inquest = 10;
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    capacitor [ index ] = inquest;
    Assert.AreEqual ( inquest, capacitor [ index ] );

    int[] expectation = [ 1, 2, 10, 4, 5 ];
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( -1, "Index must be non-negative, but it is '-1'. (Parameter 'index')" )]
  [DataRow ( 5, "For available '5' is index '5' out of bounds. (Parameter 'index')" )]
  public void Indexer_Getter_NegativeScenarios ( int index, string errMsg )
  {
    Action<Capacitor<int>> test = c => _ = c[index];
    Indexer_NegativeScenario ( test, errMsg );
  }

  [TestMethod]
  [DataRow ( -1, "Index must be non-negative, but it is '-1'. (Parameter 'index')" )]
  [DataRow ( 5, "For available '5' is index '5' out of bounds. (Parameter 'index')" )]
  public void Indexer_Setter_NegativeScenarios ( int index, string errMsg )
  {
    Action<Capacitor<int>> test = c => c[index] = default;
    Indexer_NegativeScenario ( test, errMsg );
  }

  static void Indexer_NegativeScenario ( Action<Capacitor<int>> scenario, string errMsg )
  {
    int[] source = [1,2,3,4,5];
    Capacitor<int> capacitor = new(source);
    Action test = () => scenario(capacitor);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Capacity ()
  {
    Capacitor<int> capacitor = new(0);
    Assert.AreEqual ( 0, capacitor.Capacity );

    _ = capacitor.Add ( [ 1, 2, 3, 4, 5 ] );
    Assert.AreEqual ( 5, capacitor.Capacity );

    capacitor.Capacitate ( 8 );
    Assert.AreEqual ( 8, capacitor.Capacity );
  }

  [TestMethod]
  public void Count ()
  {
    Capacitor<int> capacitor = new(0);
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 0, capacitor.storeIndex );

    _ = capacitor.Add ( [ 1, 2, 3, 4, 5 ] );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.AreEqual ( 5, capacitor.storeIndex );

    capacitor.Add ( 6 );
    Assert.AreEqual ( 6, capacitor.Count );
    Assert.AreEqual ( 6, capacitor.storeIndex );
  }

  [TestMethod]
  public void FreeCapacity ()
  {
    Capacitor<int> capacitor = new(0);
    Assert.AreEqual ( 0, capacitor.FreeCapacity );

    _ = capacitor.Add ( [ 1, 2, 3, 4, 5 ] );
    Assert.AreEqual ( 0, capacitor.FreeCapacity );

    capacitor.Capacitate ( 8 );
    Assert.AreEqual ( 3, capacitor.FreeCapacity );
  }

  [TestMethod]
  public void GrowFactorTest ()
  {
    const GrowFactor factor = GrowFactor.Five;

    Capacitor<int> capacitor = new();
    Assert.AreNotEqual ( factor, capacitor.GrowFactor );
    Assert.AreNotEqual ( factor, capacitor.growFactor );

    capacitor.GrowFactor = factor;
    Assert.AreEqual ( factor, capacitor.GrowFactor );
    Assert.AreEqual ( factor, capacitor.growFactor );
  }

  [TestMethod]
  public void GrowFactorTest_LockedFactor ()
  {
    const GrowFactor factor = GrowFactor.Five;

    Capacitor<int> capacitor = new() { LockGrowFactor = true, GrowFactor = factor };
    Assert.AreNotEqual ( factor, capacitor.GrowFactor );

    capacitor.GrowFactor = factor;
    Assert.AreNotEqual ( factor, capacitor.GrowFactor );

    capacitor = new () { GrowFactor = factor, LockGrowFactor = true };
    Assert.AreEqual ( factor, capacitor.GrowFactor );
  }


  [TestMethod]
  public void GrowFactorTest_InvalidFactor ()
  {
    const GrowFactor factor = (GrowFactor)int.MinValue;

    Capacitor<int> capacitor = new();
    Action test = () => capacitor.GrowFactor = factor;

    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    Assert.AreEqual ( "Unsupported grow factor, '-2147483648'. (Parameter 'value')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 0, true )]
  [DataRow ( 4, 0, true )]
  [DataRow ( 4, 5, false )]
  public void IsFull ( int count, int capacity, bool isFull )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0,count);
    Capacitor<int> capacitor = new(source, capacity);

    Assert.AreEqual ( isFull, capacitor.IsFull );
  }

  [TestMethod]
  public void IsReadOnly () => Assert.IsFalse ( new Capacitor<int> ().IsReadOnly );

  [TestMethod]
  public void LockGrowFactor ()
  {
    //capacitor.LockGrowFactor = true; // init-only compiler error
    Assert.IsFalse ( new Capacitor<int> ().LockGrowFactor );
    Assert.IsTrue ( new Capacitor<int> () { LockGrowFactor = true }.LockGrowFactor );
  }

  [TestMethod]
  public void Add ()
  {
    Capacitor<int> capacitor = new([1,2,3]);
    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );

    capacitor.Add ( 4 );
    Assert.AreEqual ( 4, capacitor.Count );
    Assert.AreEqual ( 6, capacitor.Capacity );
  }

  [TestMethod]
  public void Add_EnumerableRoomRequest_NullItems ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsFalse ( capacitor.Add ( null, 0 ) );
  }

  [TestMethod]
  public void Add_EnumerableRoomRequest_Enumerable ()
  {
    IEnumerable<int> addition;
    Capacitor<int> capacitor = new();

    const int count = 5;
    addition = XEnumerable.RangeEnumerable ( 1, count );
    Assert.IsTrue ( capacitor.Add ( addition, 0 ) );
    Assert.AreEqual ( count, capacitor.Count );
    Assert.AreEqual ( 8, capacitor.Capacity );

    addition = XEnumerable.RangeEnumerable ( 6, count );
    Assert.IsTrue ( capacitor.Add ( addition, count + 1 ) );
    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 11, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Add_EnumerableRoomRequest_Array ()
  {
    int[] addition;
    Capacitor<int> capacitor = new();

    const int count = 5;
    addition = XEnumerable.RangeEnumerable ( 1, count ).ToArray ();
    Assert.IsTrue ( capacitor.Add ( addition, 1000 ) );
    Assert.AreEqual ( count, capacitor.Count );
    Assert.AreEqual ( count, capacitor.Capacity );

    addition = XEnumerable.RangeEnumerable ( 6, count ).ToArray ();
    Assert.IsTrue ( capacitor.Add ( addition, 1000 ) );
    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Add_EnumerableRoomRequest_Collection ()
  {
    List<int> addition;
    Capacitor<int> capacitor = new();

    const int count = 5;
    addition = XEnumerable.RangeEnumerable ( 1, count ).ToList ();
    Assert.IsTrue ( capacitor.Add ( addition, 1000 ) );
    Assert.AreEqual ( count, capacitor.Count );
    Assert.AreEqual ( count, capacitor.Capacity );

    addition = XEnumerable.RangeEnumerable ( 6, count ).ToList ();
    Assert.IsTrue ( capacitor.Add ( addition, 1000 ) );
    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Add_EnumerableRoomRequest_ReadOnlyCollection ()
  {
    XReadOnlyCollection<int> addition;
    Capacitor<int> capacitor = new();

    const int count = 5;
    addition = new ( XEnumerable.RangeEnumerable ( 1, count ).ToList () );
    Assert.IsTrue ( capacitor.Add ( addition, 1000 ) );
    Assert.AreEqual ( count, capacitor.Count );
    Assert.AreEqual ( count, capacitor.Capacity );

    addition = new ( XEnumerable.RangeEnumerable ( 6, count ).ToList () );
    Assert.IsTrue ( capacitor.Add ( addition, 1000 ) );
    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Add_Array ()
  {
    int[]? addition;
    Capacitor<int> capacitor = new();

    const int count = 5;
    addition = XEnumerable.RangeEnumerable ( 1, count ).ToArray ();
    Assert.IsTrue ( capacitor.Add ( addition ) );
    Assert.AreEqual ( count, capacitor.Count );
    Assert.AreEqual ( count, capacitor.Capacity );

    addition = XEnumerable.RangeEnumerable ( 6, count ).ToArray ();
    Assert.IsTrue ( capacitor.Add ( addition ) );
    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );

    addition = null;
    Assert.IsFalse ( capacitor.Add ( addition ) );
  }

  [TestMethod]
  public void Add_Collection ()
  {
    List<int>? addition;
    Capacitor<int> capacitor = new();

    const int count = 5;
    addition = XEnumerable.RangeEnumerable ( 1, count ).ToList ();
    Assert.IsTrue ( capacitor.Add ( (ICollection<int>) addition ) );
    Assert.AreEqual ( count, capacitor.Count );
    Assert.AreEqual ( count, capacitor.Capacity );

    addition = XEnumerable.RangeEnumerable ( 6, count ).ToList ();
    Assert.IsTrue ( capacitor.Add ( (ICollection<int>) addition ) );
    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );

    addition = null;
    Assert.IsFalse ( capacitor.Add ( (ICollection<int>?) addition ) );
  }

  [TestMethod]
  public void Add_ReadOnlyCollection ()
  {
    XReadOnlyCollection<int>? addition;
    Capacitor<int> capacitor = new();

    const int count = 5;
    addition = new ( XEnumerable.RangeEnumerable ( 1, count ).ToList () );
    Assert.IsTrue ( capacitor.Add ( addition ) );
    Assert.AreEqual ( count, capacitor.Count );
    Assert.AreEqual ( count, capacitor.Capacity );

    addition = new ( XEnumerable.RangeEnumerable ( 6, count ).ToList () );
    Assert.IsTrue ( capacitor.Add ( addition ) );
    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );

    addition = null;
    Assert.IsFalse ( capacitor.Add ( addition ) );
  }

  [TestMethod]
  public void AllMatches ()
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> all = i => i < 10;
    Predicate<int> none = i => i < 0;
    Predicate<int> some = i => i != 3;
    Predicate<int> start = i => i != 1;
    Predicate<int> end = i => i != 5;

    Assert.IsTrue ( capacitor.AllMatches ( all ) );
    Assert.IsFalse ( capacitor.AllMatches ( none ) );
    Assert.IsFalse ( capacitor.AllMatches ( some ) );
    Assert.IsFalse ( capacitor.AllMatches ( start ) );
    Assert.IsFalse ( capacitor.AllMatches ( end ) );
  }

  [TestMethod]
  public void AllMatches_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.AllMatches ( null! );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 5 )]
  [DataRow ( 6 )]
  [DataRow ( 10 )]
  public void BinarySearch_EvenCount ( int value )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5,6,7,8,9,10]);
    int test = capacitor.BinarySearch(value);

    Assert.AreEqual ( value - 1, test );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 5 )]
  [DataRow ( 6 )]
  [DataRow ( 7 )]
  [DataRow ( 11 )]
  public void BinarySearch_OddCount ( int value )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5,6,7,8,9,10,11]);
    int test = capacitor.BinarySearch(value);

    Assert.AreEqual ( value - 1, test );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 5 )]
  [DataRow ( 6 )]
  [DataRow ( 10 )]
  public void BinarySearch_Comparer_EvenCount ( int value )
  {
    RevertedComparer comparer = new (1, 10);
    Capacitor<int> capacitor = new([1,2,3,4,5,6,7,8,9,10]);
    int test = capacitor.BinarySearch(value, comparer);

    Assert.AreNotEqual ( -1, test );
    int antiValue = RevertedComparer.Revert(value, comparer.reversor);
    Assert.AreEqual ( antiValue - 1, test );

    test = capacitor.BinarySearch ( value, null );
    Assert.AreEqual ( value - 1, test );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 5 )]
  [DataRow ( 6 )]
  [DataRow ( 7 )]
  [DataRow ( 11 )]
  public void BinarySearch_Comparer_OddCount ( int value )
  {
    RevertedComparer comparer = new (1, 11);
    Capacitor<int> capacitor = new([1,2,3,4,5,6,7,8,9,10,11]);
    int test = capacitor.BinarySearch(value, comparer);

    Assert.AreNotEqual ( -1, test );
    int antiValue = RevertedComparer.Revert(value, comparer.reversor);
    Assert.AreEqual ( antiValue - 1, test );

    test = capacitor.BinarySearch ( value, null );
    Assert.AreEqual ( value - 1, test );
  }

  [TestMethod]
  [DataRow ( 1, 0, 0, 10 )]
  [DataRow ( 1, -1, 1, 9 )]
  [DataRow ( 5, 4, 2, 8 )]
  [DataRow ( 5, -1, 0, 4 )]
  [DataRow ( 5, -1, 5, 5 )]
  [DataRow ( 6, 5, 2, 8 )]
  [DataRow ( 6, -1, 0, 5 )]
  [DataRow ( 6, -1, 6, 4 )]
  [DataRow ( 10, 9, 0, 10 )]
  [DataRow ( 10, -1, 0, 9 )]
  public void BinarySearch_OffsetCountComparer_EvenCount ( int value, int result, int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5,6,7,8,9,10]);
    int test = capacitor.BinarySearch(offset, count, value, null);

    Assert.AreEqual ( result < 0, test < 0 );
    Assert.AreEqual ( result < 0 ? test : result, test );
  }

  [TestMethod]
  [DataRow ( 1, 0, 0, 11 )]
  [DataRow ( 1, -1, 1, 10 )]
  [DataRow ( 5, 4, 1, 10 )]
  [DataRow ( 5, -1, 0, 4 )]
  [DataRow ( 5, -1, 5, 6 )]
  [DataRow ( 6, 5, 1, 10 )]
  [DataRow ( 6, -1, 1, 4 )]
  [DataRow ( 6, -1, 6, 4 )]
  [DataRow ( 7, 6, 2, 8 )]
  [DataRow ( 7, -1, 0, 6 )]
  [DataRow ( 7, -1, 7, 4 )]
  [DataRow ( 11, 10, 0, 11 )]
  [DataRow ( 11, -1, 0, 10 )]
  public void BinarySearch_OffsetCountComparer_OddCount ( int value, int result, int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5,6,7,8,9,10,11]);
    int test = capacitor.BinarySearch(offset, count, value, null);

    Assert.AreEqual ( result < 0, test < 0 );
    Assert.AreEqual ( result < 0 ? test : result, test );
  }

  [TestMethod]
  [DataRow ( 1, 9 )]
  [DataRow ( 10, 0 )]
  public void BinarySearch_OffsetCountComparer_Comparer ( int value, int result )
  {
    RevertedComparer comparer = new (1, 10);
    Capacitor<int> capacitor = new([1,2,3,4,5,6,7,8,9,10]);
    int test = capacitor.BinarySearch(0, 10, value, comparer);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 1, 0, 0 )]
  [DataRow ( 11, 10, 0 )]
  [DataRow ( 11, 11, 0 )]
  public void BinarySearch_OffsetCountComparer_EmptySegment ( int value, int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5,6,7,8,9,10,11]);
    int test = capacitor.BinarySearch(offset, count, value, null);

    Assert.IsLessThan ( 0, test );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 6, 0 )]
  public void BinarySearch_OffsetCountComparer_InvalidSegment ( int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => _ = capacitor.BinarySearch(offset, count, default, default);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = $"With available 5, given offset {offset} and count {count} produce out-of indexing.";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 0, 0, 0 )]
  public void BinarySearch_OffsetCountComparer_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Assert.IsLessThan ( 0, capacitor.BinarySearch ( index, count, 1, default ) );
  }


  [TestMethod]
  [DataRow ( 4, false )]
  [DataRow ( 10, false )]
  [DataRow ( 5, true )]
  [DataRow ( 6, true )]
  [DataRow ( 11, true )]
  public void CapacitateExact ( int to, bool result )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5], 10);
    bool test = capacitor.CapacitateExact(to);
    Assert.AreEqual ( result, test );
    Assert.AreEqual ( result ? to : 10, capacitor.Capacity );
  }


  [TestMethod]
  [DataRow ( 2, false )]
  [DataRow ( 3, false )]
  [DataRow ( 4, true )]
  [DataRow ( 100, true )]
  public void CapacitateForNext ( int forNext, bool result )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5], 8);
    bool test = capacitor.CapacitateForNext(forNext);
    Assert.AreEqual ( result, test );
    Assert.AreEqual ( result ? (5 + forNext) : 8, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 5, false )]
  [DataRow ( 8, true )]
  public void CapacitateToCount ( int capacity, bool result )
  {
    int[] source = [1,2,3,4,5];
    Capacitor<int> capacitor = new(source, capacity);
    Assert.AreEqual ( result, capacitor.CapacitateToCount () );

    Assert.AreEqual ( 5, capacitor.Count );
    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Contains ()
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Assert.IsTrue ( capacitor.Contains ( 1 ) );
    Assert.IsTrue ( capacitor.Contains ( 5 ) );
    Assert.IsFalse ( capacitor.Contains ( -1 ) );
  }

  [TestMethod]
  public void CopyTo_Array ()
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    int[] test = new int[5];
    capacitor.CopyTo ( test );

    Assert.IsTrue ( capacitor.SequenceEqual ( test ) );

    test = new int [ 6 ];
    capacitor.CopyTo ( test );

    Assert.IsTrue ( capacitor.Concat ( new int [ 1 ] ).SequenceEqual ( test ) );
  }

  [TestMethod]
  public void CopyTo_Array_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    int[] test = new int[5];
    capacitor.CopyTo ( test );
    Assert.AreEqual ( 0, test.Sum () );

    capacitor.CopyTo ( new int [ 0 ] );
  }

  [TestMethod]
  public void CopyTo_Array_NullArray ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.CopyTo((int[])null!);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Target array must be provided. (Parameter 'array')", e.Message );
  }

  [TestMethod]
  public void CopyTo_Array_ArrayOfInsufficientLength ()
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.CopyTo(new int[4]);

    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    string msg = "Insufficient target array size, available length 4 cannot accomodate 5 items. (Parameter 'array')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void CopyTo_ArrayArrayIndex ()
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    int[] test = new int[5];
    capacitor.CopyTo ( test, 0 );

    Assert.IsTrue ( capacitor.SequenceEqual ( test ) );

    test = new int [ 6 ];
    capacitor.CopyTo ( test, 1 );

    Assert.IsTrue ( new int [ 1 ].Concat ( capacitor ).SequenceEqual ( test ) );
  }

  [TestMethod]
  public void CopyTo_ArrayArrayIndex_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    int[] test = new int[5];
    capacitor.CopyTo ( test, 0 );
    Assert.AreEqual ( 0, test.Sum () );

    capacitor.CopyTo ( new int [ 0 ], 0 );
  }

  [TestMethod]
  public void CopyTo_ArrayArrayIndex_NullArray ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.CopyTo((int[])null!, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Target array must be provided. (Parameter 'array')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 4, 0 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 5, 5 )]
  public void CopyTo_ArrayArrayIndex_ArrayOfInsufficientLength ( int size, int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.CopyTo(new int[size], index);

    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    string msg = "Insufficient target array size, available length {0} cannot accomodate 5 items. (Parameter 'array')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size - index );
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 0, 5, "1,2,3,4,5" )]
  [DataRow ( 6, 1, 0, 5, "0,1,2,3,4,5" )]
  [DataRow ( 7, 1, 0, 5, "0,1,2,3,4,5,0" )]
  [DataRow ( 7, 2, 1, 3, "0,0,2,3,4,0,0" )]
  public void CopyTo_ArrayArrayIndexFromIndexCount ( int size, int index, int fromIndex, int count, string result )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    int[] test = new int[size];
    capacitor.CopyTo ( test, index, fromIndex, count );
    List<int> expectation = result.Split(',').Select(int.Parse).ToList();

    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void CopyTo_ArrayArrayIndexFromIndexCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    int[] test = new int[5];
    capacitor.CopyTo ( test, 4, 0, 0 );
    Assert.AreEqual ( 0, test.Sum () );

    capacitor.CopyTo ( new int [ 0 ], 0, 0, 0 );
  }

  [TestMethod]
  public void CopyTo_ArrayArrayIndexFromIndexCount_NullArray ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.CopyTo((int[])null!, default, default, default);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Target array must be provided. (Parameter 'array')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 0, 1 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 4, 0, 5 )]
  [DataRow ( 4, 5, 0 )]
  [DataRow ( 4, 1, 4 )]
  public void CopyTo_ArrayArrayIndexFromIndexCount_ArrayOfInsufficientLength ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.CopyTo(new int[size], index, default, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing. {3}";
    const string paramsString = "(Parameters 'array','arrayIndex','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count, paramsString );
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 4, 2 )]
  public void CopyTo_ArrayArrayIndexFromIndexCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.CopyTo(new int[6], 0, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. {2}";
    const string paramsString = "(Parameters 'fromIndex','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count, paramsString );
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 0, 0, 0 )]
  public void CopyTo_ArrayArrayIndexFromIndexCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    capacitor.CopyTo ( new int [ size ], 0, index, count );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void CopyTo_ArrayArrayIndexFromIndexCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    int [] test = new int[5];
    capacitor.CopyTo ( test, index, index, 0 );

    Assert.AreEqual ( 0, test.Sum () );
  }

  [TestMethod]
  public void ConvertAll ()
  {
    Capacitor<int> capacitor = new([1,2,3,4,5])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Converter<int, long> convertor = x => x;
    Capacitor<long> test = capacitor.ConvertAll ( convertor );

    Assert.AreEqual ( capacitor.Count, test.Count );
    Assert.AreEqual ( capacitor.Capacity, test.Capacity );
    Assert.AreEqual ( capacitor.GrowFactor, test.GrowFactor );
    Assert.AreEqual ( capacitor.LockGrowFactor, test.LockGrowFactor );
    Assert.IsTrue ( capacitor.Select ( x => (long) x ).SequenceEqual ( test ) );
  }

  [TestMethod]
  public void ConvertAll_NullConvertor ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.ConvertAll ( (Converter<int, int>) null! );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Converter must be provided. (Parameter 'converter')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Clear ( int count )
  {
    int[] source = [1,2,3,4,5,6,7,8];
    Capacitor<int> capacitor = new(count, source.ToArray());
    capacitor.Clear ();

    Assert.AreEqual ( 8, capacitor.Capacity );
    Assert.AreEqual ( 0, capacitor.Count );

    ArraySegment<int> zero = new (capacitor.store, 0, count);
    Assert.AreEqual ( 0, zero.Sum () );

    ArraySegment<int> expectation = new (source, count, 8-count);
    ArraySegment<int> test = new (capacitor.store, count, 8-count);
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void ClearToCapacity ( int count )
  {
    int[] source = Enumerable.Range(0, count).ToArray();
    Capacitor<int> capacitor = new(2, source);
    capacitor.ClearToCapacity ();

    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( count, capacitor.Capacity );
    Assert.AreEqual ( 0, capacitor.store.Sum () );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Clone ( int count )
  {
    Capacitor<int> capacitor = new(count, [1,2,3,4,5,0,0,0])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Capacitor<int> test = (Capacitor<int>)capacitor.Clone();

    Assert.AreEqual ( count, test.Count );
    Assert.AreEqual ( 8, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsFalse ( ReferenceEquals ( capacitor.store, test.store ) );
    Assert.IsTrue ( capacitor.store.SequenceEqual ( test.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 1, 4 )]
  [DataRow ( 0, 4 )]
  [DataRow ( 1, 3 )]
  public void CloneSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Capacitor<int> test = capacitor.CloneSegment(index, count);
    IListSegment<int> expectation = new(capacitor, index, count);

    Assert.AreEqual ( count, test.Count );
    Assert.AreEqual ( count, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsFalse ( ReferenceEquals ( capacitor.store, test.store ) );
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 1, 0 )]
  [DataRow ( 1, 1 )]
  [DataRow ( 5, 0 )]
  [DataRow ( 5, 4 )]
  [DataRow ( 5, 5 )]
  public void CloneSegment_EmptySegment ( int size, int index )
  {
    Capacitor<int> capacitor = new(new int [size])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Capacitor<int> test = capacitor.CloneSegment(index, 0);

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( 0, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsFalse ( ReferenceEquals ( capacitor.store, test.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 4, 2 )]
  [DataRow ( 7, 7 )]
  public void CloneSegment_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.CloneSegment ( index, count );

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void Equals ()
  {
    Capacitor<int> c1 = new  ();
    Capacitor<int> c2 = new  ();

    Assert.IsTrue ( c1.Equals ( c1 ) );
    Assert.IsTrue ( c1.Equals ( (object) c1 ) );

    Assert.IsFalse ( c1.Equals ( c2 ) );
    Assert.IsFalse ( c1.Equals ( (object) c2 ) );
  }

  [TestMethod]
  [DataRow ( 8, false, 10 )]
  [DataRow ( 8, true, null )]
  [DataRow ( 8, default, 0 )]
  [DataRow ( 5, true, default )]
  public void ExtractStore ( int size, bool trimExcess, int? resetCapacity )
  {
    int[] source = Enumerable.Range(1, size).ToArray();
    Capacitor<int> capacitor = new(5, source.ToArray());
    int [] store = capacitor.store;

    int[] test = resetCapacity is int
      ? capacitor.ExtractStore(trimExcess, resetCapacity.Value)
      : capacitor.ExtractStore( trimExcess );

    ArraySegment<int> expectation = new(source, 0, trimExcess ? 5 : 8);

    Assert.AreEqual ( trimExcess == false || size == 5, ReferenceEquals ( store, test ) );
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
    Assert.IsFalse ( ReferenceEquals ( capacitor.store, test ) );
    Assert.AreEqual ( resetCapacity ?? 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 1, 4 )]
  [DataRow ( 0, 4 )]
  [DataRow ( 1, 3 )]
  public void Fill ( int index, int count )
  {
    int[] source = [1,2,3,4,5];
    Capacitor<int> capacitor = new(source);

    capacitor.Fill ( 9, index, count );

    (int endIndex, int endCount) = GetEndSegment ( index, count );

    ArraySegment<int> e1 = new(source, 0, index);
    ArraySegment<int> e3 = new(source, endIndex, endCount);

    IListSegment<int> t1 = new(capacitor, 0, index);
    IListSegment<int> t2 = new(capacitor, index, count);
    IListSegment<int> t3 = new(capacitor, endIndex, endCount);

    Assert.IsTrue ( e1.SequenceEqual ( t1 ) );
    Assert.IsTrue ( t2.All ( x => x == 9 ) );
    Assert.IsTrue ( e3.SequenceEqual ( t3 ) );

    static (int, int) GetEndSegment ( int index, int count )
    {
      int end = index + count;
      return (end % 5, 5 - end);
    }
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void Fill_EmptySegment ( int index )
  {
    int[] source = [1,2,3,4,5];
    Capacitor<int> capacitor = new(source);

    capacitor.Fill ( default, index, 0 );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 4, 2 )]
  [DataRow ( 7, 7 )]
  public void Fill_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.Fill (default, index, count );

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  [DataRow ( 8 )]
  public void FillToCount ( int count )
  {
    int[] source = [1, 2, 3, 4, 5, 6, 7, 8];
    Capacitor<int> capacitor = new(count, source.ToArray());
    capacitor.FillToCount ( 9 );

    (int endIndex, int endCount) = GetEndSegment ( count );

    ArraySegment<int> expectation = new(source,endIndex, endCount);
    ArraySegment<int> test = new(capacitor.store, endIndex, endCount);

    Assert.AreEqual ( count, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
    Assert.IsTrue ( capacitor.All ( x => x == 9 ) );

    static (int, int) GetEndSegment ( int count ) => (count % 8, 8 - count);
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  [DataRow ( 8 )]
  public void FillToCapacity ( int count )
  {
    Capacitor<int> capacitor = new(count, [1, 2, 3, 4, 5, 6, 7, 8]);
    capacitor.FillToCapacity ( 9 );

    Assert.AreEqual ( 8, capacitor.Count );
    Assert.IsTrue ( capacitor.store.All ( x => x == 9 ) );
  }

  [TestMethod]
  [DataRow ( 0, 0, "1,3,4,7,8" )]
  [DataRow ( 1, 1, "0,2,3,6" )]
  [DataRow ( 1, 2, "0,2,3" )]
  public void FindAllIndexes ( int offsetStart, int offsetEnd, string result )
  {
    int[] baseSource = [1, 2, 1, 2, 2, 1, 1, 2, 2,   2, 1, 2, 2 ];
    int length = baseSource.Length - offsetStart;
    int count = length - 4 - offsetEnd;

    ArraySegment<int> source = new (baseSource, offsetStart, length );
    Capacitor<int> capacitor = new(count, source.ToArray());

    Predicate<int> predicate = x => x == 2;
    IEnumerable<int> test = capacitor.FindAllIndexes(predicate);

    IEnumerable<int> indexes = result.Split(',').Select(int.Parse);
    Assert.IsTrue ( indexes.SequenceEqual ( test ) );

    predicate = x => x == 3;
    Assert.HasCount ( 0, capacitor.FindAllIndexes ( predicate ) );
  }

  [TestMethod]
  public void FindAllIndexes_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    IEnumerable<int> test = capacitor.FindAllIndexes(predicate);
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void FindAllIndexes_OneElements ()
  {
    Capacitor<int> c1 = new([1]);
    Capacitor<int> c2 = new([2]);

    Predicate<int> predicate = x => x == 2;
    IEnumerable<int> test = c1.FindAllIndexes(predicate);
    Assert.HasCount ( 0, test );

    test = c2.FindAllIndexes ( predicate );
    Assert.HasCount ( 1, test );
    Assert.AreEqual ( 0, test.First () );
  }

  [TestMethod]
  public void FindAllIndexes_NoIndexingBehindArray ()
  {
    Capacitor<int> capacitor = new(5, [2, 2, 2, 2, 2]);

    Predicate<int> predicate = x => x == 2;
    _ = capacitor.FindAllIndexes ( predicate );
  }

  [TestMethod]
  public void FindAllIndexes_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindAllIndexes(predicate).First();

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 0, "2,4,6,8,10" )]
  [DataRow ( 1, 1, "2,4,6,8" )]
  [DataRow ( 2, 2, "4,6" )]
  public void FindAllItems ( int offsetStart, int offsetEnd, string result )
  {
    int[] baseSource = [1, 2, 1, 4, 6, 1, 1, 8, 10,   2, 1, 2, 2 ];
    int length = baseSource.Length - offsetStart;
    int count = length - 4 - offsetEnd;

    ArraySegment<int> source = new (baseSource, offsetStart, length );
    Capacitor<int> capacitor = new(count, source.ToArray());

    Predicate<int> predicate = x => (x & 1) == 0;
    IEnumerable<int> test = capacitor.FindAllItems(predicate);

    IEnumerable<int> indexes = result.Split(',').Select(int.Parse);
    Assert.IsTrue ( indexes.SequenceEqual ( test ) );

    predicate = x => x == 3;
    Assert.HasCount ( 0, capacitor.FindAllIndexes ( predicate ) );
  }

  [TestMethod]
  public void FindAllItems_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    IEnumerable<int> test = capacitor.FindAllItems(predicate);
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void FindAllItems_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindAllItems(predicate).First();

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, true )]
  [DataRow ( 2, true )]
  [DataRow ( 6, true )]
  [DataRow ( 8, false )]
  [DataRow ( 0, false )]
  public void FindFirstIndex ( int value, bool finds )
  {
    Capacitor<int> capacitor = new(6, [1,2,3,1,2,6,8,9]);

    Predicate<int> predicate = x => x == value;
    int test = capacitor.FindFirstIndex(predicate);

    Assert.AreEqual ( finds ? value - 1 : -1, test );
  }

  [TestMethod]
  public void FindFirstIndex_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    int test = capacitor.FindFirstIndex(predicate);
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  public void FindFirstIndex_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindFirstIndex(predicate);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, 0 )]
  [DataRow ( 1, 1, 3 )]
  [DataRow ( 2, 1, 1 )]
  [DataRow ( 2, 2, 4 )]
  [DataRow ( 3, 2, 2 )]
  [DataRow ( 3, 3, 5 )]
  [DataRow ( 8, 0, -1 )]
  [DataRow ( 0, 0, -1 )]
  public void FindFirstIndex_Offset ( int value, int offset, int result )
  {
    Capacitor<int> capacitor = new(6, [1,2,3,1,2,3,8,9]);

    Predicate<int> predicate = x => x == value;
    int test = capacitor.FindFirstIndex(predicate, offset);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  public void FindFirstIndex_Offset_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindFirstIndex(predicate, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindFirstIndex_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindFirstIndex(predicate, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, 6, 0 )]
  [DataRow ( 1, 1, 3, 3 )]
  [DataRow ( 1, 3, 3, 3 )]
  [DataRow ( 3, 0, 6, 2 )]
  [DataRow ( 3, 3, 3, 5 )]
  [DataRow ( 8, 0, 6, -1 )]
  [DataRow ( 0, 0, 6, -1 )]
  public void FindFirstIndex_OffsetCount ( int value, int offset, int count, int result )
  {
    Capacitor<int> capacitor = new(6, [1,2,3,1,2,3,8,9]);

    Predicate<int> predicate = x => x == value;
    int test = capacitor.FindFirstIndex(predicate, offset, count);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindFirstIndex_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    int test = capacitor.FindFirstIndex ( predicate, index, 0 );
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  public void FindFirstIndex_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    int test = capacitor.FindFirstIndex(predicate, 0, 0);
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 4, 2 )]
  [DataRow ( 7, 7 )]
  public void FindFirstIndex_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindFirstIndex(predicate, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 0, 0, 0 )]
  public void FindFirstIndex_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.AreEqual ( -1, capacitor.FindFirstIndex ( predicate, index, count ) );
  }

  [TestMethod]
  public void FindFirstIndex_OffsetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindFirstIndex(predicate, 0, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0 )]
  [DataRow ( 4, 5 )]
  [DataRow ( 6, -1 )]
  [DataRow ( 0, -1 )]
  public void FindFirstItem ( int value, int position )
  {
    int [] rawSource = [1,2,3,1,2,4,6,7];
    PVI[] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(6, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = position != -1;
    Assert.AreEqual ( finds, capacitor.FindFirstItem ( predicate, out PVI test ) );
    Assert.AreEqual ( position, test.Position );
  }

  [TestMethod]
  public void FindFirstItem_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindFirstItem ( predicate, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  public void FindFirstItem_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindFirstItem(predicate, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, 0 )]
  [DataRow ( 1, 1, 3 )]
  [DataRow ( 2, 1, 1 )]
  [DataRow ( 2, 2, 4 )]
  [DataRow ( 3, 2, 2 )]
  [DataRow ( 3, 3, 5 )]
  [DataRow ( 6, 0, -1 )]
  [DataRow ( 0, 0, -1 )]
  public void FindFirstItem_Offset ( int value, int offset, int position )
  {
    int [] rawSource = [1,2,3,1,2,3,6,7];
    PVI[] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(6, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = position != -1;
    Assert.AreEqual ( finds, capacitor.FindFirstItem ( predicate, offset, out PVI test ) );
    Assert.AreEqual ( position, test.Position );
  }

  [TestMethod]
  public void FindFirstItem_Offset_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindFirstItem(predicate, 0, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindFirstItem_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindFirstItem(predicate, size, out _);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, 6, 0 )]
  [DataRow ( 1, 1, 3, 3 )]
  [DataRow ( 1, 3, 3, 3 )]
  [DataRow ( 3, 0, 6, 2 )]
  [DataRow ( 3, 3, 3, 5 )]
  [DataRow ( 6, 0, 6, -1 )]
  [DataRow ( 0, 0, 6, -1 )]
  public void FindFirstItem_OffsetCount ( int value, int offset, int count, int position )
  {
    int [] rawSource = [1,2,3,1,2,3,6,7];
    PVI[] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(6, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = position != -1;
    Assert.AreEqual ( finds, capacitor.FindFirstItem ( predicate, offset, count, out PVI test ) );
    Assert.AreEqual ( position, test.Position );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindFirstItem_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindFirstItem ( predicate, index, 0, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  public void FindFirstItem_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindFirstItem ( predicate, 0, 0, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 4, 2 )]
  [DataRow ( 7, 7 )]
  public void FindFirstItem_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindFirstItem(predicate, index, count, out _);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 0, 0, 0 )]
  public void FindFirstItem_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.IsFalse ( capacitor.FindFirstItem ( predicate, index, count, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  [DataRow ( 3, 5 )]
  [DataRow ( 4, 0 )]
  [DataRow ( 6, -1 )]
  [DataRow ( 8, -1 )]
  public void FindLastIndex ( int value, int index )
  {
    Capacitor<int> capacitor = new(6, [4,2,3,1,2,3,6,7]);
    Predicate<int> predicate = x => x == value;

    Assert.AreEqual ( index, capacitor.FindLastIndex ( predicate ) );
  }

  [TestMethod]
  public void FindLastIndex_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    int test = capacitor.FindLastIndex(predicate);
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  public void FindLastIndex_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastIndex(predicate);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 2, 0, 4 )]
  [DataRow ( 1, 4, -1 )]
  [DataRow ( 3, 5, 5 )]
  [DataRow ( 6, 0, -1 )]
  [DataRow ( 0, 0, -1 )]
  public void FindLastIndex_Offset ( int value, int offset, int index )
  {
    Capacitor<int> capacitor = new(6, [4,2,3,1,2,3,6,7]);
    Predicate<int> predicate = x => x == value;

    Assert.AreEqual ( index, capacitor.FindLastIndex ( predicate, offset ) );
  }

  [TestMethod]
  public void FindLastIndex_Offset_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastIndex(predicate, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindLastIndex_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindLastIndex(predicate, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 2, 5, 4 )]
  [DataRow ( 1, 2, -1 )]
  [DataRow ( 3, 5, 5 )]
  [DataRow ( 6, 5, -1 )]
  [DataRow ( 0, 5, -1 )]
  public void FindLastIndex_RearSet ( int value, int rearSet, int index )
  {
    Capacitor<int> capacitor = new(6, [4,2,3,1,2,3,6,7]);
    Predicate<int> predicate = x => x == value;

    Assert.AreEqual ( index, capacitor.FindLastIndex ( rearSet, predicate ) );
  }

  [TestMethod]
  public void FindLastIndex_RearSet_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastIndex(0, predicate);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindLastIndex_RearSet_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindLastIndex(size, predicate);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'rearSet')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 8, 7 )]
  [DataRow ( 5, 0, 7, -1 )]
  [DataRow ( 1, 0, 8, 3 )]
  [DataRow ( 1, 0, 3, 0 )]
  [DataRow ( 2, 1, 4, 4 )]
  [DataRow ( 2, 1, 3, 1 )]
  [DataRow ( 8, 0, 8, -1 )]
  [DataRow ( 0, 0, 8, -1 )]
  public void FindLastIndex_OffsetCount ( int value, int offset, int count, int result )
  {
    Capacitor<int> capacitor = new(8, [1,2,3,1,2,3,4,5,7,8]);

    Predicate<int> predicate = x => x == value;
    int test = capacitor.FindLastIndex(predicate, offset, count);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindLastIndex_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    int test = capacitor.FindLastIndex ( predicate, index, 0 );
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  public void FindLastIndex_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    int test = capacitor.FindLastIndex(predicate, 0, 0);
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 4, 2 )]
  [DataRow ( 7, 7 )]
  public void FindLastIndex_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new(new int[5]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindLastIndex(predicate, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 0, 0, 0 )]
  public void FindLastIndex_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.AreEqual ( -1, capacitor.FindLastIndex ( predicate, index, count ) );
  }

  [TestMethod]
  public void FindLastIndex_OffsetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastIndex(predicate, 0, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 7, 8, 7 )]
  [DataRow ( 5, 6, 7, -1 )]
  [DataRow ( 1, 7, 8, 3 )]
  [DataRow ( 1, 2, 3, 0 )]
  [DataRow ( 2, 4, 5, 4 )]
  [DataRow ( 2, 1, 2, 1 )]
  [DataRow ( 8, 7, 8, -1 )]
  [DataRow ( 0, 7, 8, -1 )]
  public void FindLastIndex_RearSetCount ( int value, int rearSet, int count, int result )
  {
    Capacitor<int> capacitor = new(8, [1,2,3,1,2,3,4,5,7,8]);

    Predicate<int> predicate = x => x == value;
    int test = capacitor.FindLastIndex(rearSet, count, predicate);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindLastIndex_RearSetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    int test = capacitor.FindLastIndex ( index, 0, predicate );
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  public void FindLastIndex_RearSetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    int test = capacitor.FindLastIndex(0, 0, predicate);
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 0, 2 )]
  [DataRow ( 5, 1, 3 )]
  [DataRow ( 5, 4, 6 )]
  [DataRow ( 5, 5, 1 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void FindLastIndex_RearSetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindLastIndex(index, count, predicate);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given rearSet {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );
    msg += " (Parameters 'rearSet','count')";

    Assert.AreEqual ( msg, e.Message );
  }


  [TestMethod]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 4, 5 )]
  [DataRow ( 5, 0, 1 )]
  [DataRow ( 5, 1, 2 )]
  [DataRow ( 0, 0, 0 )]
  public void FindLastIndex_RearSetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.AreEqual ( -1, capacitor.FindLastIndex ( index, count, predicate ) );
  }


  [TestMethod]
  public void FindLastIndex_RearSetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastIndex(0, 0, predicate);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 3, 5 )]
  [DataRow ( 4, 0 )]
  [DataRow ( 6, -1 )]
  [DataRow ( 8, -1 )]
  public void FindLastItem ( int value, int index )
  {
    int[] rawSource = [4,2,3,1,2,3,6,7];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(6, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindLastItem ( predicate, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  public void FindLastItem_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindLastItem ( predicate, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  public void FindLastItem_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastItem(predicate, out int test);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 2, 0, 4 )]
  [DataRow ( 1, 4, -1 )]
  [DataRow ( 4, 0, 0 )]
  [DataRow ( 3, 5, 5 )]
  [DataRow ( 6, 0, -1 )]
  [DataRow ( 0, 0, -1 )]
  public void FindLastItem_Offset ( int value, int offset, int index )
  {
    int[] rawSource = [4,2,3,1,2,3,6,7];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(6, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindLastItem ( predicate, offset, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  public void FindLastItem_Offset_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastItem(predicate, 0, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindLastItem_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindLastItem(predicate, size, out _);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 2, 5, 4 )]
  [DataRow ( 1, 2, -1 )]
  [DataRow ( 3, 5, 5 )]
  [DataRow ( 4, 5, 0 )]
  [DataRow ( 6, 5, -1 )]
  [DataRow ( 0, 5, -1 )]
  public void FindLastItem_RearSet ( int value, int rearSet, int index )
  {
    int[] rawSource = [4,2,3,1,2,3,6,7];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(6, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindLastItem ( rearSet, predicate, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  public void FindLastItem_RearSet_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastItem(0, predicate, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindLastItem_RearSet_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindLastItem(size, predicate, out _);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'rearSet')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 8, 7 )]
  [DataRow ( 5, 0, 7, -1 )]
  [DataRow ( 1, 0, 8, 3 )]
  [DataRow ( 1, 0, 3, 0 )]
  [DataRow ( 2, 1, 4, 4 )]
  [DataRow ( 2, 1, 3, 1 )]
  [DataRow ( 8, 0, 8, -1 )]
  [DataRow ( 0, 0, 8, -1 )]
  public void FindLastItem_OffsetCount ( int value, int offset, int count, int index )
  {
    int[] rawSource = [1,2,3,1,2,3,4,5,7,8];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(8, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindLastItem ( predicate, offset, count, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindLastItem_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindLastItem ( predicate, index, 0, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  public void FindLastItem_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindLastItem ( predicate, 0, 0, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 1, 5 )]
  [DataRow ( 5, 4, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void FindLastItem_OffsetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindLastItem(predicate, index, count, out _);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 0, 0, 0 )]
  public void FindLastItem_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.IsFalse ( capacitor.FindLastItem ( predicate, index, count, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  public void FindLastItem_OffsetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastItem(predicate, 0, 0, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 7, 8, 7 )]
  [DataRow ( 5, 6, 7, -1 )]
  [DataRow ( 1, 7, 8, 3 )]
  [DataRow ( 1, 2, 3, 0 )]
  [DataRow ( 2, 4, 5, 4 )]
  [DataRow ( 2, 1, 2, 1 )]
  [DataRow ( 8, 7, 8, -1 )]
  [DataRow ( 0, 7, 8, -1 )]
  public void FindLastItem_RearSetCount ( int value, int rearSet, int count, int index )
  {
    int[] rawSource = [1,2,3,1,2,3,4,5,7,8];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(8, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindLastItem ( rearSet, count, predicate, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindLastItem_RearSetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindLastItem ( index, 0, predicate, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  public void FindLastItem_RearSetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindLastItem ( 0, 0, predicate, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 0, 2 )]
  [DataRow ( 5, 1, 3 )]
  [DataRow ( 5, 4, 6 )]
  [DataRow ( 5, 5, 1 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void FindLastItem_RearSetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindLastItem(index, count, predicate, out _);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given rearSet {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );
    msg += " (Parameters 'rearSet','count')";

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 4, 5 )]
  [DataRow ( 5, 0, 1 )]
  [DataRow ( 5, 1, 2 )]
  [DataRow ( 0, 0, 0 )]
  public void FindLastItem_RearSetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.IsFalse ( capacitor.FindLastItem ( index, count, predicate, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  public void FindLastItem_RearSetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindLastItem(0, 0, predicate, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 1, 8 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 3, 2 )]
  [DataRow ( 1, 5, 0 )]
  [DataRow ( 1, 6, -1 )]
  [DataRow ( 9, 1, -1 )]
  public void FindMthIndex ( int value, int mth, int index )
  {
    Capacitor<int> capacitor = new(9, [1,1,1, 3,2,1, 3,2,1, 9]);

    Predicate<int> predicate = x => x == value;
    int test = capacitor.FindMthIndex(predicate, mth);

    Assert.AreEqual ( index, test );
  }

  [TestMethod]
  public void FindMthIndex_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    int test = capacitor.FindMthIndex(predicate, 1);

    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  public void FindMthIndex_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindMthIndex(predicate, 1);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1, 1, 0 )]
  [DataRow ( 1, 1, 1, 1 )]
  [DataRow ( 6, 3, 1, 6 )]
  [DataRow ( 6, 3, 2, 3 )]
  [DataRow ( 8, 1, 5, 0 )]
  [DataRow ( 8, 1, 1, 8 )]
  [DataRow ( 8, 1, 6, -1 )]
  [DataRow ( 8, 0, 1, -1 )]
  public void FindMthIndex_RearSet ( int rearSet, int value, int mth, int result )
  {
    Capacitor<int> capacitor = new(9, [1,1,1, 3,2,1, 3,2,1, 9]);

    Predicate<int> predicate = x => x == value;
    int test = capacitor.FindMthIndex(predicate, mth, rearSet);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  public void FindMthIndex_RearSet_NullPredicate ()
  {
    Capacitor<int> capacitor = new(new int[1]);
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindMthIndex(predicate, 1, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindMthIndex_RearSet_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindMthIndex(predicate, 1, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'rearSet')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 7, 8, 1, 0 )]
  [DataRow ( 5, 7, 7, 1, -1 )]
  [DataRow ( 1, 7, 8, 1, 7 )]
  [DataRow ( 1, 6, 7, 1, 4 )]
  [DataRow ( 2, 6, 4, 2, 3 )]
  [DataRow ( 2, 6, 4, 1, 6 )]
  [DataRow ( 3, 5, 6, 2, 2 )]
  [DataRow ( 7, 7, 8, 1, -1 )]
  [DataRow ( 0, 7, 8, 1, -1 )]
  public void FindMthIndex_RearSetCount ( int value, int rearSet, int count, int mth, int index )
  {
    Capacitor<int> capacitor = new(8, [5,4, 3,2,1, 3,2,1, 7,8]);
    Predicate<int> predicate = x => x == value;

    Assert.AreEqual ( index, capacitor.FindMthIndex ( predicate, mth, rearSet, count ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindMthIndex_RearSetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    Assert.AreEqual ( -1, capacitor.FindMthIndex ( predicate, 1, index, 0 ) );
  }

  [TestMethod]
  public void FindMthIndex_RearSetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.AreEqual ( -1, capacitor.FindMthIndex ( predicate, 1, 0, 0 ) );
  }

  [TestMethod]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 4, 6 )]
  [DataRow ( 5, 0, 2 )]  
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void FindMthIndex_RearSetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindMthIndex(predicate, 1, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given rearSet {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );
    msg += " (Parameters 'rearSet','count')";

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 0, 1 )]
  [DataRow ( 5, 1, 2 )]
  [DataRow ( 5, 4, 5 )]
  [DataRow ( 0, 0, 0 )]
  public void FindMthIndex_RearSetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.AreEqual ( -1, capacitor.FindMthIndex ( predicate, 1, index, count ) );
  }

  [TestMethod]
  public void FindMthIndex_RearSetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindMthIndex(predicate, 1, 0, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 1, 8 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 3, 2 )]
  [DataRow ( 1, 5, 0 )]
  [DataRow ( 1, 6, -1 )]
  [DataRow ( 9, 1, -1 )]
  public void FindMthItem ( int value, int mth, int index )
  {
    int[] rawSource = [1,1,1, 3,2,1, 3,2,1, 9];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(9, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindMthItem ( predicate, mth, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  public void FindMthItem_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindMthItem ( predicate, 1, out _ ) );
  }

  [TestMethod]
  public void FindMthItem_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindMthItem(predicate, 1, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1, 1, 0 )]
  [DataRow ( 1, 1, 1, 1 )]
  [DataRow ( 6, 3, 1, 6 )]
  [DataRow ( 6, 3, 2, 3 )]
  [DataRow ( 8, 1, 5, 0 )]
  [DataRow ( 8, 1, 1, 8 )]
  [DataRow ( 8, 1, 6, -1 )]
  [DataRow ( 8, 0, 1, -1 )]
  public void FindMthItem_RearSet ( int rearSet, int value, int mth, int index )
  {
    int [] rawSource = [1,1,1, 3,2,1, 3,2,1, 9];

    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(9, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindMthItem ( predicate, mth, rearSet, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  public void FindMthItem_RearSet_NullPredicate ()
  {
    Capacitor<int> capacitor = new(new int[1]);
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindMthItem(predicate, 1, 0, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindMthItem_RearSet_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindMthItem(predicate, 1, size, out _);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'rearSet')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 7, 8, 1, 0 )]
  [DataRow ( 5, 7, 7, 1, -1 )]
  [DataRow ( 5, 7, 8, 2, -1 )]
  [DataRow ( 1, 7, 8, 1, 7 )]
  [DataRow ( 1, 6, 7, 1, 4 )]
  [DataRow ( 2, 6, 4, 2, 3 )]
  [DataRow ( 2, 6, 4, 1, 6 )]
  [DataRow ( 3, 5, 6, 2, 2 )]
  [DataRow ( 7, 7, 8, 1, -1 )]
  [DataRow ( 0, 7, 8, 1, -1 )]
  public void FindMthItem_RearSetCount ( int value, int rearSet, int count, int mth, int index )
  {
    int[] rawSource = [5,4, 3,2,1, 3,2,1, 7,8];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(8, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindMthItem ( predicate, mth, rearSet, count, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindMthItem_RearSetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindMthItem ( predicate, 1, index, 0, out _ ) );
  }

  [TestMethod]
  public void FindMthItem_RearSetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindMthItem ( predicate, 1, 0, 0, out _ ) );
  }

  [TestMethod]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 4, 6 )]
  [DataRow ( 5, 0, 2 )]  
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void FindMthItem_RearSetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindMthItem(predicate, 1, index, count, out _);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given rearSet {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );
    msg += " (Parameters 'rearSet','count')";

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 0, 1 )]
  [DataRow ( 5, 1, 2 )]
  [DataRow ( 5, 4, 5 )]
  [DataRow ( 0, 0, 0 )]
  public void FindMthItem_RearSetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.IsFalse(capacitor.FindMthItem ( predicate, 1, index, count, out _ ) );
  }

  [TestMethod]
  public void FindMthItem_RearSetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindMthItem(predicate, 1, 0, 0, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 1, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 3, 6 )]
  [DataRow ( 1, 5, 8 )]
  [DataRow ( 1, 6, -1 )]
  [DataRow ( 0, 1, -1 )]
  public void FindNthIndex ( int value, int nth, int index )
  {
    Capacitor<int> capacitor = new(9, [1,2,3, 1,2,3, 1,1,1,1]);

    Predicate<int> predicate = x => x == value;
    int test = capacitor.FindNthIndex(predicate, nth);

    Assert.AreEqual ( index, test );
  }

  [TestMethod]
  public void FindNthIndex_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    int test = capacitor.FindNthIndex(predicate, 1);

    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  public void FindNthIndex_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindNthIndex(predicate, 1);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1, 1, 0 )]
  [DataRow ( 1, 1, 1, 3 )]
  [DataRow ( 0, 1, 5, 8 )]
  [DataRow ( 8, 1, 1, 8 )]
  [DataRow ( 0, 1, 6, -1 )]
  [DataRow ( 0, 0, 1, -1 )]
  public void FindNthIndex_Offset ( int offset, int value, int nth, int result )
  {
    Capacitor<int> capacitor = new(9, [1,2,3, 1,2,3, 1,1,1,1]);

    Predicate<int> predicate = x => x == value;
    int test = capacitor.FindNthIndex(predicate, nth, offset);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  public void FindNthIndex_Offset_NullPredicate ()
  {
    Capacitor<int> capacitor = new(new int[1]);
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindNthIndex(predicate, 1, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindNthIndex_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindNthIndex(predicate, 1, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 8, 1, 7 )]
  [DataRow ( 5, 0, 7, 1, -1 )]
  [DataRow ( 1, 0, 8, 1, 0 )]
  [DataRow ( 1, 1, 7, 1, 3 )]
  [DataRow ( 2, 1, 4, 2, 4 )]
  [DataRow ( 2, 1, 4, 1, 1 )]
  [DataRow ( 2, 1, 1, 1, 1 )]
  [DataRow ( 3, 1, 6, 2, 5 )]
  [DataRow ( 3, 1, 6, 1, 2 )]
  [DataRow ( 8, 0, 8, 1, -1 )]
  [DataRow ( 0, 0, 8, 1, -1 )]
  public void FindNthIndex_OffsetCount ( int value, int offset, int count, int nth, int index )
  {
    Capacitor<int> capacitor = new(8, [1,2,3, 1,2,3, 4,5, 7,8]);
    Predicate<int> predicate = x => x == value;

    Assert.AreEqual ( index, capacitor.FindNthIndex ( predicate, nth, offset, count ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindNthIndex_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    Assert.AreEqual ( -1, capacitor.FindNthIndex ( predicate, 1, index, 0 ) );
  }

  [TestMethod]
  public void FindNthIndex_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.AreEqual ( -1, capacitor.FindNthIndex ( predicate, 1, 0, 0 ) );
  }

  [TestMethod]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 1, 5 )]
  [DataRow ( 5, 4, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void FindNthIndex_OffsetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindNthIndex(predicate, 1, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 0, 0, 0 )]
  public void FindNthIndex_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.AreEqual ( -1, capacitor.FindNthIndex ( predicate, 1, index, count ) );
  }

  [TestMethod]
  public void FindNthIndex_OffsetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindNthIndex(predicate, 1, 0, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 1, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 3, 6 )]
  [DataRow ( 1, 5, 8 )]
  [DataRow ( 1, 6, -1 )]
  [DataRow ( 0, 1, -1 )]
  public void FindNthItem ( int value, int nth, int index )
  {
    int[] rawSource = [1,2,3, 1,2,3, 1,1,1, 1];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(9, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindNthItem ( predicate, nth, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  public void FindNthItem_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = x => true;

    Assert.IsFalse ( capacitor.FindNthItem ( predicate, 1, out int test ) );
    Assert.AreEqual ( 0, test );
  }

  [TestMethod]
  public void FindNthItem_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindNthItem(predicate, 1, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1, 1, 0 )]
  [DataRow ( 1, 1, 1, 3 )]
  [DataRow ( 0, 1, 5, 8 )]
  [DataRow ( 8, 1, 1, 8 )]
  [DataRow ( 0, 1, 6, -1 )]
  [DataRow ( 0, 0, 1, -1 )]
  public void FindNthItem_Offset ( int offset, int value, int nth, int index )
  {
    int[] rawSource = [1,2,3, 1,2,3, 1,1,1, 1];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(9, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindNthItem ( predicate, nth, offset, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  public void FindNthItem_Offset_NullPredicate ()
  {
    Capacitor<int> capacitor = new(new int[1]);
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindNthItem(predicate, 1, 0, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindNthItem_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindNthItem(predicate, 1, size, out _);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 8, 1, 7 )]
  [DataRow ( 5, 0, 7, 1, -1 )]
  [DataRow ( 1, 0, 8, 1, 0 )]
  [DataRow ( 1, 1, 7, 1, 3 )]
  [DataRow ( 2, 1, 4, 2, 4 )]
  [DataRow ( 2, 1, 4, 1, 1 )]
  [DataRow ( 2, 1, 1, 1, 1 )]
  [DataRow ( 3, 1, 6, 2, 5 )]
  [DataRow ( 3, 1, 6, 1, 2 )]
  [DataRow ( 8, 0, 8, 1, -1 )]
  [DataRow ( 0, 0, 8, 1, -1 )]
  public void FindNthItem_OffsetCount ( int value, int offset, int count, int nth, int index )
  {
    int[] rawSource =[1,2,3, 1,2,3, 4,5, 7,8];
    PVI [] source = rawSource.Select((x,i) => new PVI(x,i)).ToArray();

    Capacitor<PVI> capacitor = new(8, source);
    Predicate<PVI> predicate = x => x.Value == value;

    bool finds = index != -1;
    Assert.AreEqual ( finds, capacitor.FindNthItem ( predicate, nth, offset, count, out PVI test ) );
    Assert.AreEqual ( index, test.Position );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindNthItem_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindNthItem ( predicate, 1, index, 0, out _ ) );
  }

  [TestMethod]
  public void FindNthItem_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindNthItem ( predicate, 1, 0, 0, out _ ) );
  }

  [TestMethod]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 1, 5 )]
  [DataRow ( 5, 4, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void FindNthItem_OffsetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindNthItem(predicate, 1, index, count, out _);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 0, 0, 0 )]
  public void FindNthItem_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.IsFalse ( capacitor.FindNthItem ( predicate, 1, index, count, out _ ) );
  }

  [TestMethod]
  public void FindNthItem_OffsetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindNthItem(predicate, 1, 0, 0, out _);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  // readme

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0058:Expression value is never used", Justification = "Readme style." )]
  [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Readme style" )]
  public void Capacitor_NoVersioning_Sample ()
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
