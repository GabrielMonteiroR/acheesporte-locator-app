using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.GooglePlaces
{
    public class AutocompleteResponse
    {
        [JsonPropertyName("predictions")]
        public List<Prediction> Predictions { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class Prediction
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }
    }
}

    public class PlaceDetailsResponse
    {
        [JsonPropertyName("result")]
        public PlaceDetailsResult Result { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class PlaceDetailsResult
    {
        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        [JsonPropertyName("location")]
        public PlaceLocation Location { get; set; }
    }

    public class PlaceLocation
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }
