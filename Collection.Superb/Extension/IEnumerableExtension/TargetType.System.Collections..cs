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
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<ArrayList> ArrayList ( int? capacity )
  {
    Ctor<ArrayList> ctor = (e) =>
    {
      ArrayList? result = null;
      if(capacity is int cap)
        result = new (cap);
      else if (e is ICollection coll)
        return new ArrayList ( coll);

      result ??= [];
      foreach (object item in e)
        _ = result.Add ( item );

      return result;
    };

    Empty<ArrayList> empty = () => new();
    return new ( ctor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.hashtable?view=net-10.0">Hashtable</see>.
  /// </summary>
  static public AsOrToTargetType<Hashtable> Hashtable<Item> ( Func<Item, object> keySelector, int? capacity )
    => Hashtable ( keySelector, x => x, capacity );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.hashtable?view=net-10.0">Hashtable</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<Hashtable> Hashtable<Item> ( Func<Item, object> keySelector, Func<Item, object?> valueSelector, int? capacity )
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    Ctor<Hashtable> ctor = (e) =>
    {
      Hashtable result =  capacity is int cap ? new Hashtable ( cap ) : [];
      foreach (Item item in e)
        result.Add ( keySelector(item), valueSelector(item) );

      return result;
    };

    Empty<Hashtable> empty = () => new();
    return new ( ctor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.queue?view=net-10.0">Queue</see>.
  /// </summary>
  static public AsOrToTargetType<Queue> Queue ( int? capacity )
  {
    Ctor<Queue > ctor = (e) =>
    {
      Queue? result = null;
      if(capacity is int cap)
        result = new (cap);
      else if (e is ICollection coll)
        return new Queue ( coll);

      result ??= [];
      foreach (object item in e)
        result.Enqueue ( item );

      return result;
    };

    Empty<Queue> empty = () => new();
    return new ( ctor, null, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.sortedlist?view=net-10.0">SortedList</see>.
  /// </summary>
  static public AsOrToTargetType<SortedList> SortedList<Item> ( Func<Item, object> keySelector, int? capacity )
    => SortedList ( keySelector, x => x, capacity );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.sortedlist?view=net-10.0">SortedList</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<SortedList> SortedList<Item> ( Func<Item, object> keySelector, Func<Item, object?> valueSelector, int? capacity )
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    Ctor<SortedList> ctor = (e) =>
    {
      SortedList result =  capacity is int cap ? new SortedList ( cap ) : [];
      foreach (Item item in e)
        result.Add ( keySelector(item), valueSelector(item) );

      return result;
    };

    Empty<SortedList> empty = () => new();
    return new ( ctor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.stack.-ctor?view=net-10.0">Stack</see>.
  /// </summary>
  static public AsOrToTargetType<Stack> Stack ( int? capacity )
  {
    Ctor<Stack > ctor = (e) =>
    {
      Stack ? result = null;
      if(capacity is int cap)
        result = new (cap);
      else if (e is ICollection coll)
        return new Stack  ( coll);

      result ??= [];
      foreach (object item in e)
        result.Push ( item );

      return result;
    };

    Empty<Stack> empty = () => new();
    return new ( ctor, null, empty );
  }
}