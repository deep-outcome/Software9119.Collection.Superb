## Software9119.Collection.Superb.Extension Namespace

This namespace contains types with extension methods.

### Types Available

- [`ObjectExtension`](./ObjectExtension.cs) – banal _object_ extension methods, barely noteworthy.
- `IEnumerableExtension`
    - [IEnumerableOfTExtension.ReadOnly.cs](IEnumerableOfTExtension/IEnumerableOfTExtension.ReadOnly.cs)
        - [`IList<T>? AsOrToIList<T>(IEnumerable<T>?, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableOfTExtension/IEnumerableOfTExtension.ReadOnly.cs#L47) – converts or casts `IEnumerable<T>`.
        - [`ReadOnlyCollection<T>? AsOrToReadOnlyCollection<T> (IEnumerable<T>? enumerable, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableOfTExtension/IEnumerableOfTExtension.ReadOnly.cs#L87) – converts or casts `IEnumerable<T>` and puts it into read-only collection.
        - [`ReadOnlyDictionary<Key, Source>? ToReadOnlyDictionary<Source, Key>(IEnumerable<Source>?, Func<Source, Key>, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableOfTExtension/IEnumerableOfTExtension.ReadOnly.cs#L103) – converts `IEnumerable<T>` and puts it into read-only dictionary.
        - [`ReadOnlyDictionary<Key, Value>? ToReadOnlyDictionary<Source, Key, Value>(IEnumerable<Source>?, Func<Source, Key>, Func<Source, Value>, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableOfTExtension/IEnumerableOfTExtension.ReadOnly.cs#L124) – converts `IEnumerable<T>` and puts it into read-only dictionary.
        - [`ReadOnlyDictionary<Key, Value>? AsReadOnlyDictionary<Key, Value>(IDictionary<Key, Value>? dict, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableOfTExtension/IEnumerableOfTExtension.ReadOnly.cs#L166) – wraps source dictionary into read-only dictionary.
        - Methods expose `int` capacity parameter that can be used for target type pre-capacitation, optional parameter.
        - All methods expose [`EnumerableNullBehavior`](./IEnumerableOfTExtension/EnumerableNullBehavior.cs) for driving `null` source enumerable behavior, optional parameter.