using Software9119.Collection.Superb.Segmentation.Exceptionality;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Segmentation;

/// <summary>
/// <see cref="IReadOnlyListRefSegment{T, U}"/> is parallel to <see cref="ArraySegment{U}"/> but generalized
/// to <see cref="IReadOnlyList{U}"/>.
/// </summary>
/// <remarks>
/// Everything comes with a price and for this reason is up to 
/// client code to ensure <see cref="IReadOnlyList{U}"/> passed into <see cref="IReadOnlyListRefSegment{T, U}"/> is not
/// mutated in a harmful way, most notably that it is not shrunk beyond segment defined.
/// </remarks>
public ref struct IReadOnlyListRefSegment<T, U> : IList<U?>, IReadOnlyList<U?>
  where T : struct, IReadOnlyList<U?>, allows ref struct
{
  internal T list;
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
  /// See <see cref="IReadOnlyListRefSegment{T, U}"/> is not readonly.
  /// </summary>
  readonly public bool IsReadOnly => false;

  /// <summary>
  /// The <see cref="IReadOnlyList{U}"/> over which segmentation occurs.
  /// </summary>
  readonly public T List => list;


  IEqualityComparer<U> equalityComparer;

  /// <summary>
  /// Basic constructor.
  /// </summary>  
  /// <param name="equalityComparer">
  /// If <see langword="null"/> passed-in, the <see cref="EqualityComparer{T}.Default"/>
  /// will be used. See <see cref="EqualityComparer"/> for more.
  /// </param>
  public IReadOnlyListRefSegment ( T list, IEqualityComparer<U>? equalityComparer = null )
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
  /// If <see langword="null"/> passed-in, the <see cref="EqualityComparer{T}.Default"/>
  /// will be used. See <see cref="EqualityComparer"/> for more.
  /// </param>
  /// <param name="offset">Starting index of segment.</param>
  /// <param name="count">Number of items to include.</param>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is null.</exception>
  /// <exception cref="ImpossibleSegmentationException">Thrown when the segmention settings are impossible.</exception>
  public IReadOnlyListRefSegment ( T list, int offset, int count, IEqualityComparer<U>? equalityComparer = null )
  {
    this.list = list;
    this.offset = offset;

    if (ValidateSetup ( count, out int limit, out ImpossibleSegmentationException? e ))
      throw e;

    this.limit = limit;

    EqualityComparer = equalityComparer!;
  }


  /// <summary>
  /// Equality comparer used in <see cref="Contains(U?)"/> and <see cref="IndexOf(U?)"/> methods.
  /// </summary>
  /// <remarks>Cannot be set to <see langword="null"/> because it defaults to <see cref="EqualityComparer{T}.Default"/>.</remarks>
  public IEqualityComparer<U> EqualityComparer
  {
    readonly get
    {
      return equalityComparer;
    }

    [MemberNotNull ( nameof ( equalityComparer ) )]
    set => equalityComparer = (value ?? EqualityComparer<U>.Default);
  }



  /// <summary>
  /// <see cref="IReadOnlyListRefSegment{T, U}"/> indexer.
  /// </summary>
  /// <exception cref="IndexOutOfSegmentException">If <paramref name="index"/> is negative or out of segment range.</exception>  
  /// <exception cref="NotSupportedException">On setter call.</exception>
  readonly public U? this [ int index ]
  {
    [SuppressMessage ( "Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Expected location." )]
    get
    {
      return ValidateIndex ( ref index, out IndexOutOfSegmentException? e ) ? throw e : list [ index ];
    }
    set => throw new NotSupportedException ();
  }

  readonly internal bool ValidateSetup ( int count, out int limit, [NotNullWhen ( true )] out ImpossibleSegmentationException? e )
  {
    return SegmentationValidator.ValidateSetup ( list.Count, offset: offset, count: count, out limit, out e );
  }

  readonly internal bool ValidateIndex ( ref int index, [NotNullWhen ( true )] out IndexOutOfSegmentException? e )
  {
    return SegmentationValidator.ValidateIndex ( index: ref index, offset: offset, limit: limit, count: Count, out e );
  }

  /// <returns>Returns <see langword="true"/> on first equality encounter using <see cref="EqualityComparer"/>. 
  /// Otherwise, returns <see langword="false"/>.</returns>
  readonly public bool Contains ( U? item ) => IndexOf ( item ) != -1;

  /// <summary>
  /// Copies segment into destination array.
  /// </summary>  
  /// <exception cref="ArgumentNullException">For <see langword="null"/> <paramref name="array"/>.</exception>
  /// <exception cref="ArgumentOutOfRangeException">For negative <paramref name="arrayIndex"/>.</exception>
  /// <exception cref="ArgumentException">For <paramref name="array"/> with insufficient length.</exception>
  readonly public void CopyTo ( U? [] array, int arrayIndex )
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

    T list = this.list;
    for (int i = offset ; i < limit ;)
      array [ arrayIndex++ ] = list [ i++ ];
  }

  /// <returns>
  /// Returns index of item, if found in segment. <c>-1</c> otherwise.
  /// </returns>
  readonly public int IndexOf ( U? item )
  {
    IEqualityComparer<U> comparer = EqualityComparer;
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
  readonly public void Insert ( int index, U? item ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public bool Remove ( U? item ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public void RemoveAt ( int index ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public void Add ( U? item ) => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public void Clear () => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly IEnumerator IEnumerable.GetEnumerator () => throw new NotSupportedException ();

  /// <summary>
  /// Not supported method.
  /// </summary>  
  /// <exception cref="NotSupportedException">At call.</exception>
  readonly public IEnumerator<U> GetEnumerator () => throw new NotSupportedException ();

  /// <summary>
  /// Gets the <see cref="IReadOnlyListRefEnumerator{T, U}"/> for this segment defined.
  /// </summary>
  readonly public IReadOnlyListRefEnumerator<T, U> GetRefEnumerator () => new ( list, offset: offset, limit );

  /// <summary>
  /// <see langword="false"/> by default.
  /// </summary>
  override readonly public bool Equals ( object? obj ) => false;

  /// <summary>
  /// This segment hash code.
  /// </summary>
  override readonly public int GetHashCode () => HashCode.Combine ( GetHashCode (), offset, limit );
}