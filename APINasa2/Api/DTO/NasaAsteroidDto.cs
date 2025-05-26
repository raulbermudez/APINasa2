namespace APINasa2.DTO
{
    using System.Text.Json.Serialization;
    using System.Collections.Generic;

    // Clase que representa la información de un asteroide individual recibida desde la API de la NASA.
    // Contiene los datos necesarios para saber si es peligroso, su tamaño estimado y
    // la información de su aproximación a la Tierra (fecha, velocidad, planeta).
    // Usamos esta clase para extraer los asteroides potencialmente peligrosos
    // y transformarlos en una versión simplificada que devuelve nuestra API.
    public class NasaAsteroidDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_potentially_hazardous_asteroid")]
        public bool IsPotentiallyHazardous { get; set; }

        [JsonPropertyName("estimated_diameter")]
        public EstimatedDiameter EstimatedDiameter { get; set; }

        [JsonPropertyName("close_approach_data")]
        public List<CloseApproachData> CloseApproachData { get; set; }
    }

    public class EstimatedDiameter
    {
        [JsonPropertyName("kilometers")]
        public DiameterKilometers Kilometers { get; set; }
    }

    public class DiameterKilometers
    {
        [JsonPropertyName("estimated_diameter_min")]
        public double Min { get; set; }

        [JsonPropertyName("estimated_diameter_max")]
        public double Max { get; set; }
    }

    public class CloseApproachData
    {
        [JsonPropertyName("close_approach_date")]
        public string CloseApproachDate { get; set; }

        [JsonPropertyName("relative_velocity")]
        public RelativeVelocity RelativeVelocity { get; set; }

        [JsonPropertyName("orbiting_body")]
        public string OrbitingBody { get; set; }
    }

    public class RelativeVelocity
    {
        [JsonPropertyName("kilometers_per_hour")]
        public string KilometersPerHour { get; set; }
    }

}
