using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class AsOrToTargetTypeTest
{
  [TestMethod]
  public void Constructor ()
  {
    Ctor<ArraySegment<object>> func = (e,c) => default;
    AsOrToTargetType<ArraySegment<object>> test = new(func, null, () => default);

    Assert.IsTrue ( ReferenceEquals ( func, test.Ctor ) );
    Assert.IsTrue ( test.CanCast ( default ( ArraySegment<object> ) ) );
    Assert.AreEqual ( default ( ArraySegment<object> ), test.Empty () );
  }

  [TestMethod]
  public void Constructor_CanCast ()
  {
    Ctor<ArraySegment<object>> func = (e,c) => default;
    CanCast canCast = e => e.Cast<object>().Any();
    AsOrToTargetType<ArraySegment<object>> test = new(func, canCast, () => default);

    Assert.IsFalse ( test.CanCast ( new int [ 0 ] ) );
    Assert.IsTrue ( test.CanCast ( new int [] { 1 } ) );
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
  [SuppressMessage ( "Style", "IDE0017:Simplify object initialization", Justification = "Obviousity." )]
  public void Empty_Setter ()
  {
    Ctor<int> func = (e,c) => default;
    AsOrToTargetType<int> targetType = new(func, e => default, empty: () => 0);

    targetType.Empty = () => -1;
    Assert.AreEqual ( -1, targetType.Empty () );
  }

  [TestMethod]
  public void Empty_Setter_NullValue ()
  {
    Ctor<int> func = (e,c) => default;
    AsOrToTargetType<int> targetType = new(func, e => default, () => default);

    Action test = () => targetType.Empty = null!;
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Empty constructor must be provided. (Parameter 'value')", e.Message );
  }

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0017:Simplify object initialization", Justification = "Obviousity." )]
  public void CanCast_Setter ()
  {
    Ctor<List<int>> func = (e,c) => default!;
    AsOrToTargetType<List<int>> targetType = new(func, e => default, () => default!);

    targetType.CanCast = null!;
    Assert.IsTrue ( targetType.CanCast ( new List<int> () ) );
    targetType.CanCast = e => e is not List<int>;
    Assert.IsFalse ( targetType.CanCast ( new List<int> () ) );
  }

  [TestMethod]
  [SuppressMessage ( "Style", "IDE0017:Simplify object initialization", Justification = "Obviousity." )]
  public void Ctor_Setter ()
  {
    Ctor<int> func = (e,c) => 0;
    AsOrToTargetType<int> targetType = new(func, e => default, () => default);

    targetType.Ctor = ( e, c ) => -1;
    Assert.AreEqual ( -1, targetType.Ctor ( null!, null ) );
  }

  [TestMethod]
  public void Ctor_Setter_NullValue ()
  {
    Ctor<int> func = (e,c) => default;
    AsOrToTargetType<int> targetType = new(func, e => default, () => default);

    Action test = () => targetType.Ctor = null!;
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Constructor must be provided. (Parameter 'value')", e.Message );
  }

  [TestMethod]
  public void FromTypedCtor ()
  {
    Ctor<int, List<int>> func = ( e, c ) =>
    {
      List<int> output = new (2 * c!.Value);
      output.AddRange(e);
      return output;
    };

    AsOrToTargetType<List<int>> test = AsOrToTargetType.FromTypedCtor(func, null, () => []);

    const int count = 11;
    IEnumerable<int> enumerable = Enumerable.Range(1, count);
    List<int> expectation = new ([..enumerable]);

    Assert.IsTrue ( test.CanCast ( new List<int> () ) );

    List<int> list = test.Ctor(enumerable, count);
    Assert.IsTrue ( expectation.SequenceEqual ( list ) );
    Assert.AreEqual ( 2 * count, list.Capacity );
    Assert.HasCount ( 0, test.Empty () );
  }

  [TestMethod]
  public void FromTypedCtor_CanCast ()
  {
    Ctor<int, List<int>> func = (e,c) => default!;
    CanCast canCast = e => e.Cast<object>().Any();

    AsOrToTargetType<List<int>> test = AsOrToTargetType.FromTypedCtor(func, canCast, () => []);

    Assert.IsFalse ( test.CanCast ( new int [ 0 ] ) );
    Assert.IsTrue ( test.CanCast ( new int [] { 1 } ) );
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
