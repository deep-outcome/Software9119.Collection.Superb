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
}
