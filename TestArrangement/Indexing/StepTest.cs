using Microsoft.VisualStudio.TestTools.UnitTesting;

using Software9119.Collection.Superb.Indexing;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Software9119.Collection.Superb.TestArrangement.Indexing;

[TestClass]
public class StepTest
{
  // Constructors

  [TestMethod]
  public void InitialValueConstructor ()
  {
    const int value = 65536;
    const int size = 1024;

    Step step = new (size: size, value);

    Assert.AreEqual ( value, step );
    Assert.AreEqual ( value, step.value );

    Assert.AreEqual ( size, step.Size );
  }

  [TestMethod]
  public void ZeroSizedStep_ThrowArgumentOutOfRangeException ()
  {
    Action[] ctors =  [
      () => _ = new Step ( 0, default ( int ) ),
      () => _ = new Step ( 0 )
    ];

    foreach (Action c in ctors)
    {
      ArgumentOutOfRangeException e = Assert.ThrowsExactly<ArgumentOutOfRangeException> ( c );
      Assert.AreEqual ( "Cannot make zero-sized steps! (Parameter 'size')", e.Message );
    }
  }

  [TestMethod]
  public void DefaultConstructor ()
  {
    const int size = 1;
    Step step = new (size);

    Assert.AreEqual ( 0, step );
    Assert.AreEqual ( 0, step.value );

    Assert.AreEqual ( size, step.Size );
  }

  // Casting
  [TestMethod]
  public void ToInt32 ()
  {
    Step step = new (3, 5);

    Assert.AreEqual ( 5, step.ToInt32 () );
    ++step;
    Assert.AreEqual ( 8, step.ToInt32 () );
  }

  [TestMethod]
  public void ImplicitIntCastOperator ()
  {
    Step step = new (3, 5);

    Assert.AreEqual ( 5, step );
    ++step;
    Assert.AreEqual ( 8, step );
  }

  // Arithmetic operators

  [TestMethod]
  public void IncrementOperator ()
  {
    const int size = 3;
    Step step = new (size);

    Assert.AreEqual ( size, ++step );
  }

  [TestMethod]
  public void IncrementOperator_InitialValue ()
  {
    const int size = 3;
    Step step = new (size: size, -size);

    Assert.AreEqual ( 0, ++step );
  }

  [TestMethod]
  public void DecrementOperator ()
  {
    const int size = 3;
    Step step = new (size);

    Assert.AreEqual ( -size, --step );
  }

  [TestMethod]
  public void DecrementOperator_InitialValue ()
  {
    const int size = 3;
    Step step = new (size: size, size);

    Assert.AreEqual ( 0, --step );
  }

  [TestMethod]
  public void AddOperator ()
  {
    const int size = 3;
    Step step = new (size);

    const int heterogenicStep = 89;

    step += heterogenicStep;
    Assert.AreEqual ( heterogenicStep, step );
  }

  [TestMethod]
  public void AddOperator_InitialValue ()
  {
    const int heterogenicStep = 89;
    const int size = 3;

    Step step = new (size: size, -heterogenicStep);

    step += heterogenicStep;
    Assert.AreEqual ( 0, step );
  }

  [TestMethod]
  public void SubtractOperator ()
  {
    const int size = 3;
    Step step = new (size);

    const int heterogenicStep = 89;

    step -= heterogenicStep;
    Assert.AreEqual ( -heterogenicStep, step );
  }

  [TestMethod]
  public void SubtractOperator_InitialValue ()
  {
    const int size = 3;
    const int heterogenicStep = 89;

    Step step = new (size: size, heterogenicStep);

    step -= heterogenicStep;
    Assert.AreEqual ( 0, step );
  }

  // Arithmetic operator counterpart- methods

  [TestMethod]
  public void IncrementMethod ()
  {
    const int size = 3;
    Step step = new (size);

    step.Increment ();
    Assert.AreEqual ( size, step );
  }

  [TestMethod]
  public void IncrementMethod_InitialValue ()
  {
    const int size = 3;
    Step step = new (size: size, -size);

    step.Increment ();
    Assert.AreEqual ( 0, step );
  }

