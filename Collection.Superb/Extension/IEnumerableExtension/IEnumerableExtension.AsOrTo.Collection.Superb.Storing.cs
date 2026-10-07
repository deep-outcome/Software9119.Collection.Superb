using Software9119.Collection.Superb.Storing;

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="Capacitor{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collection_superb_storing.Capacitor{Item}(int?)"/>.
  /// </remarks>
  [SuppressMessage ( "Design", "CA1002:Do not expose generic lists", Justification = "No help in here." )]
  static public Capacitor<Item>? AsOrToCapacitor<Item>
  (
    this IEnumerable<Item?>? enumerable,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<Capacitor<Item>> targetType = collection_superb_storing.Capacitor<Item>( capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="Accumulator{Item}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collection_superb_storing.Accumulator{Item}(int?)"/>.
  /// </remarks>
  [SuppressMessage ( "Design", "CA1002:Do not expose generic lists", Justification = "No help in here." )]
  static public Accumulator<Item>? AsOrToAccumulator<Item>
  (
    this IEnumerable<Item?>? enumerable,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<Accumulator<Item>> targetType = collection_superb_storing.Accumulator<Item>( capacity );
    return enumerable.AsOrTo ( targetType, behavior );
  }
}
