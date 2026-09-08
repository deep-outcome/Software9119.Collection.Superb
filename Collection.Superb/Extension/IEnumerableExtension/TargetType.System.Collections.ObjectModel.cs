using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel?view=net-10.0">
/// System.Collections.ObjectModel Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_objectmodel
{
  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.collection-1?view=net-10.0">
  /// Collection&lt;Item&gt;</see>.
  /// </summary>
  /// <remarks>
  /// Uses <see cref="system_collections_generic.IList{Item}(int?)"/> for <see cref="IList{Item}"/> production.
  /// </remarks>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<Collection<Item>> Collection<Item> ( int? capacity )
  {
    Ctor<Item, Collection<Item>> typedCtor = (e) =>
    {
      IList<Item> ilist = system_collections_generic.IList<Item>( capacity ).Ctor( e );
      return new(ilist);
    };

    Empty<Collection<Item>> empty = () => new ();
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }
}
