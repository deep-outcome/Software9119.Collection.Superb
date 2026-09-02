using System;
using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Extension;

static public partial class IEnumerableExtension
{
  // array list

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ArrayList"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with <see cref="system_collections.ArrayList ()"/>.
  /// </remarks>
  static public ArrayList? AsOrToArrayList<T> ( this IEnumerable<T> enumerable, int? capacity = null, NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.ArrayList (), capacity, behavior );


  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="ArrayList"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with <see cref="system_collections.ArrayList ()"/>.
  /// </remarks>
  static public ArrayList? AsOrToArrayList ( this IEnumerable enumerable, int? capacity = null, NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.ArrayList (), capacity, behavior );

  // hashtable

  /// <summary>
  /// Creates <see cref="Hashtable"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> 
  /// with <see cref="system_collections.Hashtable{T}(Func{T, object})"/>.
  /// </remarks>
  static public Hashtable? ToHashtable<T> (
    this IEnumerable<T> enumerable,
    Func<T, object> keySelector,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.Hashtable ( keySelector ), capacity, behavior );

  /// <summary>
  /// Creates <see cref="Hashtable"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> 
  /// with <see cref="system_collections.Hashtable{T}(Func{T, object})"/>.
  /// </remarks>
  static public Hashtable? ToHashtable (
    this IEnumerable enumerable,
    Func<object, object> keySelector,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.Hashtable ( keySelector ), capacity, behavior );

  /// <summary>
  /// Creates <see cref="Hashtable"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/>
  /// and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with 
  /// <see cref="system_collections.Hashtable{T}(Func{T, object}, Func{T, object?})"/>.
  /// </remarks>
  static public Hashtable? ToHashtable<T> (
    this IEnumerable<T> enumerable,
    Func<T, object> keySelector,
    Func<T, object> valueSelector,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.Hashtable ( keySelector, valueSelector ), capacity, behavior );

  /// <summary>
  /// Creates <see cref="Hashtable"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/> 
  /// and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with 
  /// <see cref="system_collections.Hashtable{T}(Func{T, object}, Func{T, object?})"/>.
  /// </remarks>
  static public Hashtable? ToHashtable (
    this IEnumerable enumerable,
    Func<object, object> keySelector,
    Func<object, object> valueSelector,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.Hashtable ( keySelector, valueSelector ), capacity, behavior );

  // queue

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="Queue"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with <see cref="system_collections.Queue ()"/>.
  /// </remarks>
  static public Queue? AsOrToQueue<T> ( this IEnumerable<T> enumerable, int? capacity = null, NullBehavior behavior = NullBehavior.ReturnEmpty )
   => enumerable.AsOrTo ( system_collections.Queue (), capacity, behavior );

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="Queue"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with <see cref="system_collections.Queue ()"/>.
  /// </remarks>
  static public Queue? AsOrToQueue ( this IEnumerable enumerable, int? capacity = null, NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.Queue (), capacity, behavior );

  // sorted list

  /// <summary>
  /// Creates <see cref="SortedList"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> 
  /// with <see cref="system_collections.SortedList{T}(Func{T, object})"/>.
  /// </remarks>
  static public SortedList? ToSortedList<T> (
    this IEnumerable<T> enumerable,
    Func<T, object> keySelector,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.SortedList ( keySelector ), capacity, behavior );

  /// <summary>
  /// Creates <see cref="SortedList"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> 
  /// with <see cref="system_collections.SortedList{T}(Func{T, object})"/>.
  /// </remarks>
  static public SortedList? ToSortedList (
    this IEnumerable enumerable,
    Func<object, object> keySelector,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.SortedList ( keySelector ), capacity, behavior );

  /// <summary>
  /// Creates <see cref="SortedList"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/> 
  /// and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with 
  /// <see cref="system_collections.SortedList{T}(Func{T, object}, Func{T, object?})"/>.
  /// </remarks>
  static public SortedList? ToSortedList<T> (
    this IEnumerable<T> enumerable,
    Func<T, object> keySelector,
    Func<T, object> valueSelector,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.SortedList ( keySelector, valueSelector ), capacity, behavior );

  /// <summary>
  /// Creates <see cref="SortedList"/> from <paramref name="enumerable"/> using <paramref name="keySelector"/> 
  /// and <paramref name="valueSelector"/> provided.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with 
  /// <see cref="system_collections.SortedList{T}(Func{T, object}, Func{T, object?})"/>.
  /// </remarks>
  static public SortedList? ToSortedList (
    this IEnumerable enumerable,
    Func<object, object> keySelector,
    Func<object, object> valueSelector,
    int? capacity = null,
    NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.SortedList ( keySelector, valueSelector ), capacity, behavior );

  // stack

  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="Stack"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with <see cref="system_collections.Stack ()"/>.
  /// </remarks>
  static public Stack? AsOrToStack<T> ( this IEnumerable<T> enumerable, int? capacity = null, NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.Stack (), capacity, behavior );


  /// <summary>
  /// Casts or copies <paramref name="enumerable"/> into <see cref="Stack"/>.
  /// </summary>
  /// <remarks>
  /// Calls to <see cref="AsOrTo{T}(IEnumerable, AsOrToTargetType{T}, int?, NullBehavior)"/> with <see cref="system_collections.Stack ()"/>.
  /// </remarks>
  static public Stack? AsOrToStack ( this IEnumerable enumerable, int? capacity = null, NullBehavior behavior = NullBehavior.ReturnEmpty )
    => enumerable.AsOrTo ( system_collections.Stack (), capacity, behavior );
}



