using Software9119.Collection.Superb.Storing;

using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of <c>Software9119.Collection.Superb.Storing</c> namespace.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class collection_superb_storing
{
  /// <summary>
  /// Target type for <see cref="Storing.Capacitor{Item}"/>.
  /// </summary>
  static public AsOrToTargetType<Capacitor<Item>> Capacitor<Item> ( int? capacity )
  {
    Ctor<Item, Capacitor<Item>> typedCtor = (e) => new Capacitor<Item>(e, capacity ?? 0);

    Empty<Capacitor<Item>> empty = () => new ();
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }

  /// <summary>
  /// Target type for <see cref="Storing.Accumulator{Item}"/>.
  /// </summary>
  static public AsOrToTargetType<Accumulator<Item>> Accumulator<Item> ( int? capacity )
  {
    Ctor<Item, Accumulator<Item>> typedCtor = (e) => new Accumulator<Item>(e, capacity ?? 0);

    Empty<Accumulator<Item>> empty = () => new ();
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }
}
