using System;
using System.Diagnostics.CodeAnalysis;

namespace Software9119.Collection.Superb.TestArrangement.TestAide;

[SuppressMessage ( "Design", "CA1032:Implement standard exception constructors", Justification = "Useless." )]
sealed class ShouldNotBeThrownException : Exception { }
