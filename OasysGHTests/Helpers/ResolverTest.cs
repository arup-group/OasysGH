using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using Xunit;

namespace OasysGHTests.Helpers {
  public class ResolverTest {
    const string RhinoKey = "SOFTWARE\\McNeel\\Rhinoceros";
    [Fact]
    public void ShouldReturnAValidPath() {
      string directory = RhinoResolver.FindRhinoSystemDirectory();
      Assert.True(Directory.Exists(directory));
    }

    [Fact]
    public void SubKeyMissingShouldReturnFalse() {
      Assert.Throws<NullReferenceException>(() => RhinoResolver.GetSubKeys("INVALID_KEY"));
    }

    [Fact]
    public void ShouldHaveSomeKeys() {
      var rhinoSolver = new RhinoResolver();
      Assert.True(rhinoSolver.GetRhinoSubKeys().Length > 0);
    }

    [Fact]
    public void RhinoSystemDirectoryPropertyTest() {
      string directory = RhinoResolver.RhinoSystemDirectory;
      Assert.False(string.IsNullOrWhiteSpace(directory));
      Assert.True(Directory.Exists(directory));
      Assert.Contains("Rhino", directory, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void InitializeTest() {
      if (IntPtr.Size == 8) {
        RhinoResolver.Initialize();
      }
    }

    [Fact]
    public void GetSubKeysValidKeyTest() {
      string[] keys = RhinoResolver.GetSubKeys(RhinoKey);
      Assert.NotNull(keys);
      Assert.True(keys.Length > 0);
      for (int i = 0; i < keys.Length - 1; i++) {
        Assert.True(string.Compare(keys[i], keys[i + 1], StringComparison.Ordinal) <= 0);
      }
    }

    [Fact]
    public void ResolveForRhinoAssemblies_NonExistentAssembly_ReturnsNull() {
      var args = new ResolveEventArgs("NonExistentAssembly");
      MethodInfo method = typeof(RhinoResolver).GetMethod("ResolveForRhinoAssemblies",
        BindingFlags.NonPublic | BindingFlags.Static);
      Assert.NotNull(method);
      object result = method.Invoke(null, new object[] { null, args });
      Assert.True(result == null || result is Assembly);
    }

    [Fact]
    public void GetRhinoPathFromRegistryWithInvalidKeyReturnsNull() {
      MethodInfo method = typeof(RhinoResolver).GetMethod("GetRhinoPathFromRegistry",
        BindingFlags.NonPublic | BindingFlags.Static);
      Assert.NotNull(method);
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RhinoKey)) {
        if (registryKey != null) {
          string result = method.Invoke(null, new object[] { registryKey, "InvalidVersionKey" }) as string;
          Assert.Null(result);
        }
      }
    }

    [Fact]
    public void ResolveForRhinoAssembliesTest() {
      MethodInfo method = typeof(RhinoResolver).GetMethod("ResolveForRhinoAssemblies",
        BindingFlags.NonPublic | BindingFlags.Static);
      Assert.NotNull(method);
      Assert.NotNull(method.Invoke(null, new object[] { null, new ResolveEventArgs("Grasshopper") }));
      Assert.NotNull(method.Invoke(null, new object[] { null, new ResolveEventArgs("RhinoCommon") }));
      Assert.NotNull(method.Invoke(null, new object[] { null, new ResolveEventArgs("mscorlib") }));
      Assert.Null(method.Invoke(null, new object[] { null, new ResolveEventArgs("NonExistentAssembly") }));
    }

    [Fact]
    public void GetRhinoSystemDirTest() {
      MethodInfo method = typeof(RhinoResolver).GetMethod("GetRhinoSystemDir",
        BindingFlags.NonPublic | BindingFlags.Static);
      Assert.NotNull(method);
      Assert.NotNull(method.Invoke(null, new object[] { 7 }));
      Assert.NotNull(method.Invoke(null, new object[] { 1000 }));
    }

    [Fact]
    public void GetRhinoPathFromRegistryReturnsNullWhenRhinoVersionDoesNotExist() {
      MethodInfo method = typeof(RhinoResolver).GetMethod(
            "GetRhinoPathFromRegistry",
            BindingFlags.NonPublic | BindingFlags.Static);

      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(RhinoKey)) {
        object result = method.Invoke(null, new object[] { registryKey, "1.0" });
        Assert.Null(result);
      }
    }
  }
}
