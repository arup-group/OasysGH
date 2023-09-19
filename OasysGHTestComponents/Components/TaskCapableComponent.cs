using System;
using Grasshopper.Kernel;
using static OasysGHTestComponents.OasysGHTestComponentsInfo;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OasysGH.Components.Tests {
  public class TaskCapableComponent : GH_OasysTaskCapableComponent<TaskCapableComponent.SolveResults> {
    public override Guid ComponentGuid => new Guid("78c36d29-358f-4232-8c6e-1d16a7129a7b");
    public override GH_Exposure Exposure => GH_Exposure.hidden;
    public override OasysPluginInfo PluginInfo => OasysGHTestComponentsPluginInfo.Instance;

    public class SolveResults {
      internal ConcurrentBag<int> Integers { get; set; }
    }

    public TaskCapableComponent()
      : base("Task Capable", "TCC", "A Task Capable Component", "OasysGH", "Test") { }

    protected override void RegisterInputParams(GH_InputParamManager pManager) {
      pManager.AddBooleanParameter("Dummy", "D", "A dummy input", GH_ParamAccess.item, true);
    }
    protected override void RegisterOutputParams(GH_OutputParamManager pManager) {
      pManager.AddIntegerParameter("Length", "L", "The selected length unit", GH_ParamAccess.list);
    }

    protected override void SolveInstance(IGH_DataAccess data) {
      bool solve = false;
      if (InPreSolve) {
        data.GetData(0, ref solve);

        Task<SolveResults> tsk = null;
        tsk = Task.Run(() => Compute(), CancelToken);

        TaskList.Add(tsk);
        return;
      }

      if (!GetSolveResults(data, out SolveResults results)) {
        data.GetData(0, ref solve);
        results = Compute();
      }

      if (results is null) {
        return;
      }

      data.SetDataList(0, results.Integers);
    }

    private SolveResults Compute() {
      var results = new SolveResults();
      results.Integers.Add(0);
      results.Integers.Add(1);
      results.Integers.Add(2);
      return results;
    }
  }
}
