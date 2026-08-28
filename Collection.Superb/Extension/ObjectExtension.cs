using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Software9119.Collection.Superb.Extension;

/// <summary>
/// Handy object extesion methods.
/// </summary>
static public class ObjectExtension
{
  /// <summary>
  /// <see langword="null"/> check extension method.
  /// </summary>  
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public bool IsNull<T> ( [NotNullWhen ( false )] this T? obj ) => obj is null;

  /// <summary>
  /// Not <see langword="null"/> check extension method.
  /// </summary>  
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public bool IsNotNull<T> ( [NotNullWhen ( true )] this T? obj ) => obj is not null;

  /// <summary>
  /// <see langword="default"/> check extension method.
  /// </summary>  
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public bool IsDefault<T> ( this T obj ) where T : struct => obj.Equals ( default ( T ) );

  /// <summary>
  /// Not <see langword="default"/> check extension method.
  /// </summary>  
  [MethodImpl ( MethodImplOptions.AggressiveInlining )]
  static public bool IsNotDefault<T> ( this T obj ) where T : struct => !obj.Equals ( default ( T ) );

  /// <summary>
  /// <see langword="null"/> safe equality check method.
  /// </summary>  
  /// <remarks>
  /// Truth table
  /// <code>
  /// ╔══════╦═════════╦═════════════════════╗
  /// ║ one  ║ another ║       result        ║
  /// ╠══════╬═════════╬═════════════════════╣
  /// ║ null ║ null    ║ true                ║
  /// ║ null ║ *       ║ false               ║
  /// ║ *    ║ null    ║ one.Equals(another) ║
  /// ║ *    ║ *       ║ one.Equals(another) ║
  /// ╚══════╩═════════╩═════════════════════╝
  /// </code>
  /// </remarks>
  static public bool Matches<T> ( this T? one, T? another )
  {
    if (one.IsNull ())
    {
      if (another.IsNull ())
        return true;

      return false;
    }

    return one.Equals ( another );
  }
}
