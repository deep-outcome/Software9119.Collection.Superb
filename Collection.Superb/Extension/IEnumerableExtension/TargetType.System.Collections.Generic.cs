using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
    IEqualityComparer<Key> keyComparer,
    int? capacity
  )
  where Key : notnull
    => Dictionary ( keySelector, x => x, keyComparer, capacity );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2.-ctor?view=net-10.0">
  /// Dictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<Dictionary<Key, Value>> Dictionary<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IEqualityComparer<Key> keyComparer,
    int? capacity
  )
  where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    Ctor<Item, Dictionary<Key,Value>> typedCtor = (e) =>
    {
      if (capacity is int cap)
      {
        Dictionary<Key, Value> result = new  ( cap, keyComparer );
        foreach (Item i in e )
          result.Add(keySelector(i), valueSelector(i));

        return result;
      }

      return System.Linq.Enumerable.ToDictionary(e, keySelector, valueSelector, keyComparer);
    };

    Empty<Dictionary<Key, Value>> empty = () => new (keyComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1.-ctor?view=net-10.0">
  /// HashSet&lt;Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<HashSet<Item>> HashSet<Item> ( IEqualityComparer<Item> itemComparer, int? capacity )
  {
    if (itemComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( itemComparer ), "Item comparer not provided." );

    Ctor<Item, HashSet<Item>> typedCtor = (e) =>
    {
      if (capacity is int cap)
      {
        HashSet<Item> result = new (cap, itemComparer);
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
    Ctor<Item, LinkedList<Item>> typedCtor = (e) => new (e);
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
  static public AsOrToTargetType<List<Item>> List<Item> ( int? capacity )
  {
    Ctor<Item, List<Item>> typedCtor = (e) =>
    {
      if (capacity is int cap)
      {
        List<Item> result = new (cap);
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
    IEqualityComparer<Key> keyComparer,
    int? capacity
  )
  where Key : notnull
    => OrderedDictionary ( keySelector, x => x, keyComparer, capacity );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ordereddictionary-2?view=net-10.0">
  /// OrderedDictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<OrderedDictionary<Key, Value>> OrderedDictionary<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IEqualityComparer<Key> keyComparer,
    int? capacity
  )
  where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    Ctor<Item, OrderedDictionary<Key,Value>> typedCtor = (e) =>
    {
      OrderedDictionary<Key, Value> result = capacity is int cap
        ? new  ( cap, keyComparer )
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
  static public AsOrToTargetType<PriorityQueue<Item, Priority>> PriorityQueue<Item, Priority>
  (
    IComparer<Priority> priorityComparer,
    int? capacity
  )
  {
    if (priorityComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( priorityComparer ), "Priority comparer not provided." );

    Ctor<(Item,Priority), PriorityQueue<Item,Priority>> typedCtor = (e) =>
    {
      PriorityQueue<Item,Priority> result = capacity is int cap
        ? new (cap, priorityComparer)
        : new (priorityComparer);

      result.EnqueueRange(e);
      return result;
    };

    Empty<PriorityQueue<Item,Priority>> empty = () => new ( priorityComparer );
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.queue-1?view=net-10.0">
  /// Queue&lt;Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<Queue<Item>> Queue<Item> ( int? capacity )
  {
    Ctor<Item, Queue<Item>> typedCtor = (e) =>
    {
      Queue<Item> result = capacity is int cap ? new (cap) : new ();

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

    Ctor<Item, SortedDictionary<Key,Value>> typedCtor = (e) =>
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
    IComparer<Key> keyComparer,
    int? capacity
  )
  where Key : notnull
    => SortedList ( keySelector, x => x, keyComparer, capacity );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedlist-2?view=net-10.0">
  /// SortedList&lt;Key, Value&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<SortedList<Key, Value>> SortedList<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IComparer<Key> keyComparer,
    int? capacity
  )
  where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    Ctor<Item, SortedList<Key,Value>> typedCtor = (e) =>
    {
      SortedList<Key, Value> result = capacity is int cap
        ? new  (cap, keyComparer )
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

    Ctor<Item, SortedSet<Item>> typedCtor = (e) => new ( e, itemComparer );
    Empty<SortedSet<Item>> empty = () => new (itemComparer );
    CanCast canCast = e => e is SortedSet<Item> x && ReferenceEquals ( x.Comparer, itemComparer );
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.stack-1?view=net-10.0">
  /// Stack&lt;Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<Stack<Item>> Stack<Item> ( int? capacity )
  {
    Ctor<Item, Stack<Item>> typedCtor = (e) =>
    {
      if (capacity is int cap)
      {
        Stack<Item> result = new (cap);
        foreach (Item i in e)
          result.Push(i);

        return result;
      }

      return new(e);
    };

    Empty<Stack<Item>> empty = () => [];
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.array?view=net-10.0">
  /// Item[]</see>.
  /// </summary>
  static public AsOrToTargetType<Item []> Array<Item> ( int? length )
  {
    Ctor<Item, Item[]> typedCtor = (e) =>
    {
      if (length is int len)
      {
        Item[] result = new Item[len];
        int index = -1;
        foreach (Item i in e)
          result[++index] = i;

        return result;
      }

      return System.Linq.Enumerable.ToArray(e);
    };

    Empty<Item[]> empty = System.Array.Empty<Item>;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1?view=net-10.0">
  /// IList&lt;Item&gt;</see>.
  /// </summary>
  static public AsOrToTargetType<IList<Item>> IList<Item> ( int? capacity )
  {
    Ctor<Item, IList<Item>> typedCtor = (e) =>
    {
      if (e is IList<Item> ilist)
        return ilist;

      if (e is ICollection<Item> collection)
      {
        Item[] array = new Item[collection.Count];
        collection.CopyTo ( array, 0 );
        return array;
      }

      List<Item> list = capacity is int cap ? new(cap) : new();
      list.AddRange ( e );
      return list;
    };

    Empty<IList<Item>> empty = System.Array.Empty<Item>;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }
}
