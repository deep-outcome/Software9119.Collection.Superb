using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.Segmentation;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using TestProject;

[SuppressMessage ( "Style", "IDE0008:Use explicit type", Justification = "Unimportant." )]
[SuppressMessage ( "Style", "IDE0059:Unnecessary assignment of a value", Justification = "Unimportant." )]

sealed class Program
{
  static void Main ()
  {
    IListSegment ();
    IListRefSegment ();

    IListOfTSegment ();
    IListOfTRefSegment ();

    IReadOnlyOfTListSegment ();
    IReadOnlyListOfTRefSegment ();

    AsOrTo ();
  }

  static void IListSegment ()
  {
    var segment = new IListSegment(new int [0]);
    var comparer = new IListSegmentEqualityComparer();
    var enumerator = new IListEnumerator(0, 0, new int [0]);
  }

  static void IListRefSegment ()
  {
    var segment = new IListRefSegment<StructRefList>(new StructRefList(new int [0]));
    using var enumerator = new IListRefEnumerator<ArraySegment<int>, int>(0, 0, []);
  }

  static void IListOfTSegment ()
  {
    var segment = new IListSegment<int>([]);
    var comparer = new IListSegmentEqualityComparer<int>();
    using var enumerator = new IListEnumerator<int>(0, 0, []);
  }

  static void IListOfTRefSegment ()
  {
    var segment = new IListRefSegment<ArraySegment<int>, int>([]);
    using var enumerator = new IListRefEnumerator<ArraySegment<int>, int>(0, 0, []);
  }

  static void IReadOnlyOfTListSegment ()
  {
    var segment = new IReadOnlyListSegment<int>([]);
    var comparer = new IReadOnlyListSegmentEqualityComparer<int>();
    using var enumerator = new IReadOnlyListEnumerator<int>(0, 0, []);
  }

  static void IReadOnlyListOfTRefSegment ()
  {
    var segment = new IReadOnlyListRefSegment<ArraySegment<int>, int>([]);
    using var enumerator = new IReadOnlyListRefEnumerator<ArraySegment<int>, int>(0, 0, []);
  }

  static void AsOrTo ()
  {
    IEnumerable<int> sourceOfT = Enumerable.Range(1, 10);
    IEnumerable source = Enumerable.Range(1, 10);

    // source.AsOrTo()

    // UnsupportedNullBehaviorException
    // Software9119.Collection.Superb.Extension.system_collections.Queue
    //Software9119.Collection.Superb.Extension.AsOrToTargetType
  }
}