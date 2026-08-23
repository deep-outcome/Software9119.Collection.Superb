## Software9119.Collection.Superb.Segmentation namespace

This namespace contains types to ease working with segments.

### `IList<T>` segmentation

- [`IListEnumerator`](./IListEnumerator.cs) – allows for segmented enumeration of `IList`
- [`IListEnumerator<T>`](./IListOfTEnumerator.cs) – allows for segmented enumeration of `IList<T>`
- [`IListRefEnumerator<T>`](./IListRefEnumerator.cs) – allows for segmented enumeration of `IList` implementing `ref struct`
- [`IListRefEnumerator<T, U>`](./IListOfTRefEnumerator.cs) – allows for segmented enumeration of `IList<U>` implementing `ref struct`
- [`IReadOnlyListEnumerator<T>`](./IReadOnlyListOfTEnumerator.cs) – allows for segmented enumeration of `IReadOnlyList<T>`
- [`IListSegment`](./IListSegment.cs) – segment over `IList`
- [`IListSegment<T>`](./IListSegmentOfT.cs) – segment over `IList<T>`
- [`IListRefSegment<T>`](./IListRefSegment.cs) – segment over `IList` implementing `ref struct`
- [`IListRefSegment<T,U>`](./IListRefSegmentOfT.cs) – segment over `IList<U>` implementing `ref struct`
- [`IReadOnlyListSegment<T>`](./IReadOnlyListSegmentOfT.cs) – segment over `IReadOnlyList<T>`
- [`IListSegmentEqualityComparer`](./IListSegmentEqualityComparer.cs) – `IListSegment` equality comparer
- [`IListSegmentEqualityComparer<T>`](./IListSegmentOfTEqualityComparer.cs) – `IListSegment<T>` equality comparer
- [`IReadOnlyListSegmentEqualityComparer<T>`](./IReadOnlyListSegmentOfTEqualityComparer.cs) – `IReadOnlyListSegment<T>` equality comparer

Everything comes with a price and for this reason is up to client code to ensure _'list'_ passed
into `IListSegment<T>` or `IListEnumerator<T>` and others is not mutated in the mean time in a harmful way, most notably that 
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