using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class AsOrToTargetTypeTest
{
  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void Constructor ( bool tryCast )
  {
    Func<IEnumerable, int?, ArraySegment<object>> func = (e,c) => default;
    AsOrToTargetType<ArraySegment<object>> test = new(func, tryCast, () => default);

    Assert.AreSame ( func, test.Ctor );
    Assert.AreSame ( typeof ( ArraySegment<object> ), test.TypeOfTarget );
    Assert.AreEqual ( tryCast, test.TryCast );
    Assert.AreEqual ( default ( ArraySegment<object> ), test.Empty () );
  }

  [TestMethod]
  public void Constructor_NullCtor ()
  {
    Action test = () => _ = new AsOrToTargetType<object>(null!, default, () => default!);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> (test);
    Assert.AreEqual ( "Constructor must be provided. (Parameter 'ctor')", e.Message );
  }

  [TestMethod]
  public void Constructor_NullEmpty ()
  {
    Func<IEnumerable,int?,object> ctor = (e, c) => null!;
    Action test = () => _ = new AsOrToTargetType<object>(ctor, default, null!);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> (test);
    Assert.AreEqual ( "Empty constructor must be provided. (Parameter 'empty')", e.Message );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void FromTypedCtor ( bool tryCast )
  {
    Func<IEnumerable<int>, int?, List<int>> func = ( e, c ) =>
    {
      List<int> output = new (2 * c!.Value);
      output.AddRange(e);
      return output;
    };

    AsOrToTargetType<List<int>> test = AsOrToTargetType.FromTypedCtor(func, tryCast, () => []);

    const int count = 11;
    IEnumerable<int> enumerable = Enumerable.Range(1, count);
    List<int> expectation = new ([..enumerable]);

    Assert.AreSame ( typeof ( List<int> ), test.TypeOfTarget );
    Assert.AreEqual ( tryCast, test.TryCast );

    List<int> list = test.Ctor(enumerable, count);
    Assert.IsTrue ( expectation.SequenceEqual ( list ) );
    Assert.AreEqual ( 2 * count, list.Capacity );
    Assert.HasCount ( 0, test.Empty () );
  }

  [TestMethod]
  public void FromTypedCtor_NullCtor ()
  {
    Action test = () => _ = AsOrToTargetType.FromTypedCtor<int, int>(null!, default, () => default!);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> (test);
    Assert.AreEqual ( "Constructor must be provided. (Parameter 'ctor')", e.Message );
  }

  [TestMethod]
  public void FromTypedCtor_NullEmpty ()
  {
    Func<IEnumerable<int>,int?,int> ctor = (e, c) => 0;
    Action test = () => _ = AsOrToTargetType.FromTypedCtor(ctor, default, null!);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> (test);
    Assert.AreEqual ( "Empty constructor must be provided. (Parameter 'empty')", e.Message );
  }
}
