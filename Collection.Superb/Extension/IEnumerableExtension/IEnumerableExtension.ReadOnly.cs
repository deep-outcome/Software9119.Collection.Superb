using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Extension;

using NullBehavior = EnumerableNullBehavior;

/// <summary>
/// <see cref="IEnumerable{T}"/> extension methods.
/// </summary>
static public partial class IEnumerableExtension
{

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
  public const int DefaultListCapacity = 8;
  public const int DefaultDictCapacity = 8;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


  static internal ArgumentNullException EnumerableNull ( string paramName ) => new ( paramName: paramName, "Null source enumerable encounter." );
  static internal ArgumentNullException DictionaryNull ( string paramName ) => new ( paramName: paramName, "Null source dictionary encounter." );

  /// <summary>
  /// Copies or casts <paramref name="enumerable"/> into <see cref="IList{T}"/>.
  /// </summary>
  /// <remarks>
  /// Cast/Copy Table
  /// <code>
  /// ╔════════════════╦══════════╦════════╗
  /// ║   enumerable   ║  result  ║ action ║
  /// ╠════════════════╬══════════╬════════╣
  /// ║ IEnumerable&lt;T&gt; ║ List&lt;T&gt;  ║ Copy   ║
  /// ║ ICollection&lt;T&gt; ║ T []     ║ Copy   ║  
  /// ║ IList&lt;T&gt;       ║ IList&lt;T&gt; ║ Cast   ║
  /// ╚════════════════╩══════════╩════════╝
  /// </code>
  /// <paramref name="capacity"/> can be used to capacitate <see cref="List{T}"/> sufficiently before population from <paramref name="enumerable"/>.
  /// </remarks>
  /// <exception cref="ArgumentNullException">
  /// When <paramref name="behavior"/> is <see cref="NullBehavior.ThrowException"/> and <paramref name="enumerable"/> is 
  /// <see langword="null"/>.
  /// </exception>
  /// <exception cref="UnsupportedNullBehaviorException">When <paramref name="behavior"/> is unsupported behavior.</exception>
  [SuppressMessage ( "Style", "IDE0305:Simplify collection initialization", Justification = "Obviousity." )]
  static public IList<T>? AsOrToIList<T>
  (
    this IEnumerable<T>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty,
    int capacity = DefaultListCapacity
  )
  {
    if (enumerable.IsNull ())
    {
      return behavior switch
      {
        NullBehavior.ReturnEmpty => Array.Empty<T> (),
        NullBehavior.ReturnDefault => null,
        NullBehavior.ThrowException => throw EnumerableNull ( nameof ( enumerable ) ),
        _ => throw new UnsupportedNullBehaviorException ( behavior ),
      };
    }

    if (enumerable is IList<T> ilist)
      return ilist;

    if (enumerable is ICollection<T> collection)
    {
      T[] array = new T[collection.Count];
      collection.CopyTo ( array, 0 );
      return array;
    }

    List<T> list = new(capacity);
    list.AddRange ( enumerable );
    return list;
  }

  /// <summary>
  /// <see cref="ReadOnlyCollection{T}"/> from any enumerable.
  /// </summary>
  /// <remarks>
  /// Casts <paramref name="enumerable"/> into <see cref="ReadOnlyCollection{T}"/> or delegates it
  /// to <see cref="AsOrToIList"/> for processing and then puts result into
  /// <see cref="ReadOnlyCollection{T}"/>.
  /// </remarks>
  static public ReadOnlyCollection<T>? AsOrToReadOnlyCollection<T> (
    this IEnumerable<T>? enumerable,
    NullBehavior behavior = NullBehavior.ReturnEmpty,
    int capacity = DefaultListCapacity )
  {
    if (enumerable is ReadOnlyCollection<T> coll)
      return coll;

    IList<T>? ilist = enumerable.AsOrToIList ( behavior, capacity );
    return ilist is null ? null : new ReadOnlyCollection<T> ( ilist );
  }


  /// <summary>
  /// Creates <see cref="ReadOnlyDictionary{Key,Value}"/> from <paramref name="enumerable"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="ToReadOnlyDictionary{Item,Key,Value}(IEnumerable{Item},Func{Item,Key},Func{Item,Value},NullBehavior, int)"/>.
  /// </remarks>
  static public ReadOnlyDictionary<Key, Item>? ToReadOnlyDictionary<Item, Key>
  (
    this IEnumerable<Item>? enumerable,
    Func<Item, Key> keySelector,
    NullBehavior behavior = NullBehavior.ReturnEmpty,
    int capacity = DefaultDictCapacity
  )
    where Key : notnull
  {
    return enumerable.ToReadOnlyDictionary ( keySelector, v => v, behavior, capacity );
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
  static public ReadOnlyDictionary<Key, Value>? ToReadOnlyDictionary<Item, Key, Value>
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

  /// <summary>
  /// Puts see <paramref name="dict"/> into <see cref="ReadOnlyDictionary{Key,Value}"/>.
  /// </summary>
  /// <exception cref="ArgumentNullException">  
  /// When <paramref name="behavior"/> is <see cref="NullBehavior.ThrowException"/> and <paramref name="dict"/> is 
  /// <see langword="null"/>.
  /// </exception>
  /// <exception cref="UnsupportedNullBehaviorException">When <paramref name="behavior"/> is unsupported behavior.</exception>
  static public ReadOnlyDictionary<Key, Value>? AsReadOnlyDictionary<Key, Value>
  (
    this IDictionary<Key, Value>? dict,
    NullBehavior behavior = NullBehavior.ReturnEmpty
  )
    where Key : notnull
  {
    if (dict.IsNull ())
    {
      return behavior switch
      {
        NullBehavior.ReturnEmpty => ReadOnlyDictionary<Key, Value>.Empty,
        NullBehavior.ReturnDefault => null,
        NullBehavior.ThrowException => throw DictionaryNull ( nameof ( dict ) ),
        _ => throw new UnsupportedNullBehaviorException ( behavior ),
      };
    }

    return new ReadOnlyDictionary<Key, Value> ( dict );
  }
}
