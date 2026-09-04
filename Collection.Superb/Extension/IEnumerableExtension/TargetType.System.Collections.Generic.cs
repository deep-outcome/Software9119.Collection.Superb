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
        result.UnionWith(e);
        return result;
      }

      return new(e, itemComparer);
    };

    Empty<HashSet<Item>> empty = () => new (itemComparer );
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

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=net-10.0">
  /// List&lt;Item&gt;</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Explication intent." )]
  [SuppressMessage ( "Style", "IDE0306:Simplify collection initialization", Justification = "Explication intent." )]
  static public AsOrToTargetType<List<Item>> List<Item> ()
  {
    Ctor<Item, List<Item>> typedCtor = (e, c) =>
    {
      if (c is int capacity)
      {
        List<Item> result = new (capacity);
        result.AddRange(e);
        return result;
      }

      return new(e);
    };

    Empty<List<Item>> empty = () => new ();
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ordereddictionary-2?view=net-10.0">
  /// OrderedDictionary&lt;Key, Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<OrderedDictionary<Key, Item>> OrderedDictionary<Item, Key>
  (
    Func<Item, Key> keySelector,
    IEqualityComparer<Key> keyComparer
  )
  where Key : notnull
    => OrderedDictionary ( keySelector, x => x, keyComparer );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ordereddictionary-2?view=net-10.0">
  /// OrderedDictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<OrderedDictionary<Key, Value>> OrderedDictionary<Item, Key, Value>
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

    Ctor<Item, OrderedDictionary<Key,Value>> typedCtor = (e, c) =>
    {
      OrderedDictionary<Key, Value> result = c is int capacity
        ? new  ( capacity, keyComparer )
        : new(keyComparer);

      foreach(Item i in e)
        result.Add(keySelector(i), valueSelector(i));

      return result;
    };

    Empty<OrderedDictionary<Key, Value>> empty = () => new (keyComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2?view=net-10.0">
  /// PriorityQueue&lt;Item,Priority&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<PriorityQueue<Item, Priority>> PriorityQueue<Item, Priority> ( IComparer<Priority> priorityComparer )
  {
    if (priorityComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( priorityComparer ), "Priority comparer not provided." );

    Ctor<(Item,Priority), PriorityQueue<Item,Priority>> typedCtor = (e, c) =>
    {
      PriorityQueue<Item,Priority> result = c is int capacity
        ? new (capacity, priorityComparer)
        : new (priorityComparer);

      result.EnqueueRange(e);
      return result;
    };

    Empty<PriorityQueue<Item,Priority>> empty = () => new ( priorityComparer );
    CanCast canCast = e => false;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1?view=net-10.0">
  /// Queue&lt;Item&gt;</see>.
  /// </summary>  
  static public AsOrToTargetType<Queue<Item>> Queue<Item> ()
  {
    Ctor<Item, Queue<Item>> typedCtor = (e, c) =>
    {
      Queue<Item> result = c is int capacity ? new (capacity) : new ();

      foreach(Item i in e)
        result.Enqueue(i);

      return result;
    };

    Empty<Queue<Item>> empty = () => new ();
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2?view=net-10.0">
  /// SortedDictionary&lt;Key, Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<SortedDictionary<Key, Item>> SortedDictionary<Item, Key>
  (
    Func<Item, Key> keySelector,
    IComparer<Key> keyComparer
  )
  where Key : notnull
    => SortedDictionary ( keySelector, x => x, keyComparer );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sorteddictionary-2?view=net-10.0">
  /// SortedDictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<SortedDictionary<Key, Value>> SortedDictionary<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IComparer<Key> keyComparer
  )
  where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    Ctor<Item, SortedDictionary<Key,Value>> typedCtor = (e, c) =>
    {

      SortedDictionary<Key, Value> result = new  ( keyComparer );
      foreach (Item i in e )
        result.Add(keySelector(i), valueSelector(i));

      return result;
    };

    Empty<SortedDictionary<Key, Value>> empty = () => new (keyComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedlist-2?view=net-10.0">
  /// SortedList&lt;Key, Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<SortedList<Key, Item>> SortedList<Item, Key>
  (
    Func<Item, Key> keySelector,
    IComparer<Key> keyComparer
  )
  where Key : notnull
    => SortedList ( keySelector, x => x, keyComparer );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedlist-2?view=net-10.0">
  /// SortedList&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<SortedList<Key, Value>> SortedList<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IComparer<Key> keyComparer
  )
  where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    Ctor<Item, SortedList<Key,Value>> typedCtor = (e, c) =>
    {
      SortedList<Key, Value> result = c is int capacity
        ? new  (capacity, keyComparer )
        : new (keyComparer);

      foreach (Item i in e )
        result.Add(keySelector(i), valueSelector(i));

      return result;
    };

    Empty<SortedList<Key, Value>> empty = () => new (keyComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedset-1?view=net-10.0">
  /// SortedSet&lt;Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<SortedSet<Item>> SortedSet<Item> ( IComparer<Item> itemComparer )
  {
    if (itemComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( itemComparer ), "Item comparer not provided." );

    Ctor<Item, SortedSet<Item>> typedCtor = (e, c) => new ( e, itemComparer );
    Empty<SortedSet<Item>> empty = () => new (itemComparer );
    CanCast canCast = e => e is SortedSet<Item> x && ReferenceEquals ( x.Comparer, itemComparer );
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1?view=net-10.0">
  /// Stack&lt;Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<Stack<Item>> Stack<Item> ()
  {
    Ctor<Item, Stack<Item>> typedCtor = (e, c) =>
    {
      if (c is int capacity)
      {
        Stack<Item> result = new (capacity);
        foreach (Item i in e)
          result.Push(i);

        return result;
      }

      return new(e);
    };

    Empty<Stack<Item>> empty = () => [];
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }
}
