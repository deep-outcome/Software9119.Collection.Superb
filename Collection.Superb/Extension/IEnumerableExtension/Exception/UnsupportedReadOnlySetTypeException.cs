using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Thrown when <see cref="ReadOnlySetType"/> is not one of:
/// <list type="bullet">
///   <item><see cref="ReadOnlySetType.FrozenSet"/></item>
///   <item><see cref="ReadOnlySetType.HashSet"/></item>
///   <item><see cref="ReadOnlySetType.ImmutableHashSet"/></item>
///   <item><see cref="ReadOnlySetType.ImmutableSortedSet"/></item>
///   <item><see cref="ReadOnlySetType.SortedSet"/></item>
/// </list>
/// </summary>
[SuppressMessage ( "Design", "CA1032:Implement standard exception constructors", Justification = "Intentional." )]
sealed public class UnsupportedReadOnlySetTypeException : ArgumentOutOfRangeException
{
  internal UnsupportedReadOnlySetTypeException
  (
    ReadOnlySetType setType,
    [CallerArgumentExpression ( nameof ( setType ) )] string? paramName = null
  )
    : base ( paramName: paramName, $"Unsupported set type, '{setType}'." )
  {

  }
}