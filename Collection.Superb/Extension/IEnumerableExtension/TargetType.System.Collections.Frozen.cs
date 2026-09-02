using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using FrozenDict = System.Collections.Frozen.FrozenDictionary;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen?view=net-10.0">
/// System.Collections.Frozen Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_frozen
{
  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2?view=net-10.0">
  /// FrozenDictionary&lt;Key, Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<FrozenDictionary<Key, Item>> FrozenDictionary<Item, Key> (
    Func<Item, Key> keySelector,
    IEqualityComparer<Key> keyComparer
  ) where Key : notnull
    => FrozenDictionary ( keySelector, x => x, keyComparer );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2?view=net-10.0">
  /// FrozenDictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<FrozenDictionary<Key, Value>> FrozenDictionary<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IEqualityComparer<Key> keyComparer
  )
  where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    Ctor<Item, FrozenDictionary<Key,Value>> typedCtor = (e, c) =>
    {
      FrozenDictionary<Key,Value> result = FrozenDict.ToFrozenDictionary(e, keySelector, valueSelector, keyComparer);
      return result;
    };

    Empty<FrozenDictionary<Key, Value>> empty = () => FrozenDict.ToFrozenDictionary<Key, Value> ( [], keyComparer );
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozenset-1?view=net-10.0">
  /// FrozenSet&lt;Item&gt;</see>
  /// </summary>
  static public AsOrToTargetType<FrozenSet<Item>> FrozenSet<Item> ( IEqualityComparer<Item> itemComparer )
  {
    if (itemComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( itemComparer ), "Item comparer not provided." );

    Ctor<Item, FrozenSet<Item>> typedCtor = (e, c) =>
    {
      FrozenSet<Item> result = System.Collections.Frozen.FrozenSet.ToFrozenSet(e, itemComparer);
      return result;
    };

    Empty<FrozenSet<Item>> empty = () => System.Collections.Frozen.FrozenSet.ToFrozenSet ( [], itemComparer );
    CanCast canCast = e => e is FrozenSet<Item> fs && ReferenceEquals(fs.Comparer, itemComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }
}
