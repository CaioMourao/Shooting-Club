using ShootingClub.Communication.Enums;
using System.Text.Json.Serialization;

namespace ShootingClub.Communication.Responses
{
    public abstract class ResponseArmaShortJson
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoPosseArma TipoPosse { get; set; }
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string? Calibre { get; set; }
        public string NumeroSerie { get; set; } = string.Empty;
    }
}
