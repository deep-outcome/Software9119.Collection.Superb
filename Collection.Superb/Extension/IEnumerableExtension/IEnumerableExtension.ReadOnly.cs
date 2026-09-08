using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// <see cref="IEnumerable{T}"/> extension methods.
/// </summary>
static public partial class IEnumerableExtension
{

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member  
  public const int DefaultDictCapacity = 8;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


  static internal ArgumentNullException EnumerableNull ( string paramName ) => new ( paramName: paramName, "Null source enumerable encounter." );
  static internal ArgumentNullException DictionaryNull ( string paramName ) => new ( paramName: paramName, "Null source dictionary encounter." );

  /// <summary>
  /// Creates <see cref="ReadOnlyDictionary{Key,Value}"/> from <paramref name="enumerable"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="IntoReadOnlyDictionary{Item,Key,Value}(IEnumerable{Item},Func{Item,Key},Func{Item,Value},NullBehavior, int)"/>.
  /// </remarks>
  static public ReadOnlyDictionary<Key, Item>? IntoReadOnlyDictionary<Item, Key>
  (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    NullBehavior behavior = NullBehavior.ReturnEmpty,
    int capacity = DefaultDictCapacity
  )
    where Key : notnull
  {
    return enumerable.IntoReadOnlyDictionary ( keySelector, v => v, behavior, capacity );
  }

  /// <summary>
  /// Creates <see cref="ReadOnlyDictionary{Key,Value}"/> from <paramref name="enumerable"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">
  /// When <paramref name="behavior"/> is <see cref="NullBehavior.ThrowException"/> and <paramref name="enumerable"/> is
  /// <see langword="null"/> or when either of <paramref name="keySelector"/>, <paramref name="valueSelector"/> is
  /// <see langword="null"/>.
  /// </exception>
  /// <exception cref="UnsupportedNullBehaviorException">When <paramref name="behavior"/> is unsupported behavior.</exception>
  static public ReadOnlyDictionary<Key, Value>? IntoReadOnlyDictionary<Item, Key, Value>
  (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    Func<Item, Value> valueSelector,
    NullBehavior behavior = NullBehavior.ReturnEmpty,
    int capacity = DefaultDictCapacity
  )
    where Key : notnull
  {
    if (enumerable.IsNull ())
    {
      return behavior switch
      {
        NullBehavior.ReturnEmpty => ReadOnlyDictionary<Key, Value>.Empty,
        NullBehavior.ReturnDefault => null,
        NullBehavior.ThrowException => throw EnumerableNull ( nameof ( enumerable ) ),
        _ => throw new UnsupportedNullBehaviorException ( behavior ),
      };
    }

    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    Dictionary<Key, Value> dict = new (capacity);
    foreach (Item item in enumerable)
      dict.Add ( keySelector ( item ), valueSelector ( item ) );

    return new ReadOnlyDictionary<Key, Value> ( dict );
  }

  
}
