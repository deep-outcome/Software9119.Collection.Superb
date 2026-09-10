using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using c_specialized = Software9119.Collection.Superb.Extension.system_collections_specialized;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Creates <see cref="HybridDictionary"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.HybridDictionary{Item}(Func{Item, object}, int?, bool)"/>.
  /// </remarks>
  static public HybridDictionary? IntoHybridDictionary<Item> (
    this IEnumerable<Item>? enumerable,
    Func<Item, object> keySelector,
    int? capacity = null,
    bool caseSensitive = true,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( c_specialized.HybridDictionary ( keySelector, capacity, !caseSensitive ), behavior );

  /// <summary>
  /// Creates <see cref="HybridDictionary"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.HybridDictionary{Item}(Func{Item, object}, int?, bool)"/>.
  /// </remarks>
  static public HybridDictionary? IntoHybridDictionary (
    this IEnumerable? enumerable,
    Func<object, object> keySelector,
    int? capacity = null,
    bool caseSensitive = true,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( c_specialized.HybridDictionary ( keySelector, capacity, !caseSensitive ), behavior );

  /// <summary>
  /// Creates <see cref="HybridDictionary"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/>
  /// and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_specialized.HybridDictionary{Item}(Func{Item, object}, Func{Item, object?}, int?, bool)"/>.
  /// </remarks>
  static public HybridDictionary? IntoHybridDictionary<Item> (
    this IEnumerable<Item>? enumerable,
    Func<Item, object> keySelector,
    Func<Item, object?> valueSelector,
    int? capacity = null,
    bool caseSensitive = true,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( c_specialized.HybridDictionary ( keySelector, valueSelector, capacity, !caseSensitive ), behavior );

  /// <summary>
  /// Creates <see cref="HybridDictionary"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/>
  /// and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_specialized.HybridDictionary{Item}(Func{Item, object}, Func{Item, object?}, int?, bool)"/>.
  /// </remarks>
  static public HybridDictionary? IntoHybridDictionary (
    this IEnumerable? enumerable,
    Func<object, object> keySelector,
    Func<object, object?> valueSelector,
    int? capacity = null,
    bool caseSensitive = true,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( c_specialized.HybridDictionary ( keySelector, valueSelector, capacity, !caseSensitive ), behavior );
}
