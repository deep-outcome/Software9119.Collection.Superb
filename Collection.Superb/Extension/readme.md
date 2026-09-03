## Software9119.Collection.Superb.Extension Namespace

This namespace contains types with extension methods.

### Types Available

- [`ObjectExtension`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/ObjectExtension.cs) – banal _object_ extension methods, barely noteworthy.
- `IEnumerableExtension`
    - `As` prefixed stands for:
        - Trying source enumerable cast to target type first, `AsOrTo` methods only.
        - If target type is wrapper-like, method tries casting to necessary type first, e.g. `IEnumerable<Item>` -> `IList<Item>` -> `ReadOnlyCollection<Item>`, `AsOrTo` methods only.
        - 'Putting' source enumerable into target type, like `IDictionary<Key, Value>` into `ReadOnlyDictionary<Key, Value>`.
    - `Into` prefixed method implies source enumerable enumeration (copying, transforming) into target type.
    - `AsOrTo` methods do `As` and `Into` both as described above.
    - Methods expose `int?` capacity parameter that can be used for target type pre-capacitation, optional parameter.
    - All methods expose [`EnumerableNullBehavior`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/EnumerableNullBehavior.cs) for driving `null` source enumerable behavior, optional parameter.
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>read-only related</u></strong>
        - [`IList<Item>? AsOrToIList<Item>(IEnumerable<Item>?, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L45) – converts or casts `IEnumerable<T>`.
        - [`ReadOnlyCollection<Item>? AsOrToReadOnlyCollection<Item>(IEnumerable<Item>?, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L86) – converts or casts `IEnumerable<T>` and puts it into read-only collection.
        - [`ReadOnlyDictionary<Key, Item>? IntoReadOnlyDictionary<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L105) – converts `IEnumerable<T>` and puts it into read-only dictionary.
        - [`ReadOnlyDictionary<Key, Value>? IntoReadOnlyDictionary<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, EnumerableNullBehavior, int)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L126) – converts `IEnumerable<T>` and puts it into read-only dictionary.
        - [`ReadOnlyDictionary<Key, Value>? AsReadOnlyDictionary<Key, Value>(IDictionary<Key, Value>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.ReadOnly.cs#L168) – wraps source dictionary into read-only dictionary.
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>`AsOrTo` core method</u></strong>
        - [`Target? AsOrTo<Target> (IEnumerable?, AsOrToTargetType<Target>, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.cs#L34)
        - Open to user extension method declarations.
        - For instance you can declare your own extension method for creating string list out of `IEnumerable<int>`.
        ```csharp
        private readonly static AsOrToTargetType<string> _constructor = CreateConstructor();

        private static AsOrToTargetType<string> CreateConstructor ()
        {
          Ctor<int, string> builder = (e, c) =>
          {
            const int defaultCapacity = 1000;
            StringBuilder builder = new ( c ?? defaultCapacity);

            int order = 1;
            foreach(int i in e)
              builder.AppendLine(CultureInfo.InvariantCulture, $"{order++}: {i}");

            return builder.ToString();
           };

           return AsOrToTargetType.FromTypedCtor ( builder, canCast: e => false, empty: () => "" );
        }

        public static string ToNumberStringList ( this IEnumerable<int>? enumerable )
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
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>`AsOrTo` or `Into` for chosen [`System.Collections` Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.collections?view=net-10.0) types</u></strong>
        - [`ArrayList? AsOrToArrayList<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L17) – casts or copies `IEnumerable<T>` into array list.
        - [`ArrayList? AsOrToArrayList(IEnumerable?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L27) – casts or copies `IEnumerable` into array list.
        - [`Hashtable? IntoHashtable<Item>(IEnumerable<Item>?, int?, Func<Item, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L39) – creates hash table from `IEnumerable<T>`
        - [`Hashtable? IntoHashtable<Item>(IEnumerable<Item>?, int?, Func<Item, object>, Func<Item, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L68) – creates hash table from `IEnumerable<T>`
        - [`Hashtable? IntoHashtable(IEnumerable?, int?, Func<object, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L53) – creates hash table from `IEnumerable`
        - [`Hashtable? IntoHashtable(IEnumerable?, int?, Func<object, object>, Func<object, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L84) – creates hash table from `IEnumerable`
        - [`Queue? AsOrToQueue<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L100) – casts or copies `IEnumerable<T>` into queue.
        - [`Queue? AsOrToQueue(IEnumerable?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L109) – casts or copies `IEnumerable` into queue.
        - [`SortedList? IntoSortedList<Item>(IEnumerable<Item>?, int?, Func<Item, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L121) – creates sorted list from `IEnumerable<T>`
        - [`SortedList? IntoSortedList<Item>(IEnumerable<Item>?, int?, Func<Item, object>, Func<Item, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L150) – creates sorted list from `IEnumerable<T>`
        - [`SortedList? IntoSortedList(IEnumerable?, int?, Func<object, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L135) – creates sorted list from `IEnumerable`
        - [`SortedList? IntoSortedList(IEnumerable?, int?, Func<object, object>, Func<object, object>, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L166) – creates sorted list from `IEnumerable`
        - [`Stack? AsOrToStack<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L182) – casts or copies `IEnumerable<T>` into stack.
        - [`Stack? AsOrToStack(IEnumerable?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.cs#L192) – casts or copies `IEnumerable` into stack.
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>`AsOrTo` or `Into` for chosen [`System.Collections.Frozen` Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen?view=net-10.0) types</u></strong>
        - [`FrozenDictionary<Key, Item>? IntoFrozenDictionary<Item, Key> (IEnumerable<Item>?, Func<Item, Key>, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Frozen.cs#L27) – creates frozen dictionary from `IEnumerable<T>`
        - [`FrozenDictionary<Key, Value>? IntoFrozenDictionary<Item, Key, Value> (IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Frozen.cs#L54) – creates frozen dictionary from `IEnumerable<T>`
        - [`FrozenSet<Item>? AsOrToFrozenSet<Item>(IEnumerable<Item>?, IEqualityComparer<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Frozen.cs#L90) – creates frozen set from `IEnumerable<T>`
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>`AsOrTo` or `Into` for chosen [`System.Collections.Generic ` Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic?view=net-10.0) types</u></strong>
        - [`Dictionary<Key, Item>? IntoDictionary<Item, Key> (IEnumerable<Item>?, Func<Item, Key>, IEqualityComparer<Key>?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/ec020286c2079d2a6db621b2baa335e6d85c0e91/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L26) – creates dictionary from `IEnumerable<T>`
        - [`Dictionary<Key, Value>? IntoDictionary<Item, Key, Value> (IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, int?, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/ec020286c2079d2a6db621b2baa335e6d85c0e91/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L54) – creates dictionary from `IEnumerable<T>`
        - [`HashSet<Item>? AsOrToHashSet<Item>(IEnumerable<Item>?, int?, IEqualityComparer<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/ec020286c2079d2a6db621b2baa335e6d85c0e91/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L91) – casts or copies `IEnumerable<T>` into hash set
        - [`LinkedList<Item>? AsOrToLinkedList<Item>(IEnumerable<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/ec020286c2079d2a6db621b2baa335e6d85c0e91/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L112) – casts or copies `IEnumerable<T>` into linked list