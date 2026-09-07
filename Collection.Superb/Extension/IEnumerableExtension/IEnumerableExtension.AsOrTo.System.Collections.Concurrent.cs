using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

using collections_concurrent = Software9119.Collection.Superb.Extension.system_collections_concurrent;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ConcurrentBag {Item}"/>.
  /// </summary>
  /// <remarks>  
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, NullBehavior)"/> with
  /// <see cref="collections_concurrent.ConcurrentBag {Item}()"/>.  
  /// </remarks>
  static public ConcurrentBag<Item>? AsOrToConcurrentBag<Item>
  (
    this IEnumerable<Item>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<ConcurrentBag <Item>> targetType = collections_concurrent.ConcurrentBag<Item> ();
    return enumerable.AsOrTo ( targetType, null, behavior );
  }
}
