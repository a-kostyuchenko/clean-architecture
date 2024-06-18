using System.Reflection;
using ArchitectureTests.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;
using SharedKernel;

namespace ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainEvents_Should_HaveDomainEventPostfix()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        IEnumerable<Type>? entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .And()
            .AreNotAbstract()
            .GetTypes();

        var failingTypes = new List<Type>();
        foreach (Type? entityType in entityTypes)
        {
            ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.NonPublic |
                                                                        BindingFlags.Instance);

            if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }
}
