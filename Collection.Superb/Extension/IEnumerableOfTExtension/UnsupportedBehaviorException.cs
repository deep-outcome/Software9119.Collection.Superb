using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Thrown when <see cref="EnumerableNullBehavior"/> is not one of:
/// <list type="bullet">
///   <item><see cref="EnumerableNullBehavior.ReturnDefault"/></item>
///   <item><see cref="EnumerableNullBehavior.ReturnEmpty"/></item>
///   <item><see cref="EnumerableNullBehavior.ThrowException"/></item>
/// </list>
/// </summary>
[SuppressMessage ( "Design", "CA1032:Implement standard exception constructors", Justification = "Intentional." )]
sealed public class UnsupportedBehaviorException : ArgumentOutOfRangeException
{
  internal UnsupportedBehaviorException
  (
    EnumerableNullBehavior behavior,
    [CallerArgumentExpression ( nameof ( behavior ) )] string? paramName = null
  )
    : base ( paramName: paramName, $"Unsupported behavior, '{behavior}'." )
  {

  }
}
