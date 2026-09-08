using System;
using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  static ArgumentNullException EnumerableNull ( string paramName ) => new ( paramName: paramName, "Null source enumerable encounter." );

  /// <summary>
  /// Engine behind <c>AsOrTo</c> and <c>Into</c> methods consuming <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/> .
  /// </summary>
  /// <exception cref="UnsupportedNullBehaviorException">When <paramref name="behavior"/> is unsupported behavior.</exception>
  /// <exception cref="ArgumentNullException">
  /// When <paramref name="behavior"/> is <see cref="NullBehavior.ThrowException"/> and <paramref name="enumerable"/> is
  /// <see langword="null"/>.
  /// </exception>
  /// <exception cref="ArgumentNullException">When <paramref name="asOrToType"/> is <see langword="null"/>.</exception>
  /// <remarks>
  /// When <paramref name="behavior"/> is <see cref="NullBehavior.ReturnEmpty"/>
  /// and <paramref name="enumerable"/> is <see langword="null"/>, <see cref="AsOrToTargetType{Target}.Empty"/> result
  /// is returned.
  /// </remarks>
  static public Target? AsOrTo<Target>
  (
    this IEnumerable? enumerable,
    AsOrToTargetType<Target> asOrToType,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    if (asOrToType == null)
      throw new ArgumentNullException ( paramName: nameof ( asOrToType ), "Target type is requisite." );

    if (enumerable.IsNull ())
    {
      if (behavior == NullBehavior.ReturnDefault)
        return default ( Target );

      if (behavior == NullBehavior.ReturnEmpty)
        return asOrToType.Empty ();

      enumerable = behavior switch
      {
        NullBehavior.ThrowException => throw EnumerableNull ( nameof ( enumerable ) ),
        _ => throw new UnsupportedNullBehaviorException ( behavior ),
      };
    }

    if (asOrToType.CanCast ( enumerable ))
      return (Target) enumerable;

    return asOrToType.Ctor ( enumerable );
  }
}

