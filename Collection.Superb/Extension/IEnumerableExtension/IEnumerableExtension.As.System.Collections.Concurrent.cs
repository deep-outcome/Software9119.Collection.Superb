using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

using c_concurrent = Software9119.Collection.Superb.Extension.system_collections_concurrent;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Wraps <paramref name="enumerable"/> into <see cref="OrderablePartitioner {Item}"/>.
  /// </summary>
  /// <remarks> 
  /// <list type="bullet">
  /// <item>
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_concurrent.OrderablePartitioner{Item}(EnumerablePartitionerOptions, bool)"/>.  
  /// </item>
  /// <item>
  /// <paramref name="dynamicLoadBalancing"/> is applied for <paramref name="enumerable"/> which is <see cref="IList{Item}"/> implementation.
  /// </item>
  /// <item>
  /// <paramref name="options"/> are applied for <paramref name="enumerable"/> not implementing <see cref="IList{Item}"/>.
  /// </item>
  /// </list>
  /// </remarks>
  static public OrderablePartitioner<Item>? AsOrderablePartitioner<Item>
  (
    this IEnumerable<Item>? enumerable,
    bool dynamicLoadBalancing = true,
    EnumerablePartitionerOptions options = EnumerablePartitionerOptions.None,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<OrderablePartitioner <Item>> targetType = c_concurrent.OrderablePartitioner<Item> (options, dynamicLoadBalancing);
    return enumerable.AsOrTo ( targetType, behavior );
  }

  /// <summary>
  /// Wraps <paramref name="collection"/> into <see cref="BlockingCollection {Item}"/>.
  /// </summary>
  /// <remarks> 
  /// Calls to <see cref="AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, NullBehavior)"/> with
  /// <see cref="c_concurrent.BlockingCollection{Item}(int?)"/>.
  /// </remarks>
  static public BlockingCollection<Item>? AsBlockingCollection<Item>
  (
    this IProducerConsumerCollection<Item> collection,
    int? boundedCapacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    AsOrToTargetType<BlockingCollection <Item>> targetType = c_concurrent.BlockingCollection<Item> (boundedCapacity);
    return collection.AsOrTo ( targetType, behavior );
  }
}
