using System;
using System.Reflection;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

static class Reflection
{
  const BindingFlags NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;

  static public FieldInfo GetNonPublicField ( Type ofType, string itsName )
    => ofType.GetField ( itsName, NonPublicInstance )!;

  static public object GetNonPublicFieldValue ( object of, string fieldName )
  => GetNonPublicField ( of.GetType (), fieldName ).GetValue ( of )!;

  static public object GetNonPublicFieldValue<Base> ( object of, string fieldName )
  => GetNonPublicField ( typeof ( Base ), fieldName ).GetValue ( of )!;
}
