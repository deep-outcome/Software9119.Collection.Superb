using System.Reflection;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

static class Reflection
{
  const BindingFlags NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;

  static public FieldInfo GetNonPublicField ( object of, string itsName )
    => of.GetType ().GetField ( itsName, NonPublicInstance )!;

  static public object GetNonPublicFieldValue ( object of, string fieldName )
  => GetNonPublicField ( of, fieldName ).GetValue ( of )!;
}
