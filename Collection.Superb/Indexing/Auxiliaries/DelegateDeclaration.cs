namespace Software9119.Collection.Superb.Indexing;

/// <summary>
/// Parameter names getter.
/// </summary>
public delegate T ParamNames<T> ();

/// <summary>
/// Delegate extension methods.
/// </summary>
static public class DelegateExtensions
{
  /// <summary>
  /// Get or <c>default(<typeparamref name="T"/>)</c> facade.
  /// </summary>
  /// <returns><c>default(<typeparamref name="T"/>)</c> when <paramref name="paramNames"/> is <see langword="null"/>.</returns>
  static public T SafeGet<T> ( this ParamNames<T>? paramNames ) where T : struct => paramNames?.Invoke () ?? default ( T );
}