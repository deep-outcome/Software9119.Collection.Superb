using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;
using Software9119.Collection.Superb.Ordering;
using Software9119.Collection.Superb.Segmentation;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Software9119.Collection.Superb.Storing;

/// <summary>
/// <see cref="Capacitor{T}"/> base class mostly hold static logic.
/// </summary>
[SuppressMessage ( "Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Inherited." )]
public class Capacitor
{
  /// <summary>
  /// Gets <see cref="ArgumentNullException"/> for <see langword="null"/> action.
  /// </summary>
  static protected internal ArgNullExc NullAction ( string action ) => new ( paramName: action, "Action must be provided." );
  /// <summary>
  /// Gets <see cref="ArgumentNullException"/> for <see langword="null"/> comparer.
  /// </summary>
  static protected internal ArgNullExc NullComparer ( string comparer ) => new ( paramName: comparer, "Comparer must be provided." );
  /// <summary>
  /// Gets <see cref="ArgumentNullException"/> for <see langword="null"/> convertor.
  /// </summary>
  static protected internal ArgNullExc NullConverter ( string converter ) => new ( paramName: converter, "Converter must be provided." );
  /// <summary>
  /// Gets <see cref="ArgumentNullException"/> for <see langword="null"/> match predicate.
  /// </summary>
  static protected internal ArgNullExc NullMatchPredicate ( string match ) => new ( paramName: match, "Match predicate must be provided." );
  /// <summary>
  /// Gets <see cref="ArgumentNullException"/> for <see langword="null"/> target array.
  /// </summary>
  static protected internal ArgNullExc NullTargetArray ( string array ) => new ( paramName: array, "Target array must be provided." );

  /// <summary>
  /// Gets <see cref="ArgumentOutOfRangeException"/> for insufficient array.
  /// </summary>
  static protected internal ArgOutOfRanExc InsufficientTargetArray ( string array, int length, int count )
    => new ( paramName: array, $"Insufficient target array size, available length {length} cannot accomodate {count} items." );

  /// <summary>
  /// Gets <see cref="InvalidOperationException"/> for unsupported validation result.
  /// </summary>
  static protected internal InvalidOperationException UnsupportedValidationResult ( int validation )
    => new ( $"Unsupported validation result, '{validation}'." );


  static readonly internal SegmentationParamNames OffsetCountParamNames = new (offset: "offset", "count", null);
  static readonly internal SegmentationParamNames FromIndexCountParamNames = new (offset: "fromIndex", "count", null);
  static readonly internal string[] RearSetCountParamNames = ["rearSet", "count"];

  /// <summary>
  /// Parameter names getter for: 'offset' and 'count'.
  /// </summary>
  static protected internal ParamNames<SegmentationParamNames> OffsetCountParametersGetter => () => OffsetCountParamNames;

  /// <summary>
  /// Parameter names getter for: 'fromIndex' and 'count'.
  /// </summary>
  static protected internal ParamNames<SegmentationParamNames> FromIndexCountParametersGetter => () => FromIndexCountParamNames;

  /// <summary>
  /// Parameter names getter for: 'rearSet' and 'count'.
  /// </summary>
  static protected internal Func<string []> RearSetCountParametersGetter => () => RearSetCountParamNames;

  /// <summary>
  /// Computes <paramref name="forCount"/> and <paramref name="fromIndex"/> difference.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static protected internal int AvailableCount ( int fromIndex, int forCount )
    => IndexingValidator.IndexToCountInclusiveDifference ( fromIndex, forCount );

  /// <summary>
  /// Creates store with capacity of <paramref name="capacity"/>.
  /// </summary>
  static protected internal T [] GetStoreWithCapacity<T> ( int capacity ) => capacity == 0 ? Array.Empty<T> () : new T [ capacity ];

  /// <summary>
  /// Empty <see cref="Capacitor{T}"/>
  /// </summary>
  static public Capacitor<T> Empty<T> () => new ();

  /// <summary>
  /// Validates <paramref name="index"/> is not negative and valid for target <paramref name="count"/>.
  /// </summary>
  static protected internal bool ValidateIndex
  (
    int index, NonNegativeInt32 count,
    [NotNullWhen ( true )] out IndexOutOfBoundariesException? e,
    string paramName
  )
    => IndexingValidator.ValidateIndex ( index, count, out e, paramName );

  /// <summary>
  /// Validates segmentation possibility over available.
  /// </summary>
  static protected internal int ValidateSegmentation
  (
    NonNegativeInt32 available, NonNegativeInt32 offset, NonNegativeInt32 count,
    out int limit,
    out ImpossibleSegmentationException? e,
    ParamNames<SegmentationParamNames> paramNames
  )
    => IxValidator.ValidateSegmentation ( available, offset, count, out limit, out e, paramNames );
}

