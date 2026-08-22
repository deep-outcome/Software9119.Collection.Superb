using Software9119.Collection.Superb.Segmentation;

using System.Diagnostics.CodeAnalysis;

sealed class Program
{
  [SuppressMessage ( "Style", "IDE0008:Use explicit type", Justification = "Unimportant." )]
  [SuppressMessage ( "Style", "IDE0059:Unnecessary assignment of a value", Justification = "Unimportant." )]
  static void Main ()
  {
    IListSegment ();
    IListRefSegment ();
  }

  static void IListSegment ()
  {
    var segment = new IListSegment<int>([]);
    var comparer = new IListSegmentEqualityComparer<int>();
    var enumerator = new IListEnumerator<int>(0, 0, []);
  }

  static void IListRefSegment ()
  {
    var segment = new IListRefSegment<int>([]);
    var comparer = new IListRefSegmentEqualityComparer<int>();    
  }
}