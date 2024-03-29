﻿using System;
using System.IO;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Xunit;

namespace GH_UnitNumberTests.Helpers {
  internal class DocumentHelper {

    public static GH_Document CreateDocument(string path) {
      var io = new GH_DocumentIO();

      Assert.True(File.Exists(path));
      Assert.True(io.Open(path));

      io.Document.NewSolution(true);
      return io.Document;
    }

    public static GH_Component FindComponent(GH_Document doc, string groupIdentifier) {
      foreach (IGH_DocumentObject obj in doc.Objects) {
        if (obj is GH_Group group) {
          if (group.NickName == groupIdentifier) {
            Guid componentguid = group.ObjectIDs[0];

            foreach (IGH_DocumentObject obj2 in (doc.Objects)) {
              if (obj2.InstanceGuid == componentguid) {
                return (GH_Component)obj2;
              }
            }
          }
        }
      }

      return null;
    }

    public static IGH_Param FindParameter(GH_Document doc, string groupIdentifier) {
      foreach (IGH_DocumentObject obj in doc.Objects) {
        if (obj is GH_Group group) {
          if (group.NickName == groupIdentifier) {
            Guid componentguid = group.ObjectIDs[0];

            foreach (IGH_DocumentObject obj2 in (doc.Objects)) {
              if (obj2.InstanceGuid == componentguid) {
                return (IGH_Param)(object)obj2;
              }
            }
          }
        }
      }

      return null;
    }

    public static void TestNoRuntimeMessagesInDocument(GH_Document doc, GH_RuntimeMessageLevel runtimeMessageLevel, string exceptComponentNamed = "") {
      foreach (IGH_DocumentObject obj in doc.Objects) {
        if (obj is GH_Component comp) {
          comp.CollectData();
          comp.Params.Output[0].CollectData();
          comp.Params.Output[0].VolatileData.get_Branch(0);
          if (comp.Name != exceptComponentNamed)
            Assert.Empty(comp.RuntimeMessages(runtimeMessageLevel));
        }
      }
    }
  }
}
