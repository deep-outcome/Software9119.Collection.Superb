## Software9119.Collection.Superb.Storing Namespace

This namespace contains collection data types and types for supporting them.

### Types Available

- [`GrowFactor`](./GrowFactor.cs) – grow factor enumeration, used by auto-grow capacitation calculation
- [`Capacitor<T>`](./Capacitor.cs) – `List<T>`-like collection type featuring all capabilities of .NET old-timers `List<T>` and `T[]` and more but w/o list versioning
- [`CapacitorStoreEnumerator<T>`](./CapacitorStoreEnumerator.cs) – `Capacitor<T>` enumerator
```csharp
// versioning does not exist thus version match is not enforced during enumeration

Capacitor<int> capacitor = [ 1, 2, 3, 4, 5 ];
foreach (int item in capacitor)
{
    capacitor.Add ( item );
    capacitor.Reverse ();
    capacitor.Add ( item );
}
// 3,2,1,5,1, 5,4,3,2,1, 1,5,1,2,3, fancy number generation: mission succesful
```
```csharp
// to advert unwanted changes to auto-grow factorization, grow factor can be locked
capacitor = new() { GrowFactor = GrowFactor.OneAndHalf, LockGrowFactor = true, };

// but beware of properties order in object initializer
capacitor = new() { LockGrowFactor = true, GrowFactor = GrowFactor.Five, };
// throws InvalidOperationException
```
```csharp
// take advantage of your knowledge and capacitate beforehand whenever you like to
Capacitor<int> capacitor = new ();

await capacitor.Add ( Generator ( CancellationToken.None ), roomRequest: 100 );
_ = Enumerable.Range ( 1, 100 ).SequenceEqual ( capacitor ); // true
_ = capacitor.Capacity == 100; // true

static async IAsyncEnumerable<int> Generator ( [EnumeratorCancellation] CancellationToken token )
{
    TimeSpan period = TimeSpan.FromMilliseconds(25);
    using PeriodicTimer timer = new (period);
    int loopsCount = 100;

    while (loopsCount-- > 0 && await timer.WaitForNextTickAsync ( token ))
        yield return 99 - loopsCount + 1;
}
```