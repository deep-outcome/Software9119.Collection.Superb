using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.Ordering;
using Software9119.Collection.Superb.Segmentation;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Ordering;

[TestClass]
public class BinaryInsertionOrderTest
{

  [TestMethod]
  [DataRow ( "Array must be provided. (Parameter 'array')", "a" )]
  [DataRow ( "Comparer must be provided. (Parameter 'comparer')", "c" )]
  public void Order_NullParameter ( string errMsg, string whosNull )
  {
    int [] array              = whosNull == "a" ? null! : [];
    Comparison<int> comparer  = whosNull == "c" ? null! : Comparison;

    Action test = () => BinaryInsertionOrder.Order(array, comparer);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException>(test);
    Assert.AreEqual ( errMsg, e.Message );

    test = () => BinaryInsertionOrder.Order ( array, 0, 0, comparer );
    e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( errMsg, e.Message );
  }

  [TestMethod]
  [DataRow ( 3, 3, "With available length 5, given offset 3 and count 3 produce out-of indexing in range 5–5." )]
  [DataRow ( 3, 5, "With available length 5, given offset 3 and count 5 produce out-of indexing in range 5–7." )]
  [DataRow ( 0, 6, "With available length 5, given offset 0 and count 6 produce out-of indexing in range 5–5." )]
  [DataRow ( 0, 7, "With available length 5, given offset 0 and count 7 produce out-of indexing in range 5–6." )]
  public void Order_InvalidSegmenation ( int offset, int count, string errMsg )
  {
    int [] array = [1, 2, 3, 4, 5];
    Action test = () => BinaryInsertionOrder.Order(array, offset, count: count, Comparison);
    ImpossibleSegmentationException e = Assert.ThrowsExactly<ImpossibleSegmentationException>(test);
    Assert.AreEqual ( errMsg, e.Message );
  }

  const int evenSourceSize = 10_000;
  const int oddSourceSize = 7_777;

  readonly int [] sourceSizes = [evenSourceSize, oddSourceSize];

  readonly Comparer<int> comparer = Comparer<int>.Default;
  int Comparison ( int x, int y ) => comparer.Compare ( x, y );

  [TestMethod]
  public void Ordered ()
  {
    foreach ( int s in sourceSizes )
    {
      int [] source = Enumerable.Range ( 1, s ).AsOrToArray ( s )!;

      int [] array = [.. source];
      Span<int> span = [ .. source ];

      BinaryInsertionOrder.Order ( array, Comparison );
      BinaryInsertionOrder.Order ( span, Comparison );

      Assert.IsTrue ( source.SequenceEqual ( array ) );
      Assert.IsTrue ( source.SequenceEqual ( span ) );
    }
  }

  [TestMethod]
  public void Ordered_Reversed ()
  {
    foreach ( int s in sourceSizes )
    {
      int[] source = Enumerable.Range(1, s).Reverse().AsOrToArray(s)!;

      int [] array = [ .. source ];
      Span<int> span = [ .. source ];

      BinaryInsertionOrder.Order ( array, Comparison );
      BinaryInsertionOrder.Order ( span, Comparison );

      Array.Reverse ( source );
      Assert.IsTrue ( source.SequenceEqual ( array ) );
      Assert.IsTrue ( source.SequenceEqual ( span ) );
    }
  }

  [TestMethod]
  public void Ordered_Partially ()
  {
    Random random = new ();
    foreach ( int s in sourceSizes )
    {
      foreach ( int orderer in new int [] { 0, 1 } )
      {
        int order = 1;
        int [] source = Enumerable.Range ( 1, s )
        .Chunk(1000)
        .Select(x =>
        {
          Debug.Assert(order < ++order);
          return (order, chunk: x);
        })
        .SelectMany(x =>
        {
          if ((x.order & 1) == orderer)
            return x.chunk;

          return x.chunk.Select(x => random.Next(int.MinValue, int.MaxValue));
        })
        .AsOrToArray ( s )!;

        int [] array = [.. source];
        Span<int> span = [ .. source ];

        BinaryInsertionOrder.Order ( array, Comparison );
        BinaryInsertionOrder.Order ( span, Comparison );

        Array.Sort ( source );
        Assert.IsTrue ( source.SequenceEqual ( array ) );
        Assert.IsTrue ( source.SequenceEqual ( span ) );
      }
    }
  }

