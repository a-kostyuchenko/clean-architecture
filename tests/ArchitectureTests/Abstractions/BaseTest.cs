using System.Reflection;
using Domain;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = AssemblyReference.Assembly;
    protected static readonly Assembly ApplicationAssembly = global::Application.AssemblyReference.Assembly;
    protected static readonly Assembly InfrastructureAssembly = Infrastructure.AssemblyReference.Assembly;
}
