using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic?view=net-10.0">
/// System.Collections.Generic Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_generic
{
  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.-ctor?view=net-10.0">
  /// Dictionary&lt;Key, Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<Dictionary<Key, Item>> Dictionary<Item, Key>
  (
    Func<Item, Key> keySelector,
    IEqualityComparer<Key> keyComparer
  )
  where Key : notnull
    => Dictionary ( keySelector, x => x, keyComparer );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.-ctor?view=net-10.0">
  /// Dictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<Dictionary<Key, Value>> Dictionary<Item, Key, Value>
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

    Ctor<Item, Dictionary<Key,Value>> typedCtor = (e, c) =>
    {
      if (c is int capacity)
      {
        Dictionary<Key, Value> result = new  ( capacity, keyComparer );
        foreach (Item i in e )
          result.Add(keySelector(i), valueSelector(i));

        return result;
      }

      return e.ToDictionary(keySelector, valueSelector, keyComparer);
    };

    Empty<Dictionary<Key, Value>> empty = () => new (keyComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }


  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1.-ctor?view=net-10.0">
  /// HashSet&lt;Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<HashSet<Item>> HashSet<Item> ( IEqualityComparer<Item> itemComparer )
  {
    if (itemComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( itemComparer ), "Item comparer not provided." );

    Ctor<Item, HashSet<Item>> typedCtor = (e, c) =>
    {
      if (c is int capacity)
      {
        HashSet<Item> result = new (capacity, itemComparer);
        foreach (Item i in e )
          _ = result.Add(i);

        return result;
      }

      return new(e, itemComparer);
    };

    Empty<HashSet<Item>> empty = () => new ( [], itemComparer );
    CanCast canCast = e => e is HashSet<Item> x && ReferenceEquals ( x.Comparer, itemComparer );
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1?view=net-10.0">
  /// LinkedList&lt;Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<LinkedList<Item>> LinkedList<Item> ()
  {
    Ctor<Item, LinkedList<Item>> typedCtor = (e, c) => new (e);
    Empty<LinkedList<Item>> empty = () => new ();    

    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }
}
