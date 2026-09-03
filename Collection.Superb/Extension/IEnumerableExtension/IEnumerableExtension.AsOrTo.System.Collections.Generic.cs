using System;
using System.Collections;
using System.Collections.Generic;

using collections_generic = Software9119.Collection.Superb.Extension.system_collections_generic;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Creates <see cref="Dictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_generic.Dictionary{Item, Key}(Func{Item, Key}, IEqualityComparer{Key})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public Dictionary<Key, Item>? IntoDictionary<Item, Key> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    IEqualityComparer<Key>? keyComparer = null,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<Dictionary<Key, Item>> targetType = collections_generic.Dictionary ( keySelector, keyComparer );
    return enumerable.AsOrTo ( targetType, capacity, behavior );
  }

  /// <summary>
  /// Creates <see cref="Dictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_generic.Dictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public Dictionary<Key, Value>? IntoDictionary<Item, Key, Value> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    int? capacity = null,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<Dictionary<Key, Value>> targetType = collections_generic.Dictionary
    (
      keySelector,
      valueSelector,
      keyComparer
    );
    return enumerable.AsOrTo ( targetType, capacity, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="HashSet{Item}"/> provided with <paramref name="itemComparer"/>.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_generic.HashSet{Item}(IEqualityComparer{Item})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="itemComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// <item>
  /// Cast is allowed only when source <see cref="IEnumerable"/> is <see cref="HashSet{Item}"/> and <paramref name="itemComparer"/>
  /// referentially equals to <see cref="HashSet{Item}.Comparer"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public HashSet<Item>? AsOrToHashSet<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? capacity = null,
    IEqualityComparer<Item>? itemComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    itemComparer ??= EqualityComparer<Item>.Default;
    AsOrToTargetType<HashSet<Item>> targetType = collections_generic.HashSet(itemComparer);

    return enumerable.AsOrTo ( targetType, capacity, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="LinkedList{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_generic.LinkedList{Item}()"/>.
  /// </remarks>
  static public LinkedList<Item>? AsOrToLinkedList<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<LinkedList<Item>> targetType = collections_generic.LinkedList<Item>();
    return enumerable.AsOrTo ( targetType, null, behavior );
  }
}
