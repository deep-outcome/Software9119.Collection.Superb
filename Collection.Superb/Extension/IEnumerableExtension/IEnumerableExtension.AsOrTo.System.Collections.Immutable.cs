using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

using collections_immutable = Software9119.Collection.Superb.Extension.system_collections_immutable;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ImmutableArray{Item}"/>.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_immutable.ImmutableArray{Item}(bool)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="length"/> is specified and <paramref name="enforceLengthCountMatch"/> is set to <see langword="true"/>
  /// and <paramref name="enumerable"/> is not castable and its count does not equal to <paramref name="length"/>,
  /// <see cref="System.InvalidOperationException"/> is thrown from within infrastructure.
  /// </item>
  /// </list>
  /// </remarks>
  static public ImmutableArray<Item> AsOrToImmutableArray<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? length = null,
    bool enforceLengthCountMatch = false,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ImmutableArray<Item>> targetType = collections_immutable.ImmutableArray<Item>(enforceLengthCountMatch);
    return enumerable.AsOrTo ( targetType, length, behavior );
  }

  /// <summary>
  /// Creates <see cref="ImmutableDictionary{Key, Value}"/> with <paramref name="keyComparer"/> and <paramref name="itemComparer"/>
  /// from <paramref name="enumerable"/> using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_immutable.ImmutableDictionary{Item, Key}(Func{Item, Key}, IEqualityComparer{Key}, IEqualityComparer{Item})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// <item>
  /// When <paramref name="itemComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Item}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ImmutableDictionary<Key, Item>? IntoImmutableDictionary<Item, Key> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    IEqualityComparer<Key>? keyComparer = null,
    IEqualityComparer<Item>? itemComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    itemComparer ??= EqualityComparer<Item>.Default;

    AsOrToTargetType<ImmutableDictionary<Key, Item>> targetType = collections_immutable.ImmutableDictionary
    (
      keySelector,
      keyComparer,
      itemComparer
    );
    return enumerable.AsOrTo ( targetType, null, behavior );
  }

  /// <summary>
  /// Creates <see cref="ImmutableDictionary{Key, Value}"/> with <paramref name="keyComparer"/> and <paramref name="valueComparer"/>
  /// from <paramref name="enumerable"/> using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_immutable.ImmutableDictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key}, IEqualityComparer{Value})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// <item>
  /// When <paramref name="valueComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Value}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ImmutableDictionary<Key, Value>? IntoImmutableDictionary<Item, Key, Value> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IEqualityComparer<Key>? keyComparer = null,
    IEqualityComparer<Value>? valueComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    valueComparer ??= EqualityComparer<Value>.Default;

    AsOrToTargetType<ImmutableDictionary<Key, Value>> targetType = collections_immutable.ImmutableDictionary
    (
      keySelector,
      valueSelector,
      keyComparer,
      valueComparer
    );
    return enumerable.AsOrTo ( targetType, null, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ImmutableHashSet{Item}"/> provided with <paramref name="itemComparer"/>.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_immutable.ImmutableHashSet{Item}(IEqualityComparer{Item})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="itemComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Item}.Default"/>.
  /// </item>
  /// <item>
  /// Cast is allowed only when source <see cref="IEnumerable"/> is <see cref="ImmutableHashSet{Item}"/> and <paramref name="itemComparer"/>
  /// referentially equals to <see cref="ImmutableHashSet{Item}.KeyComparer"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ImmutableHashSet<Item>? AsOrToImmutableHashSet<Item>
  (
    this IEnumerable<Item>? enumerable,
    IEqualityComparer<Item>? itemComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    itemComparer ??= EqualityComparer<Item>.Default;
    AsOrToTargetType<ImmutableHashSet<Item>> targetType = collections_immutable.ImmutableHashSet(itemComparer);

    return enumerable.AsOrTo ( targetType, null, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ImmutableList {Item}"/>.
  /// </summary>
  /// <remarks>  
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_immutable.ImmutableList {Item}()"/>.  
  /// </remarks>
  static public ImmutableList<Item>? AsOrToImmutableList<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ImmutableList <Item>> targetType = collections_immutable.ImmutableList<Item> ();
    return enumerable.AsOrTo ( targetType, null, behavior );
  }
}
