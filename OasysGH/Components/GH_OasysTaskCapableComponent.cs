using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using OasysGH.Helpers;

namespace OasysGH.Components
{
  public abstract class GH_OasysTaskCapableComponent<T> : GH_TaskCapableComponent<T>
  {
    public GH_OasysTaskCapableComponent(string name, string nickname, string description, string category, string subCategory) : base(name, nickname, description, category, subCategory)
    {
    }
    abstract public OasysPluginInfo PluginInfo { get; }

    public override void AddedToDocument(GH_Document document)
    {
      PostHog.AddedToDocument(this, PluginInfo);
      base.AddedToDocument(document);
    }

    public override void RemovedFromDocument(GH_Document document)
    {
      PostHog.RemovedFromDocument(this, PluginInfo);
      base.RemovedFromDocument(document);
    }
  }
}
