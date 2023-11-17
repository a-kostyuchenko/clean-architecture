using System.Reflection;

namespace Presentation;

public class AssemblyReference
{
    internal static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}