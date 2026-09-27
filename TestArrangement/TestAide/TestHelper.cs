using Software9119.Collection.Superb.Segmentation;

using System;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

static class TestHelper
{
  static public ArraySegment<T?> ArrSeg<T> ( T? [] array, int offset, int count ) => new ( array, offset, count );
  static public IListSegment<T?> IListSeg<T> ( IList<T?> list, int offset, int count ) => new ( list, offset, count );
}
