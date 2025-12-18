using Microsoft.Win32;
using System.Globalization;
using System.IO;
using System.Reflection;
using System;

public class RhinoResolver {
  private const string Coredllpath = "CoreDllPath";
  const string RhinoKey = "SOFTWARE\\McNeel\\Rhinoceros";
  private static Lazy<string> rhinoSystemDirectoryLazy = new Lazy<string>(FindRhinoSystemDirectory);

  public static string RhinoSystemDirectory {
    get {
      return rhinoSystemDirectoryLazy.Value;
    }
  }

  public static void Initialize() {
    AppDomain.CurrentDomain.AssemblyResolve += ResolveForRhinoAssemblies;
  }

  public static string FindRhinoSystemDirectory() {
    return GetRhinoSystemDir(GetRhinoVersion());
  }

  private static int GetRhinoVersion() {
    var currentAssembly = Assembly.GetExecutingAssembly();
    AssemblyName[] referencedAssemblies = currentAssembly.GetReferencedAssemblies();
    foreach (AssemblyName assemblyName in referencedAssemblies) {
      if (assemblyName.Version != null && (AssemblyNameEquals("RhinoCommon", assemblyName.Name) ||
         AssemblyNameEquals("Grasshopper", assemblyName.Name))) {
        int majorVersion = assemblyName.Version.Major;
        return majorVersion;
      }
    }
    return -1;
  }

  private static Assembly ResolveForRhinoAssemblies(object sender, ResolveEventArgs args) {
    var assemblyName = new AssemblyName(args.Name);
    string name = assemblyName.Name;
    Assembly assembly;
    if (TryLoadFromRhinoSystemDirectory(name, out assembly) || TryLoadGrasshopperAssembly(name, out assembly) || TryGetLoadedAssemblyByName(name, out assembly)) {
      return assembly;
    }
    return null;
  }

  private static bool TryLoadFrom(string path, out Assembly assembly) {
    assembly = null;
    if (File.Exists(path)) {
      assembly = Assembly.LoadFrom(path);
      return true;
    }
    return false;
  }

  private static bool TryLoadFromRhinoSystemDirectory(string name, out Assembly assembly) {
    string systemPath = Path.Combine(RhinoSystemDirectory, name + ".dll");
    return TryLoadFrom(systemPath, out assembly);
  }

  private static bool TryLoadGrasshopperAssembly(string name, out Assembly assembly) {
    assembly = null;
    if (AssemblyNameEquals(name, "Grasshopper")) {
      string rhinoPath = Path.GetDirectoryName(RhinoSystemDirectory);
      string grasshopperPath = Path.Combine(rhinoPath, "Plug-ins", "Grasshopper", name + ".dll");
      return TryLoadFrom(grasshopperPath, out assembly);
    }
    return false;
  }

  private static bool TryGetLoadedAssemblyByName(string name, out Assembly assembly) {
    assembly = null;
    Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
    foreach (Assembly loadedAssembly in loadedAssemblies) {
      if (AssemblyNameEquals(name, loadedAssembly.GetName().Name)) {
        assembly = loadedAssembly;
        return true;
      }
    }
    return false;
  }

  private static bool AssemblyNameEquals(string name, string assemblyName) {
    return assemblyName.Equals(name, StringComparison.OrdinalIgnoreCase);
  }

  private static string GetRhinoSystemDir(int preferredMajorVersion) {
    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RhinoKey)) {
      string preferedVersionPath = FindPreferredRhinoVersionPath(preferredMajorVersion, registryKey);
      if (!string.IsNullOrEmpty(preferedVersionPath)) {
        return preferedVersionPath;
      }
      return FindLatestRhinoVersionPath(registryKey);
    }
  }

  private static string FindLatestRhinoVersionPath(RegistryKey registryKey) {
    if (registryKey == null) return null;
    string[] subKeyNames = GetSubKeys(RhinoKey);
    for (int i = subKeyNames.Length - 1; i >= 0; i--) {
      if (double.TryParse(subKeyNames[i], NumberStyles.Any, CultureInfo.InvariantCulture, out _)) {
        return GetRhinoPathFromRegistry(registryKey, subKeyNames[i]);
      }
    }
    return null;
  }

  private static string FindPreferredRhinoVersionPath(int preferredMajorVersion, RegistryKey registryKey) {
    if (registryKey == null) return null;
    string[] subKeyNames = GetSubKeys(RhinoKey);
    if (preferredMajorVersion > 0) {
      foreach (string keyName in subKeyNames) {
        if (double.TryParse(keyName, NumberStyles.Any, CultureInfo.InvariantCulture, out double version)) {
          int majorVersion = (int)Math.Floor(version);
          if (majorVersion == preferredMajorVersion) {
            return GetRhinoPathFromRegistry(registryKey, keyName);
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
