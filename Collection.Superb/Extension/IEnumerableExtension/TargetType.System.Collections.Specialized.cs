using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized?view=net-10.0">
/// System.Collections.Specialized Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_specialized
{

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.hybriddictionary?view=net-10.0">
  /// HybridDictionary</see>.
  /// </summary>
  static public AsOrToTargetType<HybridDictionary> HybridDictionary<Item> ( Func<Item, object> keySelector, int? capacity, bool caseInsensitive )
    => HybridDictionary ( keySelector, x => x, capacity, caseInsensitive );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.hybriddictionary?view=net-10.0">
  /// HybridDictionary</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<HybridDictionary> HybridDictionary<Item>
  (
    Func<Item, object> keySelector,
    Func<Item, object?> valueSelector,
    int? capacity,
    bool caseInsensitive
  )
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    Ctor<HybridDictionary> ctor = (e) =>
    {
      HybridDictionary result =  capacity is int cap ? new ( cap, caseInsensitive ) : new(caseInsensitive);
      foreach (Item item in e)
        result.Add ( keySelector(item), valueSelector(item) );

      return result;
    };

    Empty<HybridDictionary> empty = () => new();
    return new ( ctor, e => false, empty );
  }
}
