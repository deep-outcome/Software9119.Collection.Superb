using System;
using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Serves to <see cref="AsOrToTargetType{Target}"/>.
/// </summary>
static public class AsOrToTargetType
{
  /// <summary>
  /// Creates <see cref="AsOrToTargetType{Target}"/> from typed enumerable constructor function.  
  /// </summary>  
  /// <exception cref="ArgumentNullException">When <paramref name="ctor"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentNullException">When <paramref name="empty"/> is <see langword="null"/>.</exception>
  /// <remarks>
  /// Notice:
  /// <br/>
  /// - Set <paramref name="isCastable"/> to <see langword="true"/> if casting of source enumerable in 
  /// <see cref="IEnumerableExtension.AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, EnumerableNullBehavior)"/> to target type 
  /// should be tried before enumeration to target type.
  /// <br/>  
  /// - <paramref name="empty"/> is used by 
  /// <see cref="IEnumerableExtension.AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, EnumerableNullBehavior)"/>
  /// to solve <see cref="EnumerableNullBehavior.ReturnEmpty"/> null case.
  /// </remarks>
  static public AsOrToTargetType<Target> FromTypedCtor<Item, Target>
  (
    Func<IEnumerable<Item>, int?, Target> ctor,
    bool isCastable,
    Func<Target> empty
  )
  {
    if (ctor is null)
      throw new ArgumentNullException ( paramName: nameof ( ctor ), "Constructor must be provided." );

    if (empty is null)
      throw new ArgumentNullException ( paramName: nameof ( empty ), "Empty constructor must be provided." );

    Func<IEnumerable, int?, Target> conversion = ( x, c ) =>
    {
      IEnumerable<Item> e = (IEnumerable<Item>) x;
      return ctor ( e, c );
    };

    return new AsOrToTargetType<Target> ( conversion, isCastable, empty );
  }
}

/// <summary>
/// Used by <see cref="IEnumerableExtension.AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, EnumerableNullBehavior)"/>.
/// </summary>
public class AsOrToTargetType<Target>
{
  /// <summary>
  /// Target type constructor.
  /// </summary>
  /// <exception cref="ArgumentNullException">When <paramref name="ctor"/> is <see langword="null"/>.</exception>
  /// <exception cref="ArgumentNullException">When <paramref name="empty"/> is <see langword="null"/>.</exception>
  /// <remarks>
  /// Notice:
  /// <br/>
  /// - Set <paramref name="isCastable"/> to <see langword="true"/> if casting of source enumerable in 
  /// <see cref="IEnumerableExtension.AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, EnumerableNullBehavior)"/> to target type 
  /// should be tried before enumeration to target type.
  /// <br/>  
  /// - <paramref name="empty"/> is used by 
  /// <see cref="IEnumerableExtension.AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, EnumerableNullBehavior)"/>
  /// to solve <see cref="EnumerableNullBehavior.ReturnEmpty"/> null case.
  /// </remarks>
  public AsOrToTargetType ( Func<IEnumerable, int?, Target> ctor, bool isCastable, Func<Target> empty )
  {
    if (ctor is null)
      throw new ArgumentNullException ( paramName: nameof ( ctor ), "Constructor must be provided." );

    if (empty is null)
      throw new ArgumentNullException ( paramName: nameof ( empty ), "Empty constructor must be provided." );

    Ctor = ctor;
    TryCast = isCastable;
    Empty = empty;
  }

  /// <summary>
  /// Empty instance of <typeparamref name="Target"/> constructor.
  /// </summary>  
  public Func<Target> Empty { get; private init; }

  /// <summary>
  /// Whether casting to <see cref="TypeOfTarget"/> should be tried on source enumerable.
  /// </summary>
  public bool TryCast { get; private init; }

  /// <summary>
  /// Type of <typeparamref name="Target"/>.
  /// </summary>
  public Type TypeOfTarget { get; private init; } = typeof ( Target );

  /// <summary>
  /// Target type <typeparamref name="Target"/> constructor.
  /// </summary>
  public Func<IEnumerable, int?, Target> Ctor { get; private init; }
}