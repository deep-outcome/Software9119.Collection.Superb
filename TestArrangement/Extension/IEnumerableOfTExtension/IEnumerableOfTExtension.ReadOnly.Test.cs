using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Extension;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Extension.IEnumerableOfTExtension;

[TestClass]
public partial class IEnumerableExtensionTest
{
  [TestMethod]
  [SuppressMessage ( "Design", "MSTEST0032:Assertion condition is always true", Justification = "Intentional" )]
  public void DefaultCapacities ()
  {
    Assert.AreEqual ( 8, IEnumerableExtension.DefaultListCapacity );
    Assert.AreEqual ( 8, IEnumerableExtension.DefaultDictCapacity );
  }

  [TestMethod]
  public void EnumerableNull ()
  {
    const string expectation = "Null source enumerable encounter. (Parameter 'test')";
    string test = IEnumerableExtension.EnumerableNull ( "test" ).Message;
    Assert.AreEqual ( expectation, test );
  }

  [TestMethod]
  public void DictionaryNull ()
  {
    const string expectation = "Null source dictionary encounter. (Parameter 'test')";
    string test = IEnumerableExtension.DictionaryNull ( "test" ).Message;
    Assert.AreEqual ( expectation, test );
  }

  [TestMethod]
  public void ToOrAsIList_NullSource_ReturnEmpty ()
  {
    IList<int>? test = ((IEnumerable<int>?) null).ToOrAsIList ( EnumerableNullBehavior.ReturnEmpty );
    Assert.IsTrue ( test is int [] );
    Assert.HasCount ( 0, test );
  }

  [TestMethod]
  public void ToOrAsIList_NullSource_ReturnDefault ()
  {
    IList<int>? test = ((IEnumerable<int>?) null).ToOrAsIList ( EnumerableNullBehavior.ReturnDefault );
    Assert.IsNull ( test );
  }

  [TestMethod]
  public void ToOrAsIList_NullSource_ThrowException ()
  {
    const string expectation = "Null source enumerable encounter. (Parameter 'enumerable')";
    Action test = () => ((IEnumerable<int>?) null).ToOrAsIList ( EnumerableNullBehavior.ThrowException );
    ArgumentNullException e = Assert.ThrowsExactly<ArgumentNullException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void ToOrAsIList_NullSource_UknownBehavior ()
  {
    const string expectation = "Unsupported behavior, '793'. (Parameter 'behavior')";
    Action test = () => ((IEnumerable<int>?) null).ToOrAsIList ( (EnumerableNullBehavior) 793 );
    UnsupportedBehaviorException e = Assert.ThrowsExactly<UnsupportedBehaviorException> ( test );
    Assert.AreEqual ( expectation, e.Message );
  }

  [TestMethod]
  public void ToOrAsIList_EnumerableIsIListOfT ()
  {
    int [] ilist = new int[0];
    IList<int>? test = ilist.ToOrAsIList();
    Assert.IsTrue ( ReferenceEquals ( ilist, test ) );
  }

  [TestMethod]
  public void ToOrAsIList_EnumerableIsICollectionOfT ()
  {
    HashSet<int> set = [1 ,2, 3];
    IList<int> test = set.ToOrAsIList()!;
    Assert.AreEqual ( typeof ( int [] ), test.GetType () );
    Assert.IsTrue ( set.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void ToOrAsIList_EnumerableIsEnumerable ()
  {
    IEnumerable<int> enumerable = Enumerable.Range(1, 9).Select(x => x);
    IList<int> test = enumerable.ToOrAsIList()!;
    Assert.AreEqual ( typeof ( List<int> ), test.GetType () );
    Assert.AreEqual ( 16, ((List<int>) test).Capacity );
    Assert.IsTrue ( enumerable.SequenceEqual ( test ) );
  }

  [TestMethod]
  public void ToOrAsIList_ExactCapacity ()
  {
    const int count = 3;
    IEnumerable<int> enumerable = Enumerable.Range(1, count).Select(x => x);
    IList<int> test = enumerable.ToOrAsIList(capacity: count)!;
    Assert.AreEqual ( typeof ( List<int> ), test.GetType () );
    Assert.AreEqual ( count, ((List<int>) test).Capacity );
    Assert.IsTrue ( enumerable.SequenceEqual ( test ) );
  }
}
