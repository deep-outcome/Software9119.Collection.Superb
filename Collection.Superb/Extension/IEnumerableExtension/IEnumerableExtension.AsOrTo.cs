using System;
using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  /// <summary>
  /// Engine behind <c>AsOrTo</c> methods consuming <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/> .
  /// </summary>
  /// <exception cref="UnsupportedNullBehaviorException">When <paramref name="behavior"/> is unsupported behavior.</exception>
  /// <exception cref="ArgumentNullException">
  /// When <paramref name="behavior"/> is <see cref="NullBehavior.ThrowException"/> and <paramref name="enumerable"/> is 
  /// <see langword="null"/>.
  /// </exception>
  /// <exception cref="ArgumentNullException">When <paramref name="asOrToType"/> is <see langword="null"/>.</exception>
  /// <remarks>
  /// <list type="bullet">
  /// <item>
  /// <paramref name="capacity"/> is passed to <see cref="AsOrToTargetType{Target}.Ctor"/>.
  /// </item>
  /// <item>
  /// This means it is client code responsibility to ensure correct behavior of constructor with capacity provided, e.g. 
  /// its suffieciency or  non-exceedance.
  /// </item>  
  /// <item>  
  /// When <paramref name="behavior"/> is <see cref="NullBehavior.ReturnEmpty"/> 
  /// and <paramref name="enumerable"/> is <see langword="null"/>, <see cref="AsOrToTargetType{Target}.Empty"/> result
  /// is returned.
  /// </item>  
  /// </list>
  /// </remarks>
  static public T? AsOrTo<T>
  (
    this IEnumerable enumerable,
    AsOrToTargetType<T> asOrToType,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
  {
    if (asOrToType == null)
      throw new ArgumentNullException ( paramName: nameof ( asOrToType ), "Target type is requisite." );

    if (enumerable.IsNull ())
    {
      if (behavior == NullBehavior.ReturnDefault)
        return default ( T );

      if (behavior == NullBehavior.ReturnEmpty)
        return asOrToType.Empty ();

      enumerable = behavior switch
      {
        NullBehavior.ThrowException => throw EnumerableNull ( nameof ( enumerable ) ),
        _ => throw new UnsupportedNullBehaviorException ( behavior ),
      };
    }

    if (asOrToType.CanCast ( enumerable ))
      if (enumerable.GetType () == asOrToType.TypeOfTarget)
        return (T) enumerable;

    return asOrToType.Ctor ( enumerable, capacity );
  }
}

