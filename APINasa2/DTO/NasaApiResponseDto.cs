namespace APINasa2.DTO
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    // Clase que representa la respuesta completa de la API de la NASA.
    // Contiene un diccionario donde la clave es una fecha (ej: "2025-05-23")
    // y el valor es una lista de asteroides detectados ese día.
    // Nos permite deserializar el JSON recibido para luego filtrar y transformar los datos.
    public class NasaApiResponseDto
    {
        [JsonPropertyName("near_earth_objects")]
        public Dictionary<string, List<NasaAsteroidDto>> NearEarthObjects { get; set; }
    }

}
