using System;
using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Delegate decides whether source <paramref name="e"/> can be cast to target type or not.
/// </summary>
public delegate bool CanCast ( IEnumerable e );

/// <summary>
/// Target type typed constructor delegate.
/// </summary>
public delegate Target Ctor<Item, Target> ( IEnumerable<Item> e, int? capacity );

/// <summary>
/// Target type constructor delegate.
/// </summary>
public delegate Target Ctor<Target> ( IEnumerable e, int? capacity );

/// <summary>
/// Empty target type constructor delegate.
/// </summary>
public delegate Target Empty<Target> ();

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
  /// - <paramref name="canCast"/> is used by
  /// <see cref="IEnumerableExtension.AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, EnumerableNullBehavior)"/>
  /// for checking whether source enumerable can be cast to target type. Defaults to <c>e => e is Target</c>.
  /// <br/>
  /// - <paramref name="empty"/> is used by
  /// <see cref="IEnumerableExtension.AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, EnumerableNullBehavior)"/>
  /// to solve <see cref="EnumerableNullBehavior.ReturnEmpty"/> null case.
  /// </remarks>
  static public AsOrToTargetType<Target> FromTypedCtor<Item, Target>
  (
    Ctor<Item, Target> ctor,
    CanCast? canCast,
    Empty<Target> empty
  )
  {
    if (ctor is null)
      throw new ArgumentNullException ( paramName: nameof ( ctor ), "Constructor must be provided." );

    if (empty is null)
      throw new ArgumentNullException ( paramName: nameof ( empty ), "Empty constructor must be provided." );

    Ctor<Target> conversion = ( x, c ) =>
    {
      IEnumerable<Item> e = (IEnumerable<Item>) x;
      return ctor ( e, c );
    };

    return new AsOrToTargetType<Target> ( conversion, canCast, empty );
  }
}

/// <summary>
/// Used by <see cref="IEnumerableExtension.AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, EnumerableNullBehavior)"/>.
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
  /// - <paramref name="canCast"/> is used by
  /// <see cref="IEnumerableExtension.AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, EnumerableNullBehavior)"/>
  /// for checking whether source enumerable can be cast to target type. Defaults to <c>e => e is Target</c>.
  /// <br/>
  /// - <paramref name="empty"/> is used by
  /// <see cref="IEnumerableExtension.AsOrTo{Target}(IEnumerable, AsOrToTargetType{Target}, int?, EnumerableNullBehavior)"/>
  /// to solve <see cref="EnumerableNullBehavior.ReturnEmpty"/> null case.
  /// </remarks>
  public AsOrToTargetType ( Ctor<Target> ctor, CanCast? canCast, Empty<Target> empty )
  {
    if (ctor is null)
      throw new ArgumentNullException ( paramName: nameof ( ctor ), "Constructor must be provided." );

    if (empty is null)
      throw new ArgumentNullException ( paramName: nameof ( empty ), "Empty constructor must be provided." );

    Ctor = ctor;
    CanCast = canCast ?? (e => e is Target);
    Empty = empty;
  }

  /// <summary>
  /// Empty instance of <typeparamref name="Target"/> constructor.
  /// </summary>
  public Empty<Target> Empty { get; private set; }

  /// <summary>
  /// Whether casting to <typeparamref name="Target"/> should be performed on source enumerable.
  /// </summary>
  public CanCast CanCast { get; private set; }

  /// <summary>
  /// Target type <typeparamref name="Target"/> constructor.
  /// </summary>
  public Ctor<Target> Ctor { get; private set; }
}