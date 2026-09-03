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
        Dictionary<Key, Value> dict = new  ( capacity, keyComparer );
        foreach (Item i in e )
          dict.Add(keySelector(i), valueSelector(i));

        return dict;
      }

      return e.ToDictionary(keySelector, valueSelector, keyComparer);
    };

    Empty<Dictionary<Key, Value>> empty = () => new (keyComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }
}
