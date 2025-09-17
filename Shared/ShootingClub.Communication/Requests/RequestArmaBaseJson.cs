using ShootingClub.Communication.Enums;
using System.Text.Json.Serialization;

namespace ShootingClub.Communication.Requests
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(TipoPosse))]
    [JsonDerivedType(typeof(RequestArmaExercitoJson), typeDiscriminator: (int)TipoPosseArma.Exercito)]
    [JsonDerivedType(typeof(RequestArmaPFJson), typeDiscriminator: (int)TipoPosseArma.PoliciaFederal)]
    [JsonDerivedType(typeof(RequestArmaPorteJson), typeDiscriminator: (int)TipoPosseArma.PortePessoal)]
    public abstract class RequestArmaBaseJson
    {
        [JsonPropertyName("tipoPosse")]
        public virtual TipoPosseArma TipoPosse { get; protected set; }
        public string? Cpf_proprietario { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Calibre { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
    }
}
