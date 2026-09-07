using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent?view=net-10.0">
/// System.Collections.Concurrent Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_concurrent
{
  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=net-10.0">
  /// ConcurrentBag&lt;Item&gt;</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  [SuppressMessage ( "Style", "IDE0306:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ConcurrentBag<Item>> ConcurrentBag<Item> ()
  {
    Ctor<Item, ConcurrentBag <Item>> typedCtor = (e, c) => new (e);

    Empty<ConcurrentBag <Item>> empty = () => new ();
    CanCast? canCast = null;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, canCast, empty );
  }

}
