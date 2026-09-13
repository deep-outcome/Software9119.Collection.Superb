using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Thrown when <see cref="BlockingCollectionType"/> is not one of:
/// <list type="bullet">
///   <item><see cref="BlockingCollectionType.ConcurrentBag"/></item>
///   <item><see cref="BlockingCollectionType.ConcurrentQueue"/></item>
///   <item><see cref="BlockingCollectionType.ConcurrentStack"/></item>
/// </list>
/// </summary>
[SuppressMessage ( "Design", "CA1032:Implement standard exception constructors", Justification = "Intentional." )]
sealed public class UnsupportedBlockingCollectionTypeException : ArgumentOutOfRangeException
{
  internal UnsupportedBlockingCollectionTypeException
  (
    BlockingCollectionType type,
    [CallerArgumentExpression ( nameof ( type ) )] string? paramName = null
  )
    : base ( paramName: paramName, $"Unsupported blocking collection type, '{type}'." )
  {

  }
}