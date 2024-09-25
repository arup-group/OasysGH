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
      double rhinoMajorVersion = -1;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name)) {
        string[] subKeyNames = registryKey.GetSubKeyNames();
        Array.Sort(subKeyNames);
        double.TryParse(subKeyNames[subKeyNames.Length - 1], NumberStyles.Any, CultureInfo.InvariantCulture, out rhinoMajorVersion);
      }
      RhinoResolver.Initialize();
      string expectedPath = "C:\\Program Files\\Rhino " + rhinoMajorVersion.ToString() + "\\System";
      Assert.Equal(RhinoResolver.RhinoSystemDirectory, expectedPath);
    }

    [Fact]
    public void ResolverReturnNoRhinoPathWhenRequestedVersionNotInstalled() {
      RhinoResolver.RhinoSystemDirectory = "";
      RhinoResolver.Initialize();
      RhinoResolver.RhinoMajorVersion = 1;
      Assert.Null(RhinoResolver.RhinoSystemDirectory);
    }
  }
}
