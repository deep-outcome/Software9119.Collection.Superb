using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Immutable = System.Collections.Immutable;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable?view=net-10.0">
/// System.Collections.Immutable Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_immutable
{
  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablearray-1?view=net-10.0">
  /// ImmutableArray&lt;Item&gt;</see>.
  /// </summary>    
  [SuppressMessage ( "Style", "IDE0303:Simplify collection initialization", Justification = "Obviousity." )]
  [SuppressMessage ( "Style", "IDE0301:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ImmutableArray<Item>> ImmutableArray<Item> ( bool strictLengthMatch )
  {
    Ctor<Item, ImmutableArray<Item>> typedCtor = (e, c) =>
    {
      if (c is int capacity)
      {
        ImmutableArray<Item>.Builder builder = Immutable.ImmutableArray<Item>.Empty.ToBuilder();
        builder.Capacity = capacity;
        builder.AddRange(e);

        return strictLengthMatch ? builder.MoveToImmutable() : builder.DrainToImmutable();
      }

      return Immutable.ImmutableArray.CreateRange(e);
    };

    Empty<ImmutableArray<Item>> empty = () => Immutable.ImmutableArray<Item>.Empty;
    return AsOrToTargetType.FromTypedCtor ( typedCtor, null, empty );
  }
}
