using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Target types for chosen types of
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized?view=net-10.0">
/// System.Collections.Specialized Namespace</see>.
/// </summary>
[SuppressMessage ( "Naming", "CA1707:Identifiers should not contain underscores", Justification = "Okay underscores." )]
[SuppressMessage ( "Style", "IDE1006:Naming Styles", Justification = "Okay style." )]
static public class system_collections_specialized
{

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.hybriddictionary?view=net-10.0">
  /// HybridDictionary</see>.
  /// </summary>
  static public AsOrToTargetType<HybridDictionary> HybridDictionary<Item> ( Func<Item, object> keySelector, int? capacity, bool caseInsensitive )
    => HybridDictionary ( keySelector, x => x, capacity, caseInsensitive );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.hybriddictionary?view=net-10.0">
  /// HybridDictionary</see>.
  /// </summary>
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<HybridDictionary> HybridDictionary<Item>
  (
    Func<Item, object> keySelector,
    Func<Item, object?> valueSelector,
    int? capacity,
    bool caseInsensitive
  )
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    Ctor<HybridDictionary> ctor = (e) =>
    {
      HybridDictionary result =  capacity is int cap ? new ( cap, caseInsensitive ) : new(caseInsensitive);
      foreach (Item i in e)
        result.Add ( keySelector(i), valueSelector(i) );

      return result;
    };

    Empty<HybridDictionary> empty = () => new();
    return new ( ctor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.listdictionary?view=net-10.0">
  /// ListDictionary</see>.
  /// </summary>
  static public AsOrToTargetType<ListDictionary> ListDictionary<Item>
  (
    Func<Item, object> keySelector,
    IComparer keyComparer
  )
    => ListDictionary ( keySelector, x => x, keyComparer );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.listdictionary?view=net-10.0">
  /// ListDictionary</see>.
  /// </summary>
  static public AsOrToTargetType<ListDictionary> ListDictionary<Item>
  (
    Func<Item, object> keySelector,
    Func<Item, object?> valueSelector,
    IComparer keyComparer
  )
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    if (keyComparer == null)
      throw new ArgumentNullException ( paramName: nameof ( keyComparer ), "Key comparer not provided." );

    Ctor<Item, ListDictionary> typedCtor = (e) =>
    {
      ListDictionary result = new  ( keyComparer );
      foreach (Item i in e )
        result.Add(keySelector(i), valueSelector(i));

      return result;
    };

    Empty<ListDictionary> empty = () => new (keyComparer);
    return AsOrToTargetType.FromTypedCtor ( typedCtor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.NameValueCollection?view=net-10.0">
  /// NameValueCollection</see>.
  /// </summary>  
  static public AsOrToTargetType<NameValueCollection> NameValueCollection<Item>
  (
    Func<Item, string> keySelector,
    Func<Item, string?> valueSelector,
    int? capacity,
    IEqualityComparer? keyComparer
  )
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    Ctor<NameValueCollection> ctor = (e) =>
    {
      NameValueCollection result = capacity is int cap ? new ( cap, keyComparer ) : new(keyComparer);
      foreach (Item i in e)
        result.Add ( keySelector(i), valueSelector(i) );

      return result;
    };

    Empty<NameValueCollection> empty = () => new(keyComparer);
    return new ( ctor, e => false, empty );
  }


  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.OrderedDictionary?view=net-10.0">
  /// OrderedDictionary</see>.
  /// </summary>  
  static public AsOrToTargetType<OrderedDictionary> OrderedDictionary<Item>
  (
    Func<Item, object> keySelector,
    int? capacity,
    IEqualityComparer? keyComparer
  )
    => OrderedDictionary ( keySelector, x => x, capacity, keyComparer );

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.OrderedDictionary?view=net-10.0">
  /// OrderedDictionary</see>.
  /// </summary>  
  static public AsOrToTargetType<OrderedDictionary> OrderedDictionary<Item>
  (
    Func<Item, object> keySelector,
    Func<Item, object?> valueSelector,
    int? capacity,
    IEqualityComparer? keyComparer
  )
  {
    if (keySelector == null)
      throw new ArgumentNullException ( paramName: nameof ( keySelector ), "Key selector not provided." );

    if (valueSelector == null)
      throw new ArgumentNullException ( paramName: nameof ( valueSelector ), "Value selector not provided." );

    Ctor<OrderedDictionary> ctor = (e) =>
    {
      OrderedDictionary result = capacity is int cap ? new ( cap, keyComparer ) : new(keyComparer);
      foreach (Item i in e)
        result.Add ( keySelector(i), valueSelector(i) );

      return result;
    };

    Empty<OrderedDictionary> empty = () => new(keyComparer);
    return new ( ctor, e => false, empty );
  }

  /// <summary>
  /// Target type for
  /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.collections.specialized.stringcollection?view=net-10.0">
  /// StringCollection</see>.
  /// </summary>  
  [SuppressMessage ( "Style", "IDE0028:Simplify collection initialization", Justification = "Obviousity." )]
  static public AsOrToTargetType<StringCollection> StringCollection<Item> ( Func<Item, string> selector )
  {
    if (selector == null)
      throw new ArgumentNullException ( paramName: nameof ( selector ), "Selector not provided." );

    Ctor<StringCollection> ctor = (e) =>
    {
      StringCollection result = new ();
      foreach (Item i in e)
        _ = result.Add ( selector(i) );

      return result;
    };

    Empty<StringCollection> empty = () => new();
    return new ( ctor, e => false, empty );
  }
}