  [TestMethod]
  public void DecrementMethod ()
  {
    const int size = 3;
    Step step = new (size);

    step.Decrement ();
    Assert.AreEqual ( -size, step );
  }

  [TestMethod]
  public void DecrementMethod_InitialValue ()
  {
    const int size = 3;
    Step step = new (size: size, size);

    step.Decrement ();
    Assert.AreEqual ( 0, step );
  }

  [TestMethod]
  public void AddMethod ()
  {
    const int size = 3;
    Step step = new (size);

    const int heterogenicStep = 89;

    step.Add ( heterogenicStep );
    Assert.AreEqual ( heterogenicStep, step );
  }

  [TestMethod]
  public void AddMethod_InitialValue ()
  {
    const int size = 3;
    const int heterogenicStep = 89;

    Step step = new (size, -heterogenicStep);
    step.Add ( heterogenicStep );
    Assert.AreEqual ( 0, step );
  }

  [TestMethod]
  public void SubtractMethod ()
  {
    const int size = 3;
    Step step = new (size);

    const int heterogenicStep = 89;

    step.Subtract ( heterogenicStep );
    Assert.AreEqual ( -heterogenicStep, step );
  }

  [TestMethod]
  public void SubtractMethod_InitialValue ()
  {
    const int size = 3;
    const int heterogenicStep = 89;
    Step step = new (size, heterogenicStep);

    step.Subtract ( heterogenicStep );
    Assert.AreEqual ( 0, step );
  }

  // Equality

  [TestMethod]
  public void EqualsObjectMethod ()
  {
    Step i  = new (3);
    Step you = new (2);
    object obj  = new ();

    Assert.IsFalse ( i.Equals ( obj ) );
    Assert.IsFalse ( i.Equals ( null ) );

    Assert.IsFalse ( i.Equals ( (object) you ) );
    Assert.IsTrue ( i.Equals ( (object) i ) );
  }

  [TestMethod]
  public void EqualsStepMethod ()
  {
    Step i  = new (3);
    Step you  = new (4);

    Assert.IsFalse ( i.Equals ( you ) );
        
    you = new ( 3 );

    Assert.IsTrue ( i.Equals ( you ) );
    Assert.IsFalse ( i.Equals ( --you ) );
  }

  [TestMethod]
  public void EqualOperator ()
  {
    Step i  = new (3);
    Step you  = new (4);

    Assert.IsFalse ( i == you );
        
    you = new ( 3 );

    Assert.IsTrue ( i == you );
    Assert.IsFalse ( i == --you );
  }

  [TestMethod]
  public void NotEqualOperator ()
  {
    Step i  = new (3);
    Step you  = new (4);

    Assert.IsTrue ( i != you );
        
    you = new ( 3 );

    Assert.IsFalse ( i != you );
    Assert.IsTrue ( i != --you );
  }

  // Hash code

  [TestMethod]
  [DataRow ( 1, 0 )]
  [DataRow ( 3, 5 )]
  public void GetHashCode ( int size, int value )
  {
    Step step = new(size: size, value);
    int expectation = HashCode.Combine(size, value);
    Assert.AreEqual ( expectation, step.GetHashCode () );
  }


  // Usage test

  [TestMethod]
  public void Usage ()
  {
    Step stepOdd = new (2, 0);
    Step stepEven = new (2, 1);

    List<int> source = [ .. Enumerable.Range(1, 90) ];

    int[] destination = new int[90];

    int sumOdd = 0;
    int sumEven = 0;
    int count = source.Count;
    for (; stepEven < count ;)
    {
      sumOdd += source [ stepOdd++ ];
      sumEven += source [ stepEven++ ];
    }

    Func<int, bool> evenSelector = x => x % 2 == 0;
    Assert.AreEqual ( source.Where ( x => !evenSelector ( x ) ).Sum (), sumOdd );
    Assert.AreEqual ( source.Where ( evenSelector ).Sum (), sumEven );
  }

  [TestMethod]
  public void Usage2 ()
  {
    const int size = 90;
    List<int> numbers = [ .. Enumerable.Range(0, size) ];

    Step backward = new (-1, size);
    int sum = 0;
    for (; backward > 0 ;)
      sum += numbers [ ++backward ];

    Assert.AreEqual ( numbers.Sum (), sum );
  }
}
