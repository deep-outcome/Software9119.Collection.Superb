using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent?view=net-10.0">
/// System.Collections.Concurrent Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_concurrent
{
  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=net-10.0">
  /// ConcurrentBag&lt;Item&gt;</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  [SuppressMessage ( "Style", "IDE0306:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ConcurrentBag<Item>> ConcurrentBag<Item> ()
  {
    Ctor<Item, ConcurrentBag <Item>> typedCtor = (e) => new (e);

    Empty<ConcurrentBag <Item>> empty = () => new ();
    CanCast? canCast = null;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-10.0">
  /// ConcurrentDictionary&lt;Item, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<ConcurrentDictionary<Key, Item>> ConcurrentDictionary<Item, Key>
  (
    Func<Item, Key> keySelector,
    IEqualityComparer<Key> keyComparer,
    int? capacity,
    int? concurrencyLevel
  )
    where Key : notnull
    => ConcurrentDictionary ( keySelector, x => x, keyComparer, capacity: capacity, concurrencyLevel );


  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-10.0">
  /// ConcurrentDictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<ConcurrentDictionary<Key, Value>> ConcurrentDictionary<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IEqualityComparer<Key> keyComparer,
    int? capacity,
    int? concurrencyLevel
  )
    where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    capacity ??= 31;
    concurrencyLevel ??= -1;

    Ctor<Item, ConcurrentDictionary<Key, Value>> typedCtor = (e) =>
    {
      ConcurrentDictionary<Key, Value> result = new (concurrencyLevel.Value, capacity: capacity.Value, keyComparer);
      foreach(Item i in e)
        result[keySelector(i)]  = valueSelector(i);

      return result;
    };

    Empty<ConcurrentDictionary<Key, Value>> empty = () => new(keyComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

}
