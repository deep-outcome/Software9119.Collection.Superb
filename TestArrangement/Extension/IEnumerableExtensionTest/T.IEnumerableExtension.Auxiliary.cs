using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;
using Software9119.Collection.Superb.TestArrangement.TestAide;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableExtensionTest;

#pragma warning disable CA1724
public partial class IEnumerableExtensionTest
#pragma warning restore CA1724
{

  [TestMethod]
  public void NullEnumerable ()
  {
    ArgumentNullException test = IEnumerableExtension.NullEnumerable("xXx");
    Assert.AreEqual ( "Enumerable must be provided. (Parameter 'xXx')", test?.Message );
  }

  [TestMethod]
  [DataRow ( 0, 0, true )]
  [DataRow ( 1, 0, true )]
  [DataRow ( 1, 1, true )]
  [DataRow ( 1, 2, false )]
  [DataRow ( 2, 2, true )]
  [DataRow ( 2, 3, false )]
  public void AtLeast ( int count, int atLeast, bool has )
  {
    IEnumerable<int> enumerable = XEnumerable.RangeEnumerable(0, count);
    bool test = enumerable.AtLeast(atLeast);

    Assert.AreEqual ( has, test );
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
