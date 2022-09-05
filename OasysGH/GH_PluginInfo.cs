using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OasysGH
{
  public class GH_PluginInfo
  {
    public static string ProductName { get; set; } = "";
    public static string PluginName { get; set; } = "";
    public static string Version { get; set; } = "";
    public static bool IsBeta { get; set; } = false;
    public static string PostHogApiKey { get; set; } = "";
  }
}
