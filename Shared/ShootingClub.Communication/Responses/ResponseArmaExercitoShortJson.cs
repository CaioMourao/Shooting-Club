namespace ShootingClub.Communication.Responses
{
    public class ResponseArmaExercitoShortJson : ResponseArmaShortJson
    {
        public DateOnly? ValidadeCRAF { get; set; }
        public DateOnly? ValidadeGTE { get; set; }
    }
}
