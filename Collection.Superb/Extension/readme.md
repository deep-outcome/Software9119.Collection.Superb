## Software9119.Collection.Superb.Extension Namespace

This namespace contains types with extension methods.

### Types Available

- [`ObjectExtension`](./ObjectExtension.cs) – banal _object_ extension methods, barely noteworthy.
- `IEnumerableExtension`    
    - `As` prefixed stands for:
        - Trying source enumerable cast to target type first, `AsOrTo` methods only.
        - If target type is wrapper-like, method tries casting to necessary type first, e.g. `IEnumerable<T>` -> `IList<T>` -> `ReadOnlyCollection<T>`, `AsOrTo` methods only.
        - 'Putting' source enumerable into target type, like `IDictionary<Key, Value>` into `ReadOnlyDictionary<Key, Value>`.        
    - `To` prefixed method implies source enumerable enumeration (copying, transforming) into target type.
    - `AsOrTo` methods do `As` and `To` both as described above.
    - Methods expose `int?` capacity parameter that can be used for target type pre-capacitation, optional parameter.
    - All methods expose [`EnumerableNullBehavior`](./IEnumerableExtension/EnumerableNullBehavior.cs) for driving `null` source enumerable behavior, optional parameter.
    - read-only related
        - [`IList<T>? AsOrToIList<T>(IEnumerable<T>?, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L47) – converts or casts `IEnumerable<T>`.
        - [`ReadOnlyCollection<T>? AsOrToReadOnlyCollection<T>(IEnumerable<T>?, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L87) – converts or casts `IEnumerable<T>` and puts it into read-only collection.
        - [`ReadOnlyDictionary<Key, Source>? ToReadOnlyDictionary<Source, Key>(IEnumerable<Source>?, Func<Source, Key>, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L103) – converts `IEnumerable<T>` and puts it into read-only dictionary.
        - [`ReadOnlyDictionary<Key, Value>? ToReadOnlyDictionary<Source, Key, Value>(IEnumerable<Source>?, Func<Source, Key>, Func<Source, Value>, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L124) – converts `IEnumerable<T>` and puts it into read-only dictionary.
        - [`ReadOnlyDictionary<Key, Value>? AsReadOnlyDictionary<Key, Value>(IDictionary<Key, Value>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/3caed7a843109f3c7dc27831090d86697563c247/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L166) – wraps source dictionary into read-only dictionary.
    - `AsOrTo` core method
        - [`T? AsOrTo<T> (IEnumerable, AsOrToTargetType<T>, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.cs#L36)
        - Open to user extension method declarations.
        - For instance you can declare your own extension method for creating string list out of `IEnumerable<int>`.
        ```csharp              
        private readonly static AsOrToTargetType<string> _constructor = CreateConstructor();
        
        private static AsOrToTargetType<string> CreateConstructor ()
        {
          Func<IEnumerable<int>, int?, string> builder = (e, c) =>
          {
            const int defaultCapacity = 1000;
            StringBuilder builder = new ( c ?? defaultCapacity);
            
            int order = 1;
            foreach(int i in e)
              builder.AppendLine(CultureInfo.InvariantCulture, $"{order++}: {i}");
            
            return builder.ToString();
           };
        
           return AsOrToTargetType.FromTypedCtor ( builder, isCastable: false, empty: () => "" );
        }
        
        public static string ToNumberStringList ( this IEnumerable<int> enumerable )
        {
          AsOrToTargetType<string> constructor = _constructor;
          return enumerable.AsOrTo ( constructor )!;
        }
        
        int[] numbers = [24, 34, 5, 15434, 26546, 13, 4];
        string result = numbers.ToNumberStringList()!;
        
        /*  *result*
        1: 24
        2: 34
        3: 5
        4: 15434
        5: 26546
        6: 13
        7: 4
        */
        ```
    - `AsOrTo` or `To` for chosen [`System.Collections` Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.collections?view=net-10.0) types
        - [`ArrayList? AsOrToArrayList<T>(IEnumerable<T>, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L19) – casts or copies `IEnumerable<T>` into array list.
        - [`ArrayList? AsOrToArrayList(IEnumerable, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L29) – casts or copies `IEnumerable` into array list.
        - [`Hashtable? ToHashtable<T>(IEnumerable<T>, int?, Func<T, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L40) – creates hash table from `IEnumerable<T>`
        - [`Hashtable? ToHashtable<T>(IEnumerable<T>, int?, Func<T, object>, Func<T, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L67) – creates hash table from `IEnumerable<T>`
        - [`Hashtable? ToHashtable(IEnumerable, int?, Func<object, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L53) – creates hash table from `IEnumerable`
        - [`Hashtable? ToHashtable(IEnumerable, int?, Func<object, object>, Func<object, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L82) – creates hash table from `IEnumerable`
        - [`Queue? AsOrToQueue<T>(IEnumerable<T>, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L98) – casts or copies `IEnumerable<T>` into queue.
        - [`Queue? AsOrToQueue(IEnumerable, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L107) – casts or copies `IEnumerable` into queue.
        - [`SortedList? ToSortedList<T>(IEnumerable<T>, int?, Func<T, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L118) – creates sorted list from `IEnumerable<T>`
        - [`SortedList? ToSortedList<T>(IEnumerable<T>, int?, Func<T, object>, Func<T, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L145) – creates sorted list from `IEnumerable<T>`
        - [`SortedList? ToSortedList(IEnumerable, int?, Func<object, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L131) – creates sorted list from `IEnumerable`
        - [`SortedList? ToSortedList(IEnumerable, int?, Func<object, object>, Func<object, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L160) – creates sorted list from `IEnumerable`
        - [`Stack? AsOrToStack<T>(IEnumerable<T>, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L176) – casts or copies `IEnumerable<T>` into stack.
        - [`Stack? AsOrToStack(IEnumerable, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/2812b122e978945ba7d8c4876e8afa5deab64ec1/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collection.cs#L186) – casts or copies `IEnumerable` into stack.            
    - `AsOrTo` or `To` for chosen [`System.Collections.Frozen` Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen?view=net-10.0) types
        - `FrozenDictionary<Key, Source>? ToFrozenDictionary<Source, Key> (IEnumerable<Source>, Func<Source, Key>, IEqualityComparer<Key>?, EnumerableNullBehavior)` – creates frozen dictionary from `IEnumerable<T>`
        - `FrozenDictionary<Key, Value>? ToFrozenDictionary<Source, Key, Value> (IEnumerable<Source>, Func<Source, Key>, Func<Source, Value>, IEqualityComparer<Key>?, EnumerableNullBehavior)` – creates frozen dictionary from `IEnumerable<T>`