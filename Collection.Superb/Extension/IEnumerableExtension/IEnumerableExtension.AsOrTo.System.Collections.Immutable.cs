using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

using collections_immutable = Software9119.Collection.Superb.Extension.system_collections_immutable;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ImmutableArray{Item}"/>.
  /// </summary>
  /// <remarks>  
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_immutable.ImmutableArray{Item}(bool)"/>.  
  /// </item>  
  /// <item>
  /// When <paramref name="length"/> is specified and <paramref name="enforceLengthCountMatch"/> is set to <see langword="true"/>
  /// and <paramref name="enumerable"/> is not castable and its count does not equal to <paramref name="length"/>,
  /// <see cref="System.InvalidOperationException"/> is thrown from within infrastructure.
  /// </item>
  /// </list>
  /// </remarks>  
  static public ImmutableArray<Item> AsOrToImmutableArray<Item>
  (
    this IEnumerable<Item>? enumerable,
    int? length = null,
    bool enforceLengthCountMatch = false,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ImmutableArray<Item>> targetType = collections_immutable.ImmutableArray<Item>(enforceLengthCountMatch);
    return enumerable.AsOrTo ( targetType, length, behavior );
  }
}
