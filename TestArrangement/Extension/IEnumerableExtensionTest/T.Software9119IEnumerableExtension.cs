using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

[TestClass]
public class Software9119IEnumerableExtensionTest
{
  [TestMethod]
  public void NullEnumerable ()
  {
    ArgumentNullException test = Software9119IEnumerableExtension.NullEnumerable("xXx");
    Assert.AreEqual ( "Enumerable must be provided. (Parameter 'xXx')", test?.Message );
  }

  [TestMethod]
  public void AtLeast ()
  {
    IEnumerable<int> test = XEnumerable.RangeEnumerable(0, 2);

    Assert.IsTrue ( test.AtLeast ( 1 ) );
    Assert.IsTrue ( test.AtLeast ( 2 ) );
    Assert.IsFalse ( test.AtLeast ( 3 ) );
  }

  [TestMethod]
  public void AtLeast_NullEnumerable ()
  {
    IEnumerable<int> enumerable = null!;
    Action test = () => enumerable.AtLeast(1);

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Enumerable must be provided. (Parameter 'enumerable')", e.Message );
  }

  [TestMethod]
  public void PureStrings ()
  {
    string?[] test = [null, "", " ", "aBc", null, "", " ", "XyZ", null, "", " ", "opQ"];
    string[] expectation = ["aBc", "XyZ", "opQ"];

    Assert.IsTrue ( expectation.SequenceEqual ( test.PureStrings () ) );
  }

  [TestMethod]
  public void PureStrings_NullEnumerable ()
  {
    IEnumerable<string> enumerable = null!;
    Action test = () => enumerable.PureStrings();

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Enumerable must be provided. (Parameter 'enumerable')", e.Message );
  }

  [TestMethod]
  public void PureStringEnumerator ()
  {
    string?[] source = [null, "", " ", "aBc", null, "", " ", "XyZ", null, "", " ", "opQ"];
    string[] expectation = ["aBc", "XyZ", "opQ"];

    EnumerableEnumerator<string?> test = new(source.PureStringEnumerator());
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void PureStringEnumerator_NullEnumerable ()
  {
    IEnumerable<string> enumerable = null!;
    Action test = () => enumerable.PureStringEnumerator();

    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( "Enumerable must be provided. (Parameter 'enumerable')", e.Message );
  }
}
