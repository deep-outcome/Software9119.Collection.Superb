using Software9119.Collection.Superb.Numerics;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Indexing;

/// <summary>
/// Indexing validator contains useful methods for working with indexes.
/// </summary>
static public class IndexingValidator
{
  /// <summary>
  /// Computes exclusive upper bound for <paramref name="count"/> and <paramref name="offset"/>.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public int LimitOutOf ( NonNegativeInt32 offset, NonNegativeInt32 count ) => offset + count;

  /// <summary>
  /// Computes offseted index.
  /// </summary>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public int CorrelateIndex ( NonNegativeInt32 index, NonNegativeInt32 offset ) => index + offset;

  /// <summary>
  /// Computes inclusive difference of <paramref name="from"/> and <paramref name="count"/>.
  /// </summary>
  /// <remarks>
  /// Usage example
  /// <code>
  /// int[] items = [1,2,3,4,5,6,7,8,9,10];
  /// int itemsToEndCount = IndexingValidator.IndexToCountInclusiveDifference ( 5, 10 );
  /// Array.Clear(items, 5, itemsToEndCount );
  /// // [ 1, 2, 3, 4, 5, 0, 0, 0, 0, 0 ]
  /// </code>
  /// </remarks>
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public int IndexToCountInclusiveDifference ( NonNegativeInt32 from, NonNegativeInt32 count ) => count - from;

  /// <summary>
  /// Validates <paramref name="index"/> is valid for target <paramref name="count"/>.
  /// </summary>
  /// <returns><see langword="true"/> if index is invalid.</returns>
  static public bool ValidateIndex
  (
    NonNegativeInt32 index,
    NonNegativeInt32 count,
   [NotNullWhen ( true )] out IndexOutOfBoundariesException? e,
   string? paramName = null
  )
  {
    if (index >= count)
    {
      e = IndexOutOfBoundariesException.OutOfBoundsMsg ( index: index, count, paramName );
      return true;
    }

    e = null;
    return false;
  }

  /// <summary>
  /// Validates <paramref name="index"/> is not negative and valid for target <paramref name="count"/>.
  /// </summary>
  /// <returns><see langword="true"/> if index is invalid.</returns>
  static public bool ValidateIndex
  (
    int index,
    NonNegativeInt32 count,
    [NotNullWhen ( true )] out IndexOutOfBoundariesException? e,
    string? paramName = null
  )
  {
    if (index < 0)
    {
      e = IndexOutOfBoundariesException.NegativeIndexMsg ( index, paramName );
      return true;
    }

    return ValidateIndex ( ((NonNegativeInt32) index), count, out e, paramName );
  }

  /// <summary>
  /// Validates insertion <paramref name="index"/> is not negative and valid for target <paramref name="count"/>.
  /// </summary>
  /// <returns><see langword="true"/> if index is invalid.</returns>
  static public bool ValidateInsertionIndex
  (
    int index,
    NonNegativeInt32 count,
    [NotNullWhen ( true )] out IndexOutOfBoundariesException? e,
    string? paramName = null
  )
  {
    if (index < 0)
    {
      e = IndexOutOfBoundariesException.NegativeIndexMsg ( index, paramName );
      return true;
    }

    if (index > count)
    {
      e = IndexOutOfBoundariesException.OutOfBoundsForInsertionMsg ( index: index, count, paramName );
      return true;
    }

    e = null;
    return false;
  }

  /// <summary>
  /// <list type="bullet">
  /// <item>
  /// Validates <paramref name="index"/> non-negativity and validity for source of length/count <paramref name="count"/>.
  /// </item>
  /// <item>
  /// When <paramref name="index"/> is valid, it is incremented with <paramref name="offset"/>.
  /// </item>
  /// </list>
  /// </summary>
  /// <returns><see langword="true"/> when <paramref name="index"/> is invalid.</returns>
  static public bool ValidateIndex (
    ref int index,
    NonNegativeInt32 offset,
    NonNegativeInt32 count,
    [NotNullWhen ( true )] out IndexOutOfBoundariesException? e )
  {
    if (index < 0)
    {
      e = IndexOutOfBoundariesException.NegativeIndexMsg ( index );
      return true;
    }

    if (index >= count)
    {
      e = IndexOutOfBoundariesException.OutOfBoundsMsg ( index, available: count );
      return true;
    }

    index = CorrelateIndex ( index, offset );

    e = null;
    return false;
  }

