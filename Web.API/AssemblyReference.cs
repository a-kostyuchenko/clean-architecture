using System.Reflection;

namespace Web.API;

public class AssemblyReference
{
    internal static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}