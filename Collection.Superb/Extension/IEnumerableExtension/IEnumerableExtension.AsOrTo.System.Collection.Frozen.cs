using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;

using collections_frozen = Software9119.Collection.Superb.Extension.system_collections_frozen;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Creates <see cref="FrozenDictionary{Key, Value}"/> with <paramref name="keyComaparer"/> from <paramref name="enumerable"/> 
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with 
  /// <see cref="collections_frozen.FrozenDictionary{Source, Key}(Func{Source, Key}, IEqualityComparer{Key})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComaparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public FrozenDictionary<Key, Source>? ToFrozenDictionary<Source, Key> (
    this IEnumerable<Source> enumerable,
    Func<Source, Key> keySelector,
    IEqualityComparer<Key>? keyComaparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComaparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<FrozenDictionary<Key, Source>> targetType = collections_frozen.FrozenDictionary ( keySelector, keyComaparer );
    return enumerable.AsOrTo ( targetType, null, behavior );
  }

  /// <summary>
  /// Creates <see cref="FrozenDictionary{Key, Value}"/> with <paramref name="keyComaparer"/> from <paramref name="enumerable"/> 
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with 
  /// <see cref="collections_frozen.FrozenDictionary{Source, Key, Value}(Func{Source, Key}, Func{Source, Value}, IEqualityComparer{Key})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComaparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public FrozenDictionary<Key, Value>? ToFrozenDictionary<Source, Key, Value> (
    this IEnumerable<Source> enumerable,
    Func<Source, Key> keySelector,
    Func<Source, Value> valueSelector,
    IEqualityComparer<Key>? keyComaparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComaparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<FrozenDictionary<Key, Value>> targetType = collections_frozen.FrozenDictionary
    (
      keySelector,
      valueSelector,
      keyComaparer
    );
    return enumerable.AsOrTo ( targetType, null, behavior );
  }
}