  /// <summary>
  /// Validates whether <paramref name="count"/> and <paramref name="offset"/> create
  /// valid segmentation over source of length/count <paramref name="available"/>.
  /// </summary>
  /// <returns>
  /// <list type="bullet">
  /// <item><c>-1</c> – for empty segment</item>
  /// <item><c>0</c>  – for valid segment</item>
  /// <item><c>1</c>  – for invalid segment</item>
  /// </list>
  /// </returns>
  /// <param name="limit">Is exclusive upper bound for <paramref name="offset"/> and <paramref name="count"/>.</param>
  /// <remarks>
  /// Beware of <b>false allowance</b> for empty segments.
  /// <code>
  /// ┌───────────┬────────┬───────┬──────────────────┐
  /// │ available │ offset │ count │      valid       │
  /// ├───────────┼────────┼───────┼──────────────────┤
  /// │         0 │      0 │     0 │ valid by formula │
  /// │         1 │      1 │     0 │ valid by formula │
  /// │         1 │      0 │     1 │ completely valid │
  /// │         1 │      2 │     0 │ invalid (empty)  │
  /// │         5 │      5 │     0 │ valid by formula │
  /// │         5 │      0 │     5 │ completely valid │
  /// │         5 │      1 │     4 │ completely valid │
  /// │         5 │      4 │     1 │ completely valid │
  /// │         5 │      6 │     0 │ invalid (empty)  │
  /// └───────────┴────────┴───────┴──────────────────┘
  /// </code>
  /// <list type="bullet">
  /// <item>Segment <c>|offset,count|</c> translates to half-open interval <c>[offset, offset+count)</c>.</item>
  /// <item>
  /// <paramref name="offset"/> is always validated to be less than <paramref name="available"/> with exception
  /// for case <c>|offset=available,0| → [available,available) = [available,available-1]</c>.
  /// </item>
  /// <item>
  /// See <see cref="ValidateSegmentationStrict(NonNegativeInt32, NonNegativeInt32, NonNegativeInt32, out int, out ImpSegExc?)"/>
  /// for less permissive version of this validation method.
  /// </item>
  /// </list>
  /// </remarks>
  static public int ValidateSegmentation
  (
    NonNegativeInt32 available,
    NonNegativeInt32 offset,
    NonNegativeInt32 count,
    out int limit,
    out ImpossibleSegmentationException? e,
    Func<string []>? parametersGetter = null
  )
  {
    limit = LimitOutOf ( offset, count );

    if (limit > available)
    {
      string []? parameters = parametersGetter?.Invoke();
      e = ImpossibleSegmentationException.ForwardSegmentationMsg ( available: available, offset: offset, count, parameters );
      return 1;
    }

    if (count == 0)
    {
      e = null;
      return -1;
    }

    e = null;
    return 0;
  }

