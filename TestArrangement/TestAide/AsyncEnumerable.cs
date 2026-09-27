using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class AsyncEnumerable ( int start, int count ) : IAsyncEnumerable<int>
{
  async IAsyncEnumerable<int> Generator ()
  {
    int c = count;
    int s = start;

    while (c-- > 0)
      yield return s++;
  }

  public IAsyncEnumerator<int> GetAsyncEnumerator ( CancellationToken cancellationToken = default ) 
    => Generator ().GetAsyncEnumerator ( cancellationToken );
}
