using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class AsOrToTargetTypeTest
{
  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void Constructor ( bool canCast )
  {
    Ctor<ArraySegment<object>> func = (e,c) => default;
    AsOrToTargetType<ArraySegment<object>> test = new(func, e => canCast, () => default);

    Assert.AreSame ( func, test.Ctor );
    Assert.AreEqual ( canCast, test.CanCast ( null! ) );
    Assert.AreEqual ( default ( ArraySegment<object> ), test.Empty () );
  }

  [TestMethod]
  [DataRow ( "c", "Constructor must be provided. (Parameter 'ctor')" )]
  [DataRow ( "e", "Empty constructor must be provided. (Parameter 'empty')" )]
  public void Constructor_NullParameter ( string whosNull, string msg )
  {
    Ctor<object> ctor   = whosNull == "c" ? null!  : (e, c) => null!;
    Empty<object> empty = whosNull == "e" ? null!  : () => null!;

    Action test = () => _ = new AsOrToTargetType<object>(ctor, null, empty);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> (test);
    Assert.AreEqual ( msg, e.Message );
  }

  [TestMethod]
  [DataRow ( true )]
  [DataRow ( false )]
  public void FromTypedCtor ( bool canCast )
  {
    Ctor<int, List<int>> func = ( e, c ) =>
    {
      List<int> output = new (2 * c!.Value);
      output.AddRange(e);
      return output;
    };

    AsOrToTargetType<List<int>> test = AsOrToTargetType.FromTypedCtor(func, e => canCast, () => []);

    const int count = 11;
    IEnumerable<int> enumerable = Enumerable.Range(1, count);
    List<int> expectation = new ([..enumerable]);

    Assert.AreEqual ( canCast, test.CanCast ( null! ) );

    List<int> list = test.Ctor(enumerable, count);
    Assert.IsTrue ( expectation.SequenceEqual ( list ) );
    Assert.AreEqual ( 2 * count, list.Capacity );
    Assert.HasCount ( 0, test.Empty () );
  }

  [TestMethod]
  [DataRow ( "c", "Constructor must be provided. (Parameter 'ctor')" )]
  [DataRow ( "e", "Empty constructor must be provided. (Parameter 'empty')" )]
  public void FromTypedCtor_NullParameter ( string whosNull, string msg )
  {
    Ctor<int, int> ctor = whosNull == "c"  ? null!  : (e, c) => 0;
    Empty<int> empty    = whosNull == "e"  ? null!  : () => 0;

    Action test = () => _ = AsOrToTargetType.FromTypedCtor(ctor, null, empty);
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> (test);
    Assert.AreEqual ( msg, e.Message );
  }
}
