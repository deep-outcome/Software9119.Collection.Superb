using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class RevertedComparer ( int forStart, int forCount ) : IComparer<int>
{
  readonly internal int reversor = Reversor(forStart, forCount);

  public int Compare ( int x, int y ) => x.CompareTo ( Revert ( y, reversor ) );
  static public int Revert ( int number, int reversor ) => reversor - number;
  static public int Max ( int fromStart, int andCount ) => fromStart + andCount - 1;
  static public int Reversor ( int fromStart, int andCount ) => Max ( fromStart, andCount ) + fromStart;
}

[TestClass]
public class RevertedComparerTest
{
  [TestMethod]
  [DataRow ( -5, 10 )]
  [DataRow ( -10, 10 )]
  [DataRow ( 10, 10 )]
  public void Max ( int start, int count )
  {
    int max = Enumerable.Range(start, count).Max();
    Assert.AreEqual ( max, RevertedComparer.Max ( start, count ) );
  }

  [TestMethod]
  [DataRow ( -5, 10 )]
  [DataRow ( -10, 10 )]
  [DataRow ( 10, 10 )]
  public void Reversor ( int start, int count )
  {
    int max = Enumerable.Range(start, count).Max();
    Assert.AreEqual ( max + start, RevertedComparer.Reversor ( start, count ) );
  }

  [TestMethod]
  [SuppressMessage ( "Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "Ok." )]
  [DataRow ( 1, 10 )]
  [DataRow ( 1, 40 )]
  [DataRow ( 5, 10 )]
  [DataRow ( 5, 40 )]
  [DataRow ( -5, 10 )]
  [DataRow ( -5, 40 )]
  public void Revert ( int start, int count )
  {
    IEnumerable<int> source = Enumerable.Range(start, count);
    IEnumerable<int> expectation = source.Reverse();

    int max = start + count -1;
    int reversor = start + max;
    IEnumerable<int> test = source.Select ( x => RevertedComparer.Revert ( x, reversor));
    Assert.IsTrue ( expectation.SequenceEqual ( test ) );
  }

  [TestMethod]
  [DataRow ( 1, 10 )]
  [DataRow ( 1, 40 )]
  [DataRow ( 5, 10 )]
  [DataRow ( 5, 40 )]
  [DataRow ( -5, 10 )]
  [DataRow ( -5, 40 )]
  public void Compare ( int start, int count )
  {
    RevertedComparer comparer = new (forStart: start, count);

    int max = start + count -1;

    Assert.AreEqual ( 0, comparer.Compare ( start, max ) );
    Assert.AreEqual ( 0, comparer.Compare ( start + 1, max - 1 ) );

    Assert.AreEqual ( 0, comparer.Compare ( max, start ) );
    Assert.AreEqual ( 0, comparer.Compare ( max - 1, start + 1 ) );
  }
}