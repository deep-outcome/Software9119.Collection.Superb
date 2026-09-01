using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

public partial class IEnumerableExtensionTest
{
  static public AsOrToTargetType<List<int>> TargetClass ( bool tryCast )
  {
    Func<IEnumerable<int>, int?, List<int>> ctor = ( e, c ) =>
    {
      List<int> output = c is int capacity ? new (10 * capacity) : new();
      output.AddRange(e);
      return output;
    };
    AsOrToTargetType<List<int>> targetType = AsOrToTargetType.FromTypedCtor(ctor, tryCast, () => []);
    return targetType;
  }

  static public AsOrToTargetType<ArraySegment<int>> TargetStruct ()
  {
    Func<IEnumerable<int>, int?, ArraySegment<int>> ctor = (e, c) => new ([ .. e ]);
    AsOrToTargetType<ArraySegment<int>> targetType = AsOrToTargetType.FromTypedCtor(ctor, default, () => default);
    return targetType;
  }

  // tests

  [TestMethod]
  public void AsOrTo_NullAsOrToType ()
  {
    AsOrToTargetType<List<int>> targetType = TargetClass(default);

    Action test = () => _ = (new int[0]).AsOrTo<int> ( null! );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Target type is requisite. (Parameter 'asOrToType')", e.Message );
  }

  [TestMethod]
  public void AsOrTo_NullEnumerable_ReturnDefault_Class ()
  {
    AsOrToTargetType<List<int>> targetType = TargetClass(default);

    List<int>? test = ((int []?) null)!.AsOrTo ( targetType, behavior: EnumerableNullBehavior.ReturnDefault );
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void AsOrTo_NullEnumerable_ReturnDefault_Struct ()
  {
    AsOrToTargetType<ArraySegment<int>> targetType = TargetStruct();

    ArraySegment<int> test = ((int []?) null)!.AsOrTo ( targetType, behavior: EnumerableNullBehavior.ReturnDefault );
    Assert.AreEqual ( default ( ArraySegment<int> ), test );
  }

  [TestMethod]
  public void AsOrTo_NullEnumerable_ReturnEmpty ()
  {
    AsOrToTargetType<List<int>> targetType = TargetClass(default);

    List<int> test = ((int []?) null)!.AsOrTo ( targetType, behavior: EnumerableNullBehavior.ReturnEmpty )!;
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void AsOrTo_NullEnumerable_ThrowException ()
  {
    AsOrToTargetType<List<int>> targetType = TargetClass(default);

    Action test = () => _ = ((int []?) null)!.AsOrTo ( targetType, behavior: EnumerableNullBehavior.ThrowException );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Null source enumerable encounter. (Parameter 'enumerable')", e.Message );
  }

  [TestMethod]
  public void AsOrTo_NullEnumerable_UnsupportedNullBehavior ()
  {
    AsOrToTargetType<List<int>> targetType = TargetClass(default);

    Action test = () => _ = ((int []?) null)!.AsOrTo ( targetType, behavior: (EnumerableNullBehavior)999 )!;
    UnsupportedNullBehaviorException e = Assert.ThrowsExactly<UnsupportedNullBehaviorException> ( test );
    Assert.AreEqual ( "Unsupported behavior, '999'. (Parameter 'behavior')", e.Message );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void AsOrTo_CastingVsEnumeration ( bool tryCast )
  {
    AsOrToTargetType<List<int>> targetType = TargetClass(tryCast);

    List<int> source = [0,1,2,3];
    List<int> test = source.AsOrTo ( targetType )!;
    Assert.AreEqual ( tryCast, ReferenceEquals ( source, test ) );
  }

  [TestMethod]
  [DataRow ( 4, 40 )]
  [DataRow ( null, 4 )]
  public void AsOrTo_Capacity ( int capacity, int listCapacity )
  {
    AsOrToTargetType<List<int>> targetType = TargetClass(default);

    int [] source = [0,1,2,3 ];
    List<int> test = source.AsOrTo ( targetType, capacity: capacity )!;
    Assert.IsTrue ( source.SequenceEqual ( test ) );
    Assert.AreEqual ( listCapacity, test.Capacity );
  }

  [TestMethod]
  public void AsOrTo_Enumeration ()
  {
    AsOrToTargetType<List<int>> targetType = TargetClass(default);

    IEnumerable<int> source = Enumerable.Range(1, 10).Select(x => x * 2);
    List<int> test = source.AsOrTo ( targetType  )!;
    Assert.IsTrue ( source.SequenceEqual ( test ) );
  }


  // readme sample

  [SuppressMessage ( "Style", "IDE0036:Order modifiers", Justification = "Readme style" )]
  [SuppressMessage ( "Style", "IDE0040:Remove accessibility modifiers", Justification = "Readme style." )]
  private readonly static AsOrToTargetType<string> _constructor = CreateConstructor();

  [SuppressMessage ( "Style", "IDE0036:Order modifiers", Justification = "Readme style" )]
  [SuppressMessage ( "Style", "IDE0058:Expression value is never used", Justification = "Readme style." )]
  [SuppressMessage ( "Style", "IDE0040:Remove accessibility modifiers", Justification = "Readme style." )]
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

  [SuppressMessage ( "Style", "IDE0036:Order modifiers", Justification = "Readme style" )]
  public static string ToNumberStringList ( IEnumerable<int> enumerable )
  {
    AsOrToTargetType<string> constructor = _constructor;
    return enumerable.AsOrTo ( constructor )!;
  }

  [TestMethod]
  public void ReadMeSample ()
  {
    int[] numbers = [24, 34, 5, 15434, 26546, 13, 4];
    string result = ToNumberStringList(numbers)!;

    const string expectation =
@"1: 24
2: 34
3: 5
4: 15434
5: 26546
6: 13
7: 4
";

    Assert.AreEqual ( expectation.TrimStart (), result );
  }
}
