using Software9119.Collection.Superb.Segmentation;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage ( "Style", "IDE0008:Use explicit type", Justification = "Unimportant." )]
[SuppressMessage ( "Style", "IDE0059:Unnecessary assignment of a value", Justification = "Unimportant." )]

sealed class Program
{

  static void Main ()
  {
    IListSegmentOfT ();
    IListRefSegmentOfT ();
    IReadOnlyListSegmentOfT ();
  }

  static void IListSegmentOfT ()
  {
    var segment = new IListSegment<int>([]);
    var comparer = new IListSegmentEqualityComparer<int>();
    var enumerator = new IListEnumerator<int>(0, 0, []);
  }

  static void IListRefSegmentOfT ()
  {
    var segment = new IListRefSegment<int>([]);
    var comparer = new IListRefSegmentEqualityComparer<int>();
  }

  static void IReadOnlyListSegmentOfT ()
  {
    var segment = new IReadOnlyListSegment<int>([]);
    var comparer = new IReadOnlyListSegmentEqualityComparer<int>();
    var enumerator = new IReadOnlyListEnumerator<int>(0, 0, []);
  }
}