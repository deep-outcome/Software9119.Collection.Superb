using Software9119.Collection.Superb.Segmentation;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for segmentive types.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class segment_type
{
  /// <summary>
  /// Target type for <see cref="Segmentation.IListSegment"/>
  /// </summary>
  /// <remarks>
  /// Uses <see cref="system_collections.ArrayList(int?)"/> for intermediate <see cref="IList"/>.
  /// </remarks>
  static public AsOrToTargetType<IListSegment> IListSegment ( int? capacity, bool trimCapacity, IEqualityComparer<object?> itemComparer )
  {
    if (itemComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( itemComparer ), "Item comparer not provided." );

    Ctor<IListSegment> typedCtor = (e) =>
    {
      if (e is IList list)
        return new (list, itemComparer);

      ArrayList arrayList = system_collections.ArrayList(capacity).Ctor(e);

      if(trimCapacity)
        arrayList.TrimToSize();

      return new (arrayList, itemComparer);
    };

    Empty<IListSegment> empty = () => new (Array.Empty<object>());
    return new AsOrToTargetType<IListSegment> ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for <see cref="Segmentation.IListSegment{Item}"/>
  /// </summary>
  /// <remarks>
  /// Uses <see cref="system_collections_generic.IList{Item}(int?, bool)"/>  for intermediate <see cref="IList{Item}"/>.
  /// </remarks>
  static public AsOrToTargetType<IListSegment<Item?>> IListSegment<Item>
  (
    int? capacity,
    bool trimCapacity,
    IEqualityComparer<Item?> itemComparer
  )
  {
    if (itemComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( itemComparer ), "Item comparer not provided." );

    Ctor<Item, IListSegment<Item?>> typedCtor = (e) =>
    {
      IList<Item?> list = system_collections_generic.IList<Item?>(capacity, trimCapacity).Ctor(e);
      return new (list, itemComparer);
    };

    Empty<IListSegment<Item?>> empty = () => new (Array.Empty<Item?>());
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for <see cref="Segmentation.IReadOnlyListSegment{Item}"/>
  /// </summary>
  /// <remarks>
  /// Uses <see cref="system_collections_generic.IList{Item}(int?, bool)"/>  for intermediate <see cref="IList{Item}"/>.
  /// </remarks>
  static public AsOrToTargetType<IReadOnlyListSegment<Item?>> IReadOnlyListSegment<Item>
  (
    int? capacity,
    bool trimCapacity,
    IEqualityComparer<Item?> itemComparer
  )
  {
    if (itemComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( itemComparer ), "Item comparer not provided." );

    Ctor<Item, IReadOnlyListSegment<Item?>> typedCtor = (e) =>
    {
      IList<Item?> list = system_collections_generic.IList<Item?>(capacity, trimCapacity).Ctor(e);

      IReadOnlyList<Item?> roList = list is IReadOnlyList<Item?> x ? x : new ReadOnlyCollection<Item?>(list);
      return new (roList, itemComparer);
    };

    Empty<IReadOnlyListSegment<Item?>> empty = () => new (Array.Empty<Item?>());
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for <see cref="System.ArraySegment{Item}"/>
  /// </summary>
  static public AsOrToTargetType<ArraySegment<Item?>> ArraySegment<Item> ()
  {
    Ctor<Item, ArraySegment<Item?>> typedCtor = (e) =>
    {
      Item?[] array = e is Item?[] a ? a : e.ToArray();
      return new (array);
    };

    Empty<ArraySegment<Item?>> empty = () => new (Array.Empty<Item?>());
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for <see cref="System.Memory{Item}"/>
  /// </summary>
  static public AsOrToTargetType<Memory<Item?>> Memory<Item> ()
  {
    Ctor<Item, Memory<Item?>> typedCtor = (e) =>
    {
      Item?[] array = e is Item?[] a ? a : e.ToArray();
      return new (array);
    };

    Empty<Memory<Item?>> empty = () => new (Array.Empty<Item?>());
    CanCast canCast = e => false;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

  /// <summary>
  /// Target type for <see cref="System.ReadOnlyMemory{Item}"/>
  /// </summary>
  static public AsOrToTargetType<ReadOnlyMemory<Item?>> ReadOnlyMemory<Item> ()
  {
    Ctor<Item, ReadOnlyMemory<Item?>> typedCtor = (e) =>
    {
      Item?[] array = e is Item?[] a ? a : e.ToArray();
      return new (array);
    };

    Empty<ReadOnlyMemory<Item?>> empty = () => new (Array.Empty<Item?>());
    CanCast canCast = e => false;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }
}
