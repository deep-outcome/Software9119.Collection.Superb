namespace Software9119.Collection.Superb.Storing;

/// <summary>
/// Policy applied to Pre Capacitation and Batch Capacitation.
/// </summary>
/// <remarks>
/// Policy applied when storing batch of items with obtainable size or when requesting extra capacity room.
/// </remarks>
public enum CapacitationPolicy
{
  /// <summary>
  /// If needed, target capacity is ensured exactly up to needed space for new items or requested room.
  /// </summary>
  /// <remarks>Can be more space wise but can induce extra copying on sub-sequent items storing.</remarks>
  StaticJump,
  /// <summary>
  /// If needed, target capacity is ensured with precomputed auto-grow size, same size as reached by gradual auto-grow.
  /// </summary>
  /// <remarks>High probability of spare space for sub-sequent storing.</remarks>
  GeometricJump,
}