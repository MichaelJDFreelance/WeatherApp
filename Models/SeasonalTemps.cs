using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherApp.Models
{
    public class SeasonalTemps
    {
        [BsonElement("_id")]
        public string Season  { get; set; }
        public double Average  { get; set; }
    }
}