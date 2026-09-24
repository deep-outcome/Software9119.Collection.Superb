using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

static class Reflection
{
  public const BindingFlags NonPubInst = BindingFlags.NonPublic | BindingFlags.Instance;
  public const BindingFlags PubInst =  BindingFlags.Public | BindingFlags.Instance;

  static public FieldInfo GetNonPublicField ( Type ofType, string itsName )
    => ofType.GetField ( itsName, NonPubInst )!;

  static public object GetNonPublicFieldValue ( object of, string fieldName )
  => GetNonPublicField ( of.GetType (), fieldName ).GetValue ( of )!;

  static public object GetNonPublicFieldValue<Base> ( object of, string fieldName )
  => GetNonPublicField ( typeof ( Base ), fieldName ).GetValue ( of )!;

  static public object? NonVirtualBaseCall
  (
    Type baseType,
    Type returnType,
    object on,
    string methodName,
    BindingFlags bindingFlags
  )
  {
    MethodInfo? baseMethod = baseType.GetMethod ( methodName, bindingFlags)
      ?? throw new ArgumentException ( $"Method '{methodName}' not found on base type '{baseType.Name}'." );

    Type onType = on.GetType();

    string name = $"CallNonVirtBase_{baseType.Name}_{methodName}";
    DynamicMethod dynMethod = new (name, returnType, [onType], onType, true);

    ILGenerator il = dynMethod.GetILGenerator();
    il.Emit ( OpCodes.Ldarg_0 );
    il.Emit ( OpCodes.Call, baseMethod );
    il.Emit ( OpCodes.Ret );

    Type deltype = typeof ( Func<,>).MakeGenericType(onType, returnType);
    Delegate @delegate = dynMethod.CreateDelegate ( deltype );

    return @delegate.DynamicInvoke ( on );
  }
}
