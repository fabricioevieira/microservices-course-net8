using System.Reflection;

namespace Catalog.API;

/// <remarks>
/// This custom catalog is required because Carter's default assembly scanning mechanism fails to locate
/// the entry assembly when Carter is added as a dependency in a separate layer/project (such as BuildingBlocks).
/// Carter attempts to scan all assemblies in the dependency context, but when it's referenced indirectly,
/// it cannot properly resolve the entry assembly containing the actual endpoint modules.
/// By explicitly returning only the current assembly (Catalog.API), we ensure Carter discovers and registers
/// all modules defined within this project, bypassing the cross-layer assembly resolution issue.
/// </remarks>
public class DependencyContextAssemblyCatalogCustom : DependencyContextAssemblyCatalog
{
    private readonly Assembly _assembly;
    public DependencyContextAssemblyCatalogCustom(Assembly assembly)
    {
        _assembly = assembly;
    }
    public override IReadOnlyCollection<Assembly> GetAssemblies()
    {
        return new List<Assembly> { _assembly };
    }
}
