using System.Reflection;

namespace Infrastructure;

public class AssembleReference
{
    public class AssemblyReference
    {
        internal static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}