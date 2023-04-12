using System;
using System.IO;
using Xunit;

namespace Rhino.Test {
  [CollectionDefinition("GrasshopperFixture collection")]
  public class GrasshopperCollection : ICollectionFixture<GrasshopperFixture> {
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
  }

  public class GrasshopperFixture : IDisposable {
    public Rhino.Runtime.InProcess.RhinoCore Core {
      get {
        if (null == _core) InitializeCore();
        return _core as Rhino.Runtime.InProcess.RhinoCore;
      }
    }

    public Grasshopper.Kernel.GH_DocumentIO DocIO {
      get {
        if (null == _DocIO) InitializeDocIO();
        return _DocIO as Grasshopper.Kernel.GH_DocumentIO;
      }
    }

    public Grasshopper.Plugin.GH_RhinoScriptInterface GHPlugin {
      get {
        if (null == _gHPlugin) InitializeGrasshopperPlugin();
        return _gHPlugin as Grasshopper.Plugin.GH_RhinoScriptInterface;
      }
    }

    private object _Doc { get; set; }
    private object _DocIO { get; set; }
    private static string linkFileName = "GH_UnitNumberTests.ghlink";
    private static string linkFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Grasshopper", "Libraries");
    private object _core = null;
    private object _gHPlugin = null;
    private bool _isDisposed;

    static GrasshopperFixture() {
      // This MUST be included in a static constructor to ensure that no Rhino DLLs
      // are loaded before the resolver is set up. Avoid creating other static functions
      // and members which may reference Rhino assemblies, as that may cause those
      // assemblies to be loaded before this is called.
      RhinoInside.Resolver.Initialize();
    }

    public GrasshopperFixture() {
      AddPluginToGH();

      InitializeCore();

      // setup headless units
      OasysGH.Units.Utility.SetupUnitsDuringLoad(true);
    }

    public void AddPluginToGH() {
      Directory.CreateDirectory(linkFilePath);
      StreamWriter writer = File.CreateText(Path.Combine(linkFilePath, linkFileName));
      writer.Write(Environment.CurrentDirectory);
      writer.Close();
    }

    public void Dispose() {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
      File.Delete(Path.Combine(linkFilePath, linkFileName));
    }

    protected virtual void Dispose(bool disposing) {
      if (_isDisposed) return;
      if (disposing) {
        _Doc = null;
        _DocIO = null;
        GHPlugin.CloseAllDocuments();
        _gHPlugin = null;
        Core.Dispose();
      }

      // TODO: free unmanaged resources (unmanaged objects) and override finalizer
      // TODO: set large fields to null
      _isDisposed = true;
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~GrasshopperFixture()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }
    private void InitializeCore() {
      _core = new Rhino.Runtime.InProcess.RhinoCore();
    }

    private void InitializeDocIO() {
      // we do this in a seperate function to absolutely ensure that the core is initialized before we load the GH plugin,
      // which will happen automatically when we enter the function containing GH references
      if (null == _gHPlugin) InitializeGrasshopperPlugin();
      InitializeDocIO2();
    }

    private void InitializeDocIO2() {
      var docIO = new Grasshopper.Kernel.GH_DocumentIO();
      _DocIO = docIO;
    }

    private void InitializeGrasshopperPlugin() {
      if (null == _core) InitializeCore();
      // we do this in a seperate function to absolutely ensure that the core is initialized before we load the GH plugin,
      // which will happen automatically when we enter the function containing GH references
      InitializeGrasshopperPlugin2();
    }

    private void InitializeGrasshopperPlugin2() {
      _gHPlugin = Rhino.RhinoApp.GetPlugInObject("Grasshopper");
      var ghp = _gHPlugin as Grasshopper.Plugin.GH_RhinoScriptInterface;
      ghp.RunHeadless();
    }
  }
}
