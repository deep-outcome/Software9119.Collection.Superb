using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;
using Software9119.Collection.Superb.Storing;
using Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
  public void Remove ( int at )
  {
    object[] source = new int [] { 1,2,3 }.Cast<object>().ToArray();

    List<object> expectation = new (source);
    expectation.RemoveAt ( at );

    Capacitor<object> capacitor = new (source);
    object? removee = capacitor.Remove ( at );

    Assert.AreEqual ( removee, source [ at ] );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 2 )]
  public void Remove_StoreExpulsion ( int at )
  {
    List<int> source = new ([ 1,2,3 ]);
    object[] objectSource = source.Cast<object>().ToArray();
    int[] valueSource = source.ToArray();

    source.RemoveAt ( at );

    Capacitor<object> objectC = new (objectSource);
    Capacitor<int> valueC = new (valueSource);

    _ = objectC.Remove ( at );
    _ = valueC.Remove ( at );

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

    Assert.IsFalse ( capacitor.ValidateIndex ( index, out IndexOutOfBoundariesException? e ) );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( -1, "Index must be non-negative, but it is '-1'." )]
  [DataRow ( 2, "For available '2' is index '2' out of bounds." )]
  public void ValidateIndex_Int32_NegativeScenarios ( int index, string errMsg )
  {
    Capacitor<int> capacitor = new([1,2]);

    Assert.IsTrue ( capacitor.ValidateIndex ( index, out IndexOutOfBoundariesException? e ) );
    Assert.AreEqual ( errMsg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void ValidateIndex_NonNegativeInt32_PositiveScenarios ( int index )
  {
    Capacitor<int> capacitor = new([1,2]);

    Assert.IsFalse ( capacitor.ValidateIndex ( (NonNegativeInt32) index, out IndexOutOfBoundariesException? e ) );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 2, "For available '2' is index '2' out of bounds." )]
  public void ValidateIndex_NonNegativeInt32_NegativeScenarios ( int index, string errMsg )
  {
    Capacitor<int> capacitor = new([1,2]);

    Assert.IsTrue ( capacitor.ValidateIndex ( (NonNegativeInt32) index, out IndexOutOfBoundariesException? e ) );
    Assert.AreEqual ( errMsg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 0, 1 )]
  [DataRow ( 0, 0 )]
  [DataRow ( 4, 1 )]
  [DataRow ( 4, 0 )]
  [DataRow ( 5, 0 )]
  [DataRow ( 10, 0 )]
  public void ValidateSegmentation_PositiveScenarios ( int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    bool result = capacitor.ValidateSegmentation
    (
      (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e
    );

    Assert.IsFalse ( result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 4, 2 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 10, 1 )]
  public void ValidateSegmentation_NegativeScenarios ( int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    bool result = capacitor.ValidateSegmentation
    (
      (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e
    );

    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, offset, count );

    Assert.IsTrue ( result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
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
  [DataRow ( -1, "Index must be non-negative, but it is '-1'." )]
  [DataRow ( 5, "For available '5' is index '5' out of bounds." )]
  public void Indexer_Getter_NegativeScenarios ( int index, string errMsg )
  {
    Action<Capacitor<int>> test = c => _ = c[index];
    Indexer_NegativeScenario ( test, errMsg );
  }

  [TestMethod]
  [DataRow ( -1, "Index must be non-negative, but it is '-1'." )]
  [DataRow ( 5, "For available '5' is index '5' out of bounds." )]
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
  [DataRow ( 0, 6 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  public void BinarySearch_OffsetCountComparer_InvalidSegment ( int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => _ = capacitor.BinarySearch(offset, count, default, default);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = $"With available 5, given offset {offset} and count {count} produce out-of indexing.";
    Assert.AreEqual ( msg, e.Message );
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
