using Software9119.Collection.Superb.Segmentation.Exceptionality;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IListSegment{T}"/> is parallel to <see cref="ArraySegment{T}"/> but generalized
/// to <see cref="IList{T}"/>.
/// </summary>
/// <remarks>
/// Everything comes with a price and for this reason is up to 
/// client code to ensure <see cref="IList{T}"/> passed into <see cref="IListSegment{T}"/> is not
/// mutated in a harmful way, most notably that it is not shrunk beyond segment defined.
/// </remarks>
/// <typeparam name="T"></typeparam>
public struct IListSegment<T> : IList<T?>, IReadOnlyList<T?>, IEquatable<IListSegment<T>>
{
  readonly IList<T?> list;
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
  /// See <see cref="IListSegment{T}"/> is not readonly.
  /// </summary>
  readonly public bool IsReadOnly => false;

  /// <summary>
  /// The <see cref="List{T}"/> over which segmentation occurs.
  /// </summary>
  readonly public IList<T?> List => list;


  IEqualityComparer<T> equalityComparer;

  /// <summary>
  /// Basic constructor.
  /// </summary>  
  /// <param name="equalityComparer">
  /// If <see langword="null"/> passed-in, the <see cref="EqualityComparer{T}.Default"/>
  /// will be used. See <see cref="EqualityComparer"/> for more.
  /// </param>
  public IListSegment ( IList<T?> list, IEqualityComparer<T>? equalityComparer = null )
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
  /// If <see langword="null"/> passed-in, the <see cref="EqualityComparer{T}.Default"/>
  /// will be used. See <see cref="EqualityComparer"/> for more.
  /// </param>
  /// <param name="offset">Starting index of segment.</param>
  /// <param name="count">Number of items to include.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is null.</exception>
  /// <exception cref="ImpossibleSegmentationException">Thrown when the segmention settings are impossible.</exception>
  public IListSegment ( IList<T?> list, int offset, int count, IEqualityComparer<T>? equalityComparer = null )
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
  /// Equality comparer used in <see cref="Contains(T?)"/> and <see cref="IndexOf(T?)"/> methods.
  /// </summary>
  /// <remarks>Cannot be set to <see langword="null"/> because it defaults to <see cref="EqualityComparer{T}.Default"/>.</remarks>
  public IEqualityComparer<T> EqualityComparer
  {
    readonly get
    {
      return equalityComparer;
    }

    [MemberNotNull ( nameof ( equalityComparer ) )]
    set => equalityComparer = (value ?? EqualityComparer<T>.Default);
  }



  /// <summary>
  /// <see cref="IListSegment{T}"/> indexer.
  /// </summary>
  /// <exception cref="IndexOutOfSegmentException">If <paramref name="index"/> is negative or out of segment range.</exception>  
  readonly public T? this [ int index ]
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
    return SegmentationValidator.ValidateList<T> ( list, out e );
  }

  readonly internal bool ValidateIndex ( ref int index, [NotNullWhen ( true )] out IndexOutOfSegmentException? e )
  {
    return SegmentationValidator.ValidateIndex ( index: ref index, in this, out e );
  }

  /// <summary>
  /// Sets segment values to <c>default(T)</c>.
  /// </summary>
  readonly public void Clear ()
  {
    for (int i = offset ; i < limit ; ++i)
      list [ i ] = default;
  }

  /// <returns>Returns <see langword="true"/> on first equality encounter using <see cref="EqualityComparer"/>. 
  /// Otherwise, returns <see langword="false"/>.</returns>
  readonly public bool Contains ( T? item ) => IndexOf ( item ) != -1;

  /// <summary>
  /// Copies segment into destination array.
  /// </summary>  
  /// <exception cref="ArgumentNullException">For <see langword="null"/> <paramref name="array"/>.</exception>
  /// <exception cref="ArgumentOutOfRangeException">For negative <paramref name="arrayIndex"/>.</exception>
  /// <exception cref="ArgumentException">For <paramref name="array"/> with insufficient length.</exception>
  readonly public void CopyTo ( T? [] array, int arrayIndex )
  {
    if (array == null)
      ArgumentNullException.ThrowIfNull ( argument: array );

    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException ( paramName: nameof ( arrayIndex ), arrayIndex, "Index must be non-negative." );

    if (arrayIndex + Count > array.Length)
    {
      const string template = "Array length of {0} is insufficient, starting index {1}, segement length {2}.";
      string errMsg = string.Format(template, array.Length, arrayIndex, Count);
      throw new ArgumentException ( message: errMsg, paramName: nameof ( array ) );
    }

    IList<T?> list = this.list;
    if (list is T [] arrSource)
      Array.Copy ( arrSource, offset, array, arrayIndex, Count );

    if (list is List<T?> listOfT)
      listOfT.CopyTo ( offset, array, arrayIndex, Count );

    for (int i = offset ; i < limit ;)
      array [ arrayIndex++ ] = list [ i++ ];
  }

  /// <returns>
  /// Returns index of item, if found in segment. <c>-1</c> otherwise.
  /// </returns>
  readonly public int IndexOf ( T? item )
  {
    IEqualityComparer<T> comparer = EqualityComparer;
    IList<T?> list = this.list;
    for (int i = offset ; i < limit ; ++i)
      if (comparer.Equals ( item, list [ i ] ))
        return i - offset;

    return -1;
  }

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public void Insert ( int index, T? item ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public bool Remove ( T? item ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public void RemoveAt ( int index ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public void Add ( T? item ) => throw new NotSupportedException ();


  /// <summary>
  /// Gets the <see cref="IListEnumerator{T}"/> for this segment defined.
  /// </summary>
  readonly IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();

  /// <summary>
  /// Gets the <see cref="IListEnumerator{T}"/> for this segment defined.
  /// </summary>
  readonly public IEnumerator<T> GetEnumerator () => new IListEnumerator<T> ( list, offset: offset, limit );


  /// <summary>
  /// In core, it calls to <see cref="Equals(IListSegment{T})"/>.
  /// </summary>
  override readonly public bool Equals ( object? obj )
  {
    return obj is IListSegment<T> other && Equals ( other );
  }

  /// <summary>
  /// Segment value-equals if referenced lists are referential equal and offset and count are the same.
  /// </summary>
  readonly public bool Equals ( IListSegment<T> other )
  {
    return ReferenceEquals ( list, other.list ) && offset == other.offset && limit == other.limit;
  }

  /// <summary>
  /// Hash code made out of 'signature' values of segment.
  /// </summary>
  override readonly public int GetHashCode () => HashCode.Combine ( list, offset, limit );


  /// <summary>
  /// Calls to <see cref="Equals(IListSegment{T})"/>.
  /// </summary>
  static public bool operator == ( IListSegment<T> left, IListSegment<T> right )
  {
    return left.Equals ( right );
  }

  /// <summary>
  /// Calls to <see cref="Equals(IListSegment{T})"/>.
  /// </summary>
  static public bool operator != ( IListSegment<T> left, IListSegment<T> right )
  {
    return left.Equals ( right ) == false;
  }
}