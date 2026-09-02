using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using FrozenDict = System.Collections.Frozen.FrozenDictionary;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen?view=net-10.0">
/// System.Collections.Frozen Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_frozen
{
  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary?view=net-10.0">FrozenDictionary.</see>
  /// </summary>
  static public AsOrToTargetType<FrozenDictionary<Key, Source>> FrozenDictionary<Source, Key> (
    Func<Source, Key> keySelector,
    IEqualityComparer<Key> keyComparer
  ) where Key : notnull
    => FrozenDictionary ( keySelector, x => x, keyComparer );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary?view=net-10.0">FrozenDictionary.</see>
  /// </summary>
  static public AsOrToTargetType<FrozenDictionary<Key, Value>> FrozenDictionary<Source, Key, Value>
  (
    Func<Source, Key> keySelector,
    Func<Source, Value> valueSelector,
    IEqualityComparer<Key> keyComparer
  )
  where Key : notnull
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    Func<IEnumerable<Source>, int?, FrozenDictionary<Key,Value>> typedCtor = (e, c) =>
    {
      FrozenDictionary<Key,Value> result = FrozenDict.ToFrozenDictionary(e, keySelector, valueSelector, keyComparer);
      return result;
    };

    Func<FrozenDictionary<Key, Value>> empty = () => FrozenDict.ToFrozenDictionary<Key, Value> ( [], keyComparer );
    return AsOrToTargetType.FromTypedCtor ( typedCtor, false, empty );
  }
}
