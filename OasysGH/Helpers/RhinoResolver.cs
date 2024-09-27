using Microsoft.Win32;
using System.Globalization;
using System.IO;
using System.Reflection;
using System;

public class RhinoResolver {
  private const string Coredllpath = "CoreDllPath";
  private static string rhinoSystemDirectory;
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

  private static Assembly ResolveForRhinoAssemblies(object sender, ResolveEventArgs args) {
    string name = new AssemblyName(args.Name).Name;
    string text = Path.Combine(RhinoSystemDirectory, name + ".dll");
    if (File.Exists(text)) {
      return Assembly.LoadFrom(text);
    }

    return null;
  }

  public static string FindRhinoSystemDirectory() {
    bool useLatest = RhinoMajorVersion < 0;

    string[] subKeyNames = GetSubKeys(RhinoKey);

    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RhinoKey)) {
      string text = string.Empty;
      for (int num = subKeyNames.Length - 1; num >= 0; num--) {
        if (double.TryParse(subKeyNames[num], NumberStyles.Any, CultureInfo.InvariantCulture, out double result)) {
          text = subKeyNames[num];
          if (useLatest || (int)Math.Floor(result) == RhinoMajorVersion) {
            using RegistryKey registryKey2 = registryKey.OpenSubKey(text + "\\Install");
            try {
              object value = registryKey2.GetValue(Coredllpath);
              if (value == null)
                continue;
              if (value is string path && File.Exists(path)) {
                return Path.GetDirectoryName(path);
              }
            }
            catch (Exception e) {
              Console.WriteLine(e);
            }
          }
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
