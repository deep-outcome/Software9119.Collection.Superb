using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using collections_objectmodel = Software9119.Collection.Superb.Extension.system_collections_objectmodel;
namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Wraps <paramref name="dictionary"/> into <see cref="ReadOnlyDictionary{Key, Value}"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="collections_objectmodel.ReadOnlyDictionary{Key,Value}()"/>.
  /// </remarks>
  static public ReadOnlyDictionary<Key, Value>? AsReadOnlyDictionary<Key, Value>
  (
    this IDictionary<Key, Value>? dictionary,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
    where Key : notnull
  {
    AsOrToTargetType<ReadOnlyDictionary<Key, Value>> targetType = collections_objectmodel.ReadOnlyDictionary<Key, Value>();
    return dictionary.AsOrTo ( targetType, behavior );
  }
}
