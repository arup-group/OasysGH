using OasysUnits.Serialization.JsonNet;

namespace OasysGH.Components {
  public class OasysUnitsParameterExpirationManager : ParameterExpirationManager {
    public OasysUnitsParameterExpirationManager() : base() {
      Converter = new OasysUnitsIQuantityJsonConverter();
    }
  }
}
