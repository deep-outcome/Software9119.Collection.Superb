using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

readonly struct ValueWithReference ( int value ) : System.IEquatable<ValueWithReference>
{
  public int Value => value;

  public object Obj { get; } = new object ();

  readonly bool initialized = true;

  static public implicit operator ValueWithReference ( int value ) => new ( value );

  public bool IsInitialized () => initialized;
  public bool IsUninitialized () => !initialized;

  static public bool IsInitialized ( ValueWithReference vwr ) => vwr.initialized;
  static public bool IsUninitialized ( ValueWithReference vwr ) => !vwr.initialized;

  override public bool Equals ( [NotNullWhen ( true )] object? obj ) => obj is ValueWithReference vwr && Equals ( vwr );

  public bool Equals ( ValueWithReference other ) => other.Value == value;

  override public int GetHashCode () => value.GetHashCode ();
}
