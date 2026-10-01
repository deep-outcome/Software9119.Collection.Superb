using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;
using Software9119.Collection.Superb.Segmentation;
using Software9119.Collection.Superb.Storing;
using Software9119.Collection.Superb.TestArrangement.Segmentation._equipage;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using DVI = Software9119.Collection.Superb.TestArrangement.TestAide.DoubleValueItem;
using PVI = Software9119.Collection.Superb.TestArrangement.TestAide.PositionValueItem;
using VWR = Software9119.Collection.Superb.TestArrangement.TestAide.ValueWithReference;

namespace Software9119.Collection.Superb.TestArrangement.Storing;

[TestClass]
[SuppressMessage ( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = @"¯\_x_x_/¯" )]
public class CapacitorTest
{

  [TestMethod]
  public void NullAction ()
  {
    ArgumentNullException e = Capacitor.NullAction("XXX");
    Assert.AreEqual ( "Action must be provided. (Parameter 'XXX')", e.Message );
  }

  [TestMethod]
  public void NullComparer ()
  {
    ArgumentNullException e = Capacitor.NullComparer("XXX");
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'XXX')", e.Message );
  }

  [TestMethod]
  public void NullConverter ()
  {
    ArgumentNullException e = Capacitor.NullConverter("XXX");
    Assert.AreEqual ( "Converter must be provided. (Parameter 'XXX')", e.Message );
  }

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
  public void InsufficientTargetArray ()
  {
    ArgumentOutOfRangeException e = Capacitor.InsufficientTargetArray("XXX", 2, 3);
    const string msg
      = "Insufficient target array size, available length 2 cannot accomodate 3 items. (Parameter 'XXX')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void OffsetCountParamNames ()
  {
    string [] test = Capacitor.OffsetCountParamNames;
    Assert.AreEqual ( 2, test.Length );
    Assert.IsTrue ( test.Contains ( "offset" ) );
    Assert.IsTrue ( test.Contains ( "count" ) );
  }

  [TestMethod]
  public void OffsetCountParametersGetter ()
  {
    string [] test = Capacitor.OffsetCountParametersGetter();
    Assert.IsTrue ( ReferenceEquals ( Capacitor.OffsetCountParamNames, test ) );
  }

  [TestMethod]
  public void FromIndexCountParamNames ()
  {
    string [] test = Capacitor.FromIndexCountParamNames;
    Assert.AreEqual ( 2, test.Length );
    Assert.IsTrue ( test.Contains ( "fromIndex" ) );
    Assert.IsTrue ( test.Contains ( "count" ) );
  }

  [TestMethod]
  public void FromIndexCountParametersGetter ()
  {
    string [] test = Capacitor.FromIndexCountParametersGetter();
    Assert.IsTrue ( ReferenceEquals ( Capacitor.FromIndexCountParamNames, test ) );
  }


  [TestMethod]
  public void RearSetCountParamNames ()
  {
    string [] test = Capacitor.RearSetCountParamNames;
    Assert.AreEqual ( 2, test.Length );
    Assert.IsTrue ( test.Contains ( "rearSet" ) );
    Assert.IsTrue ( test.Contains ( "count" ) );
  }

  [TestMethod]
  public void RearSetCountParametersGetter ()
  {
    string [] test = Capacitor.RearSetCountParametersGetter();
    Assert.IsTrue ( ReferenceEquals ( Capacitor.RearSetCountParamNames, test ) );
  }

  [TestMethod]
  [DataRow ( 0, 5, 5 )]
  [DataRow ( 4, 5, 1 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 6, 5, -1 )]
  public void AvailableCount ( int index, int count, int result )
  {
    int test = Capacitor.AvailableCount(index, count);
    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  public void GetStoreWithCapacity ()
  {
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), Capacitor.GetStoreWithCapacity<int> ( 0 ) ) );

    int [] test = Capacitor.GetStoreWithCapacity<int> ( 10 );
    Assert.AreEqual ( 10, test.Length );
  }

  [TestMethod]
  public void Empty ()
  {
    Capacitor<int> empty = Capacitor.Empty<int>();
    Assert.IsFalse ( ReferenceEquals ( Capacitor.Empty<int> (), empty ) );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), empty.store ) );
    Assert.AreEqual ( 0, empty.storeIndex );
    Assert.AreEqual ( GrowFactor.Two, empty.growFactor );
    Assert.IsFalse ( empty.LockGrowFactor );
  }


  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void ValidateIndex_Static_Int32_PositiveScenarios ( int index )
  {
    Assert.IsFalse ( Capacitor.ValidateIndex ( index, 2, out IndexOutOfBoundariesException? e, "" ) );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( -1, "" )]
  [DataRow ( -1, null )]
  [DataRow ( -1, "yourParam" )]
  [DataRow ( 2, "" )]
  [DataRow ( 2, null )]
  [DataRow ( 2, "yourParam" )]
  public void ValidateIndex_Static_Int32_NegativeScenarios ( int index, string paramName )
  {
    string errMsg = index == -1
      ? "Index must be non-negative integer, but it is '-1'."
      : "For available '2' is index '2' out of bounds.";

    string paramString = paramName == "yourParam" ? " (Parameter 'yourParam')" : "";
    errMsg += paramString;

    Assert.IsTrue ( Capacitor.ValidateIndex ( index, 2, out IndexOutOfBoundariesException? e, paramName ) );
    Assert.AreEqual ( errMsg, e?.Message );
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
    int result = Capacitor.ValidateSegmentation
    (
      5, (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e,
      () => []
    );

    Assert.AreEqual ( count == 0 ? -1 : 0, result );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void ValidateSegmentation_Static_NegativeScenarios ( int offset, int count )
  {
    int result = Capacitor.ValidateSegmentation
    (
      5, (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e,
      () => []
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
    Func<string[]>? parameters = withParamaters == null
      ? null
      : withParamaters == true
        ? () => ["ABC", "xYz"]
        : () => [];

    _ = Capacitor.ValidateSegmentation ( 5, 0, 6, out _, out ImpSegExc? e, parameters );

    string msg = "With available 5, given offset 0 and count 6 produce out-of indexing.{0}";
    string parametersString = withParamaters == true ? " (Parameters 'ABC','xYz')" : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, parametersString );

    Assert.AreEqual ( msg, e?.Message );
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

    Assert.IsTrue ( ReferenceEquals ( store ?? Array.Empty<int> (), capacitor.store ) );
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
    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );

    Assert.AreEqual ( capacity, capacitor.store.Length );
    Assert.AreEqual ( 0, capacitor.storeIndex );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void Constructor_Enumerable_NullItems ()
  {
    Capacitor<int> capacitor = new ((IEnumerable<int>?)null);

    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
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
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void Constructor_Enumerable_EmptyEnumerable ()
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 0);
    Capacitor<int> capacitor = new (source);

    Assert.AreEqual ( 0, capacitor.storeIndex );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  public void Constructor_Enumerable_Array ()
  {
    int[] source = Enumerable.Range(0, 9).ToArray();
    Capacitor<int> capacitor = new (source);

    Assert.AreEqual ( 9, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void Constructor_Enumerable_EmptyArray ()
  {
    int[] source = new int[0];
    Capacitor<int> capacitor = new (source);

    Assert.AreEqual ( 0, capacitor.storeIndex );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  public void Constructor_Enumerable_Collection ()
  {
    XCollection<int> source = new(Enumerable.Range(0, 9).ToList());
    Capacitor<int> capacitor = new (source);

    Assert.AreEqual ( 9, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void Constructor_Enumerable_EmptyCollection ()
  {
    XCollection<int> source = new ([]);
    Capacitor<int> capacitor = new (source);

    Assert.AreEqual ( 0, capacitor.storeIndex );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  public void Constructor_Enumerable_ReadOnlyCollection ()
  {
    XReadOnlyCollection<int> source = new(Enumerable.Range(0, 9).ToList());
    Capacitor<int> capacitor = new (source);

    Assert.AreEqual ( 9, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  public void Constructor_Enumerable_EmptyReadOnlyCollection ()
  {
    XReadOnlyCollection<int> source = new ([]);
    Capacitor<int> capacitor = new (source);

    Assert.AreEqual ( 0, capacitor.storeIndex );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_NullItems ( int capacity )
  {
    Capacitor<int> capacitor = new (null, capacity);

    Assert.AreEqual ( capacity, capacitor.store.Length );
    Assert.AreEqual ( 0, capacitor.storeIndex );
    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  [DataRow ( 0, 16 )]
  [DataRow ( 1000, 1000 )]
  public void Constructor_CapacityAndEnumerable_Enumerable ( int capacity, int expCapacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 9);
    Capacitor<int> capacitor = new (source, capacity);

    Assert.AreEqual ( expCapacity, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_EmptyEnumerable ( int capacity )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 0);
    Capacitor<int> capacitor = new (source, capacity);

    Assert.AreEqual ( capacity, capacitor.store.Length );
    Assert.AreEqual ( 0, capacitor.storeIndex );

    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 9 )]
  [DataRow ( 1000, 1000 )]
  public void Constructor_CapacityAndEnumerable_Array ( int capacity, int expCapacity )
  {
    int[] source = XEnumerable.RangeEnumerable(0, 9).ToArray();
    Capacitor<int> capacitor = new (source, capacity);

    Assert.AreEqual ( expCapacity, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_EmptyArray ( int capacity )
  {
    int[] source = new int[0];
    Capacitor<int> capacitor = new (source, capacity);

    Assert.AreEqual ( capacity, capacitor.store.Length );
    Assert.AreEqual ( 0, capacitor.storeIndex );

    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 9 )]
  [DataRow ( 1000, 1000 )]
  public void Constructor_CapacityAndEnumerable_Collection ( int capacity, int expCapacity )
  {
    XCollection<int> source = new (XEnumerable.RangeEnumerable(0, 9).ToList());
    Capacitor<int> capacitor = new (source, capacity);

    Assert.AreEqual ( expCapacity, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_EmptyCollection ( int capacity )
  {
    XCollection<int> source = new ([]);
    Capacitor<int> capacitor = new (source, capacity);

    Assert.AreEqual ( capacity, capacitor.store.Length );
    Assert.AreEqual ( 0, capacitor.storeIndex );

    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 9 )]
  [DataRow ( 1000, 1000 )]
  public void Constructor_CapacityAndEnumerable_ReadOnlyCollection ( int capacity, int expCapacity )
  {
    XReadOnlyCollection<int> source = new (XEnumerable.RangeEnumerable(0, 9).ToList());
    Capacitor<int> capacitor = new (source, capacity);

    Assert.AreEqual ( expCapacity, capacitor.store.Length );
    Assert.AreEqual ( 9, capacitor.storeIndex );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );

    Assert.AreEqual ( GrowFactor.Two, capacitor.growFactor );
    Assert.IsFalse ( capacitor.LockGrowFactor );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 10 )]
  public void Constructor_CapacityAndEnumerable_EmptyReadOnlyCollection ( int capacity )
  {
    XReadOnlyCollection<int> source = new([]);
    Capacitor<int> capacitor = new (source, capacity);

    Assert.AreEqual ( capacity, capacitor.store.Length );
    Assert.AreEqual ( 0, capacitor.storeIndex );

    Assert.AreEqual ( capacity == 0, ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
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
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void AddInsOffset ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3]);
    bool inserting = index != 3;

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.AreEqual ( index, offset.value );
    Assert.AreEqual ( inserting, offset.inserting );
  }

  [TestMethod]
  [DataRow ( 3, 3, 6 )]
  [DataRow ( 3, 4, 4 )]
  [DataRow ( 0, 3, 6 )]
  [DataRow ( 0, 4, 4 )]
  [DataRow ( 1, 3, 6 )]
  [DataRow ( 1, 4, 4 )]
  public void AddInsert ( int index, int cap, int expCap )
  {
    int insertion = 9;
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source, cap);

    List<int> expectation = source.ToList();
    expectation.Insert ( index, insertion );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    capacitor.AddInsert ( offset, insertion );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 4, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 4 )]
  [DataRow ( 1, 1 )]
  public void AddInsert_EmptyCapacitor ( int cap, int expCap )
  {
    int insertion = 9;
    Capacitor<int> capacitor = new ([], cap);

    AddInsertOffset offset = capacitor.AddInsOffset(0);
    capacitor.AddInsert ( offset, insertion );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 1, capacitor.Count );
    Assert.AreEqual ( insertion, capacitor [ 0 ] );
  }

  [TestMethod]
  public void AddInsert_OffsetEnumerableRoomRequest_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.AddInsert ( default, (IEnumerable<int>?) null, 1000 ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void AddInsert_OffsetEnumerableRoomRequest_ArrayItems ( int index )
  {
    int[] insertion = [4,5];
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 3, 5 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 8, 8 )]
  public void AddInsert_OffsetEnumerableRoomRequest_ArrayItems_EmptyCapacitor ( int cap, int expCap )
  {
    int[] insertion = [1,2,3,4,5];
    Capacitor<int> capacitor = new ([], cap);

    AddInsertOffset offset = capacitor.AddInsOffset(0);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void AddInsert_OffsetEnumerableRoomRequest_ArrayItems_EmptyArray ( int index )
  {
    int[] source = [1,2,3];
    int[] insertion = [];
    Capacitor<int> capacitor = new (source);

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void AddInsert_OffsetEnumerableRoomRequest_CollectionItems ( int index )
  {
    XCollection<int> insertion = new([4,5]);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 3, 5 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 8, 8 )]
  public void AddInsert_OffsetEnumerableRoomRequest_CollectionItems_EmptyCapacitor ( int cap, int expCap )
  {
    XCollection<int> insertion = new([1,2,3,4,5]);
    Capacitor<int> capacitor = new ([], cap);

    AddInsertOffset offset = capacitor.AddInsOffset(0);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void AddInsert_OffsetEnumerableRoomRequest_CollectionItems_EmptyCollection ( int index )
  {
    int[] source = [1,2,3];
    XCollection<int> insertion = new([]);
    Capacitor<int> capacitor = new (source);

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void AddInsert_OffsetEnumerableRoomRequest_ReadOnlyCollectionItems ( int index )
  {
    XReadOnlyCollection<int> insertion = new([4,5]);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 3, 5 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 8, 8 )]
  public void AddInsert_OffsetEnumerableRoomRequest_ReadOnlyCollectionItems_EmptyCapacitor ( int cap, int expCap )
  {
    XReadOnlyCollection<int> insertion = new([1,2,3,4,5]);
    Capacitor<int> capacitor = new ([], cap);

    AddInsertOffset offset = capacitor.AddInsOffset(0);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void AddInsert_OffsetEnumerableRoomRequest_ReadOnlyCollectionItems_EmptyReadOnlyCollection ( int index )
  {
    int[] source = [1,2,3];
    XReadOnlyCollection<int> insertion = new([]);
    Capacitor<int> capacitor = new (source);

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3, 3, 6, 0, DisplayName = "add,empty items" )]
  [DataRow ( 3, 0, 6, 1, DisplayName = "add,auto-grow" )]
  [DataRow ( 3, 2, 5, 2, DisplayName = "add,room req" )]
  [DataRow ( 3, 2, 10, 7, DisplayName = "add,room req,auto-grow" )]
  [DataRow ( 1, 3, 6, 0, DisplayName = "insert,empty items" )]
  [DataRow ( 1, 0, 6, 3, DisplayName = "insert,auto-grow" )]
  [DataRow ( 1, 0, 5, 2, DisplayName = "insert,exact-grow" )]
  [DataRow ( 1, 2, 5, 2, DisplayName = "insert,room req" )]
  [DataRow ( 1, 2, 10, 7, DisplayName = "insert,room req,auto-grow" )]
  [DataRow ( 1, 2, 12, 9, DisplayName = "insert,room req,auto-grow,exact-grow" )]
  [DataRow ( 0, 3, 6, 0, DisplayName = "insert start,empty items" )]
  [DataRow ( 0, 0, 7, 4, DisplayName = "insert start,auto-grow,exact-grow" )]
  [DataRow ( 0, 0, 6, 3, DisplayName = "insert start,exact-grow" )]
  [DataRow ( 0, 2, 5, 2, DisplayName = "insert start,room req" )]
  [DataRow ( 0, 2, 10, 7, DisplayName = "insert start,room req,auto-grow" )]
  [DataRow ( 0, 2, 13, 10, DisplayName = "insert start,room req,auto-grow,exact-grow" )]
  public void AddInsert_OffsetEnumerableRoomRequest_Enumerable ( int index, int roomReq, int cap, int count )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, count);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, roomReq ) );

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
  public void AddInsert_OffsetEnumerableRoomRequest_Enumerable_NoOverCapacitation ( int index, int capacity, int roomRequest )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, 4);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);
    _ = capacitor.CapacitateForNext ( capacity );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, roomRequest ) );

    int expectedCapacity = Math.Max(capacity,roomRequest) + 3;
    Assert.AreEqual ( expectedCapacity, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 3, 6 )]
  [DataRow ( 8, 8 )]
  public void AddInsert_OffsetEnumerableRoomRequest_Enumerable_EmptyCapacitor ( int cap, int expCap )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, 5);
    Capacitor<int> capacitor = new ([]);

    AddInsertOffset offset = capacitor.AddInsOffset(0);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, cap ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void AddInsert_OffsetEnumerableRoomRequest_Enumerable_EmptyEnumerable ( int index )
  {
    int[] source = [1,2,3];
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(0, 0);
    Capacitor<int> capacitor = new (source);

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 1003, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Likely unneeded." )]
  async public Task AddInsert_OffsetAsyncEnumerableRoomRequest_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( await capacitor.AddInsert ( default, (IAsyncEnumerable<int>?) null, 1000 ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 3, 6, 0, DisplayName = "add,empty items" )]
  [DataRow ( 3, 0, 6, 1, DisplayName = "add,auto-grow" )]
  [DataRow ( 3, 2, 5, 2, DisplayName = "add,room req" )]
  [DataRow ( 3, 2, 10, 7, DisplayName = "add,room req,auto-grow" )]
  [DataRow ( 1, 3, 6, 0, DisplayName = "insert,empty items" )]
  [DataRow ( 1, 0, 6, 3, DisplayName = "insert,auto-grow" )]
  [DataRow ( 1, 0, 5, 2, DisplayName = "insert,exact-grow" )]
  [DataRow ( 1, 2, 5, 2, DisplayName = "insert,room req" )]
  [DataRow ( 1, 2, 10, 7, DisplayName = "insert,room req,auto-grow" )]
  [DataRow ( 1, 2, 12, 9, DisplayName = "insert,room req,auto-grow,exact-grow" )]
  [DataRow ( 0, 3, 6, 0, DisplayName = "insert start,empty items" )]
  [DataRow ( 0, 0, 7, 4, DisplayName = "insert start,auto-grow,exact-grow" )]
  [DataRow ( 0, 0, 6, 3, DisplayName = "insert start,exact-grow" )]
  [DataRow ( 0, 2, 5, 2, DisplayName = "insert start,room req" )]
  [DataRow ( 0, 2, 10, 7, DisplayName = "insert start,room req,auto-grow" )]
  [DataRow ( 0, 2, 13, 10, DisplayName = "insert start,room req,auto-grow,exact-grow" )]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Likely unneeded." )]
  async public Task AddInsert_OffsetAsyncEnumerableRoomRequest ( int index, int roomReq, int cap, int count )
  {
    IAsyncEnumerable<int> insertion = new TestAide.AsyncEnumerable(4, count);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion.ToBlockingEnumerable ( CancellationToken.None ) );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( await capacitor.AddInsert ( offset, insertion, roomReq ) );

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
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Likely unneeded." )]
  async public Task AddInsert_OffsetAsyncEnumerableRoomRequest_NoOverCapacitation ( int index, int capacity, int roomRequest )
  {
    IAsyncEnumerable<int> insertion = new TestAide.AsyncEnumerable(4, 4);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);
    _ = capacitor.CapacitateForNext ( capacity );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( await capacitor.AddInsert ( offset, insertion, roomRequest ) );

    int expectedCapacity = Math.Max(capacity,roomRequest) + 3;
    Assert.AreEqual ( expectedCapacity, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 0, 8 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 3, 6 )]
  [DataRow ( 8, 8 )]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Likely unneeded." )]
  async public Task AddInsert_OffsetAsyncEnumerableRoomRequest_EmptyCapacitor ( int cap, int expCap )
  {
    IAsyncEnumerable<int> insertion = new TestAide.AsyncEnumerable(4, 5);
    Capacitor<int> capacitor = new ([]);

    AddInsertOffset offset = capacitor.AddInsOffset(0);
    Assert.IsTrue ( await capacitor.AddInsert ( offset, insertion, cap ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( insertion.ToBlockingEnumerable ( CancellationToken.None ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Likely unneeded." )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  async public Task AddInsert_OffsetAsyncEnumerableRoomRequest_EmptyEnumerable ( int index )
  {
    int[] source = [1,2,3];
    IAsyncEnumerable<int> insertion = new TestAide.AsyncEnumerable(0, 0);
    Capacitor<int> capacitor = new (source);

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( await capacitor.AddInsert ( offset, insertion, 1000 ) );

    Assert.AreEqual ( 1003, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void AddInsert_OffsetArray_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.AddInsert ( default, (int []?) null ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void AddInsert_OffsetArray ( int index, int count, int capacity )
  {
    int[] insertion = Enumerable.Range(4, count).ToArray();
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source, capacity);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion ) );

    int capCount = source.Length + count;
    Assert.AreEqual ( capCount, capacitor.Capacity );
    Assert.AreEqual ( capCount, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 3, 5 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 8, 8 )]
  public void AddInsert_OffsetArray_EmptyCapacitor ( int cap, int expCap )
  {
    int[] insertion = [1,2,3,4,5];
    Capacitor<int> capacitor = new ([], cap);

    AddInsertOffset offset = capacitor.AddInsOffset(0);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void AddInsert_OffsetArray_EmptyArray ( int index )
  {
    int[] source = [1,2,3];
    int[] insertion = [];
    Capacitor<int> capacitor = new (source);

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion ) );

    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void AddInsert_OffsetCollection_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.AddInsert ( default, (ICollection<int>?) null ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void AddInsert_OffsetCollection ( int index, int count, int capacity )
  {
    ICollection<int> insertion = new XCollection<int>(Enumerable.Range(4, count).ToList());
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source, capacity);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion ) );

    int capCount = source.Length + count;
    Assert.AreEqual ( capCount, capacitor.Capacity );
    Assert.AreEqual ( capCount, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 3, 5 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 8, 8 )]
  public void AddInsert_OffsetCollection_EmptyCapacitor ( int cap, int expCap )
  {
    ICollection<int> insertion = new XCollection<int>([1,2,3,4,5]);
    Capacitor<int> capacitor = new ([], cap);

    AddInsertOffset offset = capacitor.AddInsOffset(0);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void AddInsert_OffsetCollection_EmptyCollection ( int index )
  {
    int[] source = [1,2,3];
    ICollection<int> insertion = new XCollection<int>([]);
    Capacitor<int> capacitor = new (source);

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion ) );

    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void AddInsert_OffsetReadOnlyCollection_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.AddInsert ( default, (IReadOnlyCollection<int>?) null ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void AddInsert_OffsetReadOnlyCollection_Array ( int index, int count, int capacity )
  {
    int[] data = Enumerable.Range(4, count).ToArray();
    AddInsert_OffsetReadOnlyCollection ( index, capacity, data );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void AddInsert_OffsetReadOnlyCollection_Collection ( int index, int count, int capacity )
  {
    List<int> data = Enumerable.Range(4, count).ToList();
    AddInsert_OffsetReadOnlyCollection ( index, capacity, data );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void AddInsert_OffsetReadOnlyCollection_ReadOnlyCollection ( int index, int count, int capacity )
  {
    List<int> data = Enumerable.Range(4, count).ToList();
    IReadOnlyCollection<int>  roCollection = new XReadOnlyCollection<int> ( data );
    AddInsert_OffsetReadOnlyCollection ( index, capacity, roCollection );
  }

  static void AddInsert_OffsetReadOnlyCollection ( int index, int capacity, IReadOnlyCollection<int> insertion )
  {
    int[] source = [1,2,3];
    Capacitor<int> capacitor = new (source, capacity);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion ) );

    int capCount = source.Length + insertion.Count;
    Assert.AreEqual ( capCount, capacitor.Capacity );
    Assert.AreEqual ( capCount, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 3, 5 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 8, 8 )]
  public void AddInsert_OffsetReadOnlyCollection_EmptyCapacitor ( int cap, int expCap )
  {
    IReadOnlyCollection<int> insertion = new XReadOnlyCollection<int>([1,2,3,4,5]);
    Capacitor<int> capacitor = new ([], cap);

    AddInsertOffset offset = capacitor.AddInsOffset(0);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void AddInsert_OffsetReadOnlyCollection_EmptyReadOnlyCollection ( int index )
  {
    int[] source = [1,2,3];
    IReadOnlyCollection<int> insertion = new XReadOnlyCollection<int>([]);
    Capacitor<int> capacitor = new (source);

    AddInsertOffset offset = capacitor.AddInsOffset(index);
    Assert.IsTrue ( capacitor.AddInsert ( offset, insertion ) );

    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
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
  [SuppressMessage ( "Style", "IDE0017:Simplify object initialization", Justification = "Not holistic." )]
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
  public void BackUpTail_Addition ()
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    AddInsertOffset offset = new (5, false);
    Assert.IsNull ( capacitor.BackUpTail ( offset ) );
    Assert.AreEqual ( 5, capacitor.Count );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 2 )]
  [DataRow ( 4 )]
  public void BackUpTail_Insertion ( int index )
  {
    int[] source = [1,2,3,4,5, 6,7,8];
    Capacitor<int> capacitor = new(5, source);

    AddInsertOffset offset = new (index, true);
    int[]? test = capacitor.BackUpTail ( offset );

    Assert.IsNotNull ( test );
    Assert.AreEqual ( index, capacitor.Count );

    ArraySegment<int> expectation = new (source, offset, 5 -offset);
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
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

    Assert.AreEqual ( 2, capacitor.ItemsCountToEndInclusive ( 1 ) );
    Assert.AreEqual ( 1, capacitor.ItemsCountToEndInclusive ( 2 ) );
    Assert.AreEqual ( 0, capacitor.ItemsCountToEndInclusive ( 3 ) );
    Assert.AreEqual ( -1, capacitor.ItemsCountToEndInclusive ( 4 ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 3 )]
  public void PrepareStoreForAddIns_NoItems ( int index )
  {
    int[] source = [1,2,3];
    Capacitor<int> capacitor = new(source);
    AddInsertOffset offset = capacitor.AddInsOffset(index);

    Assert.IsFalse ( capacitor.PrepareStoreForAddIns ( offset, 0 ) );
    Assert.AreEqual ( 3, capacitor.Capacity );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void PrepareStoreForAddIns_Insertion ( int forCount )
  {
    int[] tail= [5,6,7];
    int [] source = [1,2,3,4,5,6,7];
    Capacitor<int> capacitor = new(source, 8);

    const int index = 4;
    AddInsertOffset offset = new (index, true);

    List<int> expectation = new (source);
    for (int c = forCount, ti = 0, ei = index ; c > 0 ; --c, ++ti, ++ei)
      expectation.Insert ( ei, tail [ ti ] );

    Assert.IsTrue ( capacitor.PrepareStoreForAddIns ( offset, forCount ) );

    int capacity = source.Length + forCount;
    Assert.AreEqual ( capacity, capacitor.Capacity );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor.store! ) );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void PrepareStoreForAddIns_Addition ( int forCount )
  {
    int [] source = [1,2,3,4,5,6,7];
    Capacitor<int> capacitor = new(source, 8);

    AddInsertOffset offset = new (7, false);
    Assert.IsTrue ( capacitor.PrepareStoreForAddIns ( offset, forCount ) );

    int capacity = source.Length + forCount;
    Assert.AreEqual ( capacity, capacitor.Capacity );
    Assert.IsTrue ( source.Concat ( new int [ forCount ] ).SequenceEqual ( capacitor.store! ) );
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

    object[] refSource = source.Cast<object>().ToArray();
    VWR[] withRefSource = source.Select(x => (VWR)x).ToArray();
    int[] valSource = source.ToArray();

    source.RemoveAt ( at );

    Capacitor<object> refC = new (refSource,4);
    Capacitor<VWR> withRefC = new (withRefSource,4);
    Capacitor<int> valC = new (valSource,4);

    _ = refC.RemoveAndReturn ( at );
    _ = withRefC.RemoveAndReturn ( at );
    _ = valC.RemoveAndReturn ( at );

    Assert.AreEqual ( 3, valC.store [ 2 ] );
    Assert.AreEqual ( default, withRefC.store [ 2 ] );
    Assert.AreEqual ( null, refC.store [ 2 ] );

    Assert.AreEqual ( 0, valC.store [ 3 ] );
    Assert.AreEqual ( default, withRefC.store [ 3 ] );
    Assert.AreEqual ( null, refC.store [ 3 ] );

    Assert.IsTrue ( source.SequenceEqual ( refC.Cast<int> () ) );
    Assert.IsTrue ( source.SequenceEqual ( withRefC.Select ( x => x.Value ) ) );
    Assert.IsTrue ( source.SequenceEqual ( valC ) );
  }

  [TestMethod]
  public void SetStoreWithCapacity ()
  {
    Capacitor<int> capacitor = new();
    Assert.AreEqual ( 0, capacitor.Capacity );

    capacitor.SetStoreWithCapacity ( 100 );
    Assert.AreEqual ( 100, capacitor.Capacity );

    capacitor.SetStoreWithCapacity ( 0 );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 0, 1 )]
  [DataRow ( 0, 5 )]
  [DataRow ( 2, 0 )]
  [DataRow ( 2, 2 )]
  [DataRow ( 2, 3 )]
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
      ? "Index must be non-negative integer, but it is '-1'."
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
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 2 )]
  public void ValidateInsertionIndex_Int32_PositiveScenarios ( int index )
  {
    Capacitor<int> capacitor = new([1,2]);

    Assert.IsFalse ( capacitor.ValidateInsertionIndex ( index, out IndexOutOfBoundariesException? e, "" ) );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( -1, "" )]
  [DataRow ( -1, null )]
  [DataRow ( -1, "yourParam" )]
  [DataRow ( 3, "" )]
  [DataRow ( 3, null )]
  [DataRow ( 3, "yourParam" )]
  public void ValidateInsertionIndex_Int32_NegativeScenarios ( int index, string paramName )
  {
    Capacitor<int> capacitor = new([1,2]);

    string errMsg = index == -1
      ? "Index must be non-negative integer, but it is '-1'."
      : "Cannot insert at index '3' when available is '2'.";

    string paramString = paramName == "yourParam" ? " (Parameter 'yourParam')" : "";
    errMsg += paramString;

    Assert.IsTrue ( capacitor.ValidateInsertionIndex ( index, out IndexOutOfBoundariesException? e, paramName ) );
    Assert.AreEqual ( errMsg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 0, 2 )]
  [DataRow ( 5, 1, 3 )]
  [DataRow ( 5, 4, 6 )]
  [DataRow ( 5, 5, 1 )]
  [DataRow ( 5, 7, 9 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  [DataRow ( 0, 1, 1 )]
  public void ValidateRearSetConfiguration_NegativeScenarios ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);

    Assert.AreEqual ( 1, capacitor.ValidateRearSetConfiguration ( index, count, out ImpSegExc? e, () => [] ) );

    string msg = "With available {0}, given rearSet {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 2 )]
  [DataRow ( 3 )]
  [DataRow ( 4 )]
  public void ValidateRearSetConfiguration_Parameters ( int testCase )
  {
    Func<string[]>? parameters = testCase switch
    {
      1 => null,
      2 => () => null!,
      3 => () => [],
      4 => () => ["ABC", "xYz"],
      _ => throw new ArgumentOutOfRangeException(nameof( testCase ) )
    };

    Capacitor<int> capacitor = new(new int[5]);
    Assert.AreEqual ( 1, capacitor.ValidateRearSetConfiguration ( 4, 6, out ImpSegExc? e, parameters! ) );

    string msg = "With available 5, given rearSet 4 and count 6 produce out-of indexing.{0}";
    string parametersString = testCase == 4 ? " (Parameters 'ABC','xYz')" : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, parametersString );

    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 4, 5 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 5, 0, 1 )]
  [DataRow ( 5, 1, 2 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 1, 0 )]
  [DataRow ( 1, 0, 1 )]
  public void ValidateRearSetConfiguration_PositiveScenarios ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);

    int test = count == 0 ? -1 : 0;
    Assert.AreEqual ( test, capacitor.ValidateRearSetConfiguration ( index, count, out ImpSegExc? e, () => [] ) );
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
    int test = capacitor.ValidateSegmentation
    (
      (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e,
      () => []
    );

    Assert.AreEqual ( count == 0 ? -1 : 0, test );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.IsNull ( e );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void ValidateSegmentation_NegativeScenarios ( int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    int test = capacitor.ValidateSegmentation
    (
      (NonNegativeInt32) offset, (NonNegativeInt32) count,
      out int limit, out ImpossibleSegmentationException? e,
      () => []
    );

    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, offset, count );

    Assert.AreEqual ( 1, test );
    Assert.AreEqual ( IndexingValidator.LimitOutOf ( offset, count ), limit );
    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 2 )]
  [DataRow ( 3 )]
  [DataRow ( 4 )]
  public void ValidateSegmentation_Parameters ( int testCase )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Func<string[]>? parameters = testCase switch
    {
      1 => null,
      2 => () => null!,
      3 => () => [],
      4 => () => ["ABC", "xYz"],
      _ => throw new ArgumentOutOfRangeException(nameof( testCase ) )
    };

    _ = capacitor.ValidateSegmentation ( 0, 6, out _, out ImpSegExc? e, parameters! );

    string msg = "With available 5, given offset 0 and count 6 produce out-of indexing.{0}";
    string parametersString = testCase == 4 ? " (Parameters 'ABC','xYz')" : "";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, parametersString );

    Assert.AreEqual ( msg, e?.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 2 )]
  [DataRow ( 4 )]
  public void Indexer ( int index )
  {
    const int inquest = 10;
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    List<int> expectation = capacitor.ToList();
    expectation [ index ] = inquest;

    capacitor [ index ] = inquest;
    Assert.AreEqual ( inquest, capacitor [ index ] );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( -1, "Index must be non-negative integer, but it is '-1'. (Parameter 'index')" )]
  [DataRow ( 5, "For available '5' is index '5' out of bounds. (Parameter 'index')" )]
  public void Indexer_Getter_NegativeScenarios ( int index, string errMsg )
  {
    Action<Capacitor<int>> test = c => _ = c[index];
    Indexer_NegativeScenario ( test, errMsg );
  }

  [TestMethod]
  [DataRow ( -1, "Index must be non-negative integer, but it is '-1'. (Parameter 'index')" )]
  [DataRow ( 5, "For available '5' is index '5' out of bounds. (Parameter 'index')" )]
  public void Indexer_Setter_NegativeScenarios ( int index, string errMsg )
  {
    Action<Capacitor<int>> test = c => c[index] = default;
    Indexer_NegativeScenario ( test, errMsg );
  }

  static void Indexer_NegativeScenario ( Action<Capacitor<int>> scenario, string errMsg )
  {
    int[] source = [1,2,3,4,5];
    Capacitor<int> capacitor = new(source, 8);
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

    Action test;
    Capacitor<int> capacitor;
    InvalidOperationException e;

    test = () => _ = new Capacitor<int> () { LockGrowFactor = true, GrowFactor = factor };
    e = Assert.ThrowsExactly<InvalidOperationException> ( test );
    Assert.AreEqual ( "Grow factor is locked.", e.Message );

    capacitor = new () { LockGrowFactor = true };
    test = () => capacitor.GrowFactor = factor;
    e = Assert.ThrowsExactly<InvalidOperationException> ( test );
    Assert.AreEqual ( "Grow factor is locked.", e.Message );

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
  [DataRow ( 1, 1, true )]
  [DataRow ( 4, 5, false )]
  public void IsFull ( int count, int capacity, bool isFull )
  {
    IEnumerable<int> source = Enumerable.Range(0,count);
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

    foreach (int n in new int [] { 4, 5 })
    {
      capacitor.Add ( n );
      Assert.AreEqual ( n, capacitor.Count );
      Assert.AreEqual ( 6, capacitor.Capacity );
    }

    IEnumerable<int> expectation = Enumerable.Range(1, 5);
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Add_EnumerableRoomRequest_NullItems ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsFalse ( capacitor.Add ( (IEnumerable<int>?) null, 1000 ) );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 0, capacitor.Capacity );
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
    Assert.IsTrue ( capacitor.Add ( addition, count ) );

    addition = XEnumerable.RangeEnumerable ( 0, 0 );
    Assert.IsTrue ( capacitor.Add ( addition, 1 ) );

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
    Assert.IsTrue ( capacitor.Add ( new int [ 0 ], 1000 ) );

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
    Assert.IsTrue ( capacitor.Add ( new List<int> (), 1000 ) );

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
    Assert.IsTrue ( capacitor.Add ( new XReadOnlyCollection<int> ( [] ), 1000 ) );

    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Likely unneeded." )]
  async public Task Add_AsyncEnumerableRoomRequest_NullItems ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsFalse ( await capacitor.Add ( (IAsyncEnumerable<int>?) null, 1000 ) );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Likely unneeded." )]
  async public Task Add_AsyncEnumerableRoomRequest_Enumerable ()
  {
    IAsyncEnumerable<int> addition;
    Capacitor<int> capacitor = new();

    const int count = 5;
    addition = new TestAide.AsyncEnumerable ( 1, count );
    Assert.IsTrue ( await capacitor.Add ( addition, 0 ) );
    Assert.AreEqual ( count, capacitor.Count );
    Assert.AreEqual ( 8, capacitor.Capacity );

    addition = new TestAide.AsyncEnumerable ( 6, count );
    Assert.IsTrue ( await capacitor.Add ( addition, count ) );

    addition = new TestAide.AsyncEnumerable ( 0, 0 );
    Assert.IsTrue ( await capacitor.Add ( addition, 1 ) );

    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 11, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Add_Array_NullItems ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsFalse ( capacitor.Add ( (int []?) null ) );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 0, capacitor.Capacity );
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
    Assert.IsTrue ( capacitor.Add ( new int [ 0 ] ) );

    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Add_Collection_NullItems ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsFalse ( capacitor.Add ( (ICollection<int>?) null ) );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 0, capacitor.Capacity );
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
    Assert.IsTrue ( capacitor.Add ( (ICollection<int>) new int [ 0 ] ) );

    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Add_ReadOnlyCollection_NullItems ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsFalse ( capacitor.Add ( (IReadOnlyCollection<int>?) null ) );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 0, capacitor.Capacity );
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
    Assert.IsTrue ( capacitor.Add ( new XReadOnlyCollection<int> ( [] ) ) );

    Assert.AreEqual ( 10, capacitor.Count );
    Assert.AreEqual ( 10, capacitor.Capacity );

    Assert.IsTrue ( Enumerable.Range ( 1, 10 ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void AllMatches ()
  {
    Capacitor<int> capacitor = new(5, [1,2,3,4,5, 10,11,12]);

    Predicate<int> all = i => i < 10;
    Predicate<int> none = i => i < 0;
    Predicate<int> some = i => i != 3;
    Predicate<int> start = i => i != 1;
    Predicate<int> end = i => i != 5;

    Assert.AreEqual ( 1, capacitor.AllMatches ( all ) );
    Assert.AreEqual ( 0, capacitor.AllMatches ( none ) );
    Assert.AreEqual ( 0, capacitor.AllMatches ( some ) );
    Assert.AreEqual ( 0, capacitor.AllMatches ( start ) );
    Assert.AreEqual ( 0, capacitor.AllMatches ( end ) );
  }

  [TestMethod]
  public void AllMatches_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new([]);
    Assert.AreEqual ( -1, capacitor.AllMatches ( x => true ) );
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
  public void AllMatches_Offset ()
  {
    Capacitor<int> capacitor = new(8, [1,2,3,4,5,6,7,8, 10,11,12]);

    Predicate<int> all = i => i < 10;
    Predicate<int> start = i => i != 1;
    Predicate<int> some = i => i != 5;
    Predicate<int> end = i => i != 8;

    Assert.IsTrue ( capacitor.AllMatches ( all, 0 ) );

    Assert.IsFalse ( capacitor.AllMatches ( start, 0 ) );
    Assert.IsTrue ( capacitor.AllMatches ( start, 1 ) );

    Assert.IsFalse ( capacitor.AllMatches ( some, 0 ) );
    Assert.IsTrue ( capacitor.AllMatches ( some, 5 ) );

    Assert.IsFalse ( capacitor.AllMatches ( end, 0 ) );
    Assert.IsFalse ( capacitor.AllMatches ( end, 7 ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void AllMatches_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => _ = capacitor.AllMatches(x => true, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void AllMatches_Offset_NullPredicate ()
  {
    Capacitor<int> capacitor = new([1]);
    Action test = () => capacitor.AllMatches ( null!, 0 );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  public void AllMatches_OffsetCount ()
  {
    Capacitor<int> capacitor = new(8, [1,2,3,4,5,6,7,8, 10,11,12]);

    Predicate<int> all = i => i < 10;
    Predicate<int> start = i => i != 1;
    Predicate<int> some = i => i != 5;
    Predicate<int> end = i => i != 8;

    Assert.AreEqual ( 1, capacitor.AllMatches ( all, 0, 8 ) );

    Assert.AreEqual ( 0, capacitor.AllMatches ( start, 0, 1 ) );
    Assert.AreEqual ( 1, capacitor.AllMatches ( start, 1, 1 ) );

    Assert.AreEqual ( 0, capacitor.AllMatches ( some, 0, 5 ) );
    Assert.AreEqual ( 1, capacitor.AllMatches ( some, 5, 3 ) );

    Assert.AreEqual ( 0, capacitor.AllMatches ( end, 7, 1 ) );
    Assert.AreEqual ( 1, capacitor.AllMatches ( end, 6, 1 ) );
  }

  [TestMethod]
  public void AllMatches_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Assert.AreEqual ( -1, capacitor.AllMatches ( x => true, 0, 0 ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 2 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void AllMatches_OffsetCount_EmptySegment ( int offset )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Assert.AreEqual ( -1, capacitor.AllMatches ( x => true, offset, 0 ) );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void AllMatches_OffsetCount_InvalidSegment ( int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => _ = capacitor.AllMatches(x => true, offset, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = $"With available 5, given offset {offset} and count {count} produce out-of indexing. (Parameters 'offset','count')";
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
  public void AllMatches_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);

    int result = count == 0 ? -1 : 1;
    Assert.AreEqual ( result, capacitor.AllMatches ( x => true, index, count ) );
  }

  [TestMethod]
  public void AllMatches_OffsetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new([1]);
    Action test = () => capacitor.AllMatches ( null!, 0, 0 );

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
    RevertedComparer comparer = new (1, 10);
    Capacitor<int> capacitor = new(10, [1,2,3,4,5, 6,7,8,9,10, 11,12,13]);
    int test = capacitor.BinarySearch(value, comparer);

    Assert.IsGreaterThanOrEqualTo ( 0, test );
    int antiValue = RevertedComparer.Revert(value, comparer.reversor);
    Assert.AreEqual ( antiValue - 1, test );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 5 )]
  [DataRow ( 6 )]
  [DataRow ( 7 )]
  [DataRow ( 11 )]
  public void BinarySearch_OddCount ( int value )
  {
    RevertedComparer comparer = new (1, 11);
    Capacitor<int> capacitor = new(11, [1,2,3,4,5, 6,7,8,9,10,11, 12,13]);
    int test = capacitor.BinarySearch(value, comparer);

    Assert.IsGreaterThanOrEqualTo ( 0, test );
    int antiValue = RevertedComparer.Revert(value, comparer.reversor);
    Assert.AreEqual ( antiValue - 1, test );
  }

  [TestMethod]
  public void BinarySearch_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsLessThan ( 0, capacitor.BinarySearch ( 0, Comparer<int>.Default ) );
  }

  [TestMethod]
  public void BinarySearch_NullComparer ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.BinarySearch(0, null!);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'comparer')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, 9 )]
  [DataRow ( 1, 9, 9 )]
  [DataRow ( 5, 0, 5 )]
  [DataRow ( 5, 6, -1 )]
  [DataRow ( 6, 0, 4 )]
  [DataRow ( 6, 5, -1 )]
  [DataRow ( 10, 0, 0 )]
  [DataRow ( 10, 1, -1 )]
  public void BinarySearch_Offset_EvenCount ( int value, int offset, int index )
  {
    RevertedComparer comparer = new (1, 10);
    Capacitor<int> capacitor = new(10, [1,2,3,4,5, 6,7,8,9,10, 11,12,13]);
    int test = capacitor.BinarySearch(value, offset, comparer);

    if (index < 0)
      Assert.IsLessThan ( 0, test );
    else
      Assert.AreEqual ( index, test );
  }

  [TestMethod]
  [DataRow ( 1, 0, 10 )]
  [DataRow ( 1, 10, 10 )]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 7, -1 )]
  [DataRow ( 6, 0, 5 )]
  [DataRow ( 6, 6, -1 )]
  [DataRow ( 7, 0, 4 )]
  [DataRow ( 7, 5, -1 )]
  [DataRow ( 11, 0, 0 )]
  [DataRow ( 11, 1, -1 )]
  public void BinarySearch_Offset_OddCount ( int value, int offset, int index )
  {
    RevertedComparer comparer = new (1, 11);
    Capacitor<int> capacitor = new(11, [1,2,3,4,5, 6,7,8,9,10,11, 12,13]);

    int test = capacitor.BinarySearch(value, offset, comparer);

    if (index < 0)
      Assert.IsLessThan ( 0, test );
    else
      Assert.AreEqual ( index, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void BinarySearch_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => _ = capacitor.BinarySearch(0, size, Comparer<int>.Default);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void BinarySearch_Offset_NullComparer ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.BinarySearch(0, 0, null!);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'comparer')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, 0, 10 )]
  [DataRow ( 1, -1, 1, 9 )]
  [DataRow ( 5, 4, 2, 6 )]
  [DataRow ( 5, -1, 0, 4 )]
  [DataRow ( 5, -1, 5, 5 )]
  [DataRow ( 6, 5, 2, 6 )]
  [DataRow ( 6, -1, 0, 5 )]
  [DataRow ( 6, -1, 6, 4 )]
  [DataRow ( 10, 9, 0, 10 )]
  [DataRow ( 10, -1, 0, 9 )]
  public void BinarySearch_OffsetCountComparer_EvenCount ( int value, int result, int offset, int count )
  {
    Capacitor<int> capacitor = new(10, [1,2,3,4,5, 6,7,8,9,10, 11,12,13]);
    int test = capacitor.BinarySearch(offset, count, value, Comparer<int>.Default);

    Assert.AreEqual ( result == -1, test < 0 );
    Assert.AreEqual ( result == -1 ? test : result, test );
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
    Capacitor<int> capacitor = new(11, [1,2,3,4,5, 6,7,8,9,10,11, 12,13]);
    int test = capacitor.BinarySearch(offset, count, value, Comparer<int>.Default);

    Assert.AreEqual ( result == -1, test < 0 );
    Assert.AreEqual ( result == -1 ? test : result, test );
  }

  [TestMethod]
  [DataRow ( 1, 9 )]
  [DataRow ( 10, 0 )]
  public void BinarySearch_OffsetCountComparer_Comparer ( int value, int result )
  {
    RevertedComparer comparer = new (1, 10);
    Capacitor<int> capacitor = new(10, [1,2,3,4,5, 6,7,8,9,10, 11,12,13]);
    int test = capacitor.BinarySearch(0, 10, value, comparer);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  [DataRow ( 10 )]
  [DataRow ( 11 )]
  public void BinarySearch_OffsetCountComparer_EmptySegment ( int offset )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5, 6,7,8,9,10,11]);
    int test = capacitor.BinarySearch(offset, 0, -2, Comparer<int>.Default);

    Assert.IsLessThan ( 0, test );
  }

  [TestMethod]
  public void BinarySearch_OffsetCountComparer_EmptyComparer ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsLessThan ( 0, capacitor.BinarySearch ( 0, 0, 0, Comparer<int>.Default ) );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void BinarySearch_OffsetCountComparer_InvalidSegment ( int offset, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => _ = capacitor.BinarySearch(offset, count, default, Comparer<int>.Default);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = $"With available 5, given offset {offset} and count {count} produce out-of indexing. (Parameters 'offset','count')";
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
  public void BinarySearch_OffsetCountComparer_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Assert.IsLessThan ( 0, capacitor.BinarySearch ( index, count, 1, Comparer<int>.Default ) );
  }

  [TestMethod]
  public void BinarySearch_OffsetCountComparer_NullComparer ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.BinarySearch(0, 0, 0, null!);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'comparer')", e.Message );
  }

  [TestMethod]
  [DataRow ( 4, 10, false )]
  [DataRow ( 10, 10, false )]
  [DataRow ( 5, 10, true )]
  [DataRow ( 5, 5, false )]
  [DataRow ( 6, 10, true )]
  [DataRow ( 11, 10, true )]
  public void CapacitateExactly ( int to, int capacity, bool result )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5], capacity);
    bool test = capacitor.CapacitateExactly(to);
    Assert.AreEqual ( result, test );
    Assert.AreEqual ( result ? to : capacity, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 5, false )]
  [DataRow ( 6, true )]
  public void CapacitateExactlyToCount ( int capacity, bool result )
  {
    int[] source = [1,2,3,4,5];
    Capacitor<int> capacitor = new(source, capacity);
    Assert.AreEqual ( result, capacitor.CapacitateExactlyToCount () );

    Assert.AreEqual ( 5, capacitor.Count );
    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
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
  public void Contains ()
  {
    Capacitor<int> capacitor = new(5, [1,2,3,4,5, 6,7,8]);
    Assert.IsTrue ( capacitor.Contains ( 1 ) );
    Assert.IsTrue ( capacitor.Contains ( 5 ) );
    Assert.IsFalse ( capacitor.Contains ( 6 ) );
  }

  [TestMethod]
  public void Contains_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsFalse ( capacitor.Contains ( 0 ) );
  }

  [TestMethod]
  [DataRow ( 1, 0, true )]
  [DataRow ( 1, 1, true )]
  [DataRow ( 6, 0, true )]
  [DataRow ( 6, 5, true )]
  [DataRow ( 8, 0, false )]
  [DataRow ( 0, 0, false )]
  public void Contains_Offset ( int value, int offset, bool result )
  {
    Capacitor<int> capacitor = new(6, [1,2,3, 1,2,6, 8,9]);
    bool test = capacitor.Contains(value, offset);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Contains_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };
    Action test = () => capacitor.Contains(0, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, 6, true )]
  [DataRow ( 1, 1, 3, true )]
  [DataRow ( 1, 0, 1, true )]
  [DataRow ( 3, 3, 3, true )]
  [DataRow ( 3, 5, 1, true )]
  [DataRow ( 6, 0, 6, false )]
  [DataRow ( 0, 0, 6, false )]
  public void Contains_OffsetCount ( int value, int offset, int count, bool result )
  {
    Capacitor<int> capacitor = new(6, [1,2,3, 1,2,3, 6,7]);

    Assert.AreEqual ( result, capacitor.Contains ( value, offset, count ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void Contains_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Assert.IsFalse ( capacitor.Contains ( -2, index, 0 ) );
  }

  [TestMethod]
  public void Contains_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsFalse ( capacitor.Contains ( -2, 0, 0 ) );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void Contains_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.Contains(-2, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
  public void Contains_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Assert.IsFalse ( capacitor.Contains ( -2, index, count ) );
  }

  [TestMethod]
  public void Convert ()
  {
    Capacitor<int> capacitor = new(6, [ 1,2,3 ,4,5,6, 7,8,9])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Converter<int, long> convertor = x => x;
    Capacitor<long> test = capacitor.Convert ( convertor );

    Assert.AreEqual ( 6, test.Count );
    Assert.AreEqual ( 6, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsTrue ( capacitor.Select ( x => (long) x ).SequenceEqual ( test ) );
  }

  [TestMethod]
  public void Convert_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new()
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Converter<int, long> convertor = x => x;
    Capacitor<long> test = capacitor.Convert ( convertor );

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( 0, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
  }

  [TestMethod]
  public void Convert_NullConvertor ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.Convert ( (Converter<int, int>) null! );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Converter must be provided. (Parameter 'converter')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 3 )]
  [DataRow ( 5 )]
  public void Convert_Offset ( int offset )
  {
    Capacitor<int> capacitor = new(6, [ 1,2,3 ,4,5,6, 7,8,9])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Converter<int, long> convertor = x => x;
    Capacitor<long> test = capacitor.Convert ( convertor, offset );

    int count = 6 - offset;
    Assert.AreEqual ( count, test.Count );
    Assert.AreEqual ( count, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );

    Assert.IsFalse ( ReferenceEquals ( capacitor.store, test.store ) );
    Assert.IsTrue ( IListSeg ( capacitor, offset, count ).Select ( x => (long) x ).SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Convert_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Converter<int, long> convertor = x => x;
    Action test = () => capacitor.Convert(convertor, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void Convert_Offset_NullConvertor ()
  {
    Capacitor<int> capacitor = new([1]);
    Action test = () => capacitor.Convert ( (Converter<int, int>) null!, 0 );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Converter must be provided. (Parameter 'converter')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 1, 4 )]
  [DataRow ( 0, 4 )]
  [DataRow ( 1, 3 )]
  [DataRow ( 0, 1 )]
  [DataRow ( 4, 1 )]
  public void Convert_OffsetCount_OffsetCount ( int index, int count )
  {
    Capacitor<int> capacitor = new(5, [1,2,3,4,5, 7,8,9])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Converter<int, long> convertor = x => x;
    Capacitor<long> test = capacitor.Convert(convertor, index, count);

    Assert.AreEqual ( count, test.Count );
    Assert.AreEqual ( count, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );

    Assert.IsFalse ( ReferenceEquals ( capacitor.store, test.store ) );
    Assert.IsTrue ( IListSeg ( capacitor, index, count ).Select ( x => (long) x ).SequenceEqual ( test ) );
  }

  [TestMethod]
  public void Convert_OffsetCount_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new()
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Converter<int, long> convertor = x => x;
    Capacitor<long> test = capacitor.Convert(convertor, 0, 0);

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( 0, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );

    Assert.IsTrue ( ReferenceEquals ( Array.Empty<long> (), test.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 1, 0 )]
  [DataRow ( 1, 1 )]
  [DataRow ( 5, 0 )]
  [DataRow ( 5, 4 )]
  [DataRow ( 5, 5 )]
  public void Convert_OffsetCount_EmptySegment ( int size, int index )
  {
    Capacitor<int> capacitor = new(new int [size])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Converter<int, long> convertor = x => x;
    Capacitor<long> test = capacitor.Convert(convertor, index, 0);

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( 0, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );

    Assert.IsTrue ( ReferenceEquals ( Array.Empty<long> (), test.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void Convert_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Converter<int, long> convertor = x => x;
    Action test = () => capacitor.Convert ( convertor, index, count );

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
  public void Convert_OffsetCountComparer_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);

    Converter<int, long> convertor = x => x;
    _ = capacitor.Convert ( convertor, index, count );
  }

  [TestMethod]
  public void Convert_OffsetCount_NullConvertor ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.Convert ( (Converter<int, int>) null!, 0, 0 );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Converter must be provided. (Parameter 'converter')", e.Message );
  }

  [TestMethod]
  public void CopyTo_Array ()
  {
    Capacitor<int> capacitor = new(5, [1,2,3,4,5, 6,7,8]);

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
  [DataRow ( 0, 5 )]
  [DataRow ( 0, 6 )]
  [DataRow ( 4, 1 )]
  [DataRow ( 4, 2 )]
  [DataRow ( 2, 4 )]
  public void CopyTo_ArrayFromIndex ( int offset, int size )
  {
    Capacitor<int> capacitor = new(5, [1,2,3,4,5, 6,7,8]);

    int[] test = new int[size];
    capacitor.CopyTo ( offset, test );

    int count = 5 - offset;
    IListSegment<int> expectation = IListSeg ( capacitor, offset, count );
    Assert.IsTrue ( expectation.SequenceEqual ( test.Take ( count ) ) );

    Assert.IsTrue ( test.Skip ( count ).All ( x => x == 0 ) );
  }

  [TestMethod]
  public void CopyTo_ArrayFromIndex_NullArray ()
  {
    Capacitor<int> capacitor = new([]);
    Action test = () => capacitor.CopyTo(0, (int[])null!);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Target array must be provided. (Parameter 'array')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void CopyTo_ArrayFromIndex_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => capacitor.CopyTo(size, new int[0] );

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'fromIndex')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 4, 0 )]
  [DataRow ( 3, 1 )]
  public void CopyTo_ArrayFromIndex_ArrayOfInsufficientLength ( int size, int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.CopyTo(index, new int[size]);

    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    string msg = "Insufficient target array size, available length {0} cannot accomodate {1} items. (Parameter 'array')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, 5 - index );
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5, 5 )]
  [DataRow ( 0, 5, 6 )]
  [DataRow ( 0, 1, 1 )]
  [DataRow ( 0, 1, 2 )]
  [DataRow ( 7, 1, 1 )]
  [DataRow ( 7, 1, 2 )]
  [DataRow ( 1, 6, 6 )]
  [DataRow ( 1, 6, 8 )]
  public void CopyTo_ArrayFromIndexCount ( int offset, int count, int size )
  {
    Capacitor<int> capacitor = new(8, [ 1,2,3,4, 5,6,7,8, 9,10]);

    int[] test = new int[size];
    capacitor.CopyTo ( offset, count, test );

    IListSegment<int> expectation = IListSeg ( capacitor, offset, count );
    Assert.IsTrue ( expectation.SequenceEqual ( test.Take ( count ) ) );

    Assert.IsTrue ( test.Skip ( count ).All ( x => x == 0 ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void CopyTo_ArrayFromIndexCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    capacitor.CopyTo ( index, 0, new int [ 0 ] );
  }

  [TestMethod]
  public void CopyTo_ArrayFromIndexCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    capacitor.CopyTo ( 0, 0, new int [ 0 ] );
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
  public void CopyTo_ArrayFromIndexCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    capacitor.CopyTo ( index, count, new int [ count ] );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void CopyTo_ArrayFromIndexCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.CopyTo(index, count, new int[5]);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'fromIndex','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count );

    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void CopyTo_ArrayFromIndexCount_NullArray ()
  {
    Capacitor<int> capacitor = new([]);
    Action test = () => capacitor.CopyTo(0, 0, (int[])null!);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Target array must be provided. (Parameter 'array')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 0, 1 )]
  [DataRow ( 4, 1 )]
  [DataRow ( 2, 3 )]
  public void CopyTo_ArrayFromIndexCount_ArrayOfInsufficientLength ( int index, int count )
  {
    int size = count - 1;
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.CopyTo(index, count, new int[size]);

    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    string msg = "Insufficient target array size, available length {0} cannot accomodate {1} items. (Parameter 'array')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, count );
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void CopyTo_ArrayArrayIndex ()
  {
    Capacitor<int> capacitor = new(5, [1,2,3,4,5, 6,7,8]);

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

    int[] test = new int[1];
    capacitor.CopyTo ( test, 0 );
    Assert.AreEqual ( 0, test.Sum () );
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
  [DataRow ( -1, "Index must be non-negative integer, but it is '-1'. (Parameter 'arrayIndex')" )]
  [DataRow ( 0, "For available '0' is index '0' out of bounds. (Parameter 'arrayIndex')" )]
  [DataRow ( 5, "For available '5' is index '5' out of bounds. (Parameter 'arrayIndex')" )]
  public void CopyTo_ArrayArrayIndex_IndexOutOfBounds ( int index, string errMsg )
  {
    int size = index == -1 ? 5 : index;
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.CopyTo(new int[size], index);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 4, 0 )]
  [DataRow ( 5, 1 )]
  public void CopyTo_ArrayArrayIndex_ArrayOfInsufficientLength ( int size, int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.CopyTo(new int[size], index);

    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    string msg = "Insufficient target array size, available length 4 cannot accomodate 5 items. (Parameter 'array')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 0, 8 )]
  [DataRow ( 0, 0, 9 )]
  [DataRow ( 0, 1, 9 )]
  [DataRow ( 0, 1, 10 )]
  [DataRow ( 1, 0, 7 )]
  [DataRow ( 7, 0, 1 )]
  public void CopyTo_ArrayArrayIndexFromIndex ( int sourceIndex, int targetIndex, int size )
  {
    Capacitor<int> capacitor = new(8, [ 1,2,3,4, 5,6,7,8, 9,10]);

    int[] test = new int[size];
    capacitor.CopyTo ( test, targetIndex, sourceIndex );

    int count = 8 - sourceIndex;
    IListSegment<int> expectation = IListSeg ( capacitor, sourceIndex, count );
    Assert.IsTrue ( expectation.SequenceEqual ( test.Skip ( targetIndex ).Take ( count ) ) );

    Assert.IsTrue ( test.Take ( targetIndex ).All ( x => x == 0 ) );
    Assert.IsTrue ( test.Skip ( targetIndex + count ).All ( x => x == 0 ) );
  }

  [TestMethod]
  public void CopyTo_ArrayArrayIndexFromIndex_NullArray ()
  {
    Capacitor<int> capacitor = new([]);
    Action test = () => capacitor.CopyTo((int[])null!, 0, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Target array must be provided. (Parameter 'array')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void CopyTo_ArrayArrayIndexFromIndex_IndexOutOfBounds_Source ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => capacitor.CopyTo(new int[1], 0, size );

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'fromIndex')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void CopyTo_ArrayArrayIndexFromIndex_IndexOutOfBounds_TargetArray ( int size )
  {
    Capacitor<int> capacitor = new(new int[1]);
    Action test = () => capacitor.CopyTo(new int[size], size, 0 );

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'arrayIndex')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 4, 0, 0 )]
  [DataRow ( 5, 1, 0 )]
  [DataRow ( 3, 0, 1 )]
  [DataRow ( 4, 1, 1 )]
  public void CopyTo_ArrayArrayIndexFromIndex_ArrayOfInsufficientLength ( int size, int targetIndex, int sourceIndex )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.CopyTo(new int[size], targetIndex, sourceIndex);

    ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( test );
    string msg = "Insufficient target array size, available length {0} cannot accomodate {1} items. (Parameter 'array')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size - targetIndex, 5 - sourceIndex );
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 0, 5, "1,2,3,4,5" )]
  [DataRow ( 7, 1, 0, 5, "0,1,2,3,4,5,0" )]
  [DataRow ( 7, 2, 1, 3, "0,0,2,3,4,0,0" )]
  [DataRow ( 1, 0, 0, 1, "1" )]
  [DataRow ( 2, 1, 0, 1, "0,1" )]
  [DataRow ( 1, 0, 4, 1, "5" )]
  [DataRow ( 2, 0, 4, 1, "5,0" )]
  public void CopyTo_ArrayArrayIndexFromIndexCount ( int size, int index, int fromIndex, int count, string result )
  {
    Capacitor<int> capacitor = new(5, [1,2,3,4,5, 6,7,8]);

    int[] test = new int[size];
    capacitor.CopyTo ( test, index, fromIndex, count );
    List<int> expectation = result.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 1, 1 )]
  [DataRow ( 5, 0 )]
  [DataRow ( 5, 5 )]
  public void CopyTo_ArrayArrayIndexFromIndexCount_EmptyCapacitor ( int size, int index )
  {
    Capacitor<int> capacitor = new();
    int[] test = new int[size];

    capacitor.CopyTo ( test, index, 0, 0 );
    Assert.AreEqual ( 0, test.Sum () );
  }

  [TestMethod]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 0, 0, 4 )]
  [DataRow ( 0, 0, 5 )]
  [DataRow ( 5, 0, 0 )]
  [DataRow ( 5, 4, 0 )]
  [DataRow ( 5, 5, 0 )]
  public void CopyTo_ArrayArrayIndexFromIndexCount_EmptySegment ( int size, int index, int fromIndex )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    int [] test = new int[size];

    capacitor.CopyTo ( test, index, fromIndex, 0 );
    Assert.AreEqual ( 0, test.Sum () );
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
  [DataRow ( 4, 4, 1 )]
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
  [DataRow ( 5, 1 )]
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
  [DataRow ( 5, 0, 1 )]
  [DataRow ( 5, 4, 1 )]
  [DataRow ( 5, 1, 4 )]
  [DataRow ( 5, 0, 4 )]
  [DataRow ( 5, 1, 3 )]
  [DataRow ( 5, 5, 0 )]
  [DataRow ( 5, 0, 0 )]
  [DataRow ( 0, 0, 0 )]
  public void CopyTo_ArrayArrayIndexFromIndexCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    capacitor.CopyTo ( new int [ size ], 0, index, count );
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
    Capacitor<int> capacitor = new(count, [1,2,3,4,5, 7,8,9])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Capacitor<int> test = (Capacitor<int>)capacitor.Clone();

    Assert.AreEqual ( count, test.Count );
    Assert.AreEqual ( count, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsFalse ( ReferenceEquals ( capacitor.store, test.store ) );
    Assert.IsTrue ( IListSeg ( capacitor, 0, count ).SequenceEqual ( test ) );
  }

  [TestMethod]
  public void Clone_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new()
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Capacitor<int> test = (Capacitor<int>)capacitor.Clone();

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( 0, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), test.store ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Clone_Offset ( int offset )
  {
    Capacitor<int> capacitor = new(6, [1,2,3,4,5,6, 7,8,9])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Capacitor<int> test = capacitor.Clone(offset);

    int count = 6 - offset;
    Assert.AreEqual ( count, test.Count );
    Assert.AreEqual ( count, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsFalse ( ReferenceEquals ( capacitor.store, test.store ) );
    Assert.IsTrue ( IListSeg ( capacitor, offset, count ).SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Clone_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => capacitor.Clone(size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 1, 4 )]
  [DataRow ( 0, 4 )]
  [DataRow ( 1, 3 )]
  public void Clone_OffsetCount ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Capacitor<int> test = capacitor.Clone(index, count);
    IListSegment<int> expectation = new(capacitor, index, count);

    Assert.AreEqual ( count, test.Count );
    Assert.AreEqual ( count, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsFalse ( ReferenceEquals ( capacitor.store, test.store ) );
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void Clone_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new()
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Capacitor<int> test = capacitor.Clone(0, 0);

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( 0, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), test.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 0 )]
  [DataRow ( 1, 0 )]
  [DataRow ( 1, 1 )]
  [DataRow ( 5, 0 )]
  [DataRow ( 5, 4 )]
  [DataRow ( 5, 5 )]
  public void Clone_EmptySegment ( int size, int index )
  {
    Capacitor<int> capacitor = new(new int [size])
    {
      GrowFactor = GrowFactor.Five,
      LockGrowFactor = true,
    };

    Capacitor<int> test = capacitor.Clone(index, 0);

    Assert.AreEqual ( 0, test.Count );
    Assert.AreEqual ( 0, test.Capacity );
    Assert.AreEqual ( GrowFactor.Five, test.GrowFactor );
    Assert.AreEqual ( true, test.LockGrowFactor );
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), test.store ) );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void Clone_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.Clone ( index, count );

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  [DataRow ( 8 )]
  public void Fill ( int count )
  {
    int[] source = [1, 2, 3, 4, 5, 6, 7, 8];
    Capacitor<int> capacitor = new(count, source.ToArray());
    capacitor.Fill ( 9 );

    (int endIndex, int endCount) = GetEndSegment ( count );

    ArraySegment<int> expectation = new(source,endIndex, endCount);
    ArraySegment<int> test = new(capacitor.store, endIndex, endCount);

    Assert.AreEqual ( count, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
    Assert.IsTrue ( capacitor.All ( x => x == 9 ) );

    static (int, int) GetEndSegment ( int count ) => (count % 8, 8 - count);
  }

  [TestMethod]
  public void Fill_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    capacitor.Fill ( default );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 2 )]
  [DataRow ( 5 )]
  public void Fill_Offset ( int offset )
  {
    int[] source = [ 1,2,3, 4,5,6, 7,8,9];
    Capacitor<int> capacitor = new(6, source.ToArray());
    capacitor.Fill ( 9, offset );

    Assert.IsTrue ( ArrSeg ( source, 0, offset ).SequenceEqual ( ArrSeg ( capacitor.store, 0, offset ) ) );
    Assert.IsTrue ( ArrSeg ( capacitor.store, offset, 6 - offset ).All ( x => x == 9 ) );
    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }


  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Fill_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => capacitor.Fill(0, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 5 )]
  [DataRow ( 1, 4 )]
  [DataRow ( 0, 4 )]
  [DataRow ( 1, 3 )]
  public void Fill_OffsetCount ( int index, int count )
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
  public void Fill_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    capacitor.Fill ( default, 0, 0 );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void Fill_OffsetCount_EmptySegment ( int index )
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
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void Fill_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.Fill (default, index, count );

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, index, count );

    Assert.AreEqual ( msg, e.Message );
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
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void FindFirstIndex_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindFirstIndex(predicate, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void FindFirstItem_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindFirstItem(predicate, index, count, out _);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void FindLastIndex_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new(new int[5]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindLastIndex(predicate, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing. (Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

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

    Assert.IsFalse ( capacitor.FindMthItem ( predicate, 1, index, count, out _ ) );
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
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing. (Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

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
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing. (Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

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

  [TestMethod]
  [DataRow ( 1, true )]
  [DataRow ( 11, true )]
  [DataRow ( 13, false )]
  public void FindMatch ( int value, bool result )
  {
    Capacitor<int> capacitor = new(6, [1,3,5, 7,9,11, 13,15]);
    Predicate<int> predicate = x => x == value;

    Assert.AreEqual ( result, capacitor.FindMatch ( predicate ) );
  }

  [TestMethod]
  public void FindMatch_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = x => true;

    Assert.IsFalse ( capacitor.FindMatch ( predicate ) );
  }

  [TestMethod]
  public void FindMatch_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindMatch(predicate);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, true )]
  [DataRow ( 1, 1, false )]
  [DataRow ( 11, 0, true )]
  [DataRow ( 11, 5, true )]
  [DataRow ( 13, 0, false )]
  public void FindMatch_Offset ( int value, int offset, bool result )
  {
    Capacitor<int> capacitor = new(6, [1,3,5, 7,9,11, 13,15]);
    Predicate<int> predicate = x => x == value;

    Assert.AreEqual ( result, capacitor.FindMatch ( predicate, offset ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void FindMatch_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindMatch(predicate, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void FindMatch_Offset_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindMatch(predicate, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, 6, true )]
  [DataRow ( 1, 0, 1, true )]
  [DataRow ( 1, 1, 3, true )]
  [DataRow ( 1, 3, 1, true )]
  [DataRow ( 4, 5, 1, true )]
  [DataRow ( 4, 0, 6, true )]
  [DataRow ( 8, 0, 6, false )]
  [DataRow ( 0, 0, 6, false )]
  public void FindMatch_OffsetCount ( int value, int offset, int count, bool result )
  {
    Capacitor<int> capacitor = new(6, [1,2,3,1,2,4,8,9]);

    Predicate<int> predicate = x => x == value;
    Assert.AreEqual ( result, capacitor.FindMatch ( predicate, offset, count ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void FindMatch_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindMatch ( predicate, index, 0 ) );
  }

  [TestMethod]
  public void FindMatch_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Predicate<int> predicate = x => true;
    Assert.IsFalse ( capacitor.FindMatch ( predicate, 0, 0 ) );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void FindMatch_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Predicate<int> predicate = x => default;
    Action test = () => _ = capacitor.FindMatch(predicate, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
  public void FindMatch_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Predicate<int> predicate = x => false;

    Assert.IsFalse ( capacitor.FindMatch ( predicate, index, count ) );
  }

  [TestMethod]
  public void FindMatch_OffsetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Predicate<int> predicate = null!;
    Action test = () => _ = capacitor.FindMatch(predicate, 0, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Okay." )]
  public void ForEach ()
  {
    DVI[] source = [1,3,5, 7,9,11, 13,15];

    Capacitor<DVI> capacitor = new(6, source);
    Action<DVI?> action = x => x!.Secondary *= 2;

    capacitor.ForEach ( action );

    Assert.IsTrue ( source.Take ( 6 ).All ( x => x.Secondary == x.Primary * 2 ) );
    Assert.IsTrue ( source.Skip ( 6 ).All ( x => x.Secondary == x.Primary ) );
  }

  [TestMethod]
  public void ForEach_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Action<int> action = x => throw new ShouldNotBeThrownException();

    capacitor.ForEach ( action );
  }

  [TestMethod]
  public void ForEach_NullAction ()
  {
    Capacitor<int> capacitor = new();
    Action<int> action = null!;
    Action test = () => capacitor.ForEach(action);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Action must be provided. (Parameter 'action')", e.Message );
  }

  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Okay." )]
  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 3 )]
  public void ForEach_Offset ( int offset )
  {
    DVI[] source = [1,3,5, 7,9,11, 13,15];

    Capacitor<DVI> capacitor = new(6, source);
    Action<DVI?> action = x => x!.Secondary *= 2;

    capacitor.ForEach ( action, offset );

    Assert.IsTrue ( source.Skip ( offset ).Take ( 6 - offset ).All ( x => x.Secondary == x.Primary * 2 ) );
    Assert.IsTrue ( source.Skip ( 6 ).All ( DVI.ValuesEqual ) );
    Assert.IsTrue ( source.Take ( offset ).All ( DVI.ValuesEqual ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void ForEach_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };
    Action test = () => capacitor.ForEach(action, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void ForEach_Offset_NullAction ()
  {
    Capacitor<int> capacitor = new();
    Action<int> action = null!;
    Action test = () => capacitor.ForEach(action, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Action must be provided. (Parameter 'action')", e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 0, 1 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 1, 3 )]
  [DataRow ( 3, 1 )]
  public void ForEach_OffsetCount ( int offset, int count )
  {
    DVI[] source = [1,3,5, 7,9,11, 13,15];

    Capacitor<DVI> capacitor = new(6, source);
    Action<DVI?> action = x => x!.Secondary *= 2;

    capacitor.ForEach ( action, offset, count );

    Assert.IsTrue ( source.Skip ( offset ).Take ( count ).All ( x => x.Secondary == x.Primary * 2 ) );
    Assert.IsTrue ( source.Skip ( 6 ).All ( DVI.ValuesEqual ) );
    Assert.IsTrue ( source.Take ( offset ).All ( DVI.ValuesEqual ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void ForEach_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Action<int> action = x => throw new ShouldNotBeThrownException();
    capacitor.ForEach ( action, index, 0 );
  }

  [TestMethod]
  public void ForEach_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Action<int> action = x => throw new ShouldNotBeThrownException();
    capacitor.ForEach ( action, 0, 0 );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void ForEach_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action<int> action = x => { };
    Action test = () => capacitor.ForEach(action, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
  public void ForEach_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };

    capacitor.ForEach ( action, index, count );
  }

  [TestMethod]
  public void ForEach_OffsetCount_NullAction ()
  {
    Capacitor<int> capacitor = new();
    Action<int> action = null!;
    Action test = () => capacitor.ForEach(action, 0, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Action must be provided. (Parameter 'action')", e.Message );
  }

  [TestMethod]
  public void GetHashCodeTest ()
  {
    Capacitor<int> capacitor = new ();

    int baseHash = (int)Reflection.NonVirtualBaseCall( typeof(object), typeof(int), capacitor, "GetHashCode", Reflection.PubInst )!;
    int test = capacitor.GetHashCode();

    Assert.AreNotEqual ( baseHash, test );
    Assert.AreEqual ( test, HashCode.Combine ( baseHash ) );
  }

  [TestMethod]
  public void GetEnumerator ()
  {
    Capacitor<int> capacitor = new();
    CapacitorStoreEnumerator<int> e1 = capacitor.GetEnumerator();
    IEnumerator<int> e2 = ((IEnumerable<int>)capacitor).GetEnumerator();
    IEnumerator e3 = ((IEnumerable)capacitor).GetEnumerator();

    Assert.AreEqual ( e1, e2 );
    Assert.AreEqual ( e2, e3 );
  }

  [TestMethod]
  [DataRow ( 1, true )]
  [DataRow ( 2, true )]
  [DataRow ( 6, true )]
  [DataRow ( 8, false )]
  [DataRow ( 0, false )]
  public void IndexOf ( int value, bool finds )
  {
    Capacitor<int> capacitor = new(6, [1,2,3,1,2,6,8,9]);
    int test = capacitor.IndexOf(value);

    Assert.AreEqual ( finds ? value - 1 : -1, test );
  }

  [TestMethod]
  public void IndexOf_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    int test = capacitor.IndexOf(0);
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  [DataRow ( 1, 0, 0 )]
  [DataRow ( 1, 1, 3 )]
  [DataRow ( 6, 0, 5 )]
  [DataRow ( 6, 5, 5 )]
  [DataRow ( 8, 0, -1 )]
  [DataRow ( 0, 0, -1 )]
  public void IndexOf_Offset ( int value, int offset, int index )
  {
    Capacitor<int> capacitor = new(6, [1,2,3,1,2,6,8,9]);
    int test = capacitor.IndexOf(value, offset);

    Assert.AreEqual ( index, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void IndexOf_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };
    Action test = () => capacitor.IndexOf(0, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 0, 6, 0 )]
  [DataRow ( 1, 1, 3, 3 )]
  [DataRow ( 1, 0, 1, 0 )]
  [DataRow ( 3, 3, 3, 5 )]
  [DataRow ( 3, 5, 1, 5 )]
  [DataRow ( 6, 0, 6, -1 )]
  [DataRow ( 0, 0, 6, -1 )]
  public void IndexOf_OffsetCount ( int value, int offset, int count, int position )
  {
    Capacitor<int> capacitor = new(6, [1,2,3, 1,2,3, 6,7]);

    Assert.AreEqual ( position, capacitor.IndexOf ( value, offset, count ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void IndexOf_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Assert.AreEqual ( -1, capacitor.IndexOf ( 0, index, 0 ) );
  }

  [TestMethod]
  public void IndexOf_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Assert.AreEqual ( -1, capacitor.IndexOf ( 0, 0, 0 ) );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void IndexOf_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Action test = () => capacitor.IndexOf(0, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
  public void IndexOf_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Assert.AreEqual ( -1, capacitor.IndexOf ( -1, index, count ) );
  }

  [TestMethod]
  [DataRow ( 3, 3, 6 )]
  [DataRow ( 3, 4, 4 )]
  [DataRow ( 0, 3, 6 )]
  [DataRow ( 0, 4, 4 )]
  [DataRow ( 1, 3, 6 )]
  [DataRow ( 1, 4, 4 )]
  public void Insert ( int index, int cap, int expCap )
  {
    int insertion = 9;

    List<int> expectation = [1,2,3];
    Capacitor<int> capacitor = new (expectation, cap);

    expectation.Insert ( index, insertion );
    capacitor.Insert ( index, insertion );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 4, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 0, 4 )]
  [DataRow ( 0, 1, 1 )]
  public void Insert_EmptyCapacitor ( int index, int cap, int expCap )
  {
    int insertion = 9;
    Capacitor<int> capacitor = new ([], cap);

    capacitor.Insert ( index, insertion );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 1, capacitor.Count );
    Assert.AreEqual ( insertion, capacitor [ 0 ] );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 6 )]
  public void Insert_IndexOutOfBounds ( int index )
  {
    int size = index -1;
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };
    Action test = () => capacitor.Insert(index, 0);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"Cannot insert at index '{index}' when available is '{size}'. (Parameter 'index')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void Insert_OffsetEnumerableRoomRequest_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.Insert ( default, (IEnumerable<int>?) null, 1000 ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void Insert_OffsetEnumerableRoomRequest_ArrayItems ( int index )
  {
    int[] insertion = [4,5];
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    Assert.IsTrue ( capacitor.Insert ( index, insertion, 1000 ) );

    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 0, 2 )]
  [DataRow ( 0, 3, 3 )]
  public void Insert_OffsetEnumerableRoomRequest_ArrayItems_EmptyCapacitor ( int index, int cap, int expCap )
  {
    int[] insertion = [4,5];
    Capacitor<int> capacitor = new ([], cap);

    Assert.IsTrue ( capacitor.Insert ( index, insertion, 1000 ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void Insert_OffsetEnumerableRoomRequest_CollectionItems ( int index )
  {
    XCollection<int> insertion = new([4,5]);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    Assert.IsTrue ( capacitor.Insert ( index, insertion, 1000 ) );

    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 0, 2 )]
  [DataRow ( 0, 3, 3 )]
  public void Insert_OffsetEnumerableRoomRequest_CollectionItems_EmptyCapacitor ( int index, int cap, int expCap )
  {
    XCollection<int> insertion = new([4,5]);
    Capacitor<int> capacitor = new ([], cap);

    Assert.IsTrue ( capacitor.Insert ( index, insertion, 1000 ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 2, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  public void Insert_OffsetEnumerableRoomRequest_ReadOnlyCollectionItems ( int index )
  {
    XReadOnlyCollection<int> insertion = new([4,5]);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    Assert.IsTrue ( capacitor.Insert ( index, insertion, 1000 ) );

    Assert.AreEqual ( 5, capacitor.Capacity );
    Assert.AreEqual ( 5, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0, 0, 2 )]
  [DataRow ( 0, 3, 3 )]
  public void Insert_OffsetEnumerableRoomRequest_ReadOnlyCollectionItems_EmptyCapacitor ( int index, int cap, int expCap )
  {
    XReadOnlyCollection<int> insertion = new([4,5]);
    Capacitor<int> capacitor = new ([], cap);

    Assert.IsTrue ( capacitor.Insert ( index, insertion, 1000 ) );

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
  public void Insert_OffsetEnumerableRoomRequest_Enumerable ( int index, int roomReq, int cap, int count )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, count);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    Assert.IsTrue ( capacitor.Insert ( index, insertion, roomReq ) );

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
  public void Insert_OffsetEnumerableRoomRequest_Enumerable_NoOverCapacitation ( int index, int capacity, int roomRequest )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, 4);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);
    _ = capacitor.CapacitateForNext ( capacity );

    Assert.IsTrue ( capacitor.Insert ( index, insertion, roomRequest ) );

    int expectedCapacity = Math.Max(capacity,roomRequest) + 3;
    Assert.AreEqual ( expectedCapacity, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 0, 0, 4 )]
  [DataRow ( 0, 3, 3 )]
  [DataRow ( 0, 2, 4 )]
  public void Insert_OffsetEnumerableRoomRequest_Enumerable_EmptyCapacitor ( int index, int cap, int expCap )
  {
    IEnumerable<int> insertion = XEnumerable.RangeEnumerable(4, 3);
    Capacitor<int> capacitor = new ([]);

    Assert.IsTrue ( capacitor.Insert ( index, insertion, cap ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( insertion.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 6 )]
  public void Insert_OffsetEnumerableRoomRequest_IndexOutOfBounds ( int index )
  {
    int size = index -1;
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };
    Action test = () => capacitor.Insert(index, (IEnumerable<int>?)null, 0);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"Cannot insert at index '{index}' when available is '{size}'. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Readme style." )]
  async public Task Insert_OffsetAsyncEnumerableRoomRequest_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( await capacitor.Insert ( default, (IAsyncEnumerable<int>?) null, 1000 ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
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
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Readme style." )]
  async public Task Insert_OffsetAsyncEnumerableRoomRequest_Enumerable ( int index, int roomReq, int cap, int count )
  {
    IAsyncEnumerable<int> insertion = new TestAide.AsyncEnumerable(4, count);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion.ToBlockingEnumerable ( CancellationToken.None ) );

    Assert.IsTrue ( await capacitor.Insert ( index, insertion, roomReq ) );

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
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Readme style." )]
  async public Task Insert_OffsetAsyncEnumerableRoomRequest_Enumerable_NoOverCapacitation ( int index, int capacity, int roomRequest )
  {
    IAsyncEnumerable<int> insertion = new TestAide.AsyncEnumerable(4, 4);
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source);
    _ = capacitor.CapacitateForNext ( capacity );

    Assert.IsTrue ( await capacitor.Insert ( index, insertion, roomRequest ) );

    int expectedCapacity = Math.Max(capacity,roomRequest) + 3;
    Assert.AreEqual ( expectedCapacity, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 0, 0, 4 )]
  [DataRow ( 0, 3, 3 )]
  [DataRow ( 0, 2, 4 )]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Readme style." )]
  async public Task Insert_OffsetAsyncEnumerableRoomRequest_Enumerable_EmptyCapacitor ( int index, int cap, int expCap )
  {
    IAsyncEnumerable<int> insertion = new TestAide.AsyncEnumerable(4, 3);
    Capacitor<int> capacitor = new ([]);

    Assert.IsTrue ( await capacitor.Insert ( index, insertion, cap ) );

    Assert.AreEqual ( expCap, capacitor.Capacity );
    Assert.AreEqual ( 3, capacitor.Count );
    Assert.IsTrue ( insertion.ToBlockingEnumerable ( CancellationToken.None ).SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 6 )]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Readme style." )]
  async public Task Insert_OffsetAsyncEnumerableRoomRequest_IndexOutOfBounds ( int index )
  {
    int size = index -1;
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };
    Func<Task> test = async () => await capacitor.Insert(index, (IAsyncEnumerable<int>?)null, 0);

    IndexOutOfBoundariesException e = await Assert.ThrowsExactlyAsync<IndexOutOfBoundariesException> ( test );
    string msg = $"Cannot insert at index '{index}' when available is '{size}'. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void Insert_OffsetArray_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.Insert ( default, (int []?) null ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void Insert_OffsetArray ( int index, int count, int capacity )
  {
    int[] insertion = Enumerable.Range(4, count).ToArray();
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source, capacity);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    Assert.IsTrue ( capacitor.Insert ( index, insertion ) );

    int capCount = source.Length + count;
    Assert.AreEqual ( capCount, capacitor.Capacity );
    Assert.AreEqual ( capCount, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 6 )]
  public void Insert_OffsetArray_IndexOutOfBounds ( int index )
  {
    int size = index -1;
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };
    Action test = () => capacitor.Insert(index, new int[0]);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"Cannot insert at index '{index}' when available is '{size}'. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void Insert_OffsetCollection_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.Insert ( default, (ICollection<int>?) null ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void Insert_OffsetCollection ( int index, int count, int capacity )
  {
    ICollection<int> insertion = new XCollection<int> ( Enumerable.Range(4, count).ToList());
    int[] source = [1,2,3];

    Capacitor<int> capacitor = new (source, capacity);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    Assert.IsTrue ( capacitor.Insert ( index, insertion ) );

    int capCount = source.Length + count;
    Assert.AreEqual ( capCount, capacitor.Capacity );
    Assert.AreEqual ( capCount, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 6 )]
  public void Insert_OffsetCollection_IndexOutOfBounds ( int index )
  {
    int size = index -1;
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };
    Action test = () => capacitor.Insert(index, new XCollection<int> ([]));

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"Cannot insert at index '{index}' when available is '{size}'. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void Insert_OffsetReadOnlyCollection_NullItems ()
  {
    Capacitor<int> capacitor = [];
    Assert.IsFalse ( capacitor.Insert ( default, (IReadOnlyCollection<int>?) null ) );
    Assert.AreEqual ( 0, capacitor.Capacity );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void Insert_OffsetReadOnlyCollection_Array ( int index, int count, int capacity )
  {
    int[] data = Enumerable.Range(4, count).ToArray();
    Insert_OffsetReadOnlyCollection ( index, capacity, data );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void Insert_OffsetReadOnlyCollection_Collection ( int index, int count, int capacity )
  {
    List<int> data = Enumerable.Range(4, count).ToList();
    Insert_OffsetReadOnlyCollection ( index, capacity, data );
  }

  [TestMethod]
  [DataRow ( 3, 2, 3 )]
  [DataRow ( 3, 2, 5 )]
  [DataRow ( 3, 0, 0 )]
  [DataRow ( 0, 2, 3 )]
  [DataRow ( 0, 2, 5 )]
  [DataRow ( 0, 0, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 0, 0 )]
  public void Insert_OffsetReadOnlyCollection_ReadOnlyCollection ( int index, int count, int capacity )
  {
    List<int> data = Enumerable.Range(4, count).ToList();
    IReadOnlyCollection<int> roCollection = new XReadOnlyCollection<int> ( data );
    Insert_OffsetReadOnlyCollection ( index, capacity, roCollection );
  }

  static void Insert_OffsetReadOnlyCollection ( int index, int capacity, IReadOnlyCollection<int> insertion )
  {
    int[] source = [1,2,3];
    Capacitor<int> capacitor = new (source, capacity);

    List<int> expectation = source.ToList();
    expectation.InsertRange ( index, insertion );

    Assert.IsTrue ( capacitor.Insert ( index, insertion ) );

    int capCount = source.Length + insertion.Count;
    Assert.AreEqual ( capCount, capacitor.Capacity );
    Assert.AreEqual ( capCount, capacitor.Count );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 1 )]
  [DataRow ( 6 )]
  public void Insert_OffsetReadOnlyCollection_IndexOutOfBounds ( int index )
  {
    int size = index -1;
    Capacitor<int> capacitor = new(new int[size]);
    Action<int> action = x => { };
    Action test = () => capacitor.Insert(index, new XReadOnlyCollection<int> ([]));

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"Cannot insert at index '{index}' when available is '{size}'. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 1, 5, -1 )]
  [DataRow ( 1, 6, 0 )]
  [DataRow ( 1, 7, 1 )]
  [DataRow ( 2, 5, -2 )]
  [DataRow ( 2, 6, -1 )]
  [DataRow ( 2, 7, 0 )]
  public void IsCapacitySufficient ( int next, int capacity, int reserve )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5], capacity);

    bool sufficient = reserve >= 0;
    Assert.AreEqual ( sufficient, capacitor.IsCapacitySufficient ( next, out int test ) );
    Assert.AreEqual ( reserve, test );
  }

  [TestMethod]
  [DataRow ( 3, 5 )]
  [DataRow ( 4, 0 )]
  [DataRow ( 6, -1 )]
  [DataRow ( 8, -1 )]
  public void LastIndexOf ( int value, int index )
  {
    Capacitor<int> capacitor = new(6, [4,2,3,1,2,3,6,7]);
    Assert.AreEqual ( index, capacitor.LastIndexOf ( value ) );
  }

  [TestMethod]
  public void LastIndexOf_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    int test = capacitor.LastIndexOf(0);
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  [DataRow ( 3, 5, 0 )]
  [DataRow ( 3, 5, 5 )]
  [DataRow ( 4, 0, 0 )]
  [DataRow ( 4, -1, 1 )]
  [DataRow ( 6, -1, 0 )]
  [DataRow ( 8, -1, 0 )]
  public void LastIndexOf_Offset ( float value, int index, int offset )
  {
    Capacitor<float> capacitor = new(6, [4,2,3,1,2,3,6,7]);
    Assert.AreEqual ( index, capacitor.LastIndexOf ( value, offset ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void LastIndexOf_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<float> capacitor = new(new float[size]);
    Action<float> action = x => { };
    Action test = () => capacitor.LastIndexOf(0f, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }


  [TestMethod]
  [DataRow ( 3, 5, 5 )]
  [DataRow ( 3, 2, 4 )]
  [DataRow ( 4, 0, 5 )]
  [DataRow ( 4, 0, 0 )]
  [DataRow ( 6, -1, 5 )]
  [DataRow ( 8, -1, 5 )]
  public void LastIndexOf_RearSet ( float value, int index, int rearSet )
  {
    Capacitor<float> capacitor = new(6, [4,2,3,1,2,3,6,7]);
    Assert.AreEqual ( index, capacitor.LastIndexOf ( rearSet, value ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void LastIndexOf_RearSet_IndexOutOfBounds ( int size )
  {
    Capacitor<float> capacitor = new(new float[size]);
    Action<float> action = x => { };
    Action test = () => capacitor.LastIndexOf(size, 0f);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'rearSet')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 8, 7 )]
  [DataRow ( 5, 7, 1, 7 )]
  [DataRow ( 5, 0, 7, -1 )]
  [DataRow ( 1, 0, 8, 3 )]
  [DataRow ( 1, 0, 3, 0 )]
  [DataRow ( 1, 0, 1, 0 )]
  [DataRow ( 2, 1, 4, 4 )]
  [DataRow ( 2, 1, 3, 1 )]
  [DataRow ( 7, 0, 8, -1 )]
  [DataRow ( 0, 0, 8, -1 )]
  public void LastIndexOf_OffsetCount ( float value, int offset, int count, int result )
  {
    Capacitor<float> capacitor = new(8, [1,2,3,1,2,3,4,5,7,8]);

    int test = capacitor.LastIndexOf(value, offset, count);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void LastIndexOf_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<float> capacitor = new([1,2,3,4,5]);
    int test = capacitor.LastIndexOf ( -1f, index, 0 );
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  public void LastIndexOf_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<float> capacitor = new();

    int test = capacitor.LastIndexOf(-1f, 0, 0);
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 6, 0 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 5, 1 )]
  [DataRow ( 4, 2 )]
  public void LastIndexOf_OffsetCount_InvalidSegment ( int index, int count )
  {
    Capacitor<float> capacitor = new(new float[5]);
    Action test = () => _ = capacitor.LastIndexOf(0f, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available 5, given offset {0} and count {1} produce out-of indexing. (Parameters 'offset','count')";
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
  public void LastIndexOf_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<float> capacitor = new(new float[size]);
    Assert.AreEqual ( -1, capacitor.LastIndexOf ( -1f, index, count ) );
  }

  [TestMethod]
  [DataRow ( 5, 7, 8, 7 )]
  [DataRow ( 5, 7, 1, 7 )]
  [DataRow ( 5, 6, 7, -1 )]
  [DataRow ( 1, 7, 8, 3 )]
  [DataRow ( 1, 2, 3, 0 )]
  [DataRow ( 1, 0, 1, 0 )]
  [DataRow ( 2, 4, 5, 4 )]
  [DataRow ( 2, 1, 2, 1 )]
  [DataRow ( 7, 7, 8, -1 )]
  [DataRow ( 0, 7, 8, -1 )]
  public void LastIndexOf_RearSetCount ( float value, int rearSet, int count, int result )
  {
    Capacitor<float> capacitor = new(8, [1,2,3, 1,2,3 ,4,5, 7,8]);
    int test = capacitor.LastIndexOf(rearSet, count, value);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void LastIndexOf_RearSetCount_EmptySegment ( int index )
  {
    Capacitor<float> capacitor = new([1,2,3,4,5]);

    int test = capacitor.LastIndexOf ( index, 0, -1f );
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  public void LastIndexOf_RearSetCount_EmptyCapacitor ()
  {
    Capacitor<float> capacitor = new();

    int test = capacitor.LastIndexOf(0, 0, -1f);
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
  public void LastIndexOf_RearSetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<float> capacitor = new(new float[size]);
    Action test = () => _ = capacitor.LastIndexOf(index, count, 0f);

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
  public void LastIndexOf_RearSetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<float> capacitor = new(new float[size]);

    Assert.AreEqual ( -1, capacitor.LastIndexOf ( index, count, -1f ) );
  }

  [TestMethod]
  [DataRow ( 1, 1, 0 )]
  [DataRow ( 1, 2, 3 )]
  [DataRow ( 1, 3, 6 )]
  [DataRow ( 1, 5, 8 )]
  [DataRow ( 1, 6, -1 )]
  [DataRow ( 0, 1, -1 )]
  public void NthIndexOf ( int value, int nth, int index )
  {
    Capacitor<int> capacitor = new(9, [1,2,3, 1,2,3, 1,1,1, 1]);
    int test = capacitor.NthIndexOf(value, nth);

    Assert.AreEqual ( index, test );
  }

  [TestMethod]
  public void NthIndexOf_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    int test = capacitor.NthIndexOf(-1, 1);
    Assert.AreEqual ( -1, test );
  }

  [TestMethod]
  [DataRow ( 0, 1, 1, 0 )]
  [DataRow ( 1, 1, 1, 3 )]
  [DataRow ( 0, 1, 5, 8 )]
  [DataRow ( 8, 1, 1, 8 )]
  [DataRow ( 0, 1, 6, -1 )]
  [DataRow ( 0, 0, 1, -1 )]
  public void NthIndexOf_Offset ( int offset, int value, int nth, int result )
  {
    Capacitor<int> capacitor = new(9, [1,2,3, 1,2,3, 1,1,1, 1]);

    int test = capacitor.NthIndexOf(value, nth, offset);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void NthIndexOf_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => _ = capacitor.NthIndexOf(-1, 1, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 5, 0, 8, 1, 7 )]
  [DataRow ( 5, 0, 7, 1, -1 )]
  [DataRow ( 5, 7, 1, 1, 7 )]
  [DataRow ( 1, 0, 8, 1, 0 )]
  [DataRow ( 1, 1, 7, 1, 3 )]
  [DataRow ( 2, 1, 4, 2, 4 )]
  [DataRow ( 2, 1, 4, 1, 1 )]
  [DataRow ( 2, 1, 1, 1, 1 )]
  [DataRow ( 3, 1, 6, 2, 5 )]
  [DataRow ( 3, 1, 6, 1, 2 )]
  [DataRow ( 7, 0, 8, 1, -1 )]
  [DataRow ( 0, 0, 8, 1, -1 )]
  public void NthIndexOf_OffsetCount ( int value, int offset, int count, int nth, int index )
  {
    Capacitor<int> capacitor = new(8, [1,2,3, 1,2,3, 4,5, 7,8]);

    Assert.AreEqual ( index, capacitor.NthIndexOf ( value, nth, offset, count ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void NthIndexOf_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Assert.AreEqual ( -1, capacitor.NthIndexOf ( -1, 1, index, 0 ) );
  }

  [TestMethod]
  public void NthIndexOf_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Assert.AreEqual ( -1, capacitor.NthIndexOf ( -1, 1, 0, 0 ) );
  }

  [TestMethod]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 1, 5 )]
  [DataRow ( 5, 4, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void NthIndexOf_OffsetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => _ = capacitor.NthIndexOf(0, 1, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing. (Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

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
  public void NthIndexOf_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Assert.AreEqual ( -1, capacitor.NthIndexOf ( -1, 1, index, count ) );
  }

  [TestMethod]
  [DataRow ( 1, 1, 8 )]
  [DataRow ( 1, 2, 5 )]
  [DataRow ( 1, 3, 2 )]
  [DataRow ( 1, 5, 0 )]
  [DataRow ( 1, 6, -1 )]
  [DataRow ( 9, 1, -1 )]
  public void MthIndexOf ( int value, int mth, int index )
  {
    Capacitor<int> capacitor = new(9, [1,1,1, 3,2,1, 3,2,1, 9]);
    int test = capacitor.MthIndexOf(value, mth);

    Assert.AreEqual ( index, test );
  }

  [TestMethod]
  public void MthIndexOf_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    int test = capacitor.MthIndexOf(-1, 1);

    Assert.AreEqual ( -1, test );
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
  public void MthIndexOf_RearSet ( int rearSet, int value, int mth, int result )
  {
    Capacitor<int> capacitor = new(9, [1,1,1, 3,2,1, 3,2,1, 9]);
    int test = capacitor.MthIndexOf(value, mth, rearSet);

    Assert.AreEqual ( result, test );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void MthIndexOf_RearSet_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => _ = capacitor.MthIndexOf(-1, 1, size);

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
  public void MthIndexOf_RearSetCount ( int value, int rearSet, int count, int mth, int index )
  {
    Capacitor<int> capacitor = new(8, [5,4, 3,2,1, 3,2,1, 7,8]);
    Assert.AreEqual ( index, capacitor.MthIndexOf ( value, mth, rearSet, count ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void MthIndexOf_RearSetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Assert.AreEqual ( -1, capacitor.MthIndexOf ( -1, 1, index, 0 ) );
  }

  [TestMethod]
  public void MthIndexOf_RearSetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Assert.AreEqual ( -1, capacitor.MthIndexOf ( -1, 1, 0, 0 ) );
  }

  [TestMethod]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 4, 6 )]
  [DataRow ( 5, 0, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void MthIndexOf_RearSetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => _ = capacitor.MthIndexOf(0, 1, index, count);

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
  public void MthIndexOf_RearSetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Assert.AreEqual ( -1, capacitor.MthIndexOf ( -1, 1, index, count ) );
  }

  [TestMethod]
  public void Order_Comparer ()
  {
    List<int> source = Enumerable.Range(0, 500).ToList();
    ReversiveComparer<int> comparer = new();

    Capacitor<int> capacitor = new (source);
    capacitor.Order ( comparer );

    source.Reverse ();

    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Order_Comparer_NullComparer ()
  {
    Capacitor<int> capacitor = new ();
    Action test = () => capacitor.Order ( (IComparer<int>) null! );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>(test);
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'comparer')", e.Message );

  }

  [TestMethod]
  [DataRow ( 250 )]
  [DataRow ( 0 )]
  public void Order_Comparer_Offset ( int offset )
  {
    List<int> source = Enumerable.Range(0, 500).ToList();
    ReversiveComparer<int> comparer = new();

    Capacitor<int> capacitor = new (source);
    capacitor.Order ( comparer, offset );

    source.Reverse ( offset, 500 - offset );

    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Order_Comparer_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => capacitor.Order(new ReversiveComparer<int>(), size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void Order_Comparer_Offset_NullComparer ()
  {
    Capacitor<int> capacitor = new ();
    Action test = () => capacitor.Order ( (IComparer<int>) null!, 0 );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>(test);
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'comparer')", e.Message );

  }

  [TestMethod]
  [DataRow ( 0, 500 )]
  [DataRow ( 250, 250 )]
  [DataRow ( 125, 125 )]
  public void Order_Comparer_OffsetCount ( int offset, int count )
  {
    List<int> source = Enumerable.Range(0, 500).ToList();
    ReversiveComparer<int> comparer = new();

    Capacitor<int> capacitor = new (source);
    capacitor.Order ( comparer, offset, count );

    source.Reverse ( offset, count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void Order_Comparer_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    ReversiveComparer<int> comparer = new();
    capacitor.Order ( comparer, index, 0 );
  }

  [TestMethod]
  public void Order_Comparer_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    ReversiveComparer<int> comparer = new();
    capacitor.Order ( comparer, 0, 0 );
  }

  [TestMethod]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 1, 5 )]
  [DataRow ( 5, 4, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void Order_Comparer_OffsetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    ReversiveComparer<int> comparer = new();
    Action test = () => capacitor.Order(comparer, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing. (Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

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
  public void Order_Comparer_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    ReversiveComparer<int> comparer = new();

    capacitor.Order ( comparer, index, count );
  }

  [TestMethod]
  public void Order_Comparer_OffsetCount_NullComparer ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.Order ( (IComparer<int>) null!, 0, 0 );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );

    Assert.AreEqual ( "Comparer must be provided. (Parameter 'comparer')", e.Message );
  }

  [TestMethod]
  public void Order_Comparison ()
  {
    List<int> source = Enumerable.Range(0, 500).ToList();
    Comparison<int> comparer = new ReversiveComparer<int>().Compare;

    Capacitor<int> capacitor = new (source);
    capacitor.Order ( comparer );

    source.Reverse ();

    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  public void Order_Comparison_NullComparer ()
  {
    Capacitor<int> capacitor = new ();
    Action test = () => capacitor.Order ( (Comparison<int>) null! );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>(test);
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'comparison')", e.Message );

  }

  [TestMethod]
  [DataRow ( 250 )]
  [DataRow ( 0 )]
  public void Order_Comparison_Offset ( int offset )
  {
    List<int> source = Enumerable.Range(0, 500).ToList();
    Comparison<int> comparer = new ReversiveComparer<int>().Compare;

    Capacitor<int> capacitor = new (source);
    capacitor.Order ( comparer, offset );

    source.Reverse ( offset, 500 - offset );

    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Order_Comparison_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Comparison<int> comparer = new ReversiveComparer<int>().Compare;

    Action test = () => capacitor.Order(comparer, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void Order_Comparison_Offset_NullComparer ()
  {
    Capacitor<int> capacitor = new ();
    Action test = () => capacitor.Order ( (Comparison<int>) null!, 0 );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>(test);
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'comparison')", e.Message );

  }

  [TestMethod]
  [DataRow ( 0, 500 )]
  [DataRow ( 250, 250 )]
  [DataRow ( 125, 125 )]
  public void Order_Comparison_OffsetCount ( int offset, int count )
  {
    List<int> source = Enumerable.Range(0, 500).ToList();
    Comparison<int> comparer = new ReversiveComparer<int>().Compare;

    Capacitor<int> capacitor = new (source);
    capacitor.Order ( comparer, offset, count );

    source.Reverse ( offset, count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void Order_Comparison_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);

    Comparison<int> comparer = new ReversiveComparer<int>().Compare;
    capacitor.Order ( comparer, index, 0 );
  }

  [TestMethod]
  public void Order_Comparison_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();

    Comparison<int> comparer = new ReversiveComparer<int>().Compare;
    capacitor.Order ( comparer, 0, 0 );
  }

  [TestMethod]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 1, 5 )]
  [DataRow ( 5, 4, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void Order_Comparison_OffsetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Comparison<int> comparer = new ReversiveComparer<int>().Compare;
    Action test = () => capacitor.Order(comparer, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing. (Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

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
  public void Order_Comparison_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Comparison<int> comparer = new ReversiveComparer<int>().Compare;

    capacitor.Order ( comparer, index, count );
  }

  [TestMethod]
  public void Order_Comparison_OffsetCount_NullComparer ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.Order ( (Comparison<int>) null!, 0, 0 );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Comparer must be provided. (Parameter 'comparison')", e.Message );
  }

  [TestMethod]
  public void Remove_Item ()
  {
    int[] source = [ 1, 2, 3,  1, 2, 4,  6, 7 ];

    object[] refSource = source.Cast<object>().ToArray();
    VWR[] withRefSource = source.Select(x => (VWR)x).ToArray();
    long[] valSource = source.Select(x => (long)x).ToArray();

    Capacitor<object> refC = new(6, refSource);
    Capacitor<VWR> withRefC = new(6, withRefSource);
    Capacitor<long> valC = new(6, valSource);

    List<object> refExp = refSource.Take(6).ToList();
    List<VWR> withRefExp = withRefSource.Take(6).ToList();
    List<long> valExp = valSource.Take(6).ToList();

    while (refC.Remove ( (object) 1 )) { }
    while (withRefC.Remove ( (VWR) 1 )) { }
    while (valC.Remove ( 1L )) { }

    while (refExp.Remove ( 1 )) { }
    while (withRefExp.Remove ( 1 )) { }
    while (valExp.Remove ( 1 )) { }

    Assert.IsTrue ( refC.Remove ( (object) 4 ) );
    Assert.IsTrue ( withRefC.Remove ( (VWR) 4 ) );
    Assert.IsTrue ( valC.Remove ( 4L ) );

    _ = refExp.Remove ( 4 );
    _ = withRefExp.Remove ( 4 );
    _ = valExp.Remove ( 4 );

    Assert.IsFalse ( refC.Remove ( (object) 6 ) );
    Assert.IsFalse ( withRefC.Remove ( (VWR) 6 ) );
    Assert.IsFalse ( valC.Remove ( 6L ) );

    Assert.HasCount ( refExp.Count, refC );
    Assert.HasCount ( withRefExp.Count, withRefC );
    Assert.HasCount ( valExp.Count, valC );

    Assert.IsTrue ( refExp.SequenceEqual ( refC ) );
    Assert.IsTrue ( withRefExp.SequenceEqual ( withRefC ) );
    Assert.IsTrue ( valExp.SequenceEqual ( valC ) );

    Assert.AreEqual ( null, refC.store [ 5 ] );
    Assert.AreEqual ( true, withRefC.store [ 5 ].IsUninitialized () );
    Assert.AreEqual ( valSource [ 5 ], valC.store [ 5 ] );

    Assert.IsTrue ( ArrSeg ( refSource, 6, 2 ).SequenceEqual ( ArrSeg ( refC.store, 6, 2 ) ) );
    Assert.IsTrue ( ArrSeg ( withRefSource, 6, 2 ).SequenceEqual ( ArrSeg ( withRefC.store, 6, 2 ) ) );
    Assert.IsTrue ( ArrSeg ( valSource, 6, 2 ).SequenceEqual ( ArrSeg ( valC.store, 6, 2 ) ) );
  }

  [TestMethod]
  [DataRow ( 5 )]
  [DataRow ( 3 )]
  [DataRow ( 0 )]
  public void Remove_Index ( int index )
  {
    int[] source = [ 1, 2, 3,  1, 2, 4,  6, 7 ];

    object[] refSource = source.Cast<object>().ToArray();
    VWR[] withRefSource = source.Select(x => (VWR)x).ToArray();
    long[] valSource = source.Select(x => (long)x).ToArray();

    Capacitor<object> refC = new(6, refSource);
    Capacitor<VWR> withRefC = new(6, withRefSource);
    Capacitor<long> valC = new(6, valSource);

    List<object> refExp = refSource.Take(6).ToList();
    List<VWR> withRefExp = withRefSource.Take(6).ToList();
    List<long> valExp = valSource.Take(6).ToList();

    Assert.AreEqual ( refSource [ index ], refC.Remove ( index ) );
    Assert.AreEqual ( withRefSource [ index ], withRefC.Remove ( (NonNegativeInt32) index ) );
    Assert.AreEqual ( valSource [ index ], valC.Remove ( index ) );

    refExp.RemoveAt ( index );
    withRefExp.RemoveAt ( index );
    valExp.RemoveAt ( index );

    Assert.HasCount ( refExp.Count, refC );
    Assert.HasCount ( withRefExp.Count, withRefC );
    Assert.HasCount ( valExp.Count, valC );

    Assert.IsTrue ( refExp.SequenceEqual ( refC ) );
    Assert.IsTrue ( withRefExp.SequenceEqual ( withRefC ) );
    Assert.IsTrue ( valExp.SequenceEqual ( valC ) );

    Assert.AreEqual ( null, refC.store [ 5 ] );
    Assert.AreEqual ( true, withRefC.store [ 5 ].IsUninitialized () );
    Assert.AreEqual ( valSource [ 5 ], valC.store [ 5 ] );

    Assert.IsTrue ( ArrSeg ( refSource, 6, 2 ).SequenceEqual ( ArrSeg ( refC.store, 6, 2 ) ) );
    Assert.IsTrue ( ArrSeg ( withRefSource, 6, 2 ).SequenceEqual ( ArrSeg ( withRefC.store, 6, 2 ) ) );
    Assert.IsTrue ( ArrSeg ( valSource, 6, 2 ).SequenceEqual ( ArrSeg ( valC.store, 6, 2 ) ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Remove_Index_IndexOutOfBounds ( int size )
  {
    Capacitor<long> capacitor = new(new long[size]);
    Action test = () => capacitor.Remove(size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'atIndex')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 1 )]
  [DataRow ( 9, 1 )]
  [DataRow ( 0, 5 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 0, 10 )]
  [DataRow ( 2, 4 )]
  [DataRow ( 2, 6 )]
  public void Remove_OffsetCount_Reference ( int offset, int count )
  {
    object[] source = new int[] { 1,2,3,4,5, 6,7,8,9,10, 11,12 }.Cast<object>().ToArray();

    Capacitor<object> capacitor = new(10, source.ToArray());
    List<object> expectation = source.Take(10).ToList();

    capacitor.Remove ( offset, count );
    expectation.RemoveRange ( offset, count );

    Assert.HasCount ( expectation.Count, capacitor );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );

    bool tailOkay = ArrSeg ( source, 10, 2 ).SequenceEqual ( ArrSeg ( capacitor.store, 10, 2 ) );
    Assert.IsTrue ( tailOkay );
  }

  [TestMethod]
  [DataRow ( 0, 1 )]
  [DataRow ( 9, 1 )]
  [DataRow ( 0, 5 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 0, 10 )]
  [DataRow ( 2, 4 )]
  [DataRow ( 2, 6 )]
  public void Remove_OffsetCount_ContainsReference ( int offset, int count )
  {
    VWR[] source = [ 1,2,3,4,5, 6,7,8,9,10, 11,12 ];

    Capacitor<VWR> capacitor = new(10, source.ToArray());
    List<VWR> expectation = source.Take(10).ToList();

    capacitor.Remove ( offset, count );
    expectation.RemoveRange ( offset, count );

    Assert.HasCount ( expectation.Count, capacitor );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
    Assert.IsTrue ( ArrSeg ( capacitor.store, 10 - count, count ).All ( VWR.IsUninitialized ) );

    bool tailOkay = ArrSeg ( source, 10, 2).SequenceEqual ( ArrSeg ( capacitor.store, 10, 2 ) );
    Assert.IsTrue ( tailOkay );
  }

  [TestMethod]
  [DataRow ( 0, 1 )]
  [DataRow ( 9, 1 )]
  [DataRow ( 0, 5 )]
  [DataRow ( 5, 5 )]
  [DataRow ( 0, 10 )]
  [DataRow ( 2, 4 )]
  [DataRow ( 2, 6 )]
  public void Remove_OffsetCount_Value ( int offset, int count )
  {
    int[] source = [ 1,2,3,4,5, 6,7,8,9,10, 11,12 ];

    Capacitor<int> capacitor = new(10, source.ToArray());
    List<int> expectation = source.Take(10).ToList();

    capacitor.Remove ( offset, count );
    expectation.RemoveRange ( offset, count );

    Assert.HasCount ( expectation.Count, capacitor );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );

    int limit = 10 - count;
    count = 12 - limit;

    bool tailOkay = ArrSeg ( source, limit, count ).SequenceEqual ( ArrSeg ( capacitor.store, limit, count ) );
    Assert.IsTrue ( tailOkay );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void Remove_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    capacitor.Remove ( index, 0 );
  }

  [TestMethod]
  public void Remove_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    capacitor.Remove ( 0, 0 );
  }

  [TestMethod]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 1, 5 )]
  [DataRow ( 5, 4, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void Remove_OffsetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => capacitor.Remove(index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing. (Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

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
  public void Remove_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    capacitor.Remove ( index, count );
  }

  [TestMethod]
  [DataRow ( 4 )]
  [DataRow ( 2 )]
  [DataRow ( 0 )]
  public void RemoveAt_Index ( int index )
  {
    int[] source = [ 1, 2, 3,  1, 2, 4,  6, 7 ];

    object[] refSource = source.Cast<object>().ToArray();
    VWR[] withRefSource = source.Select(x => (VWR)x).ToArray();
    long[] valSource = source.Select(x => (long)x).ToArray();

    Capacitor<object> refC = new(6, refSource);
    Capacitor<VWR> withRefC = new(6, withRefSource);
    Capacitor<long> valC = new(6, valSource);

    List<object> refExp = refSource.Take(6).ToList();
    List<VWR> withRefExp = withRefSource.Take(6).ToList();
    List<long> valExp = valSource.Take(6).ToList();

    refC.RemoveAt ( index );
    withRefC.RemoveAt ( (NonNegativeInt32) index );
    valC.RemoveAt ( index );

    refExp.RemoveAt ( index );
    withRefExp.RemoveAt ( index );
    valExp.RemoveAt ( index );

    Assert.HasCount ( refExp.Count, refC );
    Assert.HasCount ( withRefExp.Count, withRefC );
    Assert.HasCount ( valExp.Count, valC );

    Assert.IsTrue ( refExp.SequenceEqual ( refC ) );
    Assert.IsTrue ( withRefExp.SequenceEqual ( withRefC ) );
    Assert.IsTrue ( valExp.SequenceEqual ( valC ) );

    Assert.AreEqual ( null, refC.store [ 5 ] );
    Assert.AreEqual ( true, withRefC.store [ 5 ].IsUninitialized () );
    Assert.AreEqual ( valSource [ 5 ], valC.store [ 5 ] );

    Assert.IsTrue ( ArrSeg ( refSource, 6, 2 ).SequenceEqual ( ArrSeg ( refC.store, 6, 2 ) ) );
    Assert.IsTrue ( ArrSeg ( withRefSource, 6, 2 ).SequenceEqual ( ArrSeg ( withRefC.store, 6, 2 ) ) );
    Assert.IsTrue ( ArrSeg ( valSource, 6, 2 ).SequenceEqual ( ArrSeg ( valC.store, 6, 2 ) ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void RemoveAt_Index_IndexOutOfBounds ( int size )
  {
    Capacitor<long> capacitor = new(new long[size]);
    Action test = () => capacitor.RemoveAt(size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'index')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void RemoveFirst_Value ()
  {
    int[] source = [ 1,2,3, 4,5,6, 7,8,9 ];
    Capacitor<int> capacitor = new(6, source.ToArray());

    int index = 0;
    int item;
    while (capacitor.RemoveFirst ( out item ))
    {
      Assert.AreEqual ( source [ index++ ], item );
      Assert.AreEqual ( 6 - index, capacitor.Count );
    }

    Assert.AreEqual ( 0, item );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.IsTrue ( ArrSeg ( capacitor.store, 0, 6 ).All ( x => x == 6 ) );
    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  [TestMethod]
  public void RemoveFirst_Reference ()
  {
    object[] source = new int [] {  1,2,3, 4,5,6, 7,8,9 }.Cast<object>().ToArray();
    Capacitor<object> capacitor = new(6, source.ToArray());

    int index = 0;
    object? item;
    while (capacitor.RemoveFirst ( out item ))
    {
      Assert.AreEqual ( source [ index++ ], item );
      Assert.AreEqual ( 6 - index, capacitor.Count );
    }

    Assert.IsNull ( item );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.IsTrue ( ArrSeg ( capacitor.store, 0, 6 ).All ( x => x == null ) );
    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  [TestMethod]
  public void RemoveFirst_ContainsReference ()
  {
    VWR[] source = [ 1,2,3, 4,5,6, 7,8,9 ];
    Capacitor<VWR> capacitor = new(6, source.ToArray());

    int index = 0;
    VWR item;
    while (capacitor.RemoveFirst ( out item ))
    {
      Assert.AreEqual ( source [ index++ ], item );
      Assert.AreEqual ( 6 - index, capacitor.Count );
    }

    Assert.IsFalse ( item.IsInitialized () );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.IsTrue ( ArrSeg ( capacitor.store, 0, 6 ).All ( x => x.IsUninitialized () ) );
    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  [TestMethod]
  public void RemoveLast_Value ()
  {
    int[] source = [ 1,2,3, 4,5,6, 7,8,9 ];
    Capacitor<int> capacitor = new(6, source.ToArray());

    int index = 6;
    int item;
    while (capacitor.RemoveLast ( out item ))
    {
      Assert.AreEqual ( source [ --index ], item );
      Assert.AreEqual ( index, capacitor.Count );
    }

    Assert.AreEqual ( 0, item );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.IsTrue ( source.SequenceEqual ( capacitor.store! ) );
  }

  [TestMethod]
  public void RemoveLast_Reference ()
  {
    object[] source = new int [] {  1,2,3, 4,5,6, 7,8,9 }.Cast<object>().ToArray();
    Capacitor<object> capacitor = new(6, source.ToArray());

    int index = 6;
    object? item;
    while (capacitor.RemoveLast ( out item ))
    {
      Assert.AreEqual ( source [ --index ], item );
      Assert.AreEqual ( index, capacitor.Count );
    }

    Assert.IsNull ( item );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.IsTrue ( ArrSeg ( capacitor.store, 0, 6 ).All ( x => x == null ) );
    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  [TestMethod]
  public void RemoveLast_ContainsReference ()
  {
    VWR[] source = [ 1,2,3, 4,5,6, 7,8,9 ];
    Capacitor<VWR> capacitor = new(6, source.ToArray());

    int index = 6;
    VWR item;
    while (capacitor.RemoveLast ( out item ))
    {
      Assert.AreEqual ( source [ --index ], item );
      Assert.AreEqual ( index, capacitor.Count );
    }

    Assert.IsFalse ( item.IsInitialized () );
    Assert.AreEqual ( 0, capacitor.Count );
    Assert.IsTrue ( ArrSeg ( capacitor.store, 0, 6 ).All ( x => x.IsUninitialized () ) );
    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  static object [] RemoveMatchesData ()
  {
    object[] data =
    [
      ((Predicate<int>)(x => x == 1 || x == 6), 2),
      ((Predicate<int>)(x => x > 1 && x < 6), 4),
      ((Predicate<int>)(x => true), 6),
      ((Predicate<int>)(x => false), 0),
    ];

    return data;
  }

  [TestMethod]
  [DynamicData ( nameof ( RemoveMatchesData ) )]
  public void RemoveMatches ( Predicate<int> match, int count )
  {
    int[] source = [ 1,2,3, 4,5,6, 7,8,9 ];
    Capacitor<int> capacitor = new(6, source.ToArray());

    List<int> expectation = source.Take(6).ToList();

    Assert.AreEqual ( count, capacitor.RemoveMatches ( match ) );

    _ = expectation.RemoveAll ( match );
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );
    Assert.AreEqual ( 6 - count, capacitor.Count );

    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  [TestMethod]
  public void RemoveMatches_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Assert.AreEqual ( 0, capacitor.RemoveMatches ( x => true ) );
  }

  [TestMethod]
  public void RemoveMatches_NullPredicate ()
  {
    Capacitor<int> capacitor = new();
    Action test = () => capacitor.RemoveMatches(null!);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  static object [] RemoveMatchesData_Offset ()
  {
    object[] data =
    [
      ((Predicate<int>)(x => x == 1 || x == 6), 2, 0),
      ((Predicate<int>)(x => x == 1 || x == 6), 1, 1),
      ((Predicate<int>)(x => x == 1 || x == 6), 1, 5),
      ((Predicate<int>)(x => x > 1 && x < 6), 4, 0),
      ((Predicate<int>)(x => true), 6, 0),
      ((Predicate<int>)(x => true), 3, 3),
      ((Predicate<int>)(x => false), 0, 0),
    ];

    return data;
  }

  [TestMethod]
  [DynamicData ( nameof ( RemoveMatchesData_Offset ) )]
  public void RemoveMatches_Offset ( Predicate<int> match, int count, int offset )
  {
    int[] source = [ 1,2,3, 4,5,6, 7,8,9 ];
    Capacitor<int> capacitor = new(6, source.ToArray());

    Assert.AreEqual ( count, capacitor.RemoveMatches ( match, offset ) );

    List<int> expectation = source.Take(6).ToList();
    foreach (int item in ArrSeg ( source, offset, 6 - offset ))
      if (match ( item ))
        _ = expectation.Remove ( item );

    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );

    int expCount = 6 - count;
    Assert.AreEqual ( expCount, capacitor.Count );

    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void RemoveMatches_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<long> capacitor = new(new long[size]);
    Action test = () => capacitor.RemoveMatches(x => default, size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  public void RemoveMatches_Offset_NullPredicate ()
  {
    Capacitor<int> capacitor = new([0]);
    Action test = () => capacitor.RemoveMatches(null!, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [SuppressMessage ( "Style", "IDE0047:Remove unnecessary parentheses", Justification = "Okay." )]
  static object [] RemoveMatchesData_OffsetCount ()
  {
    object[] data =
    [
      ((Predicate<object>)(x => x.Equals(1) || x.Equals(9)), 0, 9, 2),
      ((Predicate<object>)(x => x.Equals(1) || x.Equals(9)), 1, 8, 1),
      ((Predicate<object>)(x => x.Equals(1) || x.Equals(9)), 0, 8, 1),
      ((Predicate<object>)(x => x.Equals(1) || x.Equals(9)), 0, 1, 1),
      ((Predicate<object>)(x => x.Equals(1) || x.Equals(9)), 8, 1, 1),

      ((Predicate<object>)(x => ((int)x > 3 && (int)x < 7) || x.Equals(2) || x.Equals(8)), 0, 9, 5),
      ((Predicate<object>)(x => (int)x>3 && (int)x<7), 0, 9, 3),
      ((Predicate<object>)(x => (int)x>3 && (int)x<7), 3, 3, 3),
      ((Predicate<object>)(x => (int)x>3 && (int)x<7), 4, 2, 2),
      ((Predicate<object>)(x => true), 0, 9, 9),
      ((Predicate<object>)(x => false), 0, 9, 0),
    ];

    return data;
  }

  [TestMethod]
  [DynamicData ( nameof ( RemoveMatchesData_OffsetCount ) )]
  public void RemoveMatches_OffsetCount_Value ( Predicate<object?> match, int offset, int count, int removed )
  {
    object[] source = new int []{ 1,2,3, 4,5,6, 7,8,9, 1,2,3 }.Cast<object>().ToArray();
    Capacitor<object> capacitor = new(9, source.ToArray());

    Assert.AreEqual ( removed, capacitor.RemoveMatches ( match, offset, count ) );

    List<object> expectation = source.Take(9).ToList();

#pragma warning disable IDE0220
    foreach (int item in ArrSeg ( source, offset, count )!)
      if (match ( item ))
        _ = expectation.Remove ( item );
#pragma warning restore IDE0220

    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );

    int expCount = 9 - removed;
    Assert.AreEqual ( expCount, capacitor.Count );

    Assert.IsTrue ( ArrSeg ( source, 9, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 9, 3 ) ) );
    Assert.IsTrue ( ArrSeg ( capacitor.store, expCount, removed ).All ( x => x == null ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void RemoveMatches_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    Assert.AreEqual ( 0, capacitor.RemoveMatches ( x => true, index, 0 ) );
  }

  [TestMethod]
  public void RemoveMatches_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Assert.AreEqual ( 0, capacitor.RemoveMatches ( x => true, 0, 0 ) );
  }

  [TestMethod]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 1, 5 )]
  [DataRow ( 5, 4, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void RemoveMatches_OffsetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () => capacitor.RemoveMatches( x => default, index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing. (Parameters 'offset','count')";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );

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
  public void RemoveMatches_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Assert.AreEqual ( 0, capacitor.RemoveMatches ( x => false, index, count ) );
  }

  [TestMethod]
  public void RemoveMatches_OffsetCount_NullPredicate ()
  {
    Capacitor<int> capacitor = new([0]);
    Action test = () => capacitor.RemoveMatches(null!, 0, 0);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Match predicate must be provided. (Parameter 'match')", e.Message );
  }

  [TestMethod]
  [DataRow ( 10 )]
  [DataRow ( 0 )]
  [DataRow ( null )]
  public void ResetStore ( int? capacity )
  {
    int[] source = [1,2,3,4,5];
    Capacitor<int> capacitor = new(5, source);

    Assert.IsTrue ( ReferenceEquals ( source, capacitor.store ) );

    if (capacity is int)
      capacitor.ResetStore ( capacity.Value );
    else
      capacitor.ResetStore ();

    Assert.IsFalse ( ReferenceEquals ( source, capacitor.store ) );
    Assert.AreEqual ( capacity ?? 0, capacitor.Capacity );

    if ((capacity ?? 0) == 0)
      Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), capacitor.store ) );
  }

  [TestMethod]
  public void Reverse ()
  {
    int[] source = [ 1,2,3, 4,5,6, 7,8,9 ];
    Capacitor<int> capacitor = new(6, source.ToArray());

    capacitor.Reverse ();

    IEnumerable<int> expectation = source.Take(6).Reverse();
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );

    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  [TestMethod]
  public void Reverse_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    capacitor.Reverse ();
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  [DataRow ( 1 )]
  [DataRow ( 3 )]
  public void Reverse_Offset ( int offset )
  {
    int[] source = [ 1,2,3, 4,5,6, 7,8,9 ];
    Capacitor<int> capacitor = new(6, source.ToArray());

    capacitor.Reverse ( offset );

    IEnumerable<int> reversion = source.Skip(offset).Take(6-offset).Reverse();
    IEnumerable<int> expectation = ArrSeg(source, 0, offset).Concat(reversion);
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );

    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 5 )]
  public void Reverse_Offset_IndexOutOfBounds ( int size )
  {
    Capacitor<long> capacitor = new(new long[size]);
    Action test = () => capacitor.Reverse(size);

    IndexOutOfBoundariesException e = Assert.ThrowsExactly<IndexOutOfBoundariesException> ( test );
    string msg = $"For available '{size}' is index '{size}' out of bounds. (Parameter 'offset')";
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( 0, 6 )]
  [DataRow ( 1, 5 )]
  [DataRow ( 0, 5 )]
  [DataRow ( 1, 4 )]
  [DataRow ( 2, 2 )]
  public void Reverse_OffsetCount ( int offset, int count )
  {
    int[] source = [ 1,2,3, 4,5,6, 7,8,9 ];
    Capacitor<int> capacitor = new(6, source.ToArray());

    capacitor.Reverse ( offset, count );

    IEnumerable<int> reversion = source.Skip(offset).Take(count).Reverse();
    ArraySegment<int> head = ArrSeg(source, 0, offset);
    int limit = IndexingValidator.LimitOutOf(offset, count);
    ArraySegment<int> tail = ArrSeg(source, limit, 6 - limit);

    IEnumerable<int> expectation =  head.Concat(reversion).Concat(tail);
    Assert.IsTrue ( expectation.SequenceEqual ( capacitor ) );

    Assert.IsTrue ( ArrSeg ( source, 6, 3 ).SequenceEqual ( ArrSeg ( capacitor.store, 6, 3 ) ) );
  }

  [TestMethod]
  [DataRow ( 0 )]
  [DataRow ( 1 )]
  [DataRow ( 4 )]
  [DataRow ( 5 )]
  public void Reverse_OffsetCount_EmptySegment ( int index )
  {
    Capacitor<int> capacitor = new([1,2,3,4,5]);
    capacitor.Reverse ( index, 0 );
  }

  [TestMethod]
  public void Reverse_OffsetCount_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    capacitor.Reverse ( 0, 0 );
  }

  [TestMethod]
  [DataRow ( 5, 0, 6 )]
  [DataRow ( 5, 6, 0 )]
  [DataRow ( 5, 1, 5 )]
  [DataRow ( 5, 4, 2 )]
  [DataRow ( 5, 7, 7 )]
  [DataRow ( 0, 1, 0 )]
  [DataRow ( 0, 0, 1 )]
  public void Reverse_OffsetCount_InvalidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    Action test = () =>  capacitor.Reverse(index, count);

    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException> ( test );
    string msg = "With available {0}, given offset {1} and count {2} produce out-of indexing.";
    msg = string.Format ( CultureInfo.InvariantCulture, msg, size, index, count );
    msg += " (Parameters 'offset','count')";

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
  public void Reverse_OffsetCount_ValidSegment ( int size, int index, int count )
  {
    Capacitor<int> capacitor = new(new int[size]);
    capacitor.Reverse ( index, count );
  }

  [TestMethod]
  public void ToArray ()
  {
    Capacitor<int> capacitor = new(6, [1,2,3,4,5,6, 7,8,9]);

    int[] test = capacitor.ToArray();
    IListSegment<int> expectation = new(capacitor, 0, 6);
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void ToArray_EmptyCapacitor ()
  {
    Capacitor<int> capacitor = new();
    Assert.IsTrue ( ReferenceEquals ( Array.Empty<int> (), capacitor.ToArray () ) );
  }

  [TestMethod]
  public void IntoArraySegment ()
  {
    int [] source = [1,2,3,4,5,6, 7,8,9];
    Capacitor<int> capacitor = new(6, source);

    ArraySegment<int> test = capacitor.IntoArraySegment(35);

    Assert.IsTrue ( ArrSeg ( source, 0, 6 ).SequenceEqual ( test ) );
    Assert.IsTrue ( ReferenceEquals ( source, test.Array ) );

    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 35, capacitor.Capacity );
    Assert.IsFalse ( ReferenceEquals ( source, capacitor.store ) );
  }

  [TestMethod]
  public void IntoIListSegment ()
  {
    int [] source = [1,2,3,4,5,6, 7,8,9];
    Capacitor<int> capacitor = new(6, source);

    IListSegment<int> test = capacitor.IntoIListSegment(35);

    Assert.IsTrue ( ArrSeg ( source, 0, 6 ).SequenceEqual ( test ) );
    Assert.IsTrue ( ReferenceEquals ( source, test.List ) );

    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 35, capacitor.Capacity );
    Assert.IsFalse ( ReferenceEquals ( source, capacitor.store ) );
  }

  [TestMethod]
  public void IntoIReadOnlyListSegment ()
  {
    int [] source = [1,2,3,4,5,6, 7,8,9];
    Capacitor<int> capacitor = new(6, source);

    IReadOnlyListSegment<int> test = capacitor.IntoIReadOnlyListSegment(35);

    Assert.IsTrue ( ArrSeg ( source, 0, 6 ).SequenceEqual ( test ) );
    Assert.IsTrue ( ReferenceEquals ( source, test.List ) );

    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 35, capacitor.Capacity );
    Assert.IsFalse ( ReferenceEquals ( source, capacitor.store ) );
  }

  [TestMethod]
  public void IntoSpan ()
  {
    int [] source = [1,2,3,4,5,6, 7,8,9];
    Capacitor<int> capacitor = new(6, source);

    Span<int> test = capacitor.IntoSpan(35);
    Assert.IsTrue ( new Span<int> ( source, 0, 6 ).SequenceEqual ( test ) );

    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 35, capacitor.Capacity );
    Assert.IsFalse ( ReferenceEquals ( source, capacitor.store ) );
  }

  [TestMethod]
  public void IntoReadOnlySpan ()
  {
    int [] source = [1,2,3,4,5,6, 7,8,9];
    Capacitor<int> capacitor = new(6, source);

    ReadOnlySpan<int> test = capacitor.IntoReadOnlySpan(35);
    Assert.IsTrue ( new ReadOnlySpan<int> ( source, 0, 6 ).SequenceEqual ( test ) );

    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 35, capacitor.Capacity );
    Assert.IsFalse ( ReferenceEquals ( source, capacitor.store ) );
  }

  [TestMethod]
  public void IntoMemory ()
  {
    int [] source = [1,2,3,4,5,6, 7,8,9];
    Capacitor<int> capacitor = new(6, source);

    Memory<int> test = capacitor.IntoMemory(35);

    Assert.IsTrue ( new Memory<int> ( source, 0, 6 ).Span.SequenceEqual ( test.Span ) );
    Assert.IsTrue ( ReferenceEquals ( source, Reflection.GetNonPublicFieldValue ( test, "_object" ) ) );

    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 35, capacitor.Capacity );
    Assert.IsFalse ( ReferenceEquals ( source, capacitor.store ) );
  }

  [TestMethod]
  public void IntoReadOnlyMemory ()
  {
    int [] source = [1,2,3,4,5,6, 7,8,9];
    Capacitor<int> capacitor = new(6, source);

    ReadOnlyMemory<int> test = capacitor.IntoReadOnlyMemory(35);

    Assert.IsTrue ( new ReadOnlyMemory<int> ( source, 0, 6 ).Span.SequenceEqual ( test.Span ) );
    Assert.IsTrue ( ReferenceEquals ( source, Reflection.GetNonPublicFieldValue ( test, "_object" ) ) );

    Assert.AreEqual ( 0, capacitor.Count );
    Assert.AreEqual ( 35, capacitor.Capacity );
    Assert.IsFalse ( ReferenceEquals ( source, capacitor.store ) );
  }


  // readme

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0058:Expression value is never used", Justification = "Readme style." )]
  [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "Readme style" )]
  public void Capacitor_NoVersioning_Sample ()
  {

    Capacitor<int> capacitor = [ 1, 2, 3, 4, 5 ];
    foreach (int item in capacitor)
    {
      capacitor.Add ( item );
      capacitor.Reverse ();
      capacitor.Add ( item );
    }

    Assert.AreEqual ( "3,2,1,5,1,5,4,3,2,1,1,5,1,2,3", string.Join ( ',', capacitor ) );
  }

  [TestMethod]
  public void Capacitor_GrowFactor_Sample ()
  {
    Capacitor<int> capacitor = new () { GrowFactor = GrowFactor.OneAndHalf, LockGrowFactor = true, };
    Assert.AreEqual ( GrowFactor.OneAndHalf, capacitor.GrowFactor );

    _ = Assert.ThrowsExactly<InvalidOperationException>
    (
      () => _ = new Capacitor<int> () { LockGrowFactor = true, GrowFactor = GrowFactor.Five, }
    );
  }

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0058:Expression value is never used", Justification = "Readme style." )]
  [SuppressMessage ( "Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Readme style." )]
  async public Task Capacitor_InsertWithPreCapacitation_Sample ()
  {
    Capacitor<int> capacitor = new ();

    await capacitor.Add ( Generator ( CancellationToken.None ), roomRequest: 100 );
    Assert.IsTrue ( Enumerable.Range ( 1, 100 ).SequenceEqual ( capacitor ) );
    Assert.AreEqual ( 100, capacitor.Capacity );

    static async IAsyncEnumerable<int> Generator ( [EnumeratorCancellation] CancellationToken token )
    {
      TimeSpan period = TimeSpan.FromMilliseconds(25);
      using PeriodicTimer timer = new (period);
      int loopsCount = 100;

      while (loopsCount-- > 0 && await timer.WaitForNextTickAsync ( token ))
        yield return 99 - loopsCount + 1;
    }
  }
}
