using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;

using collections_frozen = Software9119.Collection.Superb.Extension.system_collections_frozen;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Creates <see cref="FrozenDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_frozen.FrozenDictionary{Item, Key}(Func{Item, Key}, IEqualityComparer{Key})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public FrozenDictionary<Key, Item>? IntoFrozenDictionary<Item, Key> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<FrozenDictionary<Key, Item>> targetType = collections_frozen.FrozenDictionary ( keySelector, keyComparer );
    return enumerable.AsOrTo ( targetType, null, behavior );
  }

  /// <summary>
  /// Creates <see cref="FrozenDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_frozen.FrozenDictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public FrozenDictionary<Key, Value>? IntoFrozenDictionary<Item, Key, Value> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<FrozenDictionary<Key, Value>> targetType = collections_frozen.FrozenDictionary
    (
      keySelector,
      valueSelector,
      keyComparer
    );
    return enumerable.AsOrTo ( targetType, null, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="FrozenSet{Item}"/> provided with <paramref name="itemComparer"/>.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_frozen.FrozenSet{Item}(IEqualityComparer{Item})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="itemComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// <item>
  /// Cast is allowed only when source <see cref="IEnumerable"/> is <see cref="FrozenSet{Item}"/> and <paramref name="itemComparer"/>
  /// referentially equals to <see cref="FrozenSet{Item}.Comparer"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public FrozenSet<Item>? AsOrToFrozenSet<Item>
  (
    this IEnumerable<Item>? enumerable,
    IEqualityComparer<Item>? itemComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    itemComparer ??= EqualityComparer<Item>.Default;
    AsOrToTargetType<FrozenSet<Item>> targetType = collections_frozen.FrozenSet(itemComparer);

    return enumerable.AsOrTo ( targetType, null, behavior );
  }
}
