using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Indexing.Auxiliaries;

[TestClass]
[SuppressMessage ( "Usage", "MSTEST0037:Use proper 'Assert' methods", Justification = @"¯\_x_x_/¯" )]
public class SegmentationParamNamesTest
{
  [TestMethod]
  public void SafeDefault ()
  {
    SegmentationParamNames test = default;

    Assert.AreEqual ( "", test.Offset );
    Assert.AreEqual ( "", test.Count );
    Assert.AreEqual ( "", test.Unit );

    IEnumerator<string> typedEnumerator = test.GetEnumerator();
    IEnumerator enumerator = ((IEnumerable)test).GetEnumerator();

    Assert.IsNotNull ( typedEnumerator );
    Assert.IsNotNull ( enumerator );

    Assert.IsFalse ( typedEnumerator.MoveNext () );
    Assert.IsFalse ( enumerator.MoveNext () );

    Assert.AreEqual ( Array.Empty<string> (), test.Parameters );
  }

  [TestMethod]
  [DataRow ( "" )]
  [DataRow ( " " )]
  [DataRow ( null )]
  public void SafeEmptyValues ( string? emptyValue )
  {
    SegmentationParamNames test = new (emptyValue!, emptyValue!, emptyValue);

    Assert.AreEqual ( "", test.Offset );
    Assert.AreEqual ( "", test.Count );
    Assert.AreEqual ( "", test.Unit );

    IEnumerator<string> typedEnumerator = test.GetEnumerator();
    IEnumerator enumerator = ((IEnumerable)test).GetEnumerator();

    Assert.IsFalse ( typedEnumerator.MoveNext () );
    Assert.IsFalse ( enumerator.MoveNext () );
  }

  [TestMethod]
  [DataRow ( "" )]
  [DataRow ( " " )]
  [DataRow ( null )]
  [DataRow ( "array" )]
  public void Parameters ( string? unit )
  {
    SegmentationParamNames test = new ("index", "number", unit);

    bool validName = unit == "array";
    Assert.AreEqual ( validName ? 3 : 2, test.parameters.Length );

    Assert.AreEqual ( "index", test.Offset );
    Assert.AreEqual ( "number", test.Count );
    Assert.AreEqual ( validName ? "array" : "", test.Unit );

    List<string> expectation = ["index", "number"];
    if (validName)
      expectation.Add ( "array" );

    IEnumerator<string> typedEnumerator = test.GetEnumerator();
    IEnumerator enumerator = ((IEnumerable)test).GetEnumerator();

    EnumerableEnumerator<string> typeEnumerable = new (typedEnumerator);
    EnumerableEnumerator enumerable = new (enumerator);

    Assert.IsTrue ( expectation.SequenceEqual ( typeEnumerable ) );
    Assert.IsTrue ( expectation.SequenceEqual ( enumerable.Cast<string> () ) );
  }

  [TestMethod]
  public void Indexer ()
  {
    SegmentationParamNames test;

    test = default;
    Assert.AreEqual ( "", test [ -1 ] );
    Assert.AreEqual ( "", test [ 0 ] );
    Assert.AreEqual ( "", test [ int.MaxValue ] );

    test = new ( "index", "number", "array" );
    Assert.AreEqual ( "index", test [ 0 ] );
    Assert.AreEqual ( "number", test [ 1 ] );
    Assert.AreEqual ( "array", test [ 2 ] );
    Assert.AreEqual ( "", test [ 3 ] );

    test = new ( "index", "number", null );
    Assert.AreEqual ( "index", test [ 0 ] );
    Assert.AreEqual ( "number", test [ 1 ] );
    Assert.AreEqual ( "", test [ 2 ] );

    Action negativeIndex = () => _ = test[-1];
    IndexOutOfRangeException e = Assert.ThrowsExactly<IndexOutOfRangeException> ( negativeIndex );
    Assert.AreEqual ( "Index was outside the bounds of the array.", e.Message );
  }
}