  /// <summary>
  /// <list type="bullet">
  /// <item>
  /// Validates whether <paramref name="count"/> and <paramref name="offset"/> create
  /// valid segmentation over source of length/count <paramref name="available"/>.
  /// </item>
  /// <item><paramref name="offset"/> is never considered valid unless less than <paramref name="available"/> with exception for
  /// <paramref name="available"/> = 0 = <paramref name="offset"/>.
  /// </item>
  /// <item>
  /// This means that segment <c>|offset,count|</c> which translates to interval <c>[start=offset, end=offset+count)</c> is
  /// valid only when <c>start</c> &lt; <c>end = length</c> with exception for <c>|0,0| → [0, 0)</c>.
  /// </item>
  /// <item>See <see cref="ValidateSegmentation(NonNegativeInt32, NonNegativeInt32, NonNegativeInt32, out int, out ImpSegExc?, Func{string[]}?)"/>
  /// for more information.
  /// </item>
  /// </list>
  /// </summary>
  /// <returns>
  /// <list type="bullet">
  /// <item><c>-1</c> – for empty segment</item>
  /// <item><c>0</c>  – for valid segment</item>
  /// <item><c>1</c>  – for invalid segment</item>
  /// </list>
  /// </returns>
  /// <param name="limit">Is exclusive upper bound for <paramref name="offset"/> and <paramref name="count"/>.</param>
  [SuppressMessage ( "Style", "IDE0047:Remove unnecessary parentheses", Justification = "Who remembers precedence of logical operators?" )]
  static public int ValidateSegmentationStrict
  (
    NonNegativeInt32 available,
    NonNegativeInt32 offset,
    NonNegativeInt32 count,
    out int limit,
    out ImpossibleSegmentationException? e
  )
  {
    limit = LimitOutOf ( offset, count );

    if ((offset == available && offset != 0) || limit > available)
    {
      e = ImpossibleSegmentationException.ForwardSegmentationMsg ( available: available, offset: offset, count );
      return 1;
    }

    if (count == 0)
    {
      e = null;
      return -1;
    }

    e = null;
    return 0;
  }

  /// <summary>
  /// Validates whether <paramref name="count"/> and <paramref name="rearSet"/> create
  /// valid backward segmentation over source of length/count <paramref name="available"/>.
  /// </summary>
  /// <returns>
  /// <list type="bullet">
  /// <item><c>-1</c> – for empty segment</item>
  /// <item><c>0</c>  – for valid segment</item>
  /// <item><c>1</c>  – for invalid segment</item>
  /// </list>
  /// </returns>
  /// <remarks>
  /// Beware of <b>false allowance</b> for empty segments.
  /// <code>
  /// ┌───────────┬────────┬───────┬──────────────────┐
  /// │ available │ offset │ count │      valid       │
  /// ├───────────┼────────┼───────┼──────────────────┤
  /// │         0 │      0 │     0 │ valid by formula │
  /// │         1 │      1 │     0 │ valid by formula │
  /// │         1 │      0 │     1 │ completely valid │
  /// │         1 │      2 │     0 │ invalid (empty)  │
  /// │         5 │      5 │     0 │ valid by formula │
  /// │         5 │      4 │     5 │ completely valid │
  /// │         5 │      4 │     1 │ completely valid │
  /// │         5 │      6 │     0 │ invalid (empty)  │
  /// └───────────┴────────┴───────┴──────────────────┘
  /// </code>
  /// <list type="bullet">
  /// <item>Segment <c>|offset,count|</c> translates to half-open interval <c>[offset, offset+count)</c>.</item>
  /// <item>
  /// <paramref name="rearSet"/> is always validated to be less than <paramref name="available"/> with exception
  /// for case <c>|offset=available,0| → [available,available) = [available,available-1]</c>.
  /// </item>
  /// </list>
  /// </remarks>
  [SuppressMessage ( "Style", "IDE0047:Remove unnecessary parentheses", Justification = "" )]
  static public int ValidateBackwardSegmentation (
    NonNegativeInt32 available,
    NonNegativeInt32 rearSet,
    NonNegativeInt32 count,
    [NotNullWhen ( true )] out ImpSegExc? e,
    Func<string []>? parametersGetter = null
  )
  {
    bool empty = count == 0;
    if (rearSet > available || rearSet + 1 < count || (rearSet == available && empty == false))
    {
      string []? parameters = parametersGetter?.Invoke();
      e = ImpossibleSegmentationException.BackwardSegmentationMsg ( available, rearSet, count, parameters );
      return 1;
    }

    e = null;
    if (empty)
      return -1;

    return 0;
  }
}