  [TestMethod]
  public void RandomOrder ()
  {
    Random random = new ();
    foreach ( int s in sourceSizes )
    {
      int[] source = Enumerable
      .Repeat(() => random.Next(int.MinValue, int.MaxValue), s)
      .Select(x => x())
      .AsOrToArray(s)!;

      int [] array = [ .. source ];
      Span<int> span = [ .. source ];

      BinaryInsertionOrder.Order ( array, Comparison );
      BinaryInsertionOrder.Order ( span, Comparison );

      Array.Sort ( source );
      Assert.IsTrue ( source.SequenceEqual ( array ) );
      Assert.IsTrue ( source.SequenceEqual ( span ) );
    }
  }

  [TestMethod]
  public void ZigZag ()
  {
    foreach ( int s in sourceSizes )
    {
      foreach ( int orderer in new [] { 0, 1 } )
      {
        int[] source = Enumerable
          .Range ( 1, s )
          .Select(x => (x & 1) == orderer ? 1 : 2)
          .AsOrToArray ( s )!;

        int [] array = [ .. source ];
        Span<int> span = [ .. source ];

        BinaryInsertionOrder.Order ( array, Comparison );
        BinaryInsertionOrder.Order ( span, Comparison );

        Array.Sort ( source );
        Assert.IsTrue ( source.SequenceEqual ( array ) );
        Assert.IsTrue ( source.SequenceEqual ( span ) );
      }
    }
  }

  [TestMethod]
  public void Offsets ()
  {
    Random random = new ();
    foreach ( int s in sourceSizes )
    {
      int[] source = Enumerable
      .Repeat(() => random.Next(int.MinValue, int.MaxValue), s)
      .Select(x => x())
      .AsOrToArray(s)!;

      int offset = 1000;
      int count = s - 1000;

      int [] array = [ .. source ];
      int [] spanArray = [ .. source ];
      Span<int> span = new (spanArray, offset, count);

      BinaryInsertionOrder.Order ( array, offset, count, Comparison );
      BinaryInsertionOrder.Order ( span, Comparison );

      Array.Sort ( source, offset, count );
      Assert.IsTrue ( source.SequenceEqual ( array ) );
      Assert.IsTrue ( source.SequenceEqual ( spanArray ) );
    }
  }

  [TestMethod]
  public void SafeEmpties ()
  {
    int [] array = [];
    Span<int> span = new (array, 0, 0);

    BinaryInsertionOrder.Order ( array, Comparison );
    BinaryInsertionOrder.Order ( array, 0, 0, Comparison );
    BinaryInsertionOrder.Order ( span, Comparison );
    BinaryInsertionOrder.Order ( default ( Span<int> ), Comparison );
  }

  [TestMethod]
  public void ShortSources ()
  {
    int [] [] sources = [ [1], [1, 2], [2, 1], [1, 1],];
    foreach ( int [] s in sources )
    {
      int [] array = [ .. s ];
      Span<int> span = [.. s];

      BinaryInsertionOrder.Order ( array, Comparison );
      BinaryInsertionOrder.Order ( span, Comparison );

      Array.Sort ( s );

      Assert.IsTrue ( s.SequenceEqual ( array ) );
      Assert.IsTrue ( s.SequenceEqual ( span ) );
    }
  }

  [TestMethod]
  public void ShortSources_Offseting ()
  {
    (int [], int, int) [] sources = [
      ([2, 1, 3], 1, 1),
      ([3, 1, 2, 1], 1, 2),
      ([4, 2, 1, 1], 1, 2),
      ([4, 1, 1, 4], 1, 2)
    ];
    foreach ( (int [] s, int offset, int count) in sources )
    {
      int [] array = [ .. s ];
      int [] spanArray = [ .. s ];
      Span<int> span = new (spanArray, offset, count);

      BinaryInsertionOrder.Order ( array, offset, count, Comparison );
      BinaryInsertionOrder.Order ( span, Comparison );

      Array.Sort ( s, index: offset, count );

      Assert.IsTrue ( s.SequenceEqual ( array ) );
      Assert.IsTrue ( s.SequenceEqual ( spanArray ) );
    }
  }
}
