## Software9119.Collection.Superb.Indexing namespace

This namespace contains types to ease indexing.

### Types Available

- [`Step`](./Step.cs) – smart index type with custom stepping

##### > 1 Step Size Example
```csharp
Step stepOdd = new (2, 0);
Step stepEven = new (2, 1);

List<int> source = [ .. Enumerable.Range(1, 90) ];

int[] destination = new int[90];

int sumOdd = 0;
int sumEven = 0;
int count = source.Count;
for (; stepEven < count ;)
{
    sumOdd += source [ stepOdd++ ];
    sumEven += source [ stepEven++ ];
}

Func<int, bool> evenSelector = x => x % 2 == 0;
Assert.AreEqual ( source.Where( x => !evenSelector(x) ).Sum(), sumOdd );
Assert.AreEqual ( source.Where(evenSelector).Sum(), sumEven );
```

##### Negative Step

```csharp
const int size = 90;
List<int> numbers = [ .. Enumerable.Range(0, size) ];

Step backward = new (-1, size);
int sum = 0;
for (; backward > 0 ;)
    sum += numbers [ ++backward ];

Assert.AreEqual ( numbers.Sum (), sum );
```