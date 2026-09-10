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

  /// <summary>
  /// Creates <see cref="ListDictionary"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.ListDictionary{Item}(Func{Item, object}, IComparer)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ListDictionary? IntoListDictionary<Item> (
    this IEnumerable<Item>? enumerable,
    Func<Item, object> keySelector,
    IComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
  {
    keyComparer ??= Comparer.Default;
    return enumerable.AsOrTo ( c_specialized.ListDictionary ( keySelector, keyComparer ), behavior );
  }

  /// <summary>
  /// Creates <see cref="ListDictionary"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.ListDictionary{Item}(Func{Item, object}, IComparer)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ListDictionary? IntoListDictionary (
    this IEnumerable? enumerable,
    Func<object, object> keySelector,
    IComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
  {
    keyComparer ??= Comparer.Default;
    return enumerable.AsOrTo ( c_specialized.ListDictionary ( keySelector, keyComparer ), behavior );
  }

  /// <summary>
  /// Creates <see cref="ListDictionary"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.ListDictionary{Item}(Func{Item, object}, Func{Item, object?}, IComparer)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ListDictionary? IntoListDictionary<Item> (
    this IEnumerable<Item>? enumerable,
    Func<Item, object> keySelector,
    Func<Item, object?> valueSelector,
    IComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
  {
    keyComparer ??= Comparer.Default;
    return enumerable.AsOrTo ( c_specialized.ListDictionary ( keySelector, valueSelector, keyComparer ), behavior );
  }

  /// <summary>
  /// Creates <see cref="ListDictionary"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.ListDictionary{Item}(Func{Item, object}, Func{Item, object?}, IComparer)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ListDictionary? IntoListDictionary (
    this IEnumerable? enumerable,
    Func<object, object> keySelector,
    Func<object, object?> valueSelector,
    IComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
  {
    keyComparer ??= Comparer.Default;
    return enumerable.AsOrTo ( c_specialized.ListDictionary ( keySelector, valueSelector, keyComparer ), behavior );
  }

  /// <summary>
  /// Creates <see cref="NameValueCollection "/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.NameValueCollection{Item}(Func{Item, string}, Func{Item, string?}, int?, IEqualityComparer)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults default comparer of <see cref="NameValueCollection"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public NameValueCollection? IntoNameValueCollection<Item> (
    this IEnumerable<Item>? enumerable,
    Func<Item, string> keySelector,
    Func<Item, string?> valueSelector,
    int? capacity = null,
    IEqualityComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
  {
    return enumerable.AsOrTo ( c_specialized.NameValueCollection ( keySelector, valueSelector, capacity, keyComparer ), behavior );
  }

  /// <summary>
  /// Creates <see cref="NameValueCollection "/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.NameValueCollection{Item}(Func{Item, string}, Func{Item, string?}, int?, IEqualityComparer)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults default comparer of <see cref="NameValueCollection"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public NameValueCollection? IntoNameValueCollection (
    this IEnumerable? enumerable,
    Func<object, string> keySelector,
    Func<object, string?> valueSelector,
    int? capacity = null,
    IEqualityComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
  {
    return enumerable.AsOrTo ( c_specialized.NameValueCollection ( keySelector, valueSelector, capacity, keyComparer ), behavior );
  }

  /// <summary>
  /// Creates <see cref="OrderedDictionary "/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>  
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.OrderedDictionary{Item}(Func{Item, object}, int?, IEqualityComparer)"/>.  
  /// </remarks>
  static public OrderedDictionary? IntoOrderedDictionary<Item> (
    this IEnumerable<Item>? enumerable,
    Func<Item, object> keySelector,
    int? capacity = null,
    IEqualityComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( c_specialized.OrderedDictionary ( keySelector, capacity, keyComparer ), behavior );

  /// <summary>
  /// Creates <see cref="OrderedDictionary "/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>  
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.OrderedDictionary{Item}(Func{Item, object}, int?, IEqualityComparer)"/>.  
  /// </remarks>
  static public OrderedDictionary? IntoOrderedDictionary (
    this IEnumerable? enumerable,
    Func<object, object> keySelector,
    int? capacity = null,
    IEqualityComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( c_specialized.OrderedDictionary ( keySelector, capacity, keyComparer ), behavior );

  /// <summary>
  /// Creates <see cref="OrderedDictionary "/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>  
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.OrderedDictionary{Item}(Func{Item, object}, Func{Item, object?}, int?, IEqualityComparer)"/>.  
  /// </remarks>
  static public OrderedDictionary? IntoOrderedDictionary<Item> (
    this IEnumerable<Item>? enumerable,
    Func<Item, object> keySelector,
    Func<Item, object?> valueSelector,
    int? capacity = null,
    IEqualityComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( c_specialized.OrderedDictionary ( keySelector, valueSelector, capacity, keyComparer ), behavior );

  /// <summary>
  /// Creates <see cref="OrderedDictionary "/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>  
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/>
  /// with <see cref="c_specialized.OrderedDictionary{Item}(Func{Item, object}, Func{Item, object?}, int?, IEqualityComparer)"/>.  
  /// </remarks>
  static public OrderedDictionary? IntoOrderedDictionary (
    this IEnumerable? enumerable,
    Func<object, object> keySelector,
    Func<object, object?> valueSelector,
    int? capacity = null,
    IEqualityComparer? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( c_specialized.OrderedDictionary ( keySelector, valueSelector, capacity, keyComparer ), behavior );
}
