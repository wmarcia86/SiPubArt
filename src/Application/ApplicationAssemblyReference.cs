using System.Reflection;

namespace Application;

/// <summary>
/// Utility class. Provides a reference to the application assembly.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class ApplicationAssemblyReference
{
    internal static readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
}
