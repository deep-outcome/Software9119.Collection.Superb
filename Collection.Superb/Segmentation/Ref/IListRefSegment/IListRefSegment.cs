using Software9119.Collection.Superb.Extension;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IListRefSegment{T}"/> is parallel to <see cref="ArraySegment{U}"/> but generalized
/// to <see cref="IList"/>.
/// </summary>
/// <remarks>
/// Everything comes with a price and for this reason is up to
/// client code to ensure <see cref="IList"/> passed into <see cref="IListRefSegment{T}"/> is not
/// mutated in a harmful way, most notably that it is not shrunk beyond segment defined.
/// </remarks>
[SuppressMessage ( "Design", "CA1010:Generic interface should also be implemented", Justification = "Not interested." )]
public ref struct IListRefSegment<T> : IList
  where T : struct, IList, allows ref struct
{
  readonly Lock syncStump = new ();

  internal  T list;
  readonly internal int offset;
  readonly internal int limit;

  /// <summary>
  /// Count of items available through this segment.
  /// </summary>
  readonly public int Count => limit - offset;
  /// <summary>
  /// Segment offset to <see cref="List"/>.
  /// </summary>
  readonly public int Offset => offset;

  /// <summary>
  /// See <see cref="IListRefSegment{T}"/> is not readonly.
  /// </summary>
  readonly public bool IsReadOnly => false;

  /// <summary>
  /// Segment size is fixed.
  /// </summary>
  readonly public bool IsFixedSize => true;

  /// <summary>
  /// Segment is not thread safe.
  /// </summary>
  readonly public bool IsSynchronized => false;

  /// <summary>
  /// Segment sync root.
  /// </summary>
#pragma warning disable CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.
  readonly public object SyncRoot => syncStump;
#pragma warning restore CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.

  /// <summary>
  /// The <see cref="IList"/> over which segmentation occurs.
  /// </summary>
  readonly public T List => list;


  IEqualityComparer<object> equalityComparer;

  /// <summary>
  /// Basic constructor.
  /// </summary>
  /// <param name="equalityComparer">
  /// If <see langword="null"/> passed-in, the <c>EqualityComparer&lt;object&gt;.Default</c>
  /// will be used. See <see cref="EqualityComparer"/> for more.
  /// </param>
  public IListRefSegment ( T list, IEqualityComparer<object>? equalityComparer = null )
  {
    this.list = list;
    offset = 0;
    limit = list.Count;

    EqualityComparer = equalityComparer!;
  }

  /// <summary>
  /// Offset constructor.
  /// </summary>
  /// <param name="equalityComparer">
  /// If <see langword="null"/> passed-in, the <c>EqualityComparer&lt;object&gt;.Default</c>
  /// will be used. See <see cref="EqualityComparer"/> for more.
  /// </param>
  /// <param name="offset">Starting index of segment.</param>
  /// <param name="count">Number of items to include.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is null.</exception>
  /// <exception cref="ImpossibleSegmentationException">Thrown when the segmention settings are impossible.</exception>
  public IListRefSegment ( T list, int offset, int count, IEqualityComparer<object>? equalityComparer = null )
  {
    this.list = list;
    this.offset = offset;

    if (ValidateSetup ( count, out int limit, out ImpossibleSegmentationException? e ))
      throw e;

    this.limit = limit;

    EqualityComparer = equalityComparer!;
  }


  /// <summary>
  /// Equality comparer used in <see cref="Contains(object?)"/> and <see cref="IndexOf(object?)"/> methods.
  /// </summary>
  /// <remarks>Cannot be set to <see langword="null"/> because it defaults to <c>EqualityComparer&lt;object&gt;.Default</c>.</remarks>
  public IEqualityComparer<object> EqualityComparer
  {
    readonly get
    {
      return equalityComparer;
    }

    [MemberNotNull ( nameof ( equalityComparer ) )]
    set => equalityComparer = (value ?? EqualityComparer<object>.Default);
  }



  /// <summary>
  /// <see cref="IListRefSegment{T}"/> indexer.
  /// </summary>
  /// <exception cref="IndexOutOfSegmentException">If <paramref name="index"/> is negative or out of segment range.</exception>
  public object? this [ int index ]
  {
    [SuppressMessage ( "Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Expected location." )]
    readonly get
    {
      return ValidateIndex ( ref index, out IndexOutOfSegmentException? e ) ? throw e : list [ index ];
    }
    set
    {
      if (ValidateIndex ( ref index, out IndexOutOfSegmentException? e ))
        throw e;

      list [ index ] = value;
    }
  }

  readonly internal bool ValidateSetup ( int count, out int limit, [NotNullWhen ( true )] out ImpossibleSegmentationException? e )
  {
    return SegmentationValidator.ValidateSetup ( list.Count, offset: offset, count: count, out limit, out e );
  }

  readonly internal bool ValidateIndex ( ref int index, [NotNullWhen ( true )] out IndexOutOfSegmentException? e )
  {
    return SegmentationValidator.ValidateIndex ( index: ref index, offset: offset, limit: limit, count: Count, out e );
  }

  /// <summary>
  /// Sets segment values to <see langword="null"/>.
  /// </summary>
  readonly public void Clear ()
  {
    T list = this.list;
    for (int i = offset ; i < limit ; ++i)
      list [ i ] = default;
  }

  /// <returns>Returns <see langword="true"/> on first equality encounter using <see cref="EqualityComparer"/>.
  /// Otherwise, returns <see langword="false"/>.</returns>
  readonly public bool Contains ( object? item ) => IndexOf ( item ) != -1;

  /// <summary>
  /// Copies segment into destination array.
  /// </summary>
  /// <exception cref="ArgumentNullException">For <see langword="null"/> <paramref name="array"/>.</exception>
  /// <exception cref="ArgumentOutOfRangeException">For negative <paramref name="index"/>.</exception>
  /// <exception cref="ArgumentException">For <paramref name="array"/> with insufficient length.</exception>
  readonly public void CopyTo ( Array array, int index )
  {
    if (array.IsNull ())
      ArgumentNullException.ThrowIfNull ( argument: array );

    if (index < 0)
      throw new ArgumentOutOfRangeException ( paramName: nameof ( index ), index, "Index must be non-negative." );

    if (index + Count > array.Length)
    {
      const string template = "Array length of {0} is insufficient, starting index {1}, segement length {2}.";
      string errMsg = string.Format(template, array.Length, index, Count);
      throw new ArgumentException ( message: errMsg, paramName: nameof ( array ) );
    }

    T list = this.list;
    for (int i = offset ; i < limit ;)
      array.SetValue ( list [ i++ ], index++ );
  }

  /// <returns>
  /// Returns index of item, if found in segment. <c>-1</c> otherwise.
  /// </returns>
  readonly public int IndexOf ( object? item )
  {
    IEqualityComparer<object> comparer = EqualityComparer;
    T list = this.list;
    for (int i = offset ; i < limit ; ++i)
      if (comparer.Equals ( item, list [ i ] ))
        return i - offset;

    return -1;
  }

  /// <summary>
  /// Not supported method.
  /// </summary>
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public void Insert ( int index, object? item ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public void Remove ( object? item ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public void RemoveAt ( int index ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public int Add ( object? item ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public IEnumerator GetEnumerator () => throw new NotSupportedException ();

  /// <summary>
  /// Gets the <see cref="IListRefEnumerator{T}"/> for this segment defined.
  /// </summary>
  readonly public IListRefEnumerator<T> GetRefEnumerator () => new ( list, offset: offset, limit );

  /// <summary>
  /// <see langword="false"/> by default.
  /// </summary>
  override readonly public bool Equals ( object? obj ) => false;

  /// <summary>
  /// This segment hash code.
  /// </summary>
  override readonly public int GetHashCode () => HashCode.Combine ( GetHashCode (), offset, limit );
}