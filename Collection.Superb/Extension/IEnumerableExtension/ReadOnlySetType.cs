using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Expresses intended underlaying set type of read-only set.
/// </summary>
public enum ReadOnlySetType
{
  /// <summary>
  /// <see cref="FrozenSet{T}"/> underlaying type.
  /// </summary>
  FrozenSet,
  /// <summary>
  /// <see cref="HashSet{T}"/> underlaying type.
  /// </summary>
  HashSet,
  /// <summary>
  /// <see cref="SortedSet{T}"/>  underlaying type.
  /// </summary>
  SortedSet,
  /// <summary>
  /// <see cref="ImmutableHashSet{T}"/>  underlaying type.
  /// </summary>
  ImmutableHashSet,
  /// <summary>
  /// <see cref="ImmutableSortedSet{T}"/>  underlaying type.
  /// </summary>
  ImmutableSortedSet,
}

static class ReadOnlySetTypeExtension
{
  static public bool IsSortedSet ( this ReadOnlySetType type ) => type is (ReadOnlySetType.ImmutableSortedSet or ReadOnlySetType.SortedSet);
}