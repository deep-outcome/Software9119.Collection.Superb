## Software9119.Collection.Superb.Extension Namespace

This namespace contains types with extension methods.

### Types Available

- [`ObjectExtension`](./ObjectExtension.cs) – banal _object_ extension methods, barely noteworthy.
- `IEnumerableExtension`
    - [IEnumerableOfTExtension.ReadOnly.cs](IEnumerableOfTExtension/IEnumerableOfTExtension.ReadOnly.cs)
        - `IList<T>? ToOrAsIList<T>(IEnumerable<T>?, EnumerableNullBehavior, int)` – converts or casts `IEnumerable<T>`.
        - `ReadOnlyCollection<T>? ToOrAsReadOnlyCollection<T> (IEnumerable<T>? enumerable, EnumerableNullBehavior, int)` – converts or casts `IEnumerable<T>` and puts it into read-only collection.
        - `ReadOnlyDictionary<Key, Source>? ToReadOnlyDictionary<Source, Key>(IEnumerable<Source>?, Func<Source, Key>, EnumerableNullBehavior, int)` – converts `IEnumerable<T>` and puts it into read-only dictionary.
        - `ReadOnlyDictionary<Key, Value>? ToReadOnlyDictionary<Source, Key, Value>(IEnumerable<Source>?, Func<Source, Key>, Func<Source, Value>, EnumerableNullBehavior, int)` – converts `IEnumerable<T>` and puts it into read-only dictionary.
        - `ReadOnlyDictionary<Key, Value>? AsReadOnlyDictionary<Key, Value>(IDictionary<Key, Value>? dict, EnumerableNullBehavior)` – wraps source dictionary into read-only dictionary.
        - Methods expose `int` capacity parameter that can be used for target type pre-capacitation, optional parameter.
        - All methods expose [`EnumerableNullBehavior`](./IEnumerableOfTExtension/EnumerableNullBehavior.cs) for driving `null` source enumerable behavior, optional parameter.