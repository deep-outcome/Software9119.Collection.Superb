using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.Numerics;
using Software9119.Collection.Superb.Ordering;
using Software9119.Collection.Superb.Segmentation;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Software9119.Collection.Superb.Storing;

static class Capacitor
{
  static internal ArgNullExc NullAction ( string action ) => new ( paramName: action, "Action must be provided." );
  static internal ArgNullExc NullComparer ( string comparer ) => new ( paramName: comparer, "Comparer must be provided." );
  static internal ArgNullExc NullConverter ( string converter ) => new ( paramName: converter, "Converter must be provided." );
  static internal ArgNullExc NullMatchPredicate ( string match ) => new ( paramName: match, "Match predicate must be provided." );
  static internal ArgNullExc NullTargetArray ( string array ) => new ( paramName: array, "Target array must be provided." );


  static internal ArgOutOfRanExc InsufficientTargetArray ( string array, int length, int count )
    => new ( paramName: array, $"Insufficient target array size, available length {length} cannot accomodate {count} items." );

  static readonly internal string[] OffsetCountParamNames = ["offset", "count"];
  static internal Func<string []> OffsetCountParametersGetter = () => OffsetCountParamNames;
}

/// <summary>
/// Auto-expanding data storing structure designed specifically for managing internal store capacity with ease:
/// <list type="bullet">
/// <item>Uses exact capacitation for batch storing operations when source length can be determined.</item>
/// <item>Exposes various pre-capacitation options for client code.</item>
/// <item>Auto-capacitation operates whenever is needed, see also <see cref="GrowFactor"/>.</item>
/// </list>
/// </summary>
[SuppressMessage ( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "No." )]
[SuppressMessage ( "Design", "CA1051:Do not declare visible instance fields", Justification = "Inheritance design." )]
[DebuggerDisplay ( "(Count={Count},Capacity={Capacity})" )]
public class Capacitor<T> : IEnumerable, IEnumerable<T?>,
  ICollection<T?>, IReadOnlyCollection<T?>,
  IList<T?>, IReadOnlyList<T?>,
  IEquatable<Capacitor<T>>, ICloneable
{
  /// <summary>
  /// Constructor allowing to 'start' <see cref="Accumulator{T}"/> with pre-created internal store.
  /// </summary>
  /// <param name="count">How much of <paramref name="store"/> is considered populated.</param>
  /// <param name="store">Defaults to <see cref="Array.Empty{T}"/> when <see langword="null"/>.</param>
  /// <remarks>
  /// <paramref name="count"/> greater to store length is adjusted to fit it.
  /// </remarks>
  protected internal Capacitor ( NonNegativeInt32 count, T? []? store )
  {
    store ??= Array.Empty<T?> ();

    int length = store.Length;
    storeIndex = Math.Min ( length, count );
    this.store = store;
  }

  /// <summary>
  /// Default constructor.
  /// </summary>
  public Capacitor () : this ( null!, default ) { }

  /// <summary>
  /// Constructor with initial capacity.
  /// </summary>
  public Capacitor ( NonNegativeInt32 capacity ) : this ( (IEnumerable<T?>?) null, capacity ) { }

  /// <summary>
  /// Constructor with initial items.
  /// </summary>
  /// <remarks>
  /// Stores <paramref name="items"/> using exact capacitation or using auto-capacitation, 
  /// based on possibility to obtain <paramref name="items"/> length.
  /// </remarks>
  public Capacitor ( IEnumerable<T?>? items ) : this ( items, 0 ) { }

  /// <summary>
  /// Constructor with initial capacity and initial items.
  /// </summary>  
  /// <remarks>
  /// Sets store capacity to <paramref name="capacity"/> and then stores <paramref name="items"/> using exact
  /// capacitation or auto-capacitation for eventual next capacitation, based on possibility to obtain <paramref name="items"/> length.
  /// </remarks>  
  public Capacitor ( IEnumerable<T?>? items, NonNegativeInt32 capacity )
  {
    SetStoreWithCapacity ( capacity );
    _ = Add ( items, capacity );
  }

  /// <summary>
  /// Empty capacitor of <typeparamref name="T"/>.
  /// </summary>
  [SuppressMessage ( "Design", "CA1000:Do not declare static members on generic types", Justification = "Run-tim type is constructed anyway." )]
  static public Capacitor<T> Empty => new ();

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
  /// Internals for <see cref="Insert(int, T?)"/>.
  /// </summary>
  protected internal void AddOrInsert ( AddOrInsertOffset offset, T? item )
  {
    _ = AutoGrow ();

    if (offset.inserting)
      ShiftItemsToRight ( offset, 1 );

    store [ offset ] = item;
    ++Count;
  }

  /// <summary>
  /// Internals for: 
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, IEnumerable{T?}?, NonNegativeInt32)"/></item>
  /// <item><see cref="Add(IEnumerable{T?}?, NonNegativeInt32)"/></item>
  /// </list>
  /// </summary>  
  protected internal bool AddOrInsert ( AddOrInsertOffset offset, IEnumerable<T?>? items, NonNegativeInt32 roomRequest )
  {
    DebugValidateOffset ( offset );

    if (items == null)
      return false;

    if (items is T? [] a)
      return AddOrInsert ( offset, a );

    if (items is ICollection<T?> c)
      return AddOrInsert ( offset, c );

    if (items is IReadOnlyCollection<T?> rc)
      return AddOrInsert ( offset, rc );

    _ = CapacitateForNext ( roomRequest );

    T[]? tail = null;

    bool inserting = offset.inserting;
    if (inserting)
    {
      int index = offset.value;
      int tailSize = ItemsCountToEndInclusive(index);
      tail = new T [ tailSize ];

      Array.Copy ( store, index, tail, 0, tailSize );
      Count = index;
    }

    foreach (T? i in items)
      Add ( i );

    if (inserting)
      _ = Add ( tail );

    return true;
  }

  /// <summary>
  /// Internals for: 
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, IAsyncEnumerable{T?}?, NonNegativeInt32)"/></item>
  /// <item><see cref="Add(IAsyncEnumerable{T?}?, NonNegativeInt32)"/></item>
  /// </list>
  /// </summary>    
  async protected internal Task<bool> AddOrInsert ( AddOrInsertOffset offset, IAsyncEnumerable<T?>? items, NonNegativeInt32 roomRequest )
  {
    DebugValidateOffset ( offset );

    if (items == null)
      return false;

    _ = CapacitateForNext ( roomRequest );

    T[]? tail = null;

    bool inserting = offset.inserting;
    if (inserting)
    {
      int index = offset.value;
      int tailSize = ItemsCountToEndInclusive(index);
      tail = new T [ tailSize ];

      Array.Copy ( store, index, tail, 0, tailSize );
      Count = index;
    }

    await foreach (T? i in items.ConfigureAwait ( false ))
      Add ( i );

    if (inserting)
      _ = Add ( tail );

    return true;
  }

  /// <summary>
  /// Internals for: 
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, T?[])"/></item>
  /// <item><see cref="Add(T?[])"/></item>
  /// </list>
  /// </summary>
  protected internal bool AddOrInsert ( AddOrInsertOffset offset, T? []? items )
  {
    if (items == null)
      return false;

    int itemsLength = items.Length;
    PrepareStoreForAddIns ( offset, itemsLength );

    Array.Copy ( items, 0, store, offset, itemsLength );
    Count += itemsLength;

    return true;
  }

  /// <summary>
  /// Internals for: 
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, ICollection{T?}?)"/></item>
  /// <item><see cref="Add(ICollection{T?}?)"/></item>
  /// </list>
  /// </summary>
  protected internal bool AddOrInsert ( AddOrInsertOffset offset, ICollection<T?>? items )
  {
    if (items == null)
      return false;

    int itemsCount = items.Count;
    PrepareStoreForAddIns ( offset, itemsCount );

    items.CopyTo ( store, offset );
    Count += itemsCount;

    return true;
  }

  /// <summary>
  /// Internals for: 
  /// <list type="bullet">
  /// <item><see cref="Insert(NonNegativeInt32, IReadOnlyCollection{T?}?)"/></item>
  /// <item><see cref="Add(IReadOnlyCollection{T?}?)"/></item>
  /// </list>
  /// </summary>
  protected internal bool AddOrInsert ( AddOrInsertOffset offset, IReadOnlyCollection<T?>? items )
  {
    if (items == null)
      return false;

    if (items is T? [] a)
      return AddOrInsert ( offset, a );

    if (items is ICollection<T?> c)
      return AddOrInsert ( offset, c );

    int itemsCount = items.Count;
    PrepareStoreForAddIns ( offset, itemsCount );

    int index = offset.value;
    foreach (T? i in items)
      store [ index++ ] = i;

    Count += itemsCount;
    return true;
  }

  /// <summary>
  /// Invokes auto-grow using <see cref="GrowFactor"/> when store fully utilized.
  /// </summary>
  /// <returns><see langword="true"/> when capacity is changed.</returns>
  protected internal bool AutoGrow ()
  {
    if (IsFull)
    {

      int capacity = Capacity;
      if (capacity == 0)
      {
        store = new T [ defaultCapacity ];
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
  /// Computes <paramref name="forCount"/> and <paramref name="fromIndex"/> difference.
  /// </summary>
  static protected internal int AvailableCount ( int fromIndex, int forCount )
    => IndexingValidator.IndexToCountInclusiveDifference ( fromIndex, forCount );

  /// <summary>
  /// Resizes store to new capacity.
  /// </summary>  
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  protected internal void Capacitate ( int to ) => Array.Resize ( ref store, to );

  /// <summary>
  /// Creates offset with current <see cref="Count"/>;
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  protected internal AddOrInsertOffset CreateAddInsOffset ( int offset ) => AddOrInsertOffset.CreateUsingCount ( offset, Count );

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
  [Conditional ( "DEBUG" )]
  protected internal void DebugValidateIndex ( int index )
  {
    if (IndexingValidator.ValidateIndex ( index, Count, out IndexOutOfBoundariesException? e ))
      throw e;
  }

  /// <summary>
  /// Index validation existing only in debug compilation. 
  /// </summary>
  /// <remarks>
  /// Intended for internal methods.
  /// </remarks>
  [Conditional ( "DEBUG" )]
  protected internal void DebugValidateOffset ( AddOrInsertOffset offset )
  {
    if (ValidateInsertionIndex ( offset.value, out IndexOutOfBoundariesException? e, "" ))
      throw e;
  }

  /// <summary>
  /// Index validation existing only in debug compilation. 
  /// </summary>
  /// <remarks>
  /// Intended for internal methods.
  /// </remarks>
  [Conditional ( "DEBUG" )]
  protected internal void DebugValidateRightShift ( int index, int count )
  {
    if (IndexingValidator.ValidateIndex ( index, Capacity, out IndexOutOfBoundariesException? e1 ))
      throw e1;

    if (FreeCapacity < count)
      throw new InvalidOperationException ( "Shift is possible only up to free capacity." );
  }

  /// <summary>
  /// Creates store with capacity of <paramref name="capacity"/>.
  /// </summary>
  [MemberNotNull ( nameof ( store ) )]
  static protected internal U [] GetStoreWithCapacity<U> ( int capacity ) => capacity == 0 ? Array.Empty<U> () : new U [ capacity ];

  /// <returns>Count of stored items starting at <paramref name="fromIndex"/>.</returns>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  protected internal int ItemsCountToEndInclusive ( int fromIndex ) => IndexingValidator.IndexToCountInclusiveDifference ( fromIndex, Count );

  /// <summary>
  /// Removes item at index from store.
  /// </summary>
  protected internal T? RemoveAndReturn ( int atIndex )
  {
    DebugValidateIndex ( atIndex );

    T? [] store = this.store;
    T? item = store [ atIndex ];

    int sourceIndex = atIndex + 1;
    int itemsToEnd = ItemsCountToEndInclusive(sourceIndex);
    if (itemsToEnd != 0) // how does happend to be 0 here?
      Array.Copy ( store, sourceIndex, store, atIndex, itemsToEnd );

    int count = Count;
    --count;
    if (itemShouldDefault)
      store [ count ] = default;

    Count = count;
    return item;
  }

  /// <summary>
  /// Prepares store for insertion or addition of items, if needed.
  /// </summary>
  protected internal void PrepareStoreForAddIns ( AddOrInsertOffset offset, int itemsCount )
  {
    DebugValidateOffset ( offset );

    // mainly, prevents useless shift call
    if (itemsCount == 0)
      return;

    _ = CapacitateForNext ( itemsCount );
    if (offset.inserting)
      ShiftItemsToRight ( offset, itemsCount );
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
    DebugValidateRightShift ( from, byPositions );

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
  protected internal bool ValidateRearSetConfiguration (
    NonNegativeInt32 rearSet,
    NonNegativeInt32 count,
    [NotNullWhen ( true )] out ImpSegExc? e,
    string paramNames
  )
  {
    int itemsCount = Count;
    if (rearSet > itemsCount || rearSet + 1 < count || (rearSet == itemsCount && count != 0))
    {
      string msg = "With available {0}, given rearSet {1} and count {2} produce out-of indexing. (Parameters {3})";
      msg = string.Format ( CultureInfo.InvariantCulture, msg, itemsCount, rearSet, count, paramNames );

      e = new ImpossibleSegmentationException ( msg );
      return true;
    }

    e = null;
    return false;
  }

  /// <summary>
  /// Validates segmentation possibility.
  /// </summary>
  protected internal int ValidateSegmentation
  (
    NonNegativeInt32 offset, NonNegativeInt32 count,
    out int limit,
    [NotNullWhen ( true )] out ImpossibleSegmentationException? e,
    Func<string []>? parametersGetter = null
  )
    => IxValidator.ValidateSegmentation ( Count, offset, count, out limit, out e, parametersGetter );


  /// <summary>
  /// Validates segmentation possibility over available.
  /// </summary>
  static protected internal int ValidateSegmentation
  (
    NonNegativeInt32 available, NonNegativeInt32 offset, NonNegativeInt32 count,
    out int limit,
    [NotNullWhen ( true )] out ImpossibleSegmentationException? e,
    Func<string []>? parametersGetter = null
  )
    => IxValidator.ValidateSegmentation ( available, offset, count, out limit, out e, parametersGetter );

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
  /// Current store capacity.
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
  /// <remarks>Cannot be set when <see cref="LockGrowFactor"/> is set to <see langword="true"/>.</remarks>
  /// <exception cref="ArgumentOutOfRangeException">Upon try to set invalid <see cref="Storing.GrowFactor"/>.</exception>
  public GrowFactor GrowFactor
  {
    get => growFactor;
    set
    {
      if (LockGrowFactor)
        return;

      if (value.ToFloat ( out _ ) == false)
        throw new ArgumentOutOfRangeException ( paramName: nameof ( value ), $"Unsupported grow factor, '{value}'." );

      growFactor = value;
    }
  }

  /// <summary>
  /// Determines whether store capacity is fully utilized.
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
  /// Stores <paramref name="item"/> using auto-grow capacitation, see <see cref="GrowFactor"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  public void Add ( T? item ) => AddOrInsert ( CreateAddInsOffset ( Count ), item );

  /// <summary>
  /// <list type="bullet">
  /// <item>Stores <paramref name="items"/> using exact capacitation or using pre-capacitation and auto-capacitation.</item>
  /// <item>
  /// <paramref name="roomRequest"/> is used only when exact length of <paramref name="items"/> cannot be determined
  /// and ensures store capacity for <paramref name="roomRequest"/> more items.
  /// </item>
  /// </list>
  /// </summary>  
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Use <c>0</c> for <paramref name="roomRequest"/> for no room request.</item>
  /// <item>No capacitation happens when <see langword="false"/> is returned.</item>
  /// </list>
  /// </remarks>
  public bool Add ( IEnumerable<T?>? items, NonNegativeInt32 roomRequest )
    => AddOrInsert ( CreateAddInsOffset ( Count ), items, roomRequest );

  /// <summary>
  /// Stores <paramref name="items"/> using pre-capacitation and auto-capacitation.  
  /// </summary>  
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Use <c>0</c> for <paramref name="roomRequest"/> for no room request.</item>
  /// <item>No capacitation happens when <see langword="false"/> is returned.</item>
  /// </list>
  /// </remarks>
  async public Task<bool> Add ( IAsyncEnumerable<T?>? items, NonNegativeInt32 roomRequest )
    => await AddOrInsert ( CreateAddInsOffset ( Count ), items, roomRequest ).ConfigureAwait ( false );

  /// <summary>
  /// Stores <paramref name="items"/> using exact capacitation.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>
  public bool Add ( T? []? items ) => AddOrInsert ( CreateAddInsOffset ( Count ), items );

  /// <summary>
  /// Stores <paramref name="items"/> using exact capacitation.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>
  public bool Add ( ICollection<T?>? items ) => AddOrInsert ( CreateAddInsOffset ( Count ), items );

  /// <summary>
  /// Stores <paramref name="items"/> using exact capacitation.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>
  public bool Add ( IReadOnlyCollection<T?>? items ) => AddOrInsert ( CreateAddInsOffset ( Count ), items );

  /// <returns><see langword="true"/> if all stored items conform to <paramref name="match"/> predicate.</returns>  
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool AllMatches ( Predicate<T?> match )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    T? [] store = this.store;
    int length = store.Length;
    int index = 0;
    while (index < length)
      if (!match ( store [ index++ ] ))
        return false;

    return true;
  }

  /// <summary>
  /// Fast search on ordered store.
  /// </summary>
  /// <returns>Item index, or negative number if not found.</returns>  
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  public int BinarySearch ( T value, IComparer<T?>? comparer )
  {
    if (comparer == null)
      throw Capacitor.NullComparer ( nameof ( comparer ) );

    return Array.BinarySearch ( store, 0, Count, value, comparer );
  }

  /// <summary>
  /// Fast search on ordered store, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns>Item index, or negative number if not found.</returns>  
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  public int BinarySearch ( T value, NonNegativeInt32 offset, IComparer<T?>? comparer )
  {
    if (comparer == null)
      throw Capacitor.NullComparer ( nameof ( comparer ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return Array.BinarySearch ( store, offset, Count - offset, value, comparer );
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
  public int BinarySearch ( NonNegativeInt32 offset, NonNegativeInt32 count, T value, IComparer<T?>? comparer )
  {
    if (comparer == null)
      throw Capacitor.NullComparer ( nameof ( comparer ) );

    if (ValidateSegmentation ( offset, count, out _, out ImpSegExc? e ) == 1)
      throw e!;

    return Array.BinarySearch ( store, offset, count, value, comparer );
  }

  /// <summary>
  /// Changes storage capacity to capacity specified by <paramref name="to"/>, unless 
  /// <paramref name="to"/> is less then <see cref="Count"/>, or equal to
  /// current <see cref="Capacity"/>.
  /// </summary>
  /// <returns><see langword="true"/> when capacity is updated.</returns>  
  public bool CapacitateExact ( NonNegativeInt32 to )
  {
    if (to < Count)
      return false;

    if (to == Capacity)
      return false;

    Capacitate ( to );
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
  /// Sets capacity exactly to current <see cref="Count"/>, if not of that size already.
  /// </summary>
  /// <returns><see langword="true"/> when capacity is updated.</returns>  
  public bool CapacitateToCount ()
  {
    if (IsFull)
      return false;

    Capacitate ( Count );
    return true;
  }

  /// <returns>
  /// <see langword="true"/> when <paramref name="item"/> is found in store.
  /// </returns>
  public bool Contains ( T? item ) => IndexOf ( item ) != -1;

  /// <summary>
  /// Clones this <see cref="Capacitor{T}"/> state similar to <see cref="Clone()"/> but with
  /// items converted to <typeparamref name="To"/> type.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="converter"/> is <see langword="null"/>.</exception>
  public Capacitor<To> Convert<To> ( Converter<T?, To> converter )
  {
    if (converter == null)
      throw Capacitor.NullConverter ( nameof ( converter ) );

    int count = Count;
    T?[] store = this.store;

    To[] to = new To[count];
    for (int i = 0 ; i < count ; ++i)
      to [ i ] = converter ( store [ i ] );

    Capacitor<To> clone = CloneWithStoreAndCount(to, count);
    return clone;
  }

  /// <summary>
  /// Clones this <see cref="Capacitor{T}"/> state similar to <see cref="Clone(NonNegativeInt32)"/> but with
  /// items converted to <typeparamref name="To"/> type.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="converter"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public Capacitor<To> Convert<To> ( Converter<T?, To> converter, NonNegativeInt32 offset )
  {
    if (converter == null)
      throw Capacitor.NullConverter ( nameof ( converter ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    int count = Count - offset;

    T?[] store = this.store;
    To[] to = new To[count];

    for (int wi = 0, ri = offset ; wi < count ; ++ri, ++wi)
      to [ wi ] = converter ( store [ ri ] );

    Capacitor<To> clone = CloneWithStoreAndCount(to, count);
    return clone;
  }

  /// <summary>
  /// Clones this <see cref="Capacitor{T}"/> state similar to <see cref="Clone(NonNegativeInt32, NonNegativeInt32)"/> but with
  /// items converted to <typeparamref name="To"/> type.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="converter"/> is <see langword="null"/>.</exception>
  public Capacitor<To> Convert<To> ( Converter<T?, To> converter, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (converter == null)
      throw Capacitor.NullConverter ( nameof ( converter ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0:
      case -1: break;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    T?[] store = this.store;
    To[] to = GetStoreWithCapacity<To>(count);

    for (int wi = 0, ri = offset ; wi < count ; ++ri, ++wi)
      to [ wi ] = converter ( store [ ri ] );

    Capacitor<To> clone = CloneWithStoreAndCount(to, count);
    return clone;
  }

  /// <summary>
  /// Copies stored items into targed array.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentOutOfRangeException">When <paramref name="array"/> is of insufficient length.</exception>
  public void CopyTo ( T? [] array )
  {
    if (array == null)
      throw Capacitor.NullTargetArray ( nameof ( array ) );

    int count = Count;
    if (array.Length < count)
      throw Capacitor.InsufficientTargetArray ( nameof ( array ), array.Length, count );

    Array.Copy ( store, 0, array, 0, count );
  }

  /// <summary>
  /// Copies stored items into targed array, starting at its <paramref name="arrayIndex"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="array"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentOutOfRangeException">
  /// When <paramref name="array"/> length from <paramref name="arrayIndex"/> is insufficient.
  /// </exception>
  public void CopyTo ( T? [] array, int arrayIndex )
  {
    if (array == null)
      throw Capacitor.NullTargetArray ( nameof ( array ) );

    int length = array.Length;
    int availableLength = AvailableCount(arrayIndex, length);

    int count = Count;
    if (availableLength < count)
      throw Capacitor.InsufficientTargetArray ( nameof ( array ), availableLength, count );

    Array.Copy ( store, 0, array, arrayIndex, count );
  }

  /// <summary>
  /// Copies <paramref name="count"/> of stored items from <paramref name="fromIndex"/>
  /// into targed array, starting at its <paramref name="arrayIndex"/>.
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
  public void CopyTo ( T? [] array, NonNegativeInt32 arrayIndex, NonNegativeInt32 fromIndex, NonNegativeInt32 count )
  {
    if (array == null)
      throw Capacitor.NullTargetArray ( nameof ( array ) );

    Func<string[]> targetParams = () => [nameof ( array ), nameof ( arrayIndex ), nameof ( count )];
    int validation = ValidateSegmentation (array.Length, arrayIndex, count, out _, out ImpSegExc? e1, targetParams);
    switch (validation)
    {
      case 0:
      case -1: // let validate source segment even for empty target segment
        break;
      case 1: throw e1!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    Func<string[]> sourceParams = () => [nameof ( fromIndex ), nameof ( count )];
    validation = ValidateSegmentation ( fromIndex, count, out _, out ImpSegExc? e2, sourceParams );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e2!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    Array.Copy ( store, fromIndex, array, arrayIndex, count );
  }

  /// <summary>
  /// Sets all items stored to <see langword="default(T)"/> and <see cref="Count"/> to <c>0</c>.
  /// </summary>
  /// <remarks>Clears to <see cref="Count"/>.</remarks>
  public void Clear ()
  {
    int count = Count;
    if (count == 0)
      return;

    Array.Clear ( store, 0, count );
    Count = 0;
  }

  /// <summary>
  /// Sets whole store up to <see cref="Capacity"/> to <see langword="default(T)"/>  and <see cref="Count"/> to <c>0</c>.
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
  /// <remarks>Stored items are shallow-cloned to new store with capacity fitting <see cref="Count"/>.</remarks>
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
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  /// <remarks>Stored items are shallow-cloned to new internal store with exact capacity for items from <paramref name="offset"/>.</remarks>
  public Capacitor<T> Clone ( NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    int count = Count - offset;
    T [] store = new T[count];
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
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0:
      case -1: break;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    T [] store = GetStoreWithCapacity<T>(count);
    if (validation == 0)
      Array.Copy ( this.store, offset, store, 0, count );

    Capacitor<T> segment = CloneWithStoreAndCount(store, count);
    return segment;
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
  /// Extracts internal store and resets store to new.
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

    Array.Fill ( store, item, offset, Count - offset );
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
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    Array.Fill ( store, item, offset, count );
  }

  /// <summary>
  /// Overwrites whole store with <paramref name="item"/> up to current <see cref="Capacity"/> and
  /// sets <see cref="Count"/> accordingly.
  /// </summary>
  public void FillToCapacity ( T? item )
  {
    Array.Fill ( store, item );
    Count = Capacity;
  }

  /// <summary>
  /// Finds all indexes of stored items matching <paramref name="match"/> predicate.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public IEnumerable<int> FindAllIndexes ( Predicate<T?> match )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int count = Count;
    if (count == 0)
      yield break;

    T? [] store = this.store;
    int index = -1;
    for ( ; ; )
    {
      index += 1;
      index = Array.FindIndex ( store, index, count - index, match );

      if (index == -1 || index == count)
        yield break;

      yield return index;
    }
  }

  /// <summary>
  /// Finds all stored items matching <paramref name="match"/> predicate.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public IEnumerable<T?> FindAllItems ( Predicate<T?> match )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    if (Count == 0)
      yield break;

    foreach (int i in FindAllIndexes ( match ))
      yield return store [ i ];
  }

  /// <summary>
  /// Finds index of first item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int FindFirstIndex ( Predicate<T?> match )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int count = Count;
    if (count == 0)
      return -1;

    return Array.FindIndex ( store, 0, count, match );
  }

  /// <summary>
  /// Finds index of first item matching <paramref name="match"/> predicate, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int FindFirstIndex ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    int count = Count;
    if (count == 0)
      return -1;

    return Array.FindIndex ( store, offset, count - offset, match );
  }

  /// <summary>
  /// Finds index of first item matching <paramref name="match"/> predicate in store segment specified by <paramref name="offset"/>
  /// and <paramref name="count"/>.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int FindFirstIndex ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    return Array.FindIndex ( store, offset, count, match );
  }

  /// <summary>
  /// Finds first item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindFirstItem ( Predicate<T?> match, out T? item )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int count = Count;
    if (count == 0)
    {
      item = default ( T? );
      return false;
    }

    int index = Array.FindIndex ( store, 0, count, match );
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds first item matching <paramref name="match"/> predicate, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindFirstItem ( Predicate<T?> match, NonNegativeInt32 offset, out T? item )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    int count = Count;
    if (count == 0)
    {
      item = default ( T? );
      return false;
    }

    int index = Array.FindIndex ( store, offset, count -offset, match );
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds first item matching <paramref name="match"/> predicate in store segment specified by <paramref name="offset"/>
  /// and <paramref name="count"/>.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public bool FindFirstItem ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count, out T? item )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1:
      {
        item = default ( T? );
        return false;
      }
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    int index = Array.FindIndex ( store, offset, count, match );
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int FindLastIndex ( Predicate<T?> match )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int count = Count;
    if (count == 0)
      return -1;

    return Array.FindLastIndex ( store, count - 1, count, match );
  }

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate in store segment from <paramref name="offset"/> to <see cref="Count"/>.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int FindLastIndex ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    int count = Count;
    if (count == 0)
      return -1;

    return Array.FindLastIndex ( store, count - 1, count - offset, match );
  }

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public int FindLastIndex ( NonNegativeInt32 rearSet, Predicate<T?> match )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    if (ValidateIndex ( rearSet, out IndexOutOfBoundariesException? e, nameof ( rearSet ) ))
      throw e;

    int count = Count;
    if (count == 0)
      return -1;

    return Array.FindLastIndex ( store, rearSet, rearSet + 1, match );
  }

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when no item matches predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int FindLastIndex ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    return Array.FindLastIndex ( store, offset + count - 1, count, match );
  }

  /// <summary>
  /// Finds index of last item matching <paramref name="match"/> predicate 
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
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    const string paramNames = $"'{nameof(rearSet)}','{nameof(count)}'";
    if (ValidateRearSetConfiguration ( rearSet, count, out ImpSegExc? e, paramNames ))
      throw e;

    if (count == 0 || Count == 0)
      return -1;

    return Array.FindLastIndex ( store, rearSet, count, match );
  }

  /// <summary>
  /// Finds last item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindLastItem ( Predicate<T?> match, out T? item )
  {
    int index = FindLastIndex(match);
    if (index == -1)
    {
      item = default ( T );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds last item matching <paramref name="match"/> predicate in store segment from <paramref name="offset"/> to <see cref="Count"/>.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindLastItem ( Predicate<T?> match, NonNegativeInt32 offset, out T? item )
  {
    int index = FindLastIndex(match, offset);
    if (index == -1)
    {
      item = default ( T );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds last item matching <paramref name="match"/> predicate in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindLastItem ( NonNegativeInt32 rearSet, Predicate<T?> match, out T? item )
  {
    int index = FindLastIndex(rearSet, match);
    if (index == -1)
    {
      item = default ( T );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds last item matching <paramref name="match"/> predicate in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public bool FindLastItem ( Predicate<T?> match, NonNegativeInt32 offset, NonNegativeInt32 count, out T? item )
  {
    int index = FindLastIndex(match, offset, count);
    if (index == -1)
    {
      item = default ( T );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds item matching <paramref name="match"/> predicate
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><see langword="true"/> when item is found.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public bool FindLastItem ( NonNegativeInt32 rearSet, NonNegativeInt32 count, Predicate<T?> match, out T? item )
  {
    int index = FindLastIndex(rearSet, count, match);
    if (index == -1)
    {
      item = default ( T );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds index of mth last item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int FindMthIndex ( Predicate<T?> match, PositiveInt32 mth )
  {
    int count = Count;
    return FindMthIndex ( match, mth, count == 0 ? 0 : count - 1, count );
  }


  /// <summary>
  /// Finds index of mth last item matching <paramref name="match"/> predicate in store segment from start to <paramref name="rearSet"/>.
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
  /// Finds index of mth last item matching <paramref name="match"/> predicate
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
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    const string paramNames = $"'{nameof(rearSet)}','{nameof(count)}'";
    if (ValidateRearSetConfiguration ( rearSet, count, out ImpSegExc? e, paramNames ))
      throw e;

    if (count == 0 || Count == 0)
      return -1;

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
  /// Finds mth last item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindMthItem ( Predicate<T?> match, PositiveInt32 mth, out T? item )
  {
    int index = FindMthIndex ( match, mth );
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds mth last item matching <paramref name="match"/> predicate in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindMthItem ( Predicate<T?> match, PositiveInt32 mth, NonNegativeInt32 rearSet, out T? item )
  {
    int index = FindMthIndex ( match, mth, rearSet );
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds mth last item matching <paramref name="match"/> predicate
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public bool FindMthItem ( Predicate<T?> match, PositiveInt32 mth, NonNegativeInt32 rearSet, NonNegativeInt32 count, out T? item )
  {
    int index = FindMthIndex ( match, mth, rearSet, count );
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds index of nth item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public int FindNthIndex ( Predicate<T?> match, PositiveInt32 nth ) => FindNthIndex ( match, nth, 0, Count );

  /// <summary>
  /// Finds index of nth item matching <paramref name="match"/> predicate in store segment from <paramref name="offset"/> to <see cref="Count"/>.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int FindNthIndex ( Predicate<T?> match, PositiveInt32 nth, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return FindNthIndex ( match, nth, offset, Count - offset );
  }

  /// <summary>
  /// Finds index of nth item matching <paramref name="match"/> predicate in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int FindNthIndex ( Predicate<T?> match, PositiveInt32 nth, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    T? [] store = this.store;

    int index = offset;
    int counter = nth;

    int limit = IxValidator.LimitOutOf(offset, count);
    while (index < limit)
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
  /// Finds nth item matching <paramref name="match"/> predicate.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindNthItem ( Predicate<T?> match, PositiveInt32 nth, out T? item )
  {
    int index = FindNthIndex ( match, nth );
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds nth item matching <paramref name="match"/> predicate in store segment from <paramref name="offset"/> to <see cref="Count"/>.
  /// </summary>
  /// <returns><see langword="false"/> when not enough items match predicate.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindNthItem ( Predicate<T?> match, PositiveInt32 nth, NonNegativeInt32 offset, out T? item )
  {
    int index = FindNthIndex ( match, nth, offset );
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Finds nth item matching <paramref name="match"/> predicate in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match predicate.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public bool FindNthItem ( Predicate<T?> match, PositiveInt32 nth, NonNegativeInt32 offset, NonNegativeInt32 count, out T? item )
  {
    int index = FindNthIndex ( match, nth, offset, count );
    if (index == -1)
    {
      item = default ( T? );
      return false;
    }

    item = store [ index ];
    return true;
  }

  /// <summary>
  /// Verifies predicate match against stored items.
  /// </summary>
  /// <returns><see langword="true"/> on first match, or <see langword="false"/> when no match is found.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  public bool FindMatch ( Predicate<T?> match )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    if (Count == 0)
      return false;

    return Array.FindIndex ( store, 0, Count, match ) != -1;
  }

  /// <summary>
  /// Verifies predicate match against stored items in segment from <paramref name="offset"/> to <see cref="Count"/>.
  /// </summary>
  /// <returns><see langword="true"/> on first match, or <see langword="false"/> when no match is found.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="match"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public bool FindMatch ( Predicate<T?> match, NonNegativeInt32 offset )
  {
    if (match == null)
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return Array.FindIndex ( store, offset, Count - offset, match ) != -1;
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
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return false;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    return Array.FindIndex ( store, offset, count, match ) != -1;
  }

  /// <summary>
  /// Runs <paramref name="action"/> on each stored item.
  /// </summary>
  /// <exception cref="ArgumentNullException">Whem <paramref name="action"/> is <see langword="null"/>.</exception>
  public void ForEach ( Action<T?> action )
  {
    if (action == null)
      throw Capacitor.NullAction ( nameof ( action ) );

    T? [] store = this.store;
    int count = Count;

    for (int i = 0 ; i < count ; ++i)
      action ( store [ i ] );
  }

  /// <summary>
  /// Runs <paramref name="action"/> on each stored item in store segment from <paramref name="offset"/> to <see cref="Count"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">Whem <paramref name="action"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public void ForEach ( Action<T?> action, NonNegativeInt32 offset )
  {
    if (action == null)
      throw Capacitor.NullAction ( nameof ( action ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    T? [] store = this.store;
    int count = Count;

    for (int i = offset ; i < count ; ++i)
      action ( store [ i ] );
  }

  /// <summary>
  /// Runs <paramref name="action"/> on each stored item in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">Whem <paramref name="action"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public void ForEach ( Action<T?> action, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (action == null)
      throw Capacitor.NullAction ( nameof ( action ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    T? [] store = this.store;
    int limit = offset + count;
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
  /// Finds index of first occurence of <paramref name="item"/> in store.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  public int IndexOf ( T? item ) => Array.IndexOf ( store, item, 0, Count );

  /// <summary>
  /// Finds index of first occurence of <paramref name="item"/> in store, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int IndexOf ( T? item, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return Array.IndexOf ( store, item, offset, Count - offset );
  }

  /// <summary>
  /// Finds index of first occurence of <paramref name="item"/>
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns>
  /// <list type="bullet">
  /// <item><c>-1</c> when item is not present.</item>  
  /// </list>
  /// </returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int IndexOf ( T? item, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
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

    AddOrInsert ( CreateAddInsOffset ( index ), item );
  }

  /// <summary>
  /// <list type="bullet">
  /// <item>Stores <paramref name="items"/> using exact capacitation or using pre-capacitation and auto-capacitation.</item>
  /// <item>
  /// <paramref name="roomRequest"/> is used only when exact length of <paramref name="items"/> cannot be determined
  /// and ensures store capacity for <paramref name="roomRequest"/> more items.
  /// </item>
  /// </list>
  /// </summary>  
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>   
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Use <c>0</c> for <paramref name="roomRequest"/> for no room request.</item>
  /// <item>No capacitation happens when <see langword="false"/> is returned.</item>
  /// </list>
  /// </remarks>
  public bool Insert ( NonNegativeInt32 offset, IEnumerable<T?>? items, NonNegativeInt32 roomRequest )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return AddOrInsert ( CreateAddInsOffset ( offset ), items, roomRequest );
  }

  /// <summary>
  /// Stores <paramref name="items"/> using pre-capacitation and auto-capacitation.  
  /// </summary>  
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>   
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  /// <remarks>
  /// <list type="bullet">
  /// <item>Use <c>0</c> for <paramref name="roomRequest"/> for no room request.</item>
  /// <item>No capacitation happens when <see langword="false"/> is returned.</item>
  /// </list>
  /// </remarks>
  async public Task<bool> Insert ( NonNegativeInt32 offset, IAsyncEnumerable<T?>? items, NonNegativeInt32 roomRequest )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return await AddOrInsert ( CreateAddInsOffset ( offset ), items, roomRequest ).ConfigureAwait ( false );
  }

  /// <summary>
  /// Using exact capacitation, inserts <paramref name="items"/> into store starting at <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  public bool Insert ( NonNegativeInt32 offset, T? []? items )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return AddOrInsert ( CreateAddInsOffset ( offset ), items );
  }

  /// <summary>
  /// Using exact capacitation, inserts <paramref name="items"/> into store starting at <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  public bool Insert ( NonNegativeInt32 offset, ICollection<T?>? items )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return AddOrInsert ( CreateAddInsOffset ( offset ), items );
  }

  /// <summary>
  /// Using exact capacitation, inserts <paramref name="items"/> into store starting at <paramref name="offset"/>.
  /// </summary>
  /// <returns><see langword="false"/> when <paramref name="items"/> parameter is null.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is out of insertion bounds.</exception>
  public bool Insert ( NonNegativeInt32 offset, IReadOnlyCollection<T?>? items )
  {
    if (ValidateInsertionIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return AddOrInsert ( CreateAddInsOffset ( offset ), items );
  }

  /// <summary>
  /// Determines whether current capacity can suffice to <paramref name="forNext"/> next items.
  /// </summary>
  /// <returns><see langword="true"/> if capacity if sufficient.</returns>
  /// <remarks><paramref name="reserve"/> is difference of free capacity and <paramref name="forNext"/>.</remarks>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  public bool IsCapacitySufficient ( NonNegativeInt32 forNext, out int reserve ) => (reserve = FreeCapacity - forNext) > -1;

  /// <summary>
  /// Finds index of last occurence of <paramref name="item"/> in store.
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
  /// Finds index of last occurence of <paramref name="item"/> in store, starting at <paramref name="offset"/> specified.
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
  /// Finds index of last occurence of <paramref name="item"/> in store segment from start to <paramref name="rearSet"/>.
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
  /// Finds index of last occurence of <paramref name="item"/>
  /// in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int LastIndexOf ( T? item, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    return Array.LastIndexOf ( store, item, offset + count - 1, count );
  }

  /// <summary>
  /// Finds index of last occurence of <paramref name="item"/>
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><c>-1</c> when item is not present.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public int LastIndexOf ( NonNegativeInt32 rearSet, NonNegativeInt32 count, T? item )
  {
    const string paramNames = $"'{nameof(rearSet)}','{nameof(count)}'";
    if (ValidateRearSetConfiguration ( rearSet, count, out ImpSegExc? e, paramNames ))
      throw e;

    if (count == 0 || Count == 0)
      return -1;

    return Array.LastIndexOf ( store, item, rearSet, count );
  }

  /// <summary>
  /// Finds index of nth <paramref name="item"/> match in store.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  public int NthIndexOf ( T? item, PositiveInt32 nth ) => NthIndexOf ( item, nth, 0, Count );


  /// <summary>
  /// Finds index of nth <paramref name="item"/> match in store, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public int NthIndexOf ( T? item, PositiveInt32 nth, NonNegativeInt32 offset )
  {
    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    return NthIndexOf ( item, nth, offset, Count - offset );
  }

  /// <summary>
  /// Finds index of nth <paramref name="item"/> match in store segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public int NthIndexOf ( T? item, PositiveInt32 nth, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return -1;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    T? [] store = this.store;

    int index = offset;
    int counter = nth;

    int limit = IxValidator.LimitOutOf(offset, count);
    while (index < limit)
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
  /// Finds index of mth last <paramref name="item"/> match in store.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  public int MthIndexOf ( T item, PositiveInt32 mth )
  {
    int count = Count;
    if (count == 0)
      return -1;

    return MthIndexOf ( item, mth, count - 1, count );
  }


  /// <summary>
  /// Finds index of mth last <paramref name="item"/> match in store segment from start to <paramref name="rearSet"/>.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="rearSet"/> is greater or equal to <see cref="Count"/>.</exception>
  public int MthIndexOf ( T item, PositiveInt32 mth, NonNegativeInt32 rearSet )
  {
    if (ValidateIndex ( rearSet, out IndexOutOfBoundariesException? e, nameof ( rearSet ) ))
      throw e;

    return MthIndexOf ( item, mth, rearSet, rearSet + 1 );
  }

  /// <summary>
  /// Finds index of mth last <paramref name="item"/> match
  /// in store segment of <paramref name="count"/> from <paramref name="rearSet"/> backwards.
  /// </summary>
  /// <returns><c>-1</c> when not enough items match.</returns>  
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="rearSet"/> create impossible segmentation over store.
  /// </exception>
  public int MthIndexOf ( T? item, PositiveInt32 mth, NonNegativeInt32 rearSet, NonNegativeInt32 count )
  {

    const string paramNames = $"'{nameof(rearSet)}','{nameof(count)}'";
    if (ValidateRearSetConfiguration ( rearSet, count, out ImpSegExc? e, paramNames ))
      throw e;

    if (count == 0 || Count == 0)
      return -1;

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
  /// Orders stored items using <paramref name="comparer"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  public void Order ( IComparer<T?> comparer )
  {
    if (comparer == null)
      throw Capacitor.NullComparer ( nameof ( comparer ) );

    Array.Sort ( store, 0, Count, comparer );
  }

  /// <summary>
  /// Orders stored items, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public void Order ( IComparer<T?> comparer, NonNegativeInt32 offset )
  {
    if (comparer == null)
      throw Capacitor.NullComparer ( nameof ( comparer ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    Array.Sort ( store, offset, Count - offset, comparer );
  }

  /// <summary>
  /// Orders stored items using <paramref name="comparer"/>,
  /// in segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparer"/> is <see langword="null"/>.</exception>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public void Order ( IComparer<T?> comparer, NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    if (comparer == null)
      throw Capacitor.NullComparer ( nameof ( comparer ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
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
      throw Capacitor.NullComparer ( nameof ( comparison ) );

    BinaryInsertionOrder.Order ( store, 0, Count, comparison );
  }

  /// <summary>
  /// Orders stored items, starting at <paramref name="offset"/> specified.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="comparison"/> is <see langword="null"/>.</exception>
  /// <exception cref="IndexOutOfBoundariesException">When <paramref name="offset"/> is greater or equal to <see cref="Count"/>.</exception>
  public void Order ( Comparison<T?> comparison, NonNegativeInt32 offset )
  {
    if (comparison == null)
      throw Capacitor.NullComparer ( nameof ( comparison ) );

    if (ValidateIndex ( offset, out IndexOutOfBoundariesException? e, nameof ( offset ) ))
      throw e;

    BinaryInsertionOrder.Order ( store, offset, Count - offset, comparison );
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
      throw Capacitor.NullComparer ( nameof ( comparison ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    BinaryInsertionOrder.Order ( store, offset, count, comparison );
  }

  /// <summary>
  /// Removes first occurence of <paramref name="item"/> from store, or does nothing if not present.
  /// </summary>
  /// <returns><see langword="true"/> when item was removed.</returns>
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
    int validation = ValidateSegmentation ( offset, count, out int index, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
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
  /// Removes item from store start, if store is not empty, and sets it to <paramref name="item"/>.
  /// </summary>  
  /// <returns><see langword="true"/> if item is removed.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
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
  /// Removes item from store end, if store is not empty, and sets it to <paramref name="item"/>.
  /// </summary>  
  /// <returns><see langword="true"/> if item is removed.</returns>
  /// <remarks>If <see langword="false"/> returned, <paramref name="item"/> is set to <c>default(T?)</c>.</remarks>
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

    return RemoveMatches ( match, offset, Count - offset );
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
      throw Capacitor.NullMatchPredicate ( nameof ( match ) );

    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e );
    switch (validation)
    {
      case 0: break;
      case -1: return 0;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    // mutation of of MS List<T>.RemoveAll implementation
    // more or less one-to-one copy
    // https://github.com/dotnet/runtime/blob/33baf8ee337b20dd0f184b69a6f09be92850bf9e/src/libraries/System.Private.CoreLib/src/System/Collections/Generic/List.cs#L962

    int freeIndex = offset;
    int limit = IxValidator.LimitOutOf(offset, count);
    T?[] store = this.store;

    while (freeIndex < limit && !match ( store [ freeIndex ] )) freeIndex++;
    if (freeIndex == limit) return 0;

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
  /// Resets <see cref="Capacitor{T}"/> store <paramref name="toCapacity"/>.
  /// </summary>
  /// <remarks>
  /// Creates new internal store and sets <see cref="Count"/> to <c>0</c>.
  /// </remarks>
  public void ResetStore ( NonNegativeInt32 toCapacity = default )
  {
    SetStoreWithCapacity ( toCapacity );
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

    Array.Reverse ( store, offset, Count - offset );
  }

  /// <summary>
  /// Reverses order of stored items in segment specified by <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  /// <exception cref="ImpossibleSegmentationException">
  /// When <paramref name="count"/> and <paramref name="offset"/> create impossible segmentation over store.
  /// </exception>
  public void Reverse ( NonNegativeInt32 offset, NonNegativeInt32 count )
  {
    int validation = ValidateSegmentation ( offset, count, out _, out ImpSegExc? e, Capacitor.OffsetCountParametersGetter );
    switch (validation)
    {
      case 0: break;
      case -1: return;
      case 1: throw e!;
      default: throw new InvalidOperationException ( $"Unsupported validation result, '{validation}'." );
    }

    Array.Reverse ( store, offset, count );
  }

  /// <summary>
  /// Copies stored items into new array and returns it.
  /// </summary>
  public T [] ToArray ()
  {
    T[] array = GetStoreWithCapacity<T>(Count);
    CopyTo ( array );
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
  override public string ToString () => $"{nameof ( Capacitor<> )}: Capacity={Capacity}, Count={Count}";
}