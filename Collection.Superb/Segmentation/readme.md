## Software9119.Collection.Superb.Segmentation namespace

This namespace contains types to ease working with segments.

### `IList<T>` segmentation

- [`IListEnumerator<T>`](./IListOfTEnumerator.cs) – allows for segmented enumeration of `IList<T>`
- [`IListSegment<T>`](./IListSegmentOfT.cs) – segment over `IList<T>`
- [`IListSegmentEqualityComparer<T>`](./IListSegmentOfTEqualityComparer.cs) – `IListSegment<T>` equality comparer
- [`IListRefSegment<T>`](./IListRefSegmentOfT.cs) – segment over `IList<T>`, `ref struct` type
- [`IListRefSegmentEqualityComparer<T>`](./IListRefSegmentOfTEqualityComparer.cs) – `IListRefSegment<T>` equality comparer
- [`IReadOnlyListEnumerator<T>`](./IReadOnlyListOfTEnumerator.cs) – allows for segmented enumeration of `IReadOnlyList<T>`
- [`IReadOnlyListSegment<T>`](./IReadOnlyListSegmentOfT.cs) – segment over `IReadOnlyList<T>`
- [`IReadOnlyListSegmentEqualityComparer<T>`](./IReadOnlyListSegmentOfTEqualityComparer.cs) – `IReadOnlyListSegment<T>` equality comparer
- [`IListEnumerator`](./IListEnumerator.cs) – allows for segmented enumeration of `IList`
- [`IListSegment`](./IListSegment.cs) – segment over `IList`
- [`IListSegmentEqualityComparer`](./IListSegmentEqualityComparer.cs) – `IListSegment` equality comparer

Everything comes with a price and for this reason is up to client code to ensure `IList<T>`/`IReadOnlyList<T>` passed
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