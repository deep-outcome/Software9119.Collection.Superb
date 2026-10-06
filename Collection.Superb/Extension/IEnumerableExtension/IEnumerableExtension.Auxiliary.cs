using Software9119.Collection.Superb.Numerics;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Various <see cref="IEnumerable{T}"/> extension methods.
/// </summary>
static public partial class IEnumerableExtension
{
  static internal ArgumentNullException NullEnumerable ( string enumerable ) => new ( paramName: enumerable, "Enumerable must be provided." );

  /// <returns><see langword="true"/> when <paramref name="enumerable"/> is at least <paramref name="atLeast"/> long.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="enumerable"/> is <see langword="null"/>.</exception>
  static public bool AtLeast<Item> ( this IEnumerable<Item?> enumerable, NonNegativeInt32 atLeast )
  {
    if (enumerable == null)
      throw NullEnumerable ( nameof ( enumerable ) );

    if (atLeast == 0)
      return true;

    return enumerable.Skip ( atLeast - 1 ).Any ();
  }

  /// <summary>
  /// Only proper <see cref="string"/> enumerable.
  /// </summary>
  ///<remarks>Proper <see cref="string"/> is non-empty, non-null and non-whitespace.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="enumerable"/> is <see langword="null"/>.</exception>
  static public IEnumerable<string> PureStrings ( this IEnumerable<string?> enumerable )
  {
    if (enumerable == null)
      throw NullEnumerable ( nameof ( enumerable ) );

    return enumerable.Where ( x => string.IsNullOrWhiteSpace ( x ) == false )!;
  }

  /// <summary>
  /// Only proper <see cref="string"/> enumerator.
  /// </summary>
  ///<remarks>Proper <see cref="string"/> is non-empty, non-null and non-whitespace.</remarks>
  /// <exception cref="ArgumentNullException">When <paramref name="enumerable"/> is <see langword="null"/>.</exception>
  static public IEnumerator<string> PureStringEnumerator ( this IEnumerable<string?> enumerable )
  {
    if (enumerable == null)
      throw NullEnumerable ( nameof ( enumerable ) );

    return enumerable.Where ( x => string.IsNullOrWhiteSpace ( x ) == false ).GetEnumerator ()!;
  }
}
