using System;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

static class TestHelper
{
  static public ArraySegment<T?> ArrSeg<T> ( T? [] array, int offset, int count ) => new ( array, offset, count );
}
