using System.Collections.Concurrent;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Expresses intended underlaying set type of blocking collection.
/// </summary>
public enum BlockingCollectionType
{
  /// <summary>
  /// <see cref="ConcurrentBag{T}"/> underlaying type.
  /// </summary>
  ConcurrentBag,
  /// <summary>
  /// <see cref="ConcurrentQueue{T}"/> underlaying type.
  /// </summary>
  ConcurrentQueue,
  /// <summary>
  /// <see cref="ConcurrentStack{T}"/> underlaying type.
  /// </summary>
  ConcurrentStack
}