/// <summary>
/// Auto-expanding data storing structure designed specifically for managing internal store capacity with ease:
/// <list type="bullet">
/// <item>Uses Batch Capacitation for batch storing operations when source length can be determined.</item>
/// <item>Exposes various Pre Capacitation options for client code.</item>
/// <item>Auto Capacitation operates whenever is needed, see also <see cref="GrowFactor"/>.</item>
/// <item>Features classic <see cref="Array"/> and <see cref="List{T}"/> function and more.</item>
/// <item>Open for user extension.</item>
/// </list>
/// </summary>
/// <remarks>
/// Note 4 types of capacitations for clarity:
/// <list type="number">
/// <item>
/// Pre Capacitation – means capacity is extended via capacity room request.
/// </item>
/// <item>
/// Batch Capacitation – means, if items count to be stored is obtainable, capacity is ensured exactly to suffice such count.
/// </item>
/// <item>
/// Auto Capacitation – means auto-grow logic and it is used whenever capacity is insufficient for store operation.
/// </item>
/// <item>
/// Exact Capacitation – on demand capacitation, not participating in storing operations.
/// </item>
/// </list>
/// </remarks>
[SuppressMessage ( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "No." )]
[SuppressMessage ( "Design", "CA1051:Do not declare visible instance fields", Justification = "Inheritance design." )]
[DebuggerDisplay ( "(Count={Count},Capacity={Capacity})" )]
public class Capacitor<T> : Capacitor,
  IEnumerable, IEnumerable<T?>,
  ICollection<T?>, IReadOnlyCollection<T?>,
  IList<T?>, IReadOnlyList<T?>,
  IEquatable<Capacitor<T>>, ICloneable
{
  /// <summary>
  /// Constructor allowing to 'start' <see cref="Capacitor{T}"/> with pre-created internal store.
  /// </summary>
  /// <param name="count">How much of <paramref name="store"/> is considered populated.</param>
  /// <param name="store">Defaults to <see cref="Array.Empty{T}"/> when <see langword="null"/>.</param>
  /// <remarks>
  /// <paramref name="count"/> greater to <paramref name="store"/> length is adjusted to fit it.
  /// </remarks>
  protected internal Capacitor ( NonNegativeInt32 count, T? []? store )
  {
    store ??= Array.Empty<T?> ();
    this.store = store;

    int length = store.Length;
    storeIndex = Math.Min ( length, count );
  }

  /// <summary>
  /// Default constructor.
  /// </summary>
  public Capacitor () : this ( null, 0 ) { }

  /// <summary>
  /// Constructor with initial capacity.
  /// </summary>
  public Capacitor ( NonNegativeInt32 capacity ) : this ( null, capacity ) { }

  /// <summary>
  /// Constructor with initial items.
  /// </summary>
  /// <remarks>
  /// Stores <paramref name="items"/> using Batch Capacitation or using Auto Capacitation,
  /// based on possibility to obtain <paramref name="items"/> count.
  /// </remarks>
  public Capacitor ( IEnumerable<T?>? items ) : this ( items, 0 ) { }

  /// <summary>
  /// Constructor with initial capacity and initial items.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Sets store capacity to <paramref name="capacity"/> and then stores <paramref name="items"/>.</item>
  /// <item>
  /// For eventual next capacitation uses Batch Capacitation or Auto Capacitation, based on possibility to obtain <paramref name="items"/> count.
  /// </item>
  /// </list>
  /// </remarks>
  public Capacitor ( IEnumerable<T?>? items, NonNegativeInt32 capacity )
  {
    SetStoreWithCapacity ( capacity );
    _ = Add ( items, 0 );
  }

  /// <summary>
  /// Internal store.
  /// </summary>
  protected internal T? [] store;

  /// <summary>
  /// Internal store write index.
  /// </summary>
  protected internal int storeIndex;

  /// <summary>
  /// Minimal capacity used for store.
  /// </summary>
  protected internal const int  defaultCapacity = 4;

  /// <summary>
  /// <see cref="GrowFactor"/> backing field.
  /// </summary>
  protected internal GrowFactor growFactor = GrowFactor.Two;

  /// <remarks>
  /// Items stored at indexes greater or equal to <see cref="Count"/> should be set to default when <see langword="true"/>.
  /// </remarks>
  readonly protected internal bool itemShouldDefault = RuntimeHelpers.IsReferenceOrContainsReferences<T>();

  /// <summary>
  /// Creates offset with current <see cref="Count"/>;
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  protected internal AddInsertOffset AddInsOffset ( int offset ) => AddInsertOffset.CreateUsingCount ( offset, Count );

  /// <remarks>
  /// Internals for <see cref="Insert(int, T?)"/>.
  /// </remarks>
  protected internal void AddInsert ( AddInsertOffset offset, T? item )
  {
    DevValidateAddInsOffset ( offset );

    _ = AutoGrow ();

    if (offset.inserting)
      ShiftItemsToRight ( offset, 1 );

    store [ offset ] = item;
    ++Count;
  }

  /// <remarks>
  /// Internals for:
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, IEnumerable{T?}?, NonNegativeInt32)"/></item>
  /// <item><see cref="Add(IEnumerable{T?}?, NonNegativeInt32)"/></item>
  /// </list>
  /// </remarks>
  protected internal bool AddInsert ( AddInsertOffset offset, IEnumerable<T?>? items, NonNegativeInt32 roomRequest )
  {
    DevValidateAddInsOffset ( offset );

    if (items == null)
      return false;

    if (items is T? [] a)
      return AddInsert ( offset, a );

    if (items is ICollection<T?> c)
      return AddInsert ( offset, c );

    if (items is IReadOnlyCollection<T?> rc)
      return AddInsert ( offset, rc );

    _ = CapacitateForNext ( roomRequest );

    T?[]? tail = BackUpTail(offset);

    foreach (T? i in items)
      Add ( i );

    if (tail is not null)
      _ = Add ( tail );

    return true;
  }

  /// <remarks>
  /// Internals for:
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, IAsyncEnumerable{T?}?, NonNegativeInt32)"/></item>
  /// <item><see cref="Add(IAsyncEnumerable{T?}?, NonNegativeInt32)"/></item>
  /// </list>
  /// </remarks>
  async protected internal Task<bool> AddInsert ( AddInsertOffset offset, IAsyncEnumerable<T?>? items, NonNegativeInt32 roomRequest )
  {
    DevValidateAddInsOffset ( offset );

    if (items == null)
      return false;

    _ = CapacitateForNext ( roomRequest );

    T?[]? tail = BackUpTail(offset);

    await foreach (T? i in items.ConfigureAwait ( false ))
      Add ( i );

    if (tail is not null)
      _ = Add ( tail );

    return true;
  }

  /// <remarks>
  /// Internals for:
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, T?[])"/></item>
  /// <item><see cref="Add(T?[])"/></item>
  /// </list>
  /// </remarks>
  protected internal bool AddInsert ( AddInsertOffset offset, T? []? items )
  {
    DevValidateAddInsOffset ( offset );

    if (items == null)
      return false;

    int itemsLength = items.Length;
    if (PrepareStoreForAddIns ( offset, itemsLength ))
    {
      Array.Copy ( items, 0, store, offset, itemsLength );
      Count += itemsLength;
    }

    return true;
  }

  /// <remarks>
  /// Internals for:
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, ICollection{T?}?)"/></item>
  /// <item><see cref="Add(ICollection{T?}?)"/></item>
  /// </list>
  /// </remarks>
  protected internal bool AddInsert ( AddInsertOffset offset, ICollection<T?>? items )
  {
    DevValidateAddInsOffset ( offset );

    if (items == null)
      return false;

    int itemsCount = items.Count;
    if (PrepareStoreForAddIns ( offset, itemsCount ))
    {
      items.CopyTo ( store, offset );
      Count += itemsCount;
    }

    return true;
  }

  /// <remarks>
  /// Internals for:
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, IReadOnlyCollection{T?}?)"/></item>
  /// <item><see cref="Add(IReadOnlyCollection{T?}?)"/></item>
  /// </list>
  /// </remarks>
  protected internal bool AddInsert ( AddInsertOffset offset, IReadOnlyCollection<T?>? items )
  {
    DevValidateAddInsOffset ( offset );

    if (items == null)
      return false;

    if (items is T? [] a)
      return AddInsert ( offset, a );

    if (items is ICollection<T?> c)
      return AddInsert ( offset, c );

    int itemsCount = items.Count;
    if (PrepareStoreForAddIns ( offset, itemsCount ))
    {
      int index = offset.value;
      foreach (T? i in items)
        store [ index++ ] = i;

      Count += itemsCount;
    }

    return true;
  }

  /// <summary>
  /// Invokes auto-grow using <see cref="GrowFactor"/> when store is fully utilized.
  /// </summary>
  /// <returns><see langword="true"/> when capacity is changed.</returns>
  protected internal bool AutoGrow ()
  {
    if (IsFull)
    {

      int capacity = Capacity;
      if (capacity == 0)
      {
        store = new T? [ defaultCapacity ];
        return true;
      }

      _ = growFactor.ToFloat ( out float factor );
      int newSize = (int)(capacity * factor );
      Capacitate ( newSize );
      return true;
    }

    return false;
  }

  /// <summary>
  /// Copies all stored items from <paramref name="offset"/> to backup array.
  /// </summary>
  protected internal T? []? BackUpTail ( AddInsertOffset offset )
  {
    DevValidateAddInsOffset ( offset );

    T? []? tail = null;
    if (offset)
    {
      int tailSize = ItemsCountToEndInclusive(offset);
      tail = new T? [ tailSize ];

      Array.Copy ( store, offset, tail, 0, tailSize );
      Count = offset;
    }

    return tail;
  }

  /// <summary>
  /// Resizes store to new capacity.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  protected internal void Capacitate ( int to ) => Array.Resize ( ref store, to );

  /// <summary>
  /// Creates clone of this instance but with <paramref name="store"/> and <paramref name="count"/>.
  /// </summary>
  protected internal Capacitor<U> CloneWithStoreAndCount<U> ( U? [] store, int count )
  {
    Capacitor<U> clone = new (count, store)
    {
      growFactor = growFactor,
      LockGrowFactor = LockGrowFactor,
    };

    return clone;
  }

  /// <summary>
  /// Index validation existing only in debug compilation.
  /// </summary>
  /// <remarks>
  /// Intended for internal methods.
  /// </remarks>
  [Conditional ( "DEV_VALS" )]
  protected internal void DevValidateIndex ( int index, [CallerArgumentExpression ( nameof ( index ) )] string? param = null )
  {
    if (ValidateIndex ( index, out IndexOutOfBoundariesException? e, param! ))
      throw e;
  }

  /// <summary>
  /// Index validation existing only in debug compilation.
  /// </summary>
  /// <remarks>
  /// Intended for internal methods.
  /// </remarks>
  [Conditional ( "DEV_VALS" )]
  protected internal void DevValidateAddInsOffset ( AddInsertOffset offset,
    [CallerArgumentExpression ( nameof ( offset ) )] string? param = null )
  {
    if (ValidateInsertionIndex ( offset.value, out IndexOutOfBoundariesException? e, param! ))
      throw e;

    if (offset == false && Count != offset.value)
      throw new InvalidOperationException ( "Addition offset misuse." );
  }

  /// <summary>
  /// Index validation existing only in debug compilation.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Intended for internal methods.</item>
  /// <item>Permissive towards empty moves.</item>
  /// </list>
  /// </remarks>
  [Conditional ( "DEV_VALS" )]
  protected internal void DevValidateRightShift ( int index, int count )
  {
    if (IxValidator.ValidateIndex ( index, Capacity, out IndexOutOfBoundariesException? e1 ))
      throw e1;

    if (FreeCapacity < count)
      throw new InvalidOperationException ( "Shift is possible only up to free capacity." );
  }

  /// <summary>
  /// For valid index, it populates <paramref name="item"/> with relevant item.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="index"/> is <c>-1</c>.</returns>
  protected internal bool ItemAtIndex ( int index, out T? item )
  {
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    DevValidateIndex ( index, nameof ( index ) );

    item = store [ index ];
    return true;
  }

  /// <returns>Count of stored items starting at <paramref name="fromIndex"/>.</returns>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  protected internal int ItemsCountToEndInclusive ( int fromIndex ) => IxValidator.IndexToCountInclusiveDifference ( fromIndex, Count );

  /// <summary>
  /// Prepares store for insertion or addition of items, if needed.
  /// </summary>
  /// <returns><see langword="true"/> when <paramref name="itemsCount"/> is greater than <c>0</c>.</returns>
  protected internal bool PrepareStoreForAddIns ( AddInsertOffset offset, int itemsCount )
  {
    DevValidateAddInsOffset ( offset );

    // mainly, prevents useless shift call
    if (itemsCount == 0)
      return false;

    _ = CapacitateForNext ( itemsCount );
    if (offset)
      ShiftItemsToRight ( offset, itemsCount );

    return true;
  }

  /// <summary>
  /// Removes item at <paramref name="atIndex"/> index from store and returns it.
  /// </summary>
  protected internal T? RemoveAndReturn ( int atIndex )
  {
    DevValidateIndex ( atIndex );

    T? [] store = this.store;
    T? item = store [ atIndex ];

    int sourceIndex = atIndex + 1;
    int itemsToEnd = ItemsCountToEndInclusive(sourceIndex);
    if (itemsToEnd != 0)
      Array.Copy ( store, sourceIndex, store, atIndex, itemsToEnd );

    int count = Count;
    --count;
    if (itemShouldDefault)
      store [ count ] = default;

    Count = count;
    return item;
  }

  /// <summary>
  /// Initializes store with capacity of <paramref name="capacity"/>.
  /// </summary>
  [MemberNotNull ( nameof ( store ) )]
  protected internal void SetStoreWithCapacity ( int capacity ) => store = GetStoreWithCapacity<T> ( capacity );

  /// <summary>
  /// Shifts stored items from <paramref name="from"/> by <paramref name="byPositions"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  protected internal void ShiftItemsToRight ( int from, int byPositions )
  {
    DevValidateRightShift ( from, byPositions );

    int count = ItemsCountToEndInclusive(from);
    Array.Copy ( store, from, store, from + byPositions, count );
  }

  /// <summary>
  /// Validates index for usage with internal store.
  /// </summary>
  protected internal bool ValidateIndex ( int index, [NotNullWhen ( true )] out IndexOutOfBoundariesException? e, string paramName )
    => IndexingValidator.ValidateIndex ( index, Count, out e, paramName );

  /// <summary>
  /// Validates index for usage with internal store.
  /// </summary>
  protected internal bool ValidateIndex ( NonNegativeInt32 index, [NotNullWhen ( true )] out IndexOutOfBoundariesException? e, string paramName )
    => IndexingValidator.ValidateIndex ( index, Count, out e, paramName );

  /// <summary>
  /// Validates insertion index for usage with internal store.
  /// </summary>
  protected internal bool ValidateInsertionIndex ( int index, [NotNullWhen ( true )] out IndexOutOfBoundariesException? e, string paramName )
    => IndexingValidator.ValidateInsertionIndex ( index, Count, out e, paramName );

  /// <summary>
  /// Validates backward directed segmentation, i.e. from <paramref name="rearSet"/> by <paramref name="count"/> back to start.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0047:Remove unnecessary parentheses", Justification = "" )]
  protected internal int ValidateRearSetConfiguration (
    NonNegativeInt32 rearSet,
    NonNegativeInt32 count,
    out ImpSegExc? e,
    Func<string []> parametersGetter
  ) => IxValidator.ValidateBackwardSegmentation ( Count, rearSet, count, out e, parametersGetter );

  /// <summary>
  /// Validates segmentation possibility.
  /// </summary>
  protected internal int ValidateSegmentation
  (
    NonNegativeInt32 offset, NonNegativeInt32 count,
    out int limit,
    out ImpossibleSegmentationException? e,
    ParamNames<SegmentationParamNames> paramNames
  )
    => IxValidator.ValidateSegmentation ( Count, offset, count, out limit, out e, paramNames );

  /// <summary>
  /// Zero based-indexer.
  /// </summary>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="index"/> is out of capacitor bounds.</exception>
  /// <remarks>Limited by <see cref="Count"/>, not <see cref="Capacity"/>.</remarks>
  [SuppressMessage ( "Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Not help." )]
  public T? this [ int index ]
  {
    get
    {
      if (ValidateIndex ( index, out IndexOutOfBoundariesException? e, nameof ( index ) ))
        throw e;

      return store [ index ];
    }
    set
    {
      if (ValidateIndex ( index, out IndexOutOfBoundariesException? e, nameof ( index ) ))
        throw e;

      store [ index ] = value;
    }
  }

  /// <summary>
  /// Current storage capacity.
  /// </summary>
  public int Capacity
  {
    [MethodImpl ( MethodImplOptions.AggressiveInlining )]
    get => store.Length;
  }

  /// <summary>
  /// Actual count of items stored.
  /// </summary>
  public int Count
  {
    [MethodImpl ( MethodImplOptions.AggressiveInlining )]
    get => storeIndex;
    [MethodImpl ( MethodImplOptions.AggressiveInlining )]
    private set => storeIndex = value;
  }

  /// <summary>
  /// Currently free capacity.
  /// </summary>
  public int FreeCapacity
  {
    [MethodImpl ( MethodImplOptions.AggressiveInlining )]
    get => Capacity - Count;
  }

  /// <summary>
  /// Storage grow factor used for its auto-expansion, default is <see cref="GrowFactor.Two"/>.
  /// </summary>
  /// <exception cref="ArgumentOutOfRangeException">Upon try to set invalid <see cref="Storing.GrowFactor"/>.</exception>
  /// <exception cref="InvalidOperationException">Upon try to set value when <see cref="LockGrowFactor"/> is <see langword="true"/>.</exception>
  public GrowFactor GrowFactor
  {
    get => growFactor;
    set
    {
      if (LockGrowFactor)
        throw new InvalidOperationException ( "Grow factor is locked." );

      if (value.ToFloat ( out _ ) == false)
        throw new ArgumentOutOfRangeException ( paramName: nameof ( value ), $"Unsupported grow factor, '{value}'." );

      growFactor = value;
    }
  }

  /// <summary>
  /// Store is full when <see cref="Capacity"/> is fully utilized.
  /// </summary>
  public bool IsFull
  {
    [MethodImpl ( MethodImplOptions.AggressiveInlining )]
    get => Capacity == Count;
  }

  /// <summary>
  /// <see cref="Capacitor{T}"/> is not read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Expresses intent to freeze initial grow factor value, see <see cref="GrowFactor"/>.
  /// </summary>
  public bool LockGrowFactor { get; init; }

  /// <summary>
  /// Stores <paramref name="item"/> using Auto Capacitation.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  public void Add ( T? item )
  {
    _ = AutoGrow ();
    store [ storeIndex++ ] = item;
  }

  /// <summary>
  /// <list type="bullet">
  /// <item>Stores <paramref name="items"/> using Batch Capacitation or using Pre Capacitation and Auto Capacitation.</item>
  /// <item>
  /// <paramref name="roomRequest"/> applies only when exact count of <paramref name="items"/> cannot be determined
  /// and ensures store capacity for <paramref name="roomRequest"/> more items.
  /// </item>
  /// </list>
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Use <c>0</c> for <paramref name="roomRequest"/> for no room request.</item>
  /// <item>No capacitation happens when <see langword="false"/> is returned.</item>
  /// </list>
  /// </remarks>
  public bool Add ( IEnumerable<T?>? items, NonNegativeInt32 roomRequest )
    => AddInsert ( AddInsOffset ( Count ), items, roomRequest );

  /// <summary>
  /// Stores <paramref name="items"/> using Pre Capacitation and Auto Capacitation.
  /// </summary>
  /// <returns>Async <see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Use <c>0</c> for <paramref name="roomRequest"/> for no room request.</item>
  /// <item>No capacitation happens when <see langword="false"/> is returned.</item>
  /// </list>
  /// </remarks>
  async public Task<bool> Add ( IAsyncEnumerable<T?>? items, NonNegativeInt32 roomRequest )
    => await AddInsert ( AddInsOffset ( Count ), items, roomRequest ).ConfigureAwait ( false );

  /// <summary>
  /// Stores <paramref name="items"/> using Batch Capacitation.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  public bool Add ( T? []? items ) => AddInsert ( AddInsOffset ( Count ), items );

  /// <summary>
  /// Stores <paramref name="items"/> using Batch Capacitation.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  public bool Add ( ICollection<T?>? items ) => AddInsert ( AddInsOffset ( Count ), items );

  /// <summary>
  /// Stores <paramref name="items"/> using Batch Capacitation.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  public bool Add ( IReadOnlyCollection<T?>? items ) => AddInsert ( AddInsOffset ( Count ), items );

  /// <summary>
  /// Verifies all stored items against <paramref name="match"/> predicate.
  /// </summary>
  /// <returns>
  /// <list type="bullet">
  /// <item><c>0</c> when not all items conform <paramref name="match"/> predicate.</item>
  /// <item><c>1</c> when all items conform <paramref name="match"/> predicate.</item>
  /// <item><c>-1</c> when store is empty.</item>
  /// </list>
  /// </returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int AllMatches ( Predicate<T?> match ) => AllMatches ( match, 0, Count );

  /// <summary>
  /// Verifies all stored items, starting at <paramref name="offset"/> specified against <paramref name="match"/> predicate.
  /// </summary>
  /// <returns>
  /// <see langword="true"/> when all items conform <paramref name="match"/> predicate.
  /// </returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool AllMatches ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return AllMatches ( match, offset, ItemsCountToEndInclusive ( offset ) ) == 1;
  }

  /// <summary>
  /// Verifies all stored items in segment specified by <paramref name="count"/> and <paramref name="offset"/>
  /// against <paramref name="match"/> predicate.
  /// </summary>
  /// <returns>
  /// <list type="bullet">
  /// <item><c>0</c> when not all items conform <paramref name="match"/> predicate.</item>
  /// <item><c>1</c> when all items conform <paramref name="match"/> predicate.</item>
  /// <item><c>-1</c> when segment (or store) is empty.</item>
  /// </list>
  /// </returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int AllMatches ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation(offset, count, out int limit, out ImpSegExc? e, OffsetCountParametersGetter);
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    T? [] store = this.store;
    int index = offset;

    while (index < limit)
      if (match ( store [ index++ ] ) == false)
        return 0;

    return 1;
  }

  /// <summary>
  /// Fast search on ordered store.
  /// </summary>
  /// <returns>Item index, or negative number if not found.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  public int BinarySearch ( T? value, IComparer<T?> comparer )
  {
    if (comparer == null)
      throw NullComparer ( nameof ( comparer ) );

    return Array.BinarySearch ( store, 0, Count, value, comparer );
  }

  /// <summary>
  /// Fast search on ordered store, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns>Item index, or negative number if not found.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  public int BinarySearch ( T? value, NonNegativeInt32 offset, IComparer<T?> comparer )
  {
    if (comparer == null)
      throw NullComparer ( nameof ( comparer ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return Array.BinarySearch ( store, offset, ItemsCountToEndInclusive ( offset ), value, comparer );
  }

  /// <summary>
  /// Fast search on ordered store
  /// in segment specified by <paramref name="offset"/>
  /// and <paramref name="count"/>.
  /// </summary>
  /// <returns>Item index, or negative number if not found.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  public int BinarySearch ( NonNegativeInt32 offset, NonNegativeInt32 count, T? value, IComparer<T?> comparer )
  {
    if (comparer == null)
      throw NullComparer ( nameof ( comparer ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0:
      case -1: // let binary search return 'correct' negative index
        break;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Array.BinarySearch ( store, offset, count, value, comparer );
  }

  /// <summary>
  /// Changes storage capacity exactly to capacity specified by <paramref name="to"/>, unless
  /// <paramref name="to"/> is less than <see cref="Count"/>, or equal to
  /// current <see cref="Capacity"/>.
  /// </summary>
  /// <returns><see langword="true"/> when capacity is updated.</returns>
  public bool CapacitateExactly ( NonNegativeInt32 to )
  {
    if (to < Count)
      return false;

    if (to == Capacity)
      return false;

    Capacitate ( to );
    return true;
  }

  /// <summary>
  /// Sets capacity exactly to current <see cref="Count"/>, if not of that size already.
  /// </summary>
  /// <returns><see langword="true"/> when capacity is updated.</returns>
  public bool CapacitateExactlyToCount ()
  {
    if (IsFull)
      return false;

    Capacitate ( Count );
    return true;
  }

  /// <summary>
  /// Ensures storage capacity for <paramref name="roomRequest"/> items.
  /// </summary>
  /// <returns><see langword="true"/> when capacity is updated.</returns>
  /// <remarks>If <paramref name="roomRequest"/> is less or equal to <see cref="FreeCapacity"/>, capacity is not updated.</remarks>
  public bool CapacitateForNext ( NonNegativeInt32 roomRequest )
  {
    if (IsCapacitySufficient ( roomRequest, out int reserve ) == false)
    {
      int capacity =  Capacity - reserve;
      Capacitate ( capacity );

      return true;
    }

    return false;
  }

  /// <summary>
  /// Sets all items stored to <c>default(<typeparamref name="T"/>)</c> and <see cref="Count"/> to <c>0</c>.
  /// </summary>
  public void Clear ()
  {
    int count = Count;
    if (count == 0)
      return;

    Array.Clear ( store, 0, count );
    Count = 0;
  }

  /// <summary>
  /// Sets all items stored, starting at <paramref name="offset"/> specified
  /// to <c>default(<typeparamref name="T"/>)</c> and <see cref="Count"/> to <paramref name="offset"/>.
  /// </summary>
  public void Clear ( NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    Array.Clear ( store, offset, ItemsCountToEndInclusive ( offset ) );
    Count = offset;
  }

  /// <summary>
  /// Sets whole store up to <see cref="Capacity"/> to <c>default(<typeparamref name="T"/>)</c> and <see cref="Count"/> to <c>0</c>.
  /// </summary>
  /// <remarks>
  /// Can be usefull, for instance, before call to <see cref="ExtractStore(bool, NonNegativeInt32)"/>.
  /// </remarks>
  public void ClearToCapacity ()
  {
    int count = Capacity;
    if (count == 0)
      return;

    Array.Clear ( store, 0, count );
    Count = 0;
  }

  /// <summary>
  /// Clones current instance state and items.
  /// </summary>
  /// <remarks>Stored items are shallow-cloned to new store with capacity of <see cref="Count"/>.</remarks>
  public object Clone ()
  {
    int count = Count;
    T?[] store = GetStoreWithCapacity<T>(count);

    if (count != 0)
      Array.Copy ( this.store, 0, store, 0, count );

    Capacitor<T> clone = CloneWithStoreAndCount(store, count);
    return clone;
  }

  /// <summary>
  /// Clones current instance state and items, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <remarks>
  /// Stored items are shallow-cloned to new internal store with exact capacity to accomodate items from <paramref name="offset"/>.
  /// </remarks>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public Capacitor<T> Clone ( NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    int count = ItemsCountToEndInclusive(offset);
    T? [] store = new T?[count];
    Array.Copy ( this.store, offset, store, 0, count );

    Capacitor<T> segment = CloneWithStoreAndCount(store, count);
    return segment;
  }

  /// <summary>
  /// Clones current instance state and items from
  /// store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  /// <remarks>Stored items are shallow-cloned to new internal store with capacity of <paramref name="count"/>.</remarks>
  public Capacitor<T> Clone ( NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0:
      case -1:
        break;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    T? [] store = GetStoreWithCapacity<T>(count);
    if (0 == validation)
      Array.Copy ( this.store, offset, store, 0, count );

    Capacitor<T> segment = CloneWithStoreAndCount(store, count);
    return segment;
  }

  /// <summary>Verifies <paramref name="item"/> presence among stored items.</summary>
  /// <returns><see langword="true"/> when <paramref name="item"/> is present.</returns>
  public bool Contains ( T? item ) => Array.IndexOf ( store, item, 0, Count ) != -1;

  /// <summary>Verifies <paramref name="item"/> presence among stored items, starting at <paramref name="offset"/> specified.</summary>
  /// <returns><see langword="true"/> when <paramref name="item"/> item is present.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool Contains ( T? item, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return Array.IndexOf ( store, item, offset, ItemsCountToEndInclusive ( offset ) ) != -1;
  }

  /// <summary>
  /// Verifies <paramref name="item"/> presence among stored items
  /// in segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="true"/> when <paramref name="item"/> item is present.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public bool Contains ( T? item, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return false;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Array.IndexOf ( store, item, offset, count ) != -1;
  }

  /// <summary>
  /// Clones this <see cref="Capacitor{T}"/> state similar to <see cref="Clone()"/> but with
  /// items converted to <typeparamref name="To"/> type.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="converter"/> is <see langword="null"/>.</exception>
  public Capacitor<To> Convert<To> ( Converter<T?, To> converter ) => Convert ( converter, 0, Count );

  /// <summary>
  /// Clones this <see cref="Capacitor{T}"/> state similar to <see cref="Clone(NonNegativeInt32)"/> but with
  /// items converted to <typeparamref name="To"/> type.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="converter"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public Capacitor<To> Convert<To> ( Converter<T?, To> converter, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return Convert ( converter, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Clones this <see cref="Capacitor{T}"/> state similar to <see cref="Clone(NonNegativeInt32, NonNegativeInt32)"/> but with
  /// items converted to <typeparamref name="To"/> type.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="converter"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public Capacitor<To> Convert<To> ( Converter<T?, To> converter, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (converter == null)
      throw NullConverter ( nameof ( converter ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0:
      case -1:
        break;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    T?[] store = this.store;
    To[] to = GetStoreWithCapacity<To>(count);

    for (int wi = 0, ri = offset ; wi < count ; ++ri, ++wi)
      to [ wi ] = converter ( store [ ri ] );

    Capacitor<To> clone = CloneWithStoreAndCount(to, count);
    return clone;
  }

  /// <summary>
  /// Copies stored items into targed <paramref name="array"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentOutOfRangeException">When <paramref name="array"/> is of insufficient length.</exception>
  public void CopyTo ( T? [] array )
  {
    if (array == null)
      throw NullTargetArray ( nameof ( array ) );

    int count = Count;
    if (count == 0)
      return;

    int length = array.Length;
    if (length < count)
      throw InsufficientTargetArray ( nameof ( array ), length, count );

    Array.Copy ( store, 0, array, 0, count );
  }

  /// <summary>
  /// Copies stored items, starting at <paramref name="fromIndex"/> specified into targed <paramref name="array"/>
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentOutOfRangeException">When <paramref name="array"/> is of insufficient length.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="fromIndex"/> is greater or equal to <see cref="Count"/>.</exception>
  public void CopyTo ( NonNegativeInt32 fromIndex, T? [] array )
  {
    if (array == null)
      throw NullTargetArray ( nameof ( array ) );

    if (ValidateIndex ( fromIndex, out IndexOutOfBoundariesException? e, nameof ( fromIndex ) ))
      throw e;

    int itemsToEnd = ItemsCountToEndInclusive(fromIndex);
    int length = array.Length;

    if (length < itemsToEnd)
      throw InsufficientTargetArray ( nameof ( array ), length, itemsToEnd );

    Array.Copy ( store, fromIndex, array, 0, itemsToEnd );
  }

  /// <summary>
  /// Copies items from store segment specified by <paramref name="count"/> and <paramref name="fromIndex"/>
  /// into targed <paramref name="array"/>
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentOutOfRangeException">When <paramref name="array"/> is of insufficient length.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="fromIndex"/> create impossible segmentation over store.
  /// </exception>
  public void CopyTo ( NonNegativeInt32 fromIndex, NonNegativeInt32 count, T? [] array )
  {
    if (array == null)
      throw NullTargetArray ( nameof ( array ) );

    int validation = ValidateSegmentation ( fromIndex, count, out _, out ImpSegExc? e, FromIndexCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    int length = array.Length;
    if (length < count)
      throw InsufficientTargetArray ( nameof ( array ), length, count );

    Array.Copy ( store, fromIndex, array, 0, count );
  }

  /// <summary>
  /// Copies stored items into targed <paramref name="array"/>, starting at its <paramref name="arrayIndex"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="arrayIndex"/> is out of <paramref name="array"/> bounds.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  /// When <paramref name="array"/> length from <paramref name="arrayIndex"/> is insufficient.
  /// </exception>
  public void CopyTo ( T? [] array, int arrayIndex )
  {
    if (array == null)
      throw NullTargetArray ( nameof ( array ) );

    int length = array.Length;
    if (ValidateIndex ( arrayIndex, length, out IndexOutOfBoundariesException? e, nameof ( arrayIndex ) ))
      throw e;

    int count = Count;
    if (count == 0)
      return;

    int availableLength = AvailableCount(arrayIndex, length);
    if (availableLength < count)
      throw InsufficientTargetArray ( nameof ( array ), availableLength, count );

    Array.Copy ( store, 0, array, arrayIndex, count );
  }

  /// <summary>
  /// Copies stored items, starting at <paramref name="fromIndex"/> specified
  /// into targed <paramref name="array"/>, starting at its <paramref name="arrayIndex"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">
  /// <list type="bullet">
  /// <item>When <paramref name="arrayIndex"/> is greater or equal to <paramref name="array"/> length.</item>
  /// <item>When <paramref name="fromIndex"/> is greater or equal to <see cref="Count"/>.</item>
  /// </list>
  /// </exception>
  /// <exception cref="ArgumentOutOfRangeException">
  /// When <paramref name="array"/> length from <paramref name="arrayIndex"/> is insufficient.
  /// </exception>
  [SuppressMessage ( "Style", "IDE0018:Inline variable declaration", Justification = "Not this case." )]
  public void CopyTo ( T? [] array, NonNegativeInt32 arrayIndex, NonNegativeInt32 fromIndex )
  {
    IndexOutOfBoundariesException? e;

    if (array == null)
      throw NullTargetArray ( nameof ( array ) );

    int length = array.Length;
    if (ValidateIndex ( arrayIndex, length, out e, nameof ( arrayIndex ) ))
      throw e;

    if (ValidateIndex ( fromIndex, out e, nameof ( fromIndex ) ))
      throw e;

    int count = ItemsCountToEndInclusive(fromIndex);
    int availableLength = AvailableCount(arrayIndex, length);

    if (availableLength < count)
      throw InsufficientTargetArray ( nameof ( array ), availableLength, count );

    Array.Copy ( store, fromIndex, array, arrayIndex, count );
  }

  /// <summary>
  /// Copies stored items from segment specified by <paramref name="count"/> and <paramref name="fromIndex"/>
  /// into targed <paramref name="array"/>, starting at its <paramref name="arrayIndex"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// <list type="bullet">
  /// <item>When <paramref name="fromIndex"/> and <paramref name="count"/> create impossible segmentation over store.</item>
  /// <item>
  /// When <paramref name="arrayIndex"/> and <paramref name="count"/> create impossible segmentation over <paramref name="array"/>.
  /// </item>
  /// </list>
  /// </exception>
  [SuppressMessage ( "Style", "IDE0018:Inline variable declaration", Justification = "Not this case." )]
  public void CopyTo ( T? [] array, NonNegativeInt32 arrayIndex, NonNegativeInt32 fromIndex, NonNegativeInt32 count )
  {
    ImpossibleSegmentationException? e;

    if (array == null)
      throw NullTargetArray ( nameof ( array ) );

    ParamNames<SegmentationParamNames> targetParams = () => new (offset: nameof ( arrayIndex ), count: nameof ( count ), nameof ( array ));
    int validation = ValidateSegmentation (array.Length, arrayIndex, count, out _, out e, targetParams);
    switch (validation)
    {
      case 0:
      case -1: // let validate source segment even having empty target segment
        break;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    validation = ValidateSegmentation ( fromIndex, count, out _, out e, FromIndexCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    Array.Copy ( store, fromIndex, array, arrayIndex, count );
  }

  /// <summary>
  /// Reference equality.
  /// </summary>
  override public bool Equals ( object? obj ) => Equals ( obj as Capacitor<T> );

  /// <summary>
  /// Reference equality.
  /// </summary>
  public bool Equals ( Capacitor<T>? other ) => ReferenceEquals ( this, other );

  /// <summary>
  /// Extracts current store and resets.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="ResetStore(NonNegativeInt32)"/> with <paramref name="resetCapacity"/>.
  /// </remarks>
  public T? [] ExtractStore ( bool trimExcess = true, NonNegativeInt32 resetCapacity = default )
  {
    T? [] extract = store;
    if (trimExcess && !IsFull)
      Array.Resize ( ref extract, Count );

    ResetStore ( resetCapacity );
    return extract;
  }

  /// <summary>
  /// Overwrites all stored items with <paramref name="item"/>.
  /// </summary>
  public void Fill ( T? item ) => Array.Fill ( store, item, 0, Count );

  /// <summary>
  /// Overwrites all stored items with <paramref name="item"/>, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public void Fill ( T? item, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    Array.Fill ( store, item, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Overwrites all items in store segment specified by <paramref name="count"/> and <paramref name="offset"/>
  /// with <paramref name="item"/> .
  /// </summary>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public void Fill ( T? item, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    Array.Fill ( store, item, offset, count );
  }

  /// <summary>
  /// Overwrites whole store with <paramref name="item"/> up to current <see cref="Capacity"/> and
  /// sets <see cref="Count"/> equal capacity.
  /// </summary>
  public void FillToCapacity ( T? item )
  {
    int capacity = Capacity;
    if (capacity == 0)
      return;

    Array.Fill ( store, item );
    Count = capacity;
  }

  /// <summary>
  /// Finds all indexes of stored items matching <paramref name="match"/> predicate.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public IEnumerable<int> FindAllIndexes ( Predicate<T?> match ) => FindAllIndexes ( match, 0, Count );

  /// <summary>
  /// Finds all indexes of stored items matching <paramref name="match"/> predicate, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public IEnumerable<int> FindAllIndexes ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return FindAllIndexes ( match, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Finds all indexes of stored items matching <paramref name="match"/> predicate,
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public IEnumerable<int> FindAllIndexes ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out int limit, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return Enumerable.Empty<int> ();
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Enumerator ();
    IEnumerable<int> Enumerator ()
    {
      T? [] store = this.store;
      int index = offset;

      for (; index < limit ; ++index)
      {
        index = Array.FindIndex ( store, index, limit - index, match );

        if (index == -1)
          yield break;

        yield return index;
      }
    }
  }

  /// <summary>
  /// Finds all stored items matching <paramref name="match"/> predicate.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public IEnumerable<T?> FindAllItems ( Predicate<T?> match ) => FindAllItems ( match, 0, Count );

  /// <summary>
  /// Finds all stored items matching <paramref name="match"/> predicate, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public IEnumerable<T?> FindAllItems ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return FindAllItems ( match, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Finds all stored items matching <paramref name="match"/> predicate,
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public IEnumerable<T?> FindAllItems ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    IEnumerable<int> allIndexes = FindAllIndexes ( match, offset, count );

    return AllItems ();
    IEnumerable<T?> AllItems ()
    {
      T?[] store = this.store;
      foreach (int i in allIndexes)
        yield return store [ i ];
    }
  }

  /// <summary>
  /// Finds index of first item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int FindFirstIndex ( Predicate<T?> match ) => FindFirstIndex ( match, 0, Count );

  /// <summary>
  /// Finds index of first item matching <paramref name="match"/> predicate, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int FindFirstIndex ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return FindFirstIndex ( match, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Finds index of first item matching <paramref name="match"/> predicate,
  /// in store segment specified by <paramref name="offset"/> and <paramref name="count"/>.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int FindFirstIndex ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Array.FindIndex ( store, offset, count, match );
  }

  /// <summary>
  /// Finds first item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindFirstItem ( Predicate<T?> match, out T? item ) => FindFirstItem ( match, 0, Count, out item );

  /// <summary>
  /// Finds first item matching <paramref name="match"/> predicate, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindFirstItem ( Predicate<T?> match, NonNegativeInt32 offset, out T? item )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return FindFirstItem ( match, offset, ItemsCountToEndInclusive ( offset ), out item );
  }

  /// <summary>
  /// Finds first item matching <paramref name="match"/> predicate,
  /// in store segment specified by <paramref name="offset"/> and <paramref name="count"/>.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public bool FindFirstItem ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count, out T? item )
    => ItemAtIndex ( FindFirstIndex ( match, offset, count ), out item );

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int FindLastIndex ( Predicate<T?> match ) => FindLastIndex ( match, 0, Count );

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate,
  /// in store segment from <paramref name="offset"/> to <see cref="Count"/>.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int FindLastIndex ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return FindLastIndex ( match, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate, in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public int FindLastIndex ( NonNegativeInt32 rearSet, Predicate<T?> match )
  {
    if (ValidateIndex ( rearSet, out IndexOutOfBoundariesException? e, nameof ( rearSet ) ))
      throw e;

    return FindLastIndex ( rearSet, rearSet + 1, match );
  }

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate,
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int FindLastIndex ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Array.FindLastIndex ( store, offset + count - 1, count, match );
  }

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate,
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public int FindLastIndex ( NonNegativeInt32 rearSet, NonNegativeInt32 count, Predicate<T?> match )
  {
    if (match == null)
      throw NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateRearSetConfiguration ( rearSet, count, out ImpSegExc? e, RearSetCountParametersGetter);
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Array.FindLastIndex ( store, rearSet, count, match );
  }

  /// <summary>
  /// Finds last item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindLastItem ( Predicate<T?> match, out T? item ) => FindLastItem ( match, 0, Count, out item );

  /// <summary>
  /// Finds last item matching <paramref name="match"/> predicate, in store segment from <paramref name="offset"/> to <see cref="Count"/>.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindLastItem ( Predicate<T?> match, NonNegativeInt32 offset, out T? item )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return FindLastItem ( match, offset, ItemsCountToEndInclusive ( offset ), out item );
  }

  /// <summary>
  /// Finds last item matching <paramref name="match"/> predicate, in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindLastItem ( NonNegativeInt32 rearSet, Predicate<T?> match, out T? item )
  {
    if (ValidateIndex ( rearSet, out IndexOutOfBoundariesException? e, nameof ( rearSet ) ))
      throw e;

    return FindLastItem ( rearSet, rearSet + 1, match, out item );
  }

  /// <summary>
  /// Finds last item matching <paramref name="match"/> predicate, in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public bool FindLastItem ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count, out T? item )
    => ItemAtIndex ( FindLastIndex ( match, offset, count ), out item );

  /// <summary>
  /// Finds item matching <paramref name="match"/> predicate,
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public bool FindLastItem ( NonNegativeInt32 rearSet, NonNegativeInt32 count, Predicate<T?> match, out T? item )
    => ItemAtIndex ( FindLastIndex ( rearSet, count, match ), out item );

  /// <summary>
  /// Finds index of <paramref name="mth"/> last item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int FindMthIndex ( Predicate<T?> match, PositiveInt32 mth )
  {
    int count = Count;
    int offset = count == 0 ? 0 : count - 1;
    return FindMthIndex ( match, mth, offset, count );
  }

  /// <summary>
  /// Finds index of <paramref name="mth"/> last item matching <paramref name="match"/> predicate,
  /// in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public int FindMthIndex ( Predicate<T?> match, PositiveInt32 mth, NonNegativeInt32 rearSet )
  {
    if (ValidateIndex ( rearSet, out IndexOutOfBoundariesException? e, nameof ( rearSet ) ))
      throw e;

    return FindMthIndex ( match, mth, rearSet, rearSet + 1 );
  }

  /// <summary>
  /// Finds index of <paramref name="mth"/> last item matching <paramref name="match"/> predicate,
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public int FindMthIndex ( Predicate<T?> match, PositiveInt32 mth, NonNegativeInt32 rearSet, NonNegativeInt32 count )
  {
    if (match == null)
      throw NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateRearSetConfiguration ( rearSet, count, out ImpSegExc? e, RearSetCountParametersGetter);
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    T? [] store = this.store;

    int index = rearSet;
    int limit = rearSet - count;
    int counter = mth;

    while (index != limit)
    {
      // index -limit =index -rearSet +count =count -(rearSet -index)

      index = Array.FindLastIndex ( store, index, index - limit, match );

      if (index == -1)
        return -1;

      if (--counter == 0)
        return index;

      --index;
    }

    return -1;
  }

  /// <summary>
  /// Finds <paramref name="mth"/> last item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindMthItem ( Predicate<T?> match, PositiveInt32 mth, out T? item ) => ItemAtIndex ( FindMthIndex ( match, mth ), out item );

  /// <summary>
  /// Finds <paramref name="mth"/> last item matching <paramref name="match"/> predicate, in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindMthItem ( Predicate<T?> match, PositiveInt32 mth, NonNegativeInt32 rearSet, out T? item )
    => ItemAtIndex ( FindMthIndex ( match, mth, rearSet ), out item );

  /// <summary>
  /// Finds <paramref name="mth"/> last item matching <paramref name="match"/> predicate,
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public bool FindMthItem ( Predicate<T?> match, PositiveInt32 mth, NonNegativeInt32 rearSet, NonNegativeInt32 count, out T? item )
    => ItemAtIndex ( FindMthIndex ( match, mth, rearSet, count ), out item );

  /// <summary>
  /// Finds index of <paramref name="nth"/> item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int FindNthIndex ( Predicate<T?> match, PositiveInt32 nth ) => FindNthIndex ( match, nth, 0, Count );

  /// <summary>
  /// Finds index of <paramref name="nth"/> item matching <paramref name="match"/> predicate, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int FindNthIndex ( Predicate<T?> match, PositiveInt32 nth, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return FindNthIndex ( match, nth, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Finds index of <paramref name="nth"/> item matching <paramref name="match"/> predicate,
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int FindNthIndex ( Predicate<T?> match, PositiveInt32 nth, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out int limit, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    T? [] store = this.store;

    int index = offset;
    int counter = nth;

    while (index != limit)
    {
      index = Array.FindIndex ( store, index, limit - index, match );

      if (index == -1)
        return -1;

      if (--counter == 0)
        return index;

      ++index;
    }

    return -1;
  }

  /// <summary>
  /// Finds <paramref name="nth"/> item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindNthItem ( Predicate<T?> match, PositiveInt32 nth, out T? item ) => ItemAtIndex ( FindNthIndex ( match, nth ), out item );

  /// <summary>
  /// Finds <paramref name="nth"/> item matching <paramref name="match"/> predicate, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindNthItem ( Predicate<T?> match, PositiveInt32 nth, NonNegativeInt32 offset, out T? item )
    => ItemAtIndex ( FindNthIndex ( match, nth, offset ), out item );

  /// <summary>
  /// Finds <paramref name="nth"/> item matching <paramref name="match"/> predicate,
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public bool FindNthItem ( Predicate<T?> match, PositiveInt32 nth, NonNegativeInt32 offset, NonNegativeInt32 count, out T? item )
    => ItemAtIndex ( FindNthIndex ( match, nth, offset, count ), out item );

  /// <summary>
  /// Verifies predicate match against stored items.
  /// </summary>
  /// <returns><see langword="true"/> on first match, or <see langword="false"/> when no match is found.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindMatch ( Predicate<T?> match ) => FindMatch ( match, 0, Count );

  /// <summary>
  /// Verifies predicate match against stored items, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><see langword="true"/> on first match, or <see langword="false"/> when no match is found.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindMatch ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return FindMatch ( match, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Verifies predicate match against stored items in segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="true"/> on first match, or <see langword="false"/> when no match is found.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public bool FindMatch ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return false;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Array.FindIndex ( store, offset, count, match ) != -1;
  }

  /// <summary>
  /// Runs <paramref name="action"/> on each stored item.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
  public void ForEach ( Action<T?> action ) => ForEach ( action, 0, Count );

  /// <summary>
  /// Runs <paramref name="action"/> on each stored item, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public void ForEach ( Action<T?> action, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    ForEach ( action, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Runs <paramref name="action"/> on each stored item in segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public void ForEach ( Action<T?> action, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (action == null)
      throw NullAction ( nameof ( action ) );

    int validation = ValidateSegmentation ( offset, count, out int limit, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    T? [] store = this.store;
    for (int i = offset ; i < limit ; ++i)
      action ( store [ i ] );
  }

  /// <summary>
  /// Computes stable hash code.
  /// </summary>
  override public int GetHashCode () => HashCode.Combine ( base.GetHashCode () );

  /// <returns>
  /// <see cref="CapacitorStoreEnumerator{T}"/> enumerator.
  /// </returns>
  public CapacitorStoreEnumerator<T> GetEnumerator () => new ( this );

  /// <returns>
  /// <see cref="CapacitorStoreEnumerator{T}"/> enumerator.
  /// </returns>
  IEnumerator<T?> IEnumerable<T?>.GetEnumerator () => GetEnumerator ();

  /// <returns>
  /// <see cref="CapacitorStoreEnumerator{T}"/> enumerator.
  /// </returns>
  IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

  /// <summary>
  /// Finds index of first occurrence of <paramref name="item"/> in store.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  public int IndexOf ( T? item ) => Array.IndexOf ( store, item, 0, Count );

  /// <summary>
  /// Finds index of first occurrence of <paramref name="item"/> in store, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int IndexOf ( T? item, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return Array.IndexOf ( store, item, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Finds index of first occurrence of <paramref name="item"/>
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int IndexOf ( T? item, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Array.IndexOf ( store, item, offset, count );
  }

  /// <summary>
  /// Inserts <paramref name="item"/> at <paramref name="index"/> specified.
  /// </summary>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="index"/> is out of insertion bounds.</exception>
  public void Insert ( int index, T? item )
  {
    if (ValidateInsertionIndex ( index, out IndexOutOfBoundariesException? e, nameof ( index ) ))
      throw e;

    AddInsert ( AddInsOffset ( index ), item );
  }

  /// <summary>
  /// <list type="bullet">
  /// <item>Stores <paramref name="items"/> using Batch Capacitation or using Pre Capacitation and Auto Capacitation.</item>
  /// <item>
  /// <paramref name="roomRequest"/> applies only when exact count of <paramref name="items"/> cannot be determined
  /// and ensures store capacity for <paramref name="roomRequest"/> more items.
  /// </item>
  /// </list>
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Use <c>0</c> for <paramref name="roomRequest"/> for no room request.</item>
  /// <item>No capacitation happens when <see langword="false"/> is returned.</item>
  /// </list>
  /// </remarks>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  public bool Insert ( NonNegativeInt32 offset, IEnumerable<T?>? items, NonNegativeInt32 roomRequest )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return AddInsert ( AddInsOffset ( offset ), items, roomRequest );
  }

  /// <summary>
  /// Stores <paramref name="items"/> using Pre Capacitation and Auto Capacitation.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Use <c>0</c> for <paramref name="roomRequest"/> for no room request.</item>
  /// <item>No capacitation happens when <see langword="false"/> is returned.</item>
  /// </list>
  /// </remarks>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  async public Task<bool> Insert ( NonNegativeInt32 offset, IAsyncEnumerable<T?>? items, NonNegativeInt32 roomRequest )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return await AddInsert ( AddInsOffset ( offset ), items, roomRequest ).ConfigureAwait ( false );
  }

  /// <summary>
  /// Using Batch Capacitation, inserts <paramref name="items"/> into store starting at <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  public bool Insert ( NonNegativeInt32 offset, T? []? items )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return AddInsert ( AddInsOffset ( offset ), items );
  }

  /// <summary>
  /// Using Batch Capacitation, inserts <paramref name="items"/> into store starting at <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  public bool Insert ( NonNegativeInt32 offset, ICollection<T?>? items )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return AddInsert ( AddInsOffset ( offset ), items );
  }

  /// <summary>
  /// Using Batch Capacitation, inserts <paramref name="items"/> into store starting at <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is <see langword="null"/>.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  public bool Insert ( NonNegativeInt32 offset, IReadOnlyCollection<T?>? items )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return AddInsert ( AddInsOffset ( offset ), items );
  }

  /// <summary>
  /// Determines whether current capacity can suffice for <paramref name="forNext"/> next items.
  /// </summary>
  /// <returns><see langword="true"/> if capacity if sufficient.</returns>
  /// <remarks><paramref name="reserve"/> is difference of free capacity and <paramref name="forNext"/>.</remarks>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  public bool IsCapacitySufficient ( NonNegativeInt32 forNext, out int reserve ) => (reserve = FreeCapacity - forNext) > -1;

  /// <summary>
  /// Finds index of last occurrence of <paramref name="item"/> in store.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  public int LastIndexOf ( T? item )
  {
    int count = Count;
    if (count == 0)
      return -1;

    return Array.LastIndexOf ( store, item, count - 1, count );
  }

  /// <summary>
  /// Finds index of last occurrence of <paramref name="item"/> in store, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int LastIndexOf ( T? item, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    int count = Count;
    return Array.LastIndexOf ( store, item, count - 1, count - offset );
  }

  /// <summary>
  /// Finds index of last occurrence of <paramref name="item"/> in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public int LastIndexOf ( NonNegativeInt32 rearSet, T? item )
  {
    if (ValidateIndex ( rearSet, out IndexOutOfBoundariesException? e, nameof ( rearSet ) ))
      throw e;

    return Array.LastIndexOf ( store, item, rearSet, rearSet + 1 );
  }

  /// <summary>
  /// Finds index of last occurrence of <paramref name="item"/>
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int LastIndexOf ( T? item, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e , OffsetCountParametersGetter);
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Array.LastIndexOf ( store, item, offset + count - 1, count );
  }

  /// <summary>
  /// Finds index of last occurrence of <paramref name="item"/>
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public int LastIndexOf ( NonNegativeInt32 rearSet, NonNegativeInt32 count, T? item )
  {
    int validation = ValidateRearSetConfiguration ( rearSet, count, out ImpSegExc? e, RearSetCountParametersGetter);
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    return Array.LastIndexOf ( store, item, rearSet, count );
  }

  /// <summary>
  /// Finds index of <paramref name="mth"/> last <paramref name="item"/> match in store.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  public int MthIndexOf ( T? item, PositiveInt32 mth )
  {
    int count = Count;
    if (count == 0)
      return -1;

    return MthIndexOf ( item, mth, count - 1, count );
  }

  /// <summary>
  /// Finds index of <paramref name="mth"/> last <paramref name="item"/> match in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public int MthIndexOf ( T? item, PositiveInt32 mth, NonNegativeInt32 rearSet )
  {
    if (ValidateIndex ( rearSet, out IndexOutOfBoundariesException? e, nameof ( rearSet ) ))
      throw e;

    return MthIndexOf ( item, mth, rearSet, rearSet + 1 );
  }

  /// <summary>
  /// Finds index of <paramref name="mth"/> last <paramref name="item"/> match
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public int MthIndexOf ( T? item, PositiveInt32 mth, NonNegativeInt32 rearSet, NonNegativeInt32 count )
  {

    int validation = ValidateRearSetConfiguration ( rearSet, count, out ImpSegExc? e, RearSetCountParametersGetter);
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    T? [] store = this.store;

    int index = rearSet;
    int limit = rearSet - count;
    int counter = mth;

    while (index != limit)
    {
      // index -limit =index -rearSet +count =count -(rearSet -index)
      index = Array.LastIndexOf ( store, item, index, index - limit );

      if (index == -1)
        return -1;

      if (--counter == 0)
        return index;

      --index;
    }

    return -1;
  }

  /// <summary>
  /// Finds index of <paramref name="nth"/> <paramref name="item"/> match in store.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  public int NthIndexOf ( T? item, PositiveInt32 nth ) => NthIndexOf ( item, nth, 0, Count );

  /// <summary>
  /// Finds index of <paramref name="nth"/> <paramref name="item"/> match in store, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int NthIndexOf ( T? item, PositiveInt32 nth, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return NthIndexOf ( item, nth, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Finds index of <paramref name="nth"/> <paramref name="item"/> match
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int NthIndexOf ( T? item, PositiveInt32 nth, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out int limit, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    T? [] store = this.store;

    int index = offset;
    int counter = nth;

    while (index != limit)
    {
      index = Array.IndexOf ( store, item, index, limit - index );

      if (index == -1)
        return -1;

      if (--counter == 0)
        return index;

      ++index;
    }

    return -1;
  }

  /// <summary>
  /// Orders stored items using <paramref name="comparer"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  public void Order ( IComparer<T?> comparer )
  {
    if (comparer == null)
      throw NullComparer ( nameof ( comparer ) );

    int count = Count;
    if (count == 0)
      return;

    Array.Sort ( store, 0, count, comparer );
  }

  /// <summary>
  /// Orders stored items, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public void Order ( IComparer<T?> comparer, NonNegativeInt32 offset )
  {
    if (comparer == null)
      throw NullComparer ( nameof ( comparer ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    Array.Sort ( store, offset, ItemsCountToEndInclusive ( offset ), comparer );
  }

  /// <summary>
  /// Orders stored items using <paramref name="comparer"/>,
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public void Order ( IComparer<T?> comparer, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (comparer == null)
      throw NullComparer ( nameof ( comparer ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    Array.Sort ( store, offset, count, comparer );
  }

  /// <summary>
  /// Orders stored items using <paramref name="comparison"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparison"/> is <see langword="null"/>.</exception>
  public void Order ( Comparison<T?> comparison )
  {
    if (comparison == null)
      throw NullComparer ( nameof ( comparison ) );

    int count = Count;
    if (count == 0)
      return;

    BinaryInsertionOrder.Order ( store, 0, count, comparison );
  }

  /// <summary>
  /// Orders stored items, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparison"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public void Order ( Comparison<T?> comparison, NonNegativeInt32 offset )
  {
    if (comparison == null)
      throw NullComparer ( nameof ( comparison ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    BinaryInsertionOrder.Order ( store, offset, ItemsCountToEndInclusive ( offset ), comparison );
  }

  /// <summary>
  /// Orders stored items using <paramref name="comparison"/>,
  /// in segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparison"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public void Order ( Comparison<T?> comparison, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (comparison == null)
      throw NullComparer ( nameof ( comparison ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    BinaryInsertionOrder.Order ( store, offset, count, comparison );
  }

  /// <summary>
  /// Removes first occurrence of <paramref name="item"/> from store.
  /// </summary>
  /// <returns><see langword="false"/> when item is not present.</returns>
  public bool Remove ( T? item )
  {
    int index = IndexOf ( item );

    if (index == -1)
      return false;

    _ = RemoveAndReturn ( index );

    return true;
  }

  /// <summary>
  /// Removes item at <paramref name="atIndex"/> specified.
  /// </summary>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="atIndex"/> is greater or equal to <see cref="Count"/>.</exception>
  /// <returns>Item removed.</returns>
  public T? Remove ( NonNegativeInt32 atIndex )
  {
    if (ValidateIndex ( atIndex, out IndexOutOfBoundariesException? e, nameof ( atIndex ) ))
      throw e;

    return RemoveAndReturn ( atIndex );
  }

  /// <summary>
  /// Removes segment of items defined by <paramref name="offset"/> and <paramref name="count"/>
  /// from store.
  /// </summary>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public void Remove ( NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out int index, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    T? [] store = this.store;

    int itemsToEnd = ItemsCountToEndInclusive(index);
    if (itemsToEnd != 0)
      Array.Copy ( store, index, store, offset, itemsToEnd );

    int newCount = Count - count;
    if (itemShouldDefault)
      Array.Clear ( store, newCount, count );

    Count = newCount;
  }

  /// <summary>
  /// Removes item at <paramref name="index"/> specified.
  /// </summary>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="index"/> is out of capacitor bounds.</exception>
  public void RemoveAt ( int index )
  {
    if (ValidateIndex ( index, out IndexOutOfBoundariesException? e, nameof ( index ) ))
      throw e;

    _ = RemoveAndReturn ( index );
  }

  /// <summary>
  /// Removes item from store start and sets it to <paramref name="item"/>.
  /// </summary>
  /// <returns><see langword="false"/> when store is empty.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  public bool RemoveFirst ( out T? item )
  {
    if (Count == 0)
    {
      item = default ( T? );
      return false;
    }

    item = RemoveAndReturn ( 0 );
    return true;
  }

  /// <summary>
  /// Removes item from store end and sets it to <paramref name="item"/>.
  /// </summary>
  /// <returns><see langword="false"/> when store is empty.</returns>
  /// <remarks>If <see langword="false"/> is returned, <paramref name="item"/> is set to <c>default(<typeparamref name="T"/>)</c>.</remarks>
  public bool RemoveLast ( out T? item )
  {
    int count = Count;
    if (count == 0)
    {
      item = default ( T? );
      return false;
    }

    item = RemoveAndReturn ( count - 1 );
    return true;
  }

  /// <summary>
  /// Removes all items matching <paramref name="match"/> predicate from store.
  /// </summary>
  /// <returns>Count of items removed.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int RemoveMatches ( Predicate<T?> match ) => RemoveMatches ( match, 0, Count );

  /// <summary>
  /// Removes all items matching <paramref name="match"/> predicate from store, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns>Count of items removed.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int RemoveMatches ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return RemoveMatches ( match, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Removes all items matching <paramref name="match"/> predicate
  /// from store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns>Count of items removed.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int RemoveMatches ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out int limit, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return 0;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    // mutation of of MS List<T>.RemoveAll implementation
    // more or less one-to-one copy
    // https://github.com/dotnet/runtime/blob/33baf8ee337b20dd0f184b69a6f09be92850bf9e/src/libraries/System.Private.CoreLib/src/System/Collections/Generic/List.cs#L962

    int freeIndex = offset;
    T?[] store = this.store;

    for ( ; ; )
    {
      if (match ( store [ freeIndex ] ))
        break;

      if (++freeIndex == limit)
        return 0;
    }

    int current = freeIndex + 1;
    int itemsCount = Count;

    for ( ; ; )
    {
      while (current < limit && match ( store [ current ] )) current++;

      if (current < itemsCount)
        store [ freeIndex++ ] = store [ current++ ];
      else
        break;
    }

    int total = itemsCount - freeIndex;
    if (itemShouldDefault)
      Array.Clear ( store, freeIndex, total );

    Count = freeIndex;
    return total;
  }

  /// <summary>
  /// Resets <see cref="Capacitor{T}"/> store <paramref name="withCapacity"/>.
  /// </summary>
  /// <remarks>
  /// Creates new internal store and sets <see cref="Count"/> to <c>0</c>, always.
  /// </remarks>
  public void ResetStore ( NonNegativeInt32 withCapacity = default )
  {
    SetStoreWithCapacity ( withCapacity );
    Count = 0;
  }

  /// <summary>
  /// Reverses order of stored items.
  /// </summary>
  public void Reverse () => Array.Reverse ( store, 0, Count );

  /// <summary>
  /// Reverses order of stored items, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public void Reverse ( NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    Array.Reverse ( store, offset, ItemsCountToEndInclusive ( offset ) );
  }

  /// <summary>
  /// Reverses order of stored items in segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public void Reverse ( NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw UnsupportedValidationResult ( validation );
    }

    Array.Reverse ( store, offset, count );
  }

  /// <summary>
  /// Copies stored items into new array and returns it.
  /// </summary>
  public T? [] ToArray ()
  {
    int count = Count;
    T? [] array = GetStoreWithCapacity<T>(count);
    Array.Copy ( store, 0, array, 0, count );
    return array;
  }

  /// <summary>
  /// Enwraps store into <see cref="ArraySegment{T}"/> without trimming excess capacity and resets itself.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="ResetStore(NonNegativeInt32)"/> with <paramref name="resetCapacity"/>.
  /// </remarks>
  public ArraySegment<T?> IntoArraySegment ( NonNegativeInt32 resetCapacity = default )
  {
    ArraySegment<T?> result = new ( store, 0, Count );
    ResetStore ( resetCapacity );
    return result;
  }

  /// <summary>
  /// Enwraps store into <see cref="IListSegment{T}"/> without trimming excess capacity and resets itself.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="ResetStore(NonNegativeInt32)"/> with <paramref name="resetCapacity"/>.
  /// </remarks>
  public IListSegment<T?> IntoIListSegment ( NonNegativeInt32 resetCapacity = default )
  {
    IListSegment<T?> result = new ( store, 0, Count );
    ResetStore ( resetCapacity );
    return result;
  }

  /// <summary>
  /// Enwraps store into <see cref="IReadOnlyListSegment{T}"/> without trimming excess capacity and resets itself.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="ResetStore(NonNegativeInt32)"/> with <paramref name="resetCapacity"/>.
  /// </remarks>
  public IReadOnlyListSegment<T?> IntoIReadOnlyListSegment ( NonNegativeInt32 resetCapacity = default )
  {
    IReadOnlyListSegment<T?> result = new ( store, 0, Count );
    ResetStore ( resetCapacity );
    return result;
  }

  /// <summary>
  /// Enwraps store into <see cref="Span{T}"/> without trimming excess capacity and resets itself.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="ResetStore(NonNegativeInt32)"/> with <paramref name="resetCapacity"/>.
  /// </remarks>
  public Span<T?> IntoSpan ( NonNegativeInt32 resetCapacity = default )
  {
    Span<T?> result = new ( store, 0, Count );
    ResetStore ( resetCapacity );
    return result;
  }

  /// <summary>
  /// Enwraps store into <see cref="ReadOnlySpan{T}"/> without trimming excess capacity and resets itself.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="ResetStore(NonNegativeInt32)"/> with <paramref name="resetCapacity"/>.
  /// </remarks>
  public ReadOnlySpan<T?> IntoReadOnlySpan ( NonNegativeInt32 resetCapacity = default )
  {
    ReadOnlySpan<T?> result = new ( store, 0, Count );
    ResetStore ( resetCapacity );
    return result;
  }

  /// <summary>
  /// Enwraps store into <see cref="Memory{T}"/> without trimming excess capacity and resets itself.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="ResetStore(NonNegativeInt32)"/> with <paramref name="resetCapacity"/>.
  /// </remarks>
  public Memory<T?> IntoMemory ( NonNegativeInt32 resetCapacity = default )
  {
    Memory<T?> result = new ( store, 0, Count );
    ResetStore ( resetCapacity );
    return result;
  }

  /// <summary>
  /// Enwraps store into <see cref="ReadOnlyMemory{T}"/> without trimming excess capacity and resets itself.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="ResetStore(NonNegativeInt32)"/> with <paramref name="resetCapacity"/>.
  /// </remarks>
  public ReadOnlyMemory<T?> IntoReadOnlyMemory ( NonNegativeInt32 resetCapacity = default )
  {
    ReadOnlyMemory<T?> result = new ( store, 0, Count );
    ResetStore ( resetCapacity );
    return result;
  }

  /// <summary>
  /// String representation.
  /// </summary>
  override public string ToString () => $"{nameof ( Capacitor<> )}#{GetHashCode ()}: Capacity={Capacity}, Count={Count}";
}