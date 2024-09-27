using OasysGH.Helpers;
using System.IO;
using System.Reflection;
using System;
using Xunit;
using Rhino.Runtime;
using Microsoft.Win32;
using System.Globalization;

namespace OasysGHTests.Helpers {
  public class ResolverTest {
    [Fact]
    public void ResolverReturnLatestInstalledRhinoPath() {
      string name = "SOFTWARE\\McNeel\\Rhinoceros";
      int initialRhinoMajorVersion = RhinoResolver.RhinoMajorVersion;
      double rhinoMajorVersion = -1;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name)) {
        string[] subKeyNames = registryKey.GetSubKeyNames();
        Array.Sort(subKeyNames);
        double.TryParse(subKeyNames[subKeyNames.Length - 1], NumberStyles.Any, CultureInfo.InvariantCulture, out rhinoMajorVersion);
      }
      RhinoResolver.Initialize();
      string expectedPath = $"C:\\Program Files\\Rhino {rhinoMajorVersion}\\System";
      Assert.Equal(RhinoResolver.RhinoSystemDirectory, expectedPath);
      RhinoResolver.RhinoMajorVersion = initialRhinoMajorVersion;
      RhinoResolver.RhinoSystemDirectory = "";
    }

    [Fact]
    public void ResolverReturnNoRhinoPathWhenRequestedVersionNotInstalled() {
      int initialRhinoMajorVersion = RhinoResolver.RhinoMajorVersion;
      RhinoResolver.Initialize();
      RhinoResolver.RhinoMajorVersion = 1;
      Assert.Null(RhinoResolver.RhinoSystemDirectory);
      RhinoResolver.RhinoMajorVersion = initialRhinoMajorVersion;
      RhinoResolver.RhinoSystemDirectory = "";
    }
  }
}
