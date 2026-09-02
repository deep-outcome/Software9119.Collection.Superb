using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections?view=net-10.0">System.Collections Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections
{
  /// <summary>
  /// Target type for 
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.arraylist?view=net-10.0">ArrayList</see>.
  /// </summary>
  static public AsOrToTargetType<ArrayList> ArrayList ()
  {
    Ctor<ArrayList> ctor = (e, c) =>
    {
      ArrayList? result = null;
      if(c is int cap)
        result = new (cap);
      else if (e is ICollection coll)
        return new ArrayList ( coll);

      result ??= [];
      foreach (object item in e)
        _ = result.Add ( item );

      return result;
    };

    return new ( ctor, e => true, () => [] );
  }

  /// <summary>
  /// Target type for 
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.hashtable?view=net-10.0"/>.
  /// </summary>
  static public AsOrToTargetType<Hashtable> Hashtable<T> ( Func<T, object> keySelector ) => Hashtable ( keySelector, x => x );

  /// <summary>
  /// Target type for 
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.hashtable?view=net-10.0"/>.
  /// </summary>
  static public AsOrToTargetType<Hashtable> Hashtable<T> ( Func<T, object> keySelector, Func<T, object?> valueSelector )
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    Ctor<Hashtable> ctor = (e, c) =>
    {
      Hashtable result =  c is int cap ? new Hashtable ( cap ) : [];
      foreach (T item in e)
        result.Add ( keySelector(item), valueSelector(item) );

      return result;
    };

    return new ( ctor, e => false, () => [] );
  }

  /// <summary>
  /// Target type for 
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.queue?view=net-10.0"/>.
  /// </summary>
  static public AsOrToTargetType<Queue> Queue ()
  {
    Ctor<Queue > ctor = (e, c) =>
    {
      Queue? result = null;
      if(c is int cap)
        result = new (cap);
      else if (e is ICollection coll)
        return new Queue ( coll);

      result ??= [];
      foreach (object item in e)
        result.Enqueue ( item );

      return result;
    };

    return new ( ctor, e => true, () => [] );
  }

  /// <summary>
  /// Target type for 
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.sortedlist?view=net-10.0"/>.
  /// </summary>
  static public AsOrToTargetType<SortedList> SortedList<T> ( Func<T, object> keySelector ) => SortedList ( keySelector, x => x );

  /// <summary>
  /// Target type for 
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.sortedlist?view=net-10.0"/>.
  /// </summary>
  static public AsOrToTargetType<SortedList> SortedList<T> ( Func<T, object> keySelector, Func<T, object?> valueSelector )
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    Ctor<SortedList> ctor = (e, c) =>
    {
      SortedList result =  c is int cap ? new SortedList ( cap ) : [];
      foreach (T item in e)
        result.Add ( keySelector(item), valueSelector(item) );

      return result;
    };

    return new ( ctor, e => false, () => [] );
  }

  /// <summary>
  /// Target type for 
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.stack.-ctor?view=net-10.0"/>.
  /// </summary>
  static public AsOrToTargetType<Stack> Stack ()
  {
    Ctor<Stack > ctor = (e, c) =>
    {
      Stack ? result = null;
      if(c is int cap)
        result = new (cap);
      else if (e is ICollection coll)
        return new Stack  ( coll);

      result ??= [];
      foreach (object item in e)
        result.Push ( item );

      return result;
    };

    return new ( ctor, e => true, () => [] );
  }
}