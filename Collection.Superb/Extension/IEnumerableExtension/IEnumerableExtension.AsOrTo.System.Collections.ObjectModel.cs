using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using collections_objectmodel = Software9119.Collection.Superb.Extension.system_collections_objectmodel;

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
  /// <see cref="collections_objectmodel.Collection{Item}(int?)"/>.
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
    AsOrToTargetType<Collection<Item>> targetType = collections_objectmodel.Collection<Item>( capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ObservableCollection{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_objectmodel.ObservableCollection{Item}()"/>.
  /// </remarks>
  static public ObservableCollection<Item>? AsOrToObservableCollection<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ObservableCollection<Item>> targetType = collections_objectmodel.ObservableCollection<Item>( );
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
  /// <see cref="collections_objectmodel.ReadOnlyCollection{Item}(int?)"/>.
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
    AsOrToTargetType<ReadOnlyCollection<Item>> targetType = collections_objectmodel.ReadOnlyCollection<Item>( capacity );
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
  /// <see cref="collections_objectmodel.ReadOnlyDictionary{Item, Key}(Func{Item, Key}, IEqualityComparer{Key}, int?)"/>.
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
    AsOrToTargetType<ReadOnlyDictionary<Key, Item>> targetType = collections_objectmodel.ReadOnlyDictionary ( keySelector, keyComparer, capacity );
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
  /// <see cref="collections_objectmodel.ReadOnlyDictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key}, int?)"/>.
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
    AsOrToTargetType<ReadOnlyDictionary<Key, Value>> targetType = collections_objectmodel.ReadOnlyDictionary
    (
      keySelector,
      valueSelector,
      keyComparer,
      capacity
    );
    return enumerable.AsOrTo ( targetType, behavior );
  }
}
