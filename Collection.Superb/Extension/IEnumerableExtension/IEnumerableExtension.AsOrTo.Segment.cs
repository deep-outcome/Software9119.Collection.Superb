using System;
using Software9119.Collection.Superb.Segmentation;

using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{

  /// <summary>
  /// Casts <paramref name="enumerable"/> directly into <see cref="IListSegment"/>, or casts or copies <paramref name="enumerable"/>
  /// into intermediate <see cref="IList"/> before wrapping into <see cref="IListSegment"/>.
  /// </summary>  
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="segment_type.IListSegment(int?, bool, IEqualityComparer{object}?)"/>.
  /// </remarks>
  /// <param name="capacity">Capacity for intermediate <see cref="IList"/>.</param>.
  /// <param name="trimCapacity">Whether to trim excess capacity of created intermediate <see cref="IList"/>.</param>
  /// <param name="equalityComparer">When <see langword="null"/>, it defaults to <c>EqualityComparer&lt;object&gt;.Default</c>.</param>
  static public IListSegment AsOrToIListSegment<Item> (
    this IEnumerable<Item?>? enumerable,
    int? capacity = null,
    bool trimCapacity = false,
    IEqualityComparer<object?>? equalityComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    equalityComparer ??= EqualityComparer<object?>.Default;
    return enumerable.AsOrTo ( segment_type.IListSegment ( capacity, trimCapacity, equalityComparer ), behavior );
  }

  /// <summary>
  /// Casts <paramref name="enumerable"/> directly into <see cref="IListSegment"/>, or casts or copies <paramref name="enumerable"/>
  /// into intermediate <see cref="IList"/> before wrapping into <see cref="IListSegment"/>.
  /// </summary>  
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="segment_type.IListSegment(int?, bool, IEqualityComparer{object}?)"/>.
  /// </remarks>
  /// <param name="capacity">Capacity for intermediate <see cref="IList"/>.</param>.
  /// <param name="trimCapacity">Whether to trim excess capacity of created intermediate <see cref="IList"/>.</param>
  /// <param name="equalityComparer">When <see langword="null"/>, it defaults to <c>EqualityComparer&lt;object&gt;.Default</c>.</param>
  static public IListSegment AsOrToIListSegment (
    this IEnumerable enumerable,
    int? capacity = null,
    bool trimCapacity = false,
    IEqualityComparer<object?>? equalityComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    equalityComparer ??= EqualityComparer<object?>.Default;
    return enumerable.AsOrTo ( segment_type.IListSegment ( capacity, trimCapacity, equalityComparer ), behavior );
  }

  /// <summary>
  /// Casts <paramref name="enumerable"/> directly into <see cref="IListSegment{Item}"/>, or casts or copies <paramref name="enumerable"/>
  /// into intermediate <see cref="IList{Item}"/> before wrapping into <see cref="IListSegment{Item}"/>.
  /// </summary>  
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="segment_type.IListSegment{Item}(int?, bool, IEqualityComparer{Item})"/>.
  /// </item>
  /// <item>
  /// See <see cref="AsOrToIList{Item}(IEnumerable{Item}, int?, bool, NullBehavior)"/> for details on intermediate <see cref="IList{Item}"/>.
  /// </item>
  /// </list>
  /// </remarks>
  /// <param name="capacity">Capacity for intermediate <see cref="IList{Item}"/>.</param>.
  /// <param name="trimCapacity">Whether to trim excess capacity of created intermediate <see cref="IList{Item}"/>.</param>
  /// <param name="equalityComparer">When <see langword="null"/>, it defaults to <c>EqualityComparer&lt;object&gt;.Default</c>.</param>
  static public IListSegment<Item?> AsOrToTypedIListSegment<Item> (
    this IEnumerable<Item?>? enumerable,
    int? capacity = null,
    bool trimCapacity = false,
    IEqualityComparer<Item?>? equalityComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    equalityComparer ??= EqualityComparer<Item?>.Default;
    return enumerable.AsOrTo ( segment_type.IListSegment ( capacity, trimCapacity, equalityComparer ), behavior );
  }

  /// <summary>
  /// Casts <paramref name="enumerable"/> directly into <see cref="IReadOnlyListSegment{Item}"/>,
  /// or casts or copies <paramref name="enumerable"/> into intermediate <see cref="IList{Item}"/> 
  /// and warrants <see cref="IReadOnlyList{Item}"/> before wrapping into <see cref="IReadOnlyListSegment{Item}"/>.
  /// </summary>  
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="segment_type.IReadOnlyListSegment{Item}(int?, bool, IEqualityComparer{Item})"/>.
  /// </item>
  /// <item>
  /// See <see cref="AsOrToIList{Item}(IEnumerable{Item}, int?, bool, NullBehavior)"/> for details on intermediate <see cref="IList{Item}"/>.
  /// </item>
  /// </list>
  /// </remarks>
  /// <param name="capacity">Capacity for intermediate <see cref="IList{Item}"/>.</param>.
  /// <param name="trimCapacity">Whether to trim excess capacity of created intermediate <see cref="IList{Item}"/>.</param>
  /// <param name="equalityComparer">When <see langword="null"/>, it defaults to <c>EqualityComparer&lt;object&gt;.Default</c>.</param>
  static public IReadOnlyListSegment<Item?> AsOrToIReadOnlyListSegment<Item> (
    this IEnumerable<Item?>? enumerable,
    int? capacity = null,
    bool trimCapacity = false,
    IEqualityComparer<Item?>? equalityComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    equalityComparer ??= EqualityComparer<Item?>.Default;
    return enumerable.AsOrTo ( segment_type.IReadOnlyListSegment ( capacity, trimCapacity, equalityComparer ), behavior );
  }

  /// <summary>
  /// Casts <paramref name="enumerable"/> directly into <see cref="ArraySegment{Item}"/>,
  /// or casts or copies <paramref name="enumerable"/> into intermediate <see cref="Array"/> 
  /// before wrapping into <see cref="ArraySegment{Item}"/>.
  /// </summary>  
  /// <remarks>  
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="segment_type.ArraySegment{Item}"/>.
  /// </remarks>
  static public ArraySegment<Item?> AsOrToArraySegment<Item> (
    this IEnumerable<Item?>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  ) => enumerable.AsOrTo ( segment_type.ArraySegment<Item> (), behavior );

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into intermediate <see cref="Array"/> 
  /// before wrapping into <see cref="Memory{Item}"/>.
  /// </summary>  
  /// <remarks>  
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="segment_type.Memory{Item}"/>.
  /// </remarks>
  static public Memory<Item?> IntoMemory<Item> (
    this IEnumerable<Item?>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  ) => enumerable.AsOrTo ( segment_type.Memory<Item> (), behavior );

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into intermediate <see cref="Array"/> 
  /// before wrapping into <see cref="ReadOnlyMemory{Item}"/>.
  /// </summary>  
  /// <remarks>  
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="segment_type.ReadOnlyMemory{Item}"/>.
  /// </remarks>
  static public ReadOnlyMemory<Item?> IntoReadOnlyMemory<Item> (
    this IEnumerable<Item?>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  ) => enumerable.AsOrTo ( segment_type.ReadOnlyMemory<Item> (), behavior );
}
