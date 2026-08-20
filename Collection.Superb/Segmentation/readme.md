## Software9119.Collection.Superb.Segmentation namespace

This namespace contains types to ease working with segments.

### `IList<T>` segmentation

- [`IListEnumerator<T>`](./IListEnumerator.cs) – allows for segmented enumeration
- [`IListSegment<T>`](./IListSegment.cs) – segment over `IList<T>`
- [`IListSegmentEqualityComparer<T>`](./IListSegmentEqualityComparer.cs) – `IListSegment<T>` equality comparer

Everything comes with a price and for this reason is up to client code to ensure `IList<T>` passed
into `IListSegment<T>` or `IListEnumerator<T>` is not mutated in a harmful way, most notably that 
it is not shrunk beyond segment defined.

```csharp
[TestMethod]  
public void ShrunkedReallyBadSample ()
{
  List<int> list = [1,2 ,3, 4, 5, 6];
  IListSegment<int> segment = new (list, offset: 3, count: 3);
  Assert.AreEqual ( 15, segment.Sum () ); // 4 +5 +6
  list.RemoveAt ( 5 ); // 6 gone
  var e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( () => segment [ 2 ] ); // List<int> angry
  Assert.Contains ( "System.Collections.Generic.List`1.get_Item(Int32 index)", e.StackTrace! ); // yep, it's List<int>
}
```