using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.Dictionary{Item, Key}(Func{Item, Key}, IEqualityComparer{Key}, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public Dictionary<Key, Item>? IntoDictionary<Item, Key> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    int? capacity = null,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<Dictionary<Key, Item>> targetType = collections_generic.Dictionary ( keySelector, keyComparer, capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="Dictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.Dictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key}, int?)"/>.
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
      keyComparer,
      capacity
    );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="HashSet{Item}"/> provided with <paramref name="itemComparer"/>.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.HashSet{Item}(IEqualityComparer{Item}, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="itemComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Item}.Default"/>.
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
    AsOrToTargetType<HashSet<Item>> targetType = collections_generic.HashSet(itemComparer, capacity );

    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="LinkedList{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.LinkedList{Item}()"/>.
  /// </remarks>
  static public LinkedList<Item>? AsOrToLinkedList<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<LinkedList<Item>> targetType = collections_generic.LinkedList<Item>();
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="List{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.List{Item}(int?)"/>.
  /// </remarks>
  [SuppressMessage ( "Design", "CA1002:Do not expose generic lists", Justification = "No help in here." )]
  static public List<Item>? AsOrToList<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<List<Item>> targetType = collections_generic.List<Item>( capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="OrderedDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.OrderedDictionary{Item, Key}(Func{Item, Key}, IEqualityComparer{Key}, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public OrderedDictionary<Key, Item>? IntoOrderedDictionary<Item, Key> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    int? capacity = null,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<OrderedDictionary<Key, Item>> targetType = collections_generic.OrderedDictionary ( keySelector, keyComparer, capacity  );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="OrderedDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.OrderedDictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key}, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public OrderedDictionary<Key, Value>? IntoOrderedDictionary<Item, Key, Value> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    int? capacity = null,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<OrderedDictionary<Key, Value>> targetType = collections_generic.OrderedDictionary
    (
      keySelector,
      valueSelector,
      keyComparer,
      capacity
    );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="PriorityQueue{Item, Priority}"/> with <paramref name="priorityComparer"/> from <paramref name="enumerable"/>.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.PriorityQueue{Item, Priority}(IComparer{Priority}, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="priorityComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Priority}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public PriorityQueue<Item, Priority>? IntoPriorityQueue<Item, Priority>
  (
    this IEnumerable<(Item, Priority)>? enumerable,
    int? capacity = null,
    IComparer<Priority>? priorityComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    priorityComparer ??= Comparer<Priority>.Default;
    AsOrToTargetType<PriorityQueue<Item, Priority>> targetType = collections_generic.PriorityQueue<Item, Priority>(priorityComparer, capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="Queue{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.Queue{Item}(int?)"/>.
  /// </remarks>
  static public Queue<Item>? AsOrToTypedQueue<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<Queue<Item>> targetType = collections_generic.Queue<Item>( capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="SortedDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.SortedDictionary{Item, Key}(Func{Item, Key}, IComparer{Key})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public SortedDictionary<Key, Item>? IntoSortedDictionary<Item, Key> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    IComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= Comparer<Key>.Default;
    AsOrToTargetType<SortedDictionary<Key, Item>> targetType = collections_generic.SortedDictionary ( keySelector, keyComparer );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="SortedDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.SortedDictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IComparer{Key})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public SortedDictionary<Key, Value>? IntoSortedDictionary<Item, Key, Value> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= Comparer<Key>.Default;
    AsOrToTargetType<SortedDictionary<Key, Value>> targetType = collections_generic.SortedDictionary
    (
      keySelector,
      valueSelector,
      keyComparer
    );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="SortedList{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.SortedList{Item, Key}(Func{Item, Key}, IComparer{Key}, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public SortedList<Key, Item>? IntoTypedSortedList<Item, Key> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    int? capacity = null,
    IComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= Comparer<Key>.Default;
    AsOrToTargetType<SortedList<Key, Item>> targetType = collections_generic.SortedList ( keySelector, keyComparer, capacity  );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="SortedList{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.SortedList{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IComparer{Key}, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public SortedList<Key, Value>? IntoTypedSortedList<Item, Key, Value> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    int? capacity = null,
    IComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= Comparer<Key>.Default;
    AsOrToTargetType<SortedList<Key, Value>> targetType = collections_generic.SortedList
    (
      keySelector,
      valueSelector,
      keyComparer,
      capacity
    );
    return enumerable.AsOrTo ( targetType, behavior );
  }


  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="SortedSet{Item}"/> provided with <paramref name="itemComparer"/>.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.SortedSet{Item}(IComparer{Item})"/>.
  /// </item>
  /// <item>
  /// When <paramref name="itemComparer"/> is <see langword="null"/>, it defaults to <see cref="Comparer{Key}.Default"/>.
  /// </item>
  /// <item>
  /// Cast is allowed only when source <see cref="IEnumerable"/> is <see cref="SortedSet{Item}"/> and <paramref name="itemComparer"/>
  /// referentially equals to <see cref="SortedSet{Item}.Comparer"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public SortedSet<Item>? AsOrToSortedSet<Item>
  (
    this IEnumerable<Item>? enumerable,
    IComparer<Item>? itemComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    itemComparer ??= Comparer<Item>.Default;
    AsOrToTargetType<SortedSet<Item>> targetType = collections_generic.SortedSet(itemComparer);

    return enumerable.AsOrTo ( targetType, behavior );
  }


  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="Stack{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.Stack{Item}(int?)"/>.
  /// </remarks>
  static public Stack<Item>? AsOrToTypedStack<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<Stack<Item>> targetType = collections_generic.Stack<Item>( capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="Array"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.Array{Item}(int?)"/>.
  /// </remarks>
  static public Item []? AsOrToArray<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? length = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    if (length < 0)
      throw new ArgumentOutOfRangeException ( paramName: nameof ( length ), "Array length must be non-negative." );

    AsOrToTargetType<Item[]> targetType = collections_generic.Array<Item>( length );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="IList{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Cast/Copy Table
  /// <code>
  /// ╔════════════════╦══════════╦════════╗
  /// ║   enumerable   ║  result  ║ action ║
  /// ╠════════════════╬══════════╬════════╣
  /// ║ IEnumerable&lt;T&gt; ║ List&lt;T&gt;  ║ Copy   ║
  /// ║ ICollection&lt;T&gt; ║ T []     ║ Copy   ║
  /// ║ IList&lt;T&gt;       ║ IList&lt;T&gt; ║ Cast   ║
  /// ╚════════════════╩══════════╩════════╝
  /// </code>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_generic.IList{Item}(int?)"/>.
  /// </item>
  /// <item>
  /// <paramref name="capacity"/> can be used for <see cref="List{Item}"/> pre-capacitation.
  /// </item>
  /// </list>
  /// </remarks>
  [SuppressMessage ( "Style", "IDE0305:Simplify collection initialization", Justification = "Obviousity." )]
  static public IList<Item>? AsOrToIList<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<IList<Item>> targetType = collections_generic.IList<Item>( capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }
}
