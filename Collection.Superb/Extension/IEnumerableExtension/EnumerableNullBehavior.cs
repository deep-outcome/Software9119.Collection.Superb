using System;
using System.Collections;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Behavior that drives method behavior when source enumerable is <see langword="null"/>.
/// </summary>
public enum EnumerableNullBehavior
{
  /// <summary>
  /// <see langword="default"/> is returned.
  /// </summary>
  /// <remarks>
  /// <list type="bullet">
  ///   <item><see langword="null"/> for reference types (and <see cref="Nullable{T}"/>).</item>
  ///   <item><see langword="default"/> for all <see langword="struct"/> types.</item>
  /// </list>
  /// </remarks>
  ReturnDefault,
  /// <summary>
  /// Empty target type is returned.
  /// </summary>
  /// <remarks>
  /// See also 
  /// <see cref="AsOrToTargetType{Target}.AsOrToTargetType(Func{IEnumerable, int?, Target}, bool, Func{Target})"/>.
  /// </remarks>
  ReturnEmpty,
  /// <summary>
  /// <see cref="System.ArgumentNullException"/> is thrown.
  /// </summary>
  ThrowException,
}