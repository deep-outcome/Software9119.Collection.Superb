using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

using c_concurrent = Software9119.Collection.Superb.Extension.system_collections_concurrent;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ConcurrentBag {Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_concurrent.ConcurrentBag {Item}()"/>.
  /// </remarks>
  static public ConcurrentBag<Item>? AsOrToConcurrentBag<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ConcurrentBag <Item>> targetType = c_concurrent.ConcurrentBag<Item> ();
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="ConcurrentDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_concurrent.ConcurrentDictionary{Item, Key}(Func{Item, Key}, IEqualityComparer{Key}, int?, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ConcurrentDictionary<Key, Item>? IntoConcurrentDictionary<Item, Key> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    int? capacity = null,
    int? concurrencyLevel = null,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<ConcurrentDictionary<Key, Item>> targetType = c_concurrent.ConcurrentDictionary
    (
      keySelector,
      keyComparer,
      capacity: capacity,
      concurrencyLevel
    );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="ConcurrentDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_concurrent.ConcurrentDictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key}, int?, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ConcurrentDictionary<Key, Value>? IntoConcurrentDictionary<Item, Key, Value> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    int? capacity = null,
    int? concurrencyLevel = null,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<ConcurrentDictionary<Key, Value>> targetType = c_concurrent.ConcurrentDictionary
    (
      keySelector,
      valueSelector,
      keyComparer,
      capacity: capacity,
      concurrencyLevel
    );

    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ConcurrentQueue {Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_concurrent.ConcurrentQueue {Item}()"/>.
  /// </remarks>
  static public ConcurrentQueue<Item>? AsOrToConcurrentQueue<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ConcurrentQueue <Item>> targetType = c_concurrent.ConcurrentQueue<Item> ();
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ConcurrentStack {Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_concurrent.ConcurrentStack {Item}()"/>.
  /// </remarks>
  static public ConcurrentStack<Item>? AsOrToConcurrentStack<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ConcurrentStack <Item>> targetType = c_concurrent.ConcurrentStack<Item> ();
    return enumerable.AsOrTo ( targetType, behavior );
  }
}
