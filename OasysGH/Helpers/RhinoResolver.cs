using Microsoft.Win32;
using System.Globalization;
using System.IO;
using System.Reflection;
using System;

public class RhinoResolver {
  private const string Coredllpath = "CoreDllPath";
  private static string rhinoSystemDirectory = string.Empty;
  const string RhinoKey = "SOFTWARE\\McNeel\\Rhinoceros";

  public static string RhinoSystemDirectory {
    get {
      if (string.IsNullOrWhiteSpace(rhinoSystemDirectory)) {
        rhinoSystemDirectory = FindRhinoSystemDirectory();
      }
      return rhinoSystemDirectory;
    }
    set { rhinoSystemDirectory = value; }
  }

  public static int RhinoMajorVersion { get; set; }


  public static void Initialize() {
    if (IntPtr.Size != 8) {
      throw new Exception("Only 64 bit applications can use Rhino");
    }

    RhinoMajorVersion = -1;
    AppDomain.CurrentDomain.AssemblyResolve += ResolveForRhinoAssemblies;
  }

  public static string FindRhinoSystemDirectory() {
    return GetRhinoSystemDir(GetExpectedRhinoPath());
  }

  private static int GetExpectedRhinoPath() {
    var currentAssembly = Assembly.GetExecutingAssembly();
    AssemblyName[] referencedAssemblies = currentAssembly.GetReferencedAssemblies();
    foreach (AssemblyName assemblyName in referencedAssemblies) {
      if (assemblyName.Name.Equals("RhinoCommon", StringComparison.OrdinalIgnoreCase)||
        assemblyName.Name.Equals("Grasshopper", StringComparison.OrdinalIgnoreCase)) {
        if (assemblyName.Version != null) {
          int majorVersion = assemblyName.Version.Major;
          return majorVersion;
        }
      }
    }
    return -1;
  }

  private static Assembly ResolveForRhinoAssemblies(object sender, ResolveEventArgs args) {
    var assemblyName = new AssemblyName(args.Name);
    string name = assemblyName.Name;

    // First try to load from Rhino System directory
    string systemPath = Path.Combine(RhinoSystemDirectory, name + ".dll");
    if (File.Exists(systemPath)) {
      return Assembly.LoadFrom(systemPath);
    }

    // Special handling for Grasshopper - try to find it in the Plug-ins folder
    if (name.Equals("Grasshopper", StringComparison.OrdinalIgnoreCase)) {
      string rhinoPath = Path.GetDirectoryName(RhinoSystemDirectory);
      string grasshopperPath = Path.Combine(rhinoPath, "Plug-ins", "Grasshopper", name + ".dll");
      if (File.Exists(grasshopperPath)) {
        return Assembly.LoadFrom(grasshopperPath);
      }
    }

    // Try to load from already loaded assemblies that match by name (version-agnostic)
    Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
    foreach (Assembly loadedAssembly in loadedAssemblies) {
      if (loadedAssembly.GetName().Name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
        return loadedAssembly;
      }
    }

    return null;
  }

  private static string GetRhinoSystemDir(int preferredMajorVersion) {
    string[] subKeyNames = GetSubKeys(RhinoKey);

    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RhinoKey)) {
      if (registryKey == null) return null;

      // If preferredMajorVersion is specified, look for that version first
      if (preferredMajorVersion > 0) {
        foreach (string keyName in subKeyNames) {
          if (double.TryParse(keyName, NumberStyles.Any, CultureInfo.InvariantCulture, out double version)) {
            int majorVersion = (int)Math.Floor(version);
            if (majorVersion == preferredMajorVersion) {
              string path = GetRhinoPathFromRegistry(registryKey, keyName);
              if (!string.IsNullOrEmpty(path)) {
                return path;
              }
            }
          }
        }
      }

      // Fall back to latest version (highest version number)
      for (int i = subKeyNames.Length - 1; i >= 0; i--) {
        if (double.TryParse(subKeyNames[i], NumberStyles.Any, CultureInfo.InvariantCulture, out _)) {
          string path = GetRhinoPathFromRegistry(registryKey, subKeyNames[i]);
          if (!string.IsNullOrEmpty(path)) {
            return path;
          }
        }
      }
    }

    return null;
  }

  private static string GetRhinoPathFromRegistry(RegistryKey baseKey, string versionKey) {
    using (RegistryKey installKey = baseKey.OpenSubKey(versionKey + "\\Install")) {
      if (installKey != null) {
        object value = installKey.GetValue(Coredllpath);
        if (value is string path && File.Exists(path)) {
          return Path.GetDirectoryName(path);
        }
      }
    }
    return null;
  }

  public string[] GetRhinoSubKeys() {
    return GetSubKeys(RhinoKey);
  }

  public static string[] GetSubKeys(string rhinoKey) {
    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(rhinoKey)) {
      string[] subKeyNames = registryKey.GetSubKeyNames();
      Array.Sort(subKeyNames);
      return subKeyNames;
    }
  }
}
