using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using c_generic = Software9119.Collection.Superb.Extension.system_collections_generic;
using ObjectModel = System.Collections.ObjectModel;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel?view=net-10.0">
/// System.Collections.ObjectModel Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_objectmodel
{
  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.collection-1?view=net-10.0">
  /// Collection&lt;Item&gt;</see>.
  /// </summary>
  /// <remarks>
  /// Uses <see cref="c_generic.IList{Item}(int?)"/> for <see cref="IList{Item}"/> production.
  /// </remarks>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<Collection<Item>> Collection<Item> ( int? capacity )
  {
    Ctor<Item, Collection<Item>> typedCtor = (e) =>
    {
      IList<Item> ilist = c_generic.IList<Item>( capacity ).Ctor( e );
      return new(ilist);
    };

    Empty<Collection<Item>> empty = () => new ();
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.observablecollection-1?view=net-10.0">
  /// ObservableCollection&lt;Item&gt;</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ObservableCollection<Item>> ObservableCollection<Item> ()
  {
    Ctor<Item, ObservableCollection<Item>> typedCtor = (e) =>
    {
      if (e is IList<Item> list)
        return new ObservableCollection<Item>(list);

      return new(e);
    };

    Empty<ObservableCollection<Item>> empty = () => new ();
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.ReadOnlyCollection-1?view=net-10.0">
  /// ReadOnlyCollection&lt;Item&gt;</see>.
  /// </summary>
  /// <remarks>
  /// Uses <see cref="c_generic.IList{Item}(int?)"/> for <see cref="IList{Item}"/> production.
  /// </remarks>
  [SuppressMessage ( "Style", "IDE0028:Simplify ReadOnlyCollection initialization", Justification = "Obviousity." )]
  [SuppressMessage ( "Style", "IDE0301:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ReadOnlyCollection<Item>> ReadOnlyCollection<Item> ( int? capacity )
  {
    Ctor<Item, ReadOnlyCollection<Item>> typedCtor = (e) =>
    {
      IList<Item> ilist = c_generic.IList<Item>( capacity ).Ctor( e );
      return new(ilist);
    };

    Empty<ReadOnlyCollection<Item>> empty = () => ObjectModel.ReadOnlyCollection<Item>.Empty;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlydictionary-2?view=net-10.0">
  /// ReadOnlyDictionary&lt;Key, Item&gt;</see>.
  /// </summary>
  /// <remarks>
  /// <c>As</c>-only.
  /// </remarks>
  static public AsOrToTargetType<ReadOnlyDictionary<Key, Value>> ReadOnlyDictionary<Key, Value> ()
    where Key : notnull
  {
    Ctor<KeyValuePair<Key, Value>, ReadOnlyDictionary<Key,Value>> typedCtor = (e) => new ( ( IDictionary<Key, Value>) e );

    Empty<ReadOnlyDictionary<Key, Value>> empty = () => ObjectModel.ReadOnlyDictionary<Key, Value>.Empty;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }


  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlydictionary-2?view=net-10.0">
  /// ReadOnlyDictionary&lt;Key, Item&gt;</see>.
  /// </summary>
  /// <remarks>Calls to <see cref="ReadOnlyDictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key}, int?)"/>.</remarks>
  static public AsOrToTargetType<ReadOnlyDictionary<Key, Item>> ReadOnlyDictionary<Item, Key>
  (
    Func<Item, Key> keySelector,
    IEqualityComparer<Key> keyComparer,
    int? capacity
  )
  where Key : notnull
    => ReadOnlyDictionary ( keySelector, x => x, keyComparer, capacity );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlydictionary-2?view=net-10.0">
  /// ReadOnlyDictionary&lt;Key, Value&gt;</see>.
  /// </summary>
  /// <remarks>
  /// Uses
  /// <see cref="c_generic.Dictionary{Item, Key, Value}(Func{Item, Key}, Func{Item, Value}, IEqualityComparer{Key}, int?)"/> for
  /// intermediate <see cref="Dictionary{Key, Value}"/> production.
  /// </remarks>
  [SuppressMessage ( "Style", "IDE0028:Simplify ReadOnlyCollection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ReadOnlyDictionary<Key, Value>> ReadOnlyDictionary<Item, Key, Value>
  (
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    IEqualityComparer<Key> keyComparer,
    int? capacity
  )
  where Key : notnull
  {

    Ctor<Item, ReadOnlyDictionary<Key,Value>> typedCtor = (e) =>
    {
      Dictionary<Key, Value> dict = c_generic
      .Dictionary(keySelector, valueSelector, keyComparer, capacity)
      .Ctor(e);

      return new (dict);
    };

    Empty<ReadOnlyDictionary<Key, Value>> empty = () => ObjectModel.ReadOnlyDictionary<Key, Value>.Empty;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.ReadOnlyObservableCollection-1?view=net-10.0">
  /// ReadOnlyObservableCollection&lt;Item&gt;</see>.
  /// </summary>
  /// <remarks>
  ///  Uses
  /// <see cref="ObservableCollection{Item}"/> for intermediate <see cref="ObservableCollection{Item}"/> production.
  /// </remarks>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ReadOnlyObservableCollection<Item>> ReadOnlyObservableCollection<Item> ()
  {
    Ctor<Item, ReadOnlyObservableCollection<Item>> typedCtor = (e) =>
    {
      ObservableCollection<Item> oc = e is ObservableCollection<Item> x ? x
        : ObservableCollection<Item>().Ctor(e);

      return new(oc);
    };

    Empty<ReadOnlyObservableCollection<Item>> empty = () => ObjectModel.ReadOnlyObservableCollection<Item>.Empty;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }
}
