using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Software9119.Collection.Superb.Indexing;

/// <summary>
/// Validation parameter names.
/// </summary>
[SuppressMessage ( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "Okay." )]
readonly public struct SegmentationParamNames ( string offset, string count, string? unit ) : IEnumerable<string>
{
  readonly internal string[] parameters = string.IsNullOrWhiteSpace(unit) ? [offset, count] : [offset, count, unit];

  internal string this [ int index ]
  {
    get
    {
      string [] parameters = Parameters;
      if (index >= parameters.Length)
        return "";

      string name = parameters [ index ];
      if (string.IsNullOrWhiteSpace ( name )) return "";

      return name;
    }
  }

  internal string [] Parameters => parameters ?? Array.Empty<string> ();

  /// <summary>
  /// Offset parameter name.
  /// </summary>  
  public string Offset => this [ 0 ];
  /// <summary>
  /// Count parameter name.
  /// </summary>  
  public string Count => this [ 1 ];
  /// <summary>
  /// Unit parameter name.
  /// </summary>  
  public string Unit => this [ 2 ];

  /// <summary>
  /// Number of valid parameter names for provision.
  /// </summary>
  public int ParamsCount => Parameters.PureStringsOnly ().Count ();

  /// <summary>
  /// Enumerator of parameters in order: <see cref="Offset"/>, <see cref="Count"/>, <see cref="Unit"/>.
  /// </summary>
  /// <remarks>
  /// Only pure string are enumerated.
  /// </remarks>
  public IEnumerator<string> GetEnumerator () => Parameters.PureStringEnumerator ();
  /// <summary>
  /// Enumerator of parameters in order: <see cref="Offset"/>, <see cref="Count"/>, <see cref="Unit"/>.
  /// </summary>
  /// <remarks>
  /// Only pure string are enumerated.
  /// </remarks>
  IEnumerator IEnumerable.GetEnumerator () => Parameters.PureStringEnumerator ();
}
