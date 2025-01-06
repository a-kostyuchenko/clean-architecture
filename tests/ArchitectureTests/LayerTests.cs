using ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class LayerTests : BaseTest
{
    private const string DomainNamespace = "Domain";
    private const string ApplicationNamespace = "Application";
    private const string InfrastructureNamespace = "Infrastructure";
    private const string PresentationNamespace = "Presentation";
    private const string WebNamespace = "Web.API";
    
    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        string[] otherProjects =
        [
            ApplicationNamespace,
            InfrastructureNamespace,
            PresentationNamespace,
            WebNamespace
        ];

        Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        string[] otherProjects =
        [
            InfrastructureNamespace,
            PresentationNamespace,
            WebNamespace
        ];

        Types
            .InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult().ShouldBeSuccessful();
    }

    [Fact]
    public void Handlers_Should_Have_DependencyOnDomain()
    {
        Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        string[] otherProjects =
        [
            PresentationNamespace,
            WebNamespace
        ];

        Types
            .InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult()
            .ShouldBeSuccessful();
    }
}
