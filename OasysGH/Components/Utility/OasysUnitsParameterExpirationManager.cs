using OasysUnits.Serialization.JsonNet;

namespace OasysGH.Components.Utility {
  public class OasysUnitsParameterExpirationManager : JsonExpirationManager {
    public OasysUnitsParameterExpirationManager() : base() {
      Converter = new OasysUnitsIQuantityJsonConverter();
    }
  }
}
