using OasysUnits.Serialization.JsonNet;

namespace OasysGH.Components {
  public class OasysUnitsParameterExpirationManager : ParameterExpirationManager {
    public OasysUnitsParameterExpirationManager(int maxParamIndex) : base(maxParamIndex) {
      Converter = new OasysUnitsIQuantityJsonConverter();
    }
  }
}
