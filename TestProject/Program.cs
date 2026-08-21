using Software9119.Collection.Superb.Segmentation;

class Program
{
  static void Main ( string [] args )
  {
    var segment = new IListSegment<int>([]);
    var comparer = new IListSegmentEqualityComparer<int>();
    var enumerator = new IListEnumerator<int>(0, 0, []);
  }
}