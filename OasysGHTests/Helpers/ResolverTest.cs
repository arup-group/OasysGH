using OasysGH.Helpers;
using System.IO;
using System.Reflection;
using System;
using Xunit;
using Rhino.Runtime;
using Microsoft.Win32;
using System.Globalization;
using Xunit.Abstractions;

namespace OasysGHTests.Helpers {
  public class ResolverTest {

    [Fact]
    public void ShouldReturnAValidPath() {
      RhinoResolver.RhinoMajorVersion = -1;
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
  }
}
