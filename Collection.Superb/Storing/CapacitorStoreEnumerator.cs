using System;
using System.Collections;
using System.Collections.Generic;

namespace Software9119.Collection.Superb.Storing;

/// <summary>
/// <see cref="Capacitor{T}"/> store enumerator.
/// </summary>
/// <remarks>
/// Changes made to store during enumeration are reflected by <see cref="CapacitorStoreEnumerator{T}"/>, however changes
/// made to <see cref="Capacitor{T}"/> are not, specifically changes to count or store replacements.
/// </remarks>
public struct CapacitorStoreEnumerator<T> : IEnumerator<T?>
{
  readonly internal T?[] store;
  readonly internal int count;

  internal int index;
  internal const int resetIndex = -1;

  /// <summary>
  /// Constructor.
  /// </summary>
  public CapacitorStoreEnumerator ( Capacitor<T?> capacitor )
  {
    if (capacitor == null)
      throw new ArgumentNullException ( paramName: nameof ( capacitor ), "Capacitor must be provided." );

    store = capacitor.store;
    count = capacitor.Count;

    Reset ();
  }

  /// <summary>
  /// Nothing to dispose.
  /// </summary>
  readonly public void Dispose () { }

  /// <summary>
  /// Advances enumerator by one item.
  /// </summary>
  /// <returns><see langword="true"/> when enumeration advances, <see langword="false"/> when enumeration reached its end already.</returns>
  public bool MoveNext ()
  {
    if (index < count - 1)
    {
      ++index;
      return true;
    }

    return false;
  }

  /// <summary>
  /// Current element or <c>default(<typeparamref name="T"/>)</c>, if enumeration is not yet started.
  /// </summary>
  /// <remarks>Keeps last enumeration item after enumeration end.</remarks>
  /// <exception cref="NullReferenceException">For <c>default(<see cref="CapacitorStoreEnumerator{T}"/>)</c>.</exception>
  readonly public T? Current => index == resetIndex ? default ( T? ) : store [ index ];

  /// <summary>
  /// Current element or <c>default(<typeparamref name="T"/>)</c>, if enumeration is not yet started.
  /// </summary>
  /// <remarks>Keeps last enumeration item after enumeration end.</remarks>
  /// <exception cref="NullReferenceException">For <c>default(<see cref="CapacitorStoreEnumerator{T}"/>)</c>.</exception>
  readonly object? IEnumerator.Current => Current;

  /// <summary>
  /// Resets enumeration to its start.
  /// </summary>
  public void Reset () => index = resetIndex;
}
