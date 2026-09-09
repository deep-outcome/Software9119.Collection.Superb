using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using c_frozen = Software9119.Collection.Superb.Extension.system_collections_frozen;
using c_generic = Software9119.Collection.Superb.Extension.system_collections_generic;
using c_immutable = Software9119.Collection.Superb.Extension.system_collections_immutable;
using c_objectmodel = Software9119.Collection.Superb.Extension.system_collections_objectmodel;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Casts <paramref name="enumerable"/> directly into <see cref="Collection{Item}"/>, or casts or copies <paramref name="enumerable"/>
  /// into intermediate <see cref="IList{Item}"/> before wrapping it into <see cref="Collection{Item}"/>.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_objectmodel.Collection{Item}(int?)"/>.
  /// </item>
  /// <item>
  /// <paramref name="capacity"/> can be used for <see cref="List{Item}"/> pre-capacitation,
  /// see <see cref="AsOrToIList{Item}(IEnumerable{Item}, int?, NullBehavior)"/> for details.
  /// </item>
  /// </list>
  /// </remarks>
  static public Collection<Item>? AsOrToCollection<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<Collection<Item>> targetType = c_objectmodel.Collection<Item>( capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ObservableCollection{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_objectmodel.ObservableCollection{Item}()"/>.
  /// </remarks>
  static public ObservableCollection<Item>? AsOrToObservableCollection<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ObservableCollection<Item>> targetType = c_objectmodel.ObservableCollection<Item>( );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts <paramref name="enumerable"/> directly into <see cref="ReadOnlyCollection{Item}"/>, or casts or copies <paramref name="enumerable"/>
  /// into intermediate <see cref="IList{Item}"/> before wrapping it into <see cref="ReadOnlyCollection{Item}"/>.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_objectmodel.ReadOnlyCollection{Item}(int?)"/>.
  /// </item>
  /// <item>
  /// <paramref name="capacity"/> can be used for <see cref="List{Item}"/> pre-capacitation,
  /// see <see cref="AsOrToIList{Item}(IEnumerable{Item}, int?, NullBehavior)"/> for details.
  /// </item>
  /// </list>
  /// </remarks>
  static public ReadOnlyCollection<Item>? AsOrToReadOnlyCollection<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ReadOnlyCollection<Item>> targetType = c_objectmodel.ReadOnlyCollection<Item>( capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="ReadOnlyDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_objectmodel.ReadOnlyDictionary{Item, Key}(Func{Item, Key}, IEqualityComparer{Key}, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ReadOnlyDictionary<Key, Item>? IntoReadOnlyDictionary<Item, Key> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    int? capacity = null,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<ReadOnlyDictionary<Key, Item>> targetType = c_objectmodel.ReadOnlyDictionary ( keySelector, keyComparer, capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Creates <see cref="ReadOnlyDictionary{Key, Value}"/> with <paramref name="keyComparer"/> from <paramref name="enumerable"/>
  /// using <paramref name="keySelector"/> and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_objectmodel.ReadOnlyDictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key}, int?)"/>.
  /// </item>
  /// <item>
  /// When <paramref name="keyComparer"/> is <see langword="null"/>, it defaults to <see cref="EqualityComparer{Key}.Default"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public ReadOnlyDictionary<Key, Value>? IntoReadOnlyDictionary<Item, Key, Value> (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    int? capacity = null,
    IEqualityComparer<Key>? keyComparer = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    where Key : notnull
  {
    keyComparer ??= EqualityComparer<Key>.Default;
    AsOrToTargetType<ReadOnlyDictionary<Key, Value>> targetType = c_objectmodel.ReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      keyComparer,
      capacity
    );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts <paramref name="enumerable"/> directly into <see cref="ReadOnlyObservableCollection{Item}"/>, or casts or copies 
  /// <paramref name="enumerable"/> into intermediate <see cref="ObservableCollection{Item}"/> before wrapping it into 
  /// <see cref="ReadOnlyObservableCollection{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_objectmodel.ReadOnlyObservableCollection{Item}()"/>.
  /// </remarks>
  static public ReadOnlyObservableCollection<Item>? AsOrToReadOnlyObservableCollection<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ReadOnlyObservableCollection<Item>> targetType = c_objectmodel.ReadOnlyObservableCollection<Item>( );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// <list type="bullet">
  /// <item>
  /// Casts <paramref name="enumerable"/> directly into <see cref="ReadOnlySet{Item}"/>, or casts or copies 
  /// <paramref name="enumerable"/> into intermediate <see cref="ISet{Item}"/> before wrapping it into 
  /// <see cref="ReadOnlySet{Item}"/>.
  /// </item>
  /// <item><paramref name="equalityComparer"/> defaults to <see cref="EqualityComparer{T}.Default"/> when <see langword="null"/>.</item>
  /// <item><paramref name="sortingComparer"/> defaults to <see cref="Comparer{T}.Default"/> when <see langword="null"/>.</item>
  /// </list>
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_objectmodel.ReadOnlySet{Item}(Ctor{ISet{Item}})"/> which is provided, based on <paramref name="setType"/> value,
  /// with set constructor of one of:
  /// <list type="bullet">
  /// <item><see cref="c_frozen.FrozenSet{Item}(IEqualityComparer{Item})"/></item>
  /// <item><see cref="c_generic.HashSet{Item}(IEqualityComparer{Item}, int?)"/></item>
  /// <item><see cref="c_immutable.ImmutableHashSet{Item}(IEqualityComparer{Item})"/></item>
  /// <item><see cref="c_immutable.ImmutableSortedSet{Item}(IComparer{Item})"/></item>
  /// <item><see cref="c_generic.SortedSet{Item}(IComparer{Item})"/></item>
  /// </list>
  /// </remarks>
  /// <exception cref="UnsupportedReadOnlySetTypeException">When <paramref name="setType"/> is unknown.</exception>
  static public ReadOnlySet<Item>? AsOrToReadOnlySet<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? capacity = null,
    IComparer<Item>? sortingComparer = null,
    IEqualityComparer<Item>? equalityComparer = null,
    ReadOnlySetType setType = ReadOnlySetType.HashSet,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    if (setType.IsSortedSet ())
      sortingComparer ??= Comparer<Item>.Default;
    else
      equalityComparer ??= EqualityComparer<Item>.Default;

    Ctor<ISet<Item>> setCtor = setType switch
    {
      ReadOnlySetType.HashSet => c_generic.HashSet(equalityComparer!, capacity).Ctor,
      ReadOnlySetType.FrozenSet => c_frozen.FrozenSet(equalityComparer!).Ctor,
      ReadOnlySetType.ImmutableHashSet => c_immutable.ImmutableHashSet(equalityComparer!).Ctor,
      ReadOnlySetType.ImmutableSortedSet =>  c_immutable.ImmutableSortedSet(sortingComparer!).Ctor,
      ReadOnlySetType.SortedSet => c_generic.SortedSet(sortingComparer!).Ctor,
      _ => throw new UnsupportedReadOnlySetTypeException(setType)
    };

    AsOrToTargetType<ReadOnlySet<Item>> targetType = c_objectmodel.ReadOnlySet(setCtor);
    return enumerable.AsOrTo ( targetType, behavior );
  }
}
