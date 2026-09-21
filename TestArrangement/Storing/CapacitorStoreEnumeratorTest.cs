using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Storing;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Storing;

[TestClass]
public class CapacitorStoreEnumeratorTest
{
  const int resetIndex = CapacitorStoreEnumerator<int>.resetIndex;

  [TestMethod]
  public void Constructor ()
  {
    Capacitor<int> capacitor = [1,2,3];
    CapacitorStoreEnumerator<int> enumerator = new (capacitor);

    Assert.AreEqual ( resetIndex, enumerator.index );
    Assert.AreEqual ( -1, enumerator.index );
    Assert.AreEqual ( capacitor.Count, enumerator.count );
    Assert.IsTrue ( ReferenceEquals ( capacitor.store, enumerator.store ) );
  }

  [TestMethod]
  public void Constructor_NullCapacitor ()
  {
    Capacitor<int> capacitor = null!;
    Action test = () => _ = new CapacitorStoreEnumerator<int> ( capacitor );

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Capacitor must be provided. (Parameter 'capacitor')", e.Message );
  }

  [TestMethod]
  public void Default_IsHalfSafeForUse ()
  {
    CapacitorStoreEnumerator<object> enumerator = default;

    Assert.AreEqual ( 0, enumerator.index );
    Assert.IsFalse ( enumerator.MoveNext () );
    Assert.AreEqual ( 0, enumerator.index );

    Action test = () => _ = enumerator.Current;
    NullReferenceException e = Assert.ThrowsExactly<NullReferenceException> ( test );
    Assert.AreEqual ( "Object reference not set to an instance of an object.", e.Message );
  }

  [TestMethod]
  public void Enumeration ()
  {
    int[] source = [1, 2, 3, 4, 5];
    Capacitor<int> capacitor = new (source);
    CapacitorStoreEnumerator<int> enumerator = new (capacitor);
    EnumerableEnumerator<int> enumerable = new (enumerator);

    Assert.IsTrue ( source.SequenceEqual ( enumerable ) );
  }

  [TestMethod]
  public void Empty ()
  {
    int[] source = [];
    Capacitor<int> capacitor = new(source);
    CapacitorStoreEnumerator<int> enumerator = new (capacitor);
    EnumerableEnumerator<int> enumerable = new (enumerator);

    Assert.IsTrue ( source.SequenceEqual ( enumerable ) );
    enumerator.Reset ();
    Assert.IsTrue ( source.SequenceEqual ( enumerable ) );
  }

  [TestMethod]
  public void MoveNext ()
  {
    Capacitor<int> capacitor = [1,2];
    CapacitorStoreEnumerator<int> enumerator = new (capacitor);

    (int, bool) [] steps = [(0, true), (1, true), (1, false)];
    foreach ((int increment, bool moved) in steps)
    {
      Assert.AreEqual ( moved, enumerator.MoveNext () );
      Assert.AreEqual ( 0 + increment, enumerator.index );
      Assert.AreEqual ( 1 + increment, enumerator.Current );
    }
  }

  [TestMethod]
  public void Current ()
  {
    Capacitor<int> capacitor = [3 ];
    CapacitorStoreEnumerator<int> enumerator = new (capacitor);

    Assert.AreEqual ( 0, enumerator.Current );
    Assert.AreEqual ( 0, ((IEnumerator) enumerator).Current );

    _ = enumerator.MoveNext ();

    Assert.AreEqual ( 3, enumerator.Current );
    Assert.AreEqual ( 3, ((IEnumerator) enumerator).Current );
  }

  [TestMethod]
  public void Reset ()
  {
    Capacitor<int> capacitor = [1];
    CapacitorStoreEnumerator<int> enumerator = new (capacitor);

    _ = enumerator.MoveNext ();
    enumerator.Reset ();

    Assert.AreEqual ( resetIndex, enumerator.index );
    Assert.AreEqual ( -1, enumerator.index );
    Assert.AreEqual ( 0, enumerator.Current );
  }

  // veryfing strunct equality works as it should, not truly implemented
  [TestMethod]
  public void Equals_Object ()
  {
    int[] source = [1, 2, 3, 4, 5];

    Capacitor<int> c1 = new (source);
    Capacitor<int> c2 = new (source);

    CapacitorStoreEnumerator<int> e1 = new (c1);
    CapacitorStoreEnumerator<int> e2 = new (c1);

    Assert.IsTrue ( e1.Equals ( (object) e2 ) );

    _ = e2.MoveNext ();
    Assert.IsFalse ( e1.Equals ( (object) e2 ) );

    c1.Add ( 6 );
    e2 = new ( c1 );
    Assert.IsFalse ( e1.Equals ( (object) e2 ) );

    e2 = new ( c2 );
    Assert.IsFalse ( e1.Equals ( (object) e2 ) );
  }
}
