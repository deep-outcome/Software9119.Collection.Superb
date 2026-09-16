using Software9119.Collection.Superb.Extension;

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Segmentation;

class SegmentingValidator
{
  static public bool ValidateList
  (
    [NotNullWhen ( false )] IEnumerable? list,
    [NotNullWhen ( true )] out ArgumentNullException? e,
    [CallerArgumentExpression ( nameof ( list ) )] string? listParamName = null
  )
  {
    if (list.IsNull ())
    {
      e = new ArgumentNullException ( paramName: listParamName, message: "Null list provided." );
      return true;
    }

    e = null;
    return false;
  }
}
