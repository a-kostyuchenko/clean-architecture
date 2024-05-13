using System.Reflection;

namespace Web.API;

public sealed class AssemblyReference
{
    internal static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
