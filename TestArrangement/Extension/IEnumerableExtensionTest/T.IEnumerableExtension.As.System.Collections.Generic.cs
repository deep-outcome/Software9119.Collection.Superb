using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void AsOrderablePartitioner_IList ( bool loadBalance )
  {
    List<int> source = [ .. Enumerable.Range(0, 10) ];
    OrderablePartitioner<int> test = source.AsOrderablePartitioner(dynamicLoadBalancing: loadBalance)!;

    string typeName = test.GetType().Name;
    Assert.StartsWith ( loadBalance ? "DynamicPartitionerForIList" : "StaticIndexRangePartitionerForIList", typeName );

    IEnumerable<EnumerableEnumerator<int>> partions = test.GetPartitions(1)
      .Select(x => new EnumerableEnumerator<int>(x));
    Assert.IsTrue ( partions.SelectMany ( x => x ).SequenceEqual ( source ) );
  }

  [TestMethod]
  [DataRow ( EnumerablePartitionerOptions.None )]
  [DataRow ( EnumerablePartitionerOptions.NoBuffering )]
  public void AsOrderablePartitioner_Enumerable ( EnumerablePartitionerOptions opts )
  {
    IEnumerable<int> source = XEnumerable.RangeEnumerable(0, 10);
    OrderablePartitioner<int> test = source.AsOrderablePartitioner(options: opts)!;

    bool _useSingleChunking = (bool)Reflection.GetNonPublicFieldValue(test, "_useSingleChunking");
    Assert.AreEqual ( opts == EnumerablePartitionerOptions.NoBuffering, _useSingleChunking );

    IEnumerable<EnumerableEnumerator<int>> partions = test.GetPartitions(1)
      .Select(x => new EnumerableEnumerator<int>(x));
    Assert.IsTrue ( partions.SelectMany ( x => x ).SequenceEqual ( source ) );
  }

  [TestMethod]
  [DataRow ( NullBehavior.ReturnDefault )]
  [DataRow ( null )]
  public void AsOrderablePartitioner_NullBehavior ( NullBehavior? behavior )
  {
    IEnumerable<int> source = null!;
    bool returnsDefault = behavior is NullBehavior.ReturnDefault;
    OrderablePartitioner<int>? test = returnsDefault
      ? source.AsOrderablePartitioner(behavior: behavior!.Value)
      : source.AsOrderablePartitioner();

    Assert.AreEqual ( test?.GetPartitions ( 1 ).Single ().MoveNext () ?? true, returnsDefault );
  }

  // readme

  long MyComplexComputation ( int x ) => x;

  [TestMethod]
  public void Concurrent_Sample ()
  {
    // orderable partitioner sample
    IEnumerable<long> source = Enumerable.Range(0, 1000_000).Select(MyComplexComputation);
    OrderablePartitioner<long> partitioner = source.AsOrderablePartitioner()!;

    IEnumerable<IEnumerator<long>> partions = partitioner.GetPartitions(100);

    IEnumerable<EnumerableEnumerator<long>> enumerables = partions.Select(x => new EnumerableEnumerator<long>(x));
    Assert.AreEqual ( source.Sum (), enumerables.SelectMany ( x => x ).Sum () );
  }
}
