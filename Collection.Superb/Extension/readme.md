## Software9119.Collection.Superb.Extension Namespace

This namespace contains types with extension methods.

### Types Available

- [`ObjectExtension`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/ObjectExtension.cs) – banal _object_ extension methods, barely noteworthy.
- `IEnumerableExtension`
    - `As` prefix of method expresses its casting and wrapping behavior, i.e:
        - Trying source enumerable cast to target type before actual enumeration is done.
        - If target type is wrapper-like, method tries casting to expected intermediate type before enumeration, e.g. `IEnumerable<Item>` -> `IList<Item>` -> `ReadOnlyCollection<Item>`.
        - 'Putting' source enumerable into target type, like `IDictionary<Key, Value>` into `ReadOnlyDictionary<Key, Value>`.
    - `Into` prefixed method implies source enumerable enumeration (copying, transforming) into target type.
    - `AsOrTo` methods do `As` and `Into` both as described above.
    - Methods expose `int?` capacity parameter that can be used for target type pre-capacitation, optional parameter.
    - All methods expose [`EnumerableNullBehavior`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/EnumerableNullBehavior.cs) for driving `null` source enumerable behavior, optional parameter.    
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>`AsOrTo` core method</u></strong>
        - [`Target? AsOrTo<Target>(IEnumerable?, AsOrToTargetType<Target>, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.cs#L34)
        - Open to user extension method declarations.
        - For instance you can declare your own extension method for creating string list out of `IEnumerable<int>`.
        ```csharp
        private readonly static AsOrToTargetType<string> _constructor = CreateConstructor();

        private static AsOrToTargetType<string> CreateConstructor ()
        {
          Ctor<int, string> builder = (e) =>
          {
            const int defaultCapacity = 1000;
            StringBuilder builder = new (defaultCapacity);

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
        ```csharp
        // sorted list example
        Func<int, object> keySelector = x => x * 10;
        Func<int, object> valueSelector = x => x * 20;
        IEnumerable<int> source = Enumerable.Range(0, 10);

        SortedList list = source.IntoSortedList(keySelector, valueSelector, capacity: 1000)!;
        ```
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
        ```csharp
        // frozen dictionary sample
        Func<int, int> keySelector = x => x * 10;
        Func<int, int> valueSelector = x => x * 20;
        IEnumerable<int> source = Enumerable.Range(0, 10);

        FrozenDictionary<int, int> dict = source.IntoFrozenDictionary(keySelector, valueSelector, behavior: EnumerableNullBehavior.ReturnDefault)!;
        ```
        - [`FrozenDictionary<Key, Item>? IntoFrozenDictionary<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Frozen.cs#L27) – creates frozen dictionary from `IEnumerable<T>`
        - [`FrozenDictionary<Key, Value>? IntoFrozenDictionary<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Frozen.cs#L54) – creates frozen dictionary from `IEnumerable<T>`
        - [`FrozenSet<Item>? AsOrToFrozenSet<Item>(IEnumerable<Item>?, IEqualityComparer<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Frozen.cs#L90) – creates frozen set from `IEnumerable<T>`
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>`AsOrTo` or `Into` for chosen [`System.Collections.Generic` Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic?view=net-10.0) types</u></strong>
        ```csharp
        // ordered dictionary sample
        MyKeyComparer<int> comparer = new ();
        Func<int, int> keySelector = x => x * 10;
        Func<int, int> valueSelector = x => x * 20;
        IEnumerable<int> source = Enumerable.Range(0, 10);

        OrderedDictionary<int, int> dict = source.IntoOrderedDictionary(keySelector, valueSelector, keyComparer: comparer)!;
        ```
        - [`Dictionary<Key, Item>? IntoDictionary<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, int?, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L27) – creates dictionary from `IEnumerable<T>`
        - [`Dictionary<Key, Value>? IntoDictionary<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, int?, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L55) – creates dictionary from `IEnumerable<T>`
        - [`HashSet<Item>? AsOrToHashSet<Item>(IEnumerable<Item>?, int?, IEqualityComparer<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L92) – casts or copies `IEnumerable<T>` into hash set
        - [`LinkedList<Item>? AsOrToLinkedList<Item>(IEnumerable<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L113) – casts or copies `IEnumerable<T>` into linked list
        - [`List<Item>? AsOrToList<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L131) – casts or copies `IEnumerable<T>` into list
        - [`OrderedDictionary<Key, Item>? IntoOrderedDictionary<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, int?, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L157) – creates ordered dictionary from `IEnumerable<T>`
        - [`OrderedDictionary<Key, Value>? IntoOrderedDictionary<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, int?, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L185) – creates ordered dictionary from `IEnumerable<T>`
        - [`PriorityQueue<Item, Priority>? IntoPriorityQueue<Item, Priority>(IEnumerable<(Item, Priority)>?, int?, IComparer<Priority>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L218) – creates priority queue from `IEnumerable<(TElement,TPriority)>`
        - [`Queue<Item>? AsOrToTypedQueue<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L238) – casts or copies `IEnumerable<T>` into queue
        - [`SortedDictionary<Key, Item>? IntoSortedDictionary<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, IComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L264) – creates sorted dictionary from `IEnumerable<T>`
        - [`SortedDictionary<Key, Value>? IntoSortedDictionary<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, IComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L291) – creates sorted dictionary from `IEnumerable<T>`
        - [`SortedList<Key, Item>? IntoTypedSortedList<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, int?, IComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L324) – creates sorted list from `IEnumerable<T>`
        - [`SortedList<Key, Value>? IntoTypedSortedList<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, int?, IComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L352) – creates sorted list from `IEnumerable<T>`
        - [`SortedSet<Item>? AsOrToSortedSet<Item>(IEnumerable<Item>?, IComparer<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L390) – casts or copies `IEnumerable<T>` into sorted set
        - [`Stack<Item>? AsOrToTypedStack<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L411) – casts or copies `IEnumerable<T>` into stack
        - `IList<Item>? AsOrToIList<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)` – casts or copies `IEnumerable<T>` into `IList<T>`
    - <a href="https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Generic.cs#L429"><strong style="background-color:rgba(186 246 226 / 0.63)"><u><code>Item []? AsOrToArray<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)</code></u></strong></a> – casts or copies `IEnumerable<T>` into array
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>`AsOrTo` or `Into` for chosen [`System.Collections.Immutable` Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable?view=net-10.0) types</u></strong>
        ```csharp
        // immutable array sample
        const int count = 10;
        IEnumerable<int> source = Enumerable.Range(0, count);
        ImmutableArray<int>? array = source.AsOrToImmutableArray(length: count, enforceLengthCountMatch: true);
        ```
        - [`ImmutableArray<Item> AsOrToImmutableArray<Item>(IEnumerable<Item>?, int?, bool, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L28) – casts or copies `IEnumerable<T>` into immutable array
        - [`ImmutableDictionary<Key, Item>? IntoImmutableDictionary<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, IEqualityComparer<Key>?, IEqualityComparer<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L58) – creates immutable dictionary from `IEnumerable<T>`
        - [`ImmutableDictionary<Key, Value>? IntoImmutableDictionary<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, IEqualityComparer<Key>?, IEqualityComparer<Value>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L96) – creates immutable dictionary from `IEnumerable<T>`
        - [`ImmutableHashSet<Item>? AsOrToImmutableHashSet<Item>(IEnumerable<Item>?, IEqualityComparer<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L136) – casts or copies `IEnumerable<T>` into immutable hash set
        - [`ImmutableList<Item>? AsOrToImmutableList<Item>(IEnumerable<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L156) – casts or copies `IEnumerable<T>` into immutable list
        - [`ImmutableQueue<Item>? AsOrToImmutableQueue<Item>(IEnumerable<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L173) – casts or copies `IEnumerable<T>` into immutable queue
        - [`ImmutableSortedDictionary<Key, Item>? IntoImmutableSortedDictionary<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, IComparer<Key>?, IEqualityComparer<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L202) – creates immutable sorted dictionary from `IEnumerable<T>`
        - [`ImmutableSortedDictionary<Key, Value>? IntoImmutableSortedDictionary<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, IComparer<Key>?, IEqualityComparer<Value>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L240) – creates immutable sorted dictionary from `IEnumerable<T>`
        - [`ImmutableSortedSet<Item>? AsOrToImmutableSortedSet<Item>(IEnumerable<Item>?, IComparer<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L280) – casts or copies `IEnumerable<T>` into immutable sorted set
        - [`ImmutableStack<Item>? AsOrToImmutableStack<Item>(IEnumerable<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Immutable.cs#L300) – casts or copies `IEnumerable<T>` into immutable stack
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>`AsOrTo` or `Into` for chosen [`System.Collections.Concurrent` Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent?view=net-10.0) types</u></strong>
        ```csharp
        // orderable partitioner sample
        IEnumerable<long> source = Enumerable.Range(0, 1000_000).Select(MyComplexComputation);
        OrderablePartitioner<long> partitioner = source.AsOrderablePartitioner()!;

        IEnumerable<IEnumerator<long>> partions = partitioner.GetPartitions(100);
        ```
        - `BlockingCollection<Item>? AsBlockingCollection<Item>(IProducerConsumerCollection<Item>, int?, EnumerableNullBehavior)` – wraps `IProducerConsumerCollection<T>` into blocking collection
        - [`ConcurrentBag<Item>? AsOrToConcurrentBag<Item>(IEnumerable<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Concurrent.cs#L19) – casts or copies `IEnumerable<T>` into concurrent bag
        - [`ConcurrentDictionary<Key, Item>? IntoConcurrentDictionary<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, int?, int?, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Concurrent.cs#L44) – creates concurrent dictionary from `IEnumerable<T>`
        - [`ConcurrentDictionary<Key, Value>? IntoConcurrentDictionary<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, int?, int?, IEqualityComparer<Key>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Concurrent.cs#L79) – creates concurrent dictionary from `IEnumerable<T>`
        - [`ConcurrentQueue<Item>? AsOrToConcurrentQueue<Item>(IEnumerable<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Concurrent.cs#L109) – casts or copies `IEnumerable<T>` into concurrent queue
        - [`ConcurrentStack<Item>? AsOrToConcurrentStack<Item>(IEnumerable<Item>?, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.AsOrTo.System.Collections.Concurrent.cs#L126) – casts or copies `IEnumerable<T>` into concurrent stack
        - [`OrderablePartitioner<Item>? AsOrderablePartitioner<Item>(IEnumerable<Item>?, bool, EnumerablePartitionerOptions, EnumerableNullBehavior)`](https://github.com/deep-outcome/Software9119.Collection.Superb/blob/HEAD/Collection.Superb/Extension/IEnumerableExtension/IEnumerableExtension.As.System.Collections.Generic.cs#L28) – wraps `IEnumerable<T>` into orderable partitioner
    - <strong style="background-color:rgba(186 246 226 / 0.63)"><u>`AsOrTo` or `Into` for chosen [`System.Collections.ObjectModel` Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.collections.objectmodel?view=net-10.0) types</u></strong>
        - `Collection<Item>? AsOrToCollection<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)` – casts `IEnumerable<T>` into collection, or casts or copies it into intemediate `IList<T>` before wrapping to collection
        - `ObservableCollection<Item>? AsOrToObservableCollection<Item>(IEnumerable<Item>?, EnumerableNullBehavior)` – casts or copies `IEnumerable<T>` into observable collection
        - `ReadOnlyCollection<Item>? AsOrToReadOnlyCollection<Item>(IEnumerable<Item>?, int?, EnumerableNullBehavior)` – casts `IEnumerable<T>` into read-only collection, or casts or copies it into intemediate `IList<T>` before wrapping to read-only collection
        - `ReadOnlyDictionary<Key, Value>? AsReadOnlyDictionary<Key, Value>(IDictionary<Key, Value>?, EnumerableNullBehavior)` – wraps `IDictionary<Key, Value>` into read-only dictionary
        - `ReadOnlyDictionary<Key, Item>? IntoReadOnlyDictionary<Item, Key>(IEnumerable<Item>?, Func<Item, Key>, int?, IEqualityComparer<Key>?, EnumerableNullBehavior)` – creates read-only dictionary from `IEnumerable<T>`
        - `ReadOnlyDictionary<Key, Value>? IntoReadOnlyDictionary<Item, Key, Value>(IEnumerable<Item>?, Func<Item, Key>, Func<Item, Value>, int?, IEqualityComparer<Key>?, EnumerableNullBehavior)` – creates read-only dictionary from `IEnumerable<T>`