using Software9119.Collection.Superb.Segmentation.Exceptionality;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IListSegment"/> is parallel to <see cref="ArraySegment{T}"/> but generalized
/// to <see cref="IList"/>.
/// </summary>
/// <remarks>
/// Everything comes with a price and for this reason is up to 
/// client code to ensure <see cref="IList"/> passed into <see cref="IListSegment"/> is not
/// mutated in a harmful way, most notably that it is not shrunk beyond segment defined.
/// </remarks>
[SuppressMessage ( "Design", "CA1010:Generic interface should also be implemented", Justification = "Not interested." )]
public struct IListSegment : IList, IEquatable<IListSegment>
{
  readonly Lock syncStump = new ();

  readonly IList list;
  internal int offset;
  internal int limit;

  /// <summary>
  /// Count of items available through this segment.
  /// </summary>
  readonly public int Count => limit - offset;
  /// <summary>
  /// Segment offset to <see cref="List"/>.
  /// </summary>
  readonly public int Offset => offset;

  /// <summary>
  /// See <see cref="IListSegment"/> is not readonly.
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
  readonly public IList List => list;


  IEqualityComparer<object> equalityComparer;

  /// <summary>
  /// Basic constructor.
  /// </summary>  
  /// <param name="equalityComparer">
  /// If <see langword="null"/> passed-in, the <c>EqualityComparer&lt;object&gt;.Default</c>
  /// will be used. See <see cref="EqualityComparer"/> for more.
  /// </param>
  public IListSegment ( IList list, IEqualityComparer<object>? equalityComparer = null )
  {

    this.list = list;
    if (ValidateList ( out ArgumentNullException? ae ))
    {
      throw ae;
    }
    offset = 0;
#pragma warning disable CA1062 // Validate arguments of public methods
    limit = list.Count;
#pragma warning restore CA1062 // Validate arguments of public methods

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
  public IListSegment ( IList list, int offset, int count, IEqualityComparer<object>? equalityComparer = null )
  {
    this.list = list;

    if (ValidateList ( out ArgumentNullException? ae ))
      throw ae;

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
  /// <see cref="IListSegment"/> indexer.
  /// </summary>
  /// <exception cref="IndexOutOfSegmentException">If <paramref name="index"/> is negative or out of segment range.</exception>  
  readonly public object? this [ int index ]
  {
    [SuppressMessage ( "Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Expected location." )]
    get
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

  [MemberNotNullWhen ( false, nameof ( list ) )]
  readonly internal bool ValidateList ( [NotNullWhen ( true )] out ArgumentNullException? e )
  {
    return SegmentationValidator.ValidateList ( list, out e );
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
    if (array == null)
      ArgumentNullException.ThrowIfNull ( argument: array );

    if (index < 0)
      throw new ArgumentOutOfRangeException ( paramName: nameof ( index ), index, "Index must be non-negative." );

    if (index + Count > array.Length)
    {
      const string template = "Array length of {0} is insufficient, starting index {1}, segement length {2}.";
      string errMsg = string.Format(template, array.Length, index, Count);
      throw new ArgumentException ( message: errMsg, paramName: nameof ( array ) );
    }

    IList list = this.list;
    if (list is Array arrSource)
      Array.Copy ( arrSource, offset, array, index, Count );

    for (int i = offset ; i < limit ;)
      array.SetValue ( list [ i++ ], index++ );
  }

  /// <returns>
  /// Returns index of item, if found in segment. <c>-1</c> otherwise.
  /// </returns>
  readonly public int IndexOf ( object? item )
  {
    IEqualityComparer<object> comparer = EqualityComparer;
    IList list = this.list;
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
  /// Gets the <see cref="IListEnumerator"/> for this segment defined.
  /// </summary>
  readonly public IEnumerator GetEnumerator () => new IListEnumerator ( list, offset: offset, limit );


  /// <summary>
  /// In core, it calls to <see cref="Equals(IListSegment)"/>.
  /// </summary>
  override readonly public bool Equals ( object? obj )
  {
    return obj is IListSegment other && Equals ( other );
  }

  /// <summary>
  /// Segment value-equals if referenced lists are referential equal and offset and count are the same.
  /// </summary>
  readonly public bool Equals ( IListSegment other )
  {
    return ReferenceEquals ( list, other.list ) && offset == other.offset && limit == other.limit;
  }

  /// <summary>
  /// Hash code made out of 'signature' values of segment.
  /// </summary>
  override readonly public int GetHashCode () => HashCode.Combine ( list, offset, limit );


  /// <summary>
  /// Calls to <see cref="Equals(IListSegment)"/>.
  /// </summary>
  static public bool operator == ( IListSegment left, IListSegment right )
  {
    return left.Equals ( right );
  }

  /// <summary>
  /// Calls to <see cref="Equals(IListSegment)"/>.
  /// </summary>
  static public bool operator != ( IListSegment left, IListSegment right )
  {
    return left.Equals ( right ) == false;
  }
}