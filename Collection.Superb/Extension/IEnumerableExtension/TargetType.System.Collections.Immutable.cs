using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Immutable = System.Collections.Immutable;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable?view=net-10.0">
/// System.Collections.Immutable Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_immutable
{
  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablearray-1?view=net-10.0">
  /// ImmutableArray&lt;Item&gt;</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0303:Simplify collection initialization", Justification = "Obviousity." )]
  [SuppressMessage ( "Style", "IDE0301:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ImmutableArray<Item>> ImmutableArray<Item> ( bool strictLengthMatch )
  {
    Ctor<Item, ImmutableArray<Item>> typedCtor = (e, c) =>
    {
      if (c is int capacity)
      {
        ImmutableArray<Item>.Builder builder = Immutable.ImmutableArray.CreateBuilder<Item>();
        builder.Capacity = capacity;
        builder.AddRange(e);

        return strictLengthMatch ? builder.MoveToImmutable() : builder.DrainToImmutable();
      }

      return Immutable.ImmutableArray.CreateRange(e);
    };

    Empty<ImmutableArray<Item>> empty = () => Immutable.ImmutableArray<Item>.Empty;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutabledictionary-2?view=net-10.0">
  /// ImmutableDictionary&lt;Item, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<ImmutableDictionary<Key, Item>> ImmutableDictionary<Item, Key>
  (
    Func<Item, Key> keySelector,
    IEqualityComparer<Key> keyComparer,
    IEqualityComparer<Item> itemComparer
  )
    where Key : notnull
    => ImmutableDictionary ( keySelector, x => x, keyComparer, itemComparer );


  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutabledictionary-2?view=net-10.0">
  /// ImmutableDictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<ImmutableDictionary<Key, Value>> ImmutableDictionary<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IEqualityComparer<Key> keyComparer,
    IEqualityComparer<Value> valueComparer
  )
    where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    if (valueComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( valueComparer ), "Value comparer not provided." );

    Ctor<Item, ImmutableDictionary<Key, Value>> typedCtor = (e, c) =>
    {
      return Immutable.ImmutableDictionary.ToImmutableDictionary
      (
        e,
        keySelector,
        valueSelector,
        keyComparer,
        valueComparer
      );
    };

    Empty<ImmutableDictionary<Key, Value>> empty = () => Immutable.ImmutableDictionary.Create<Key, Value>(keyComparer, valueComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablehashset-1?view=net-10.0">
  /// ImmutableHashSet&lt;Item&gt;</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0301:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ImmutableHashSet<Item>> ImmutableHashSet<Item> ( IEqualityComparer<Item> itemComparer )
  {
    if (itemComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( itemComparer ), "Item comparer not provided." );

    Ctor<Item, ImmutableHashSet<Item>> typedCtor = (e, c) => Immutable.ImmutableHashSet.ToImmutableHashSet(e, itemComparer);

    Empty<ImmutableHashSet<Item>> empty = () => Immutable.ImmutableHashSet.Create (itemComparer);
    CanCast canCast = e => e is ImmutableHashSet<Item> x && ReferenceEquals(x.KeyComparer, itemComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.ImmutableList-1?view=net-10.0">
  /// ImmutableList&lt;Item&gt;</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0301:Simplify collection initialization", Justification = "Obviousity." )]
  [SuppressMessage ( "Style", "IDE0303:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ImmutableList<Item>> ImmutableList<Item> ()
  {
    Ctor<Item, ImmutableList <Item>> typedCtor = (e, c) => Immutable.ImmutableList.CreateRange (e);

    Empty<ImmutableList <Item>> empty = () => Immutable.ImmutableList<Item>.Empty;
    CanCast? canCast = null;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.ImmutableQueue-1?view=net-10.0">
  /// ImmutableQueue&lt;Item&gt;</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0301:Simplify collection initialization", Justification = "Obviousity." )]
  [SuppressMessage ( "Style", "IDE0303:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ImmutableQueue<Item>> ImmutableQueue<Item> ()
  {
    Ctor<Item, ImmutableQueue <Item>> typedCtor = (e, c) => Immutable.ImmutableQueue.CreateRange (e);

    Empty<ImmutableQueue <Item>> empty = () => Immutable.ImmutableQueue<Item>.Empty;
    CanCast? canCast = null;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.ImmutableSortedDictionary-2?view=net-10.0">
  /// ImmutableSortedDictionary&lt;Item, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<ImmutableSortedDictionary<Key, Item>> ImmutableSortedDictionary<Item, Key>
  (
    Func<Item, Key> keySelector,
    IComparer<Key> keyComparer,
    IEqualityComparer<Item> itemComparer
  )
    where Key : notnull
    => ImmutableSortedDictionary ( keySelector, x => x, keyComparer, itemComparer );


  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.ImmutableSortedDictionary-2?view=net-10.0">
  /// ImmutableSortedDictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<ImmutableSortedDictionary<Key, Value>> ImmutableSortedDictionary<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IComparer<Key> keyComparer,
    IEqualityComparer<Value> valueComparer
  )
    where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    if (valueComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( valueComparer ), "Value comparer not provided." );

    Ctor<Item, ImmutableSortedDictionary<Key, Value>> typedCtor = (e, c) =>
    {
      return Immutable.ImmutableSortedDictionary.ToImmutableSortedDictionary
      (
        e,
        keySelector,
        valueSelector,
        keyComparer,
        valueComparer
      );
    };

    Empty<ImmutableSortedDictionary<Key, Value>> empty = () => Immutable.ImmutableSortedDictionary.Create<Key, Value>(keyComparer, valueComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }
}
