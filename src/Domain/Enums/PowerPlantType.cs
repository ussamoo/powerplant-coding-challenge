using System.Runtime.Serialization;

namespace Domain.Enums
{
    public enum PowerPlantType
    {
        [EnumMember(Value = "gasfired")]
        GasFired = 0,
        [EnumMember(Value = "turbojet")]
        Turbojet,
        [EnumMember(Value = "wind")]
        WindTurbine
    }
}
