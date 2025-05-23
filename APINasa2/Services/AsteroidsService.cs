namespace APINasa2.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using APINasa2.DTO;

    public class AsteroidsService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "DEMO_KEY";

        public AsteroidsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método principal que devolvera los 3 más peligrosos
        public async Task<List<AsteroidDto>> GetTopHazardousAsteroidsAsync(int days)
        {
            DateTime startDate = DateTime.UtcNow.Date;
            DateTime endDate = startDate.AddDays(days);

            string json = await GetNasaApiDataAsync(startDate, endDate);
            NasaApiResponseDto nasaData = DeserializeNasaResponse(json);
            List<NasaAsteroidDto> hazardousAsteroids = GetHazardousAsteroids(nasaData);

            List<AsteroidDto> mappedAsteroids = hazardousAsteroids
            .Select(MapToAsteroidDto)
            .OrderByDescending(a => a.Diametro)
            .ToList();

            // Si hay mas de 3 asteroides tengo que filtrar los 3 más peligrosos
            if (mappedAsteroids.Count > 3)
            {
                mappedAsteroids = mappedAsteroids.Take(3).ToList();
            }


            return mappedAsteroids;
        }

        // Método que obtiene los datos de la API de NASA
        private async Task<string> GetNasaApiDataAsync(DateTime startDate, DateTime endDate)
        {
            string url = $"https://api.nasa.gov/neo/rest/v1/feed?start_date={startDate:yyyy-MM-dd}&end_date={endDate:yyyy-MM-dd}&api_key={ApiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error al obtener datos de la API de NASA.");
            }

            string json = await response.Content.ReadAsStringAsync();
            return json;
        }

        // Método que deserializa la respuesta JSON de la API de NASA
        private NasaApiResponseDto DeserializeNasaResponse(string json)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            NasaApiResponseDto nasaResponse = JsonSerializer.Deserialize<NasaApiResponseDto>(json, options);
            return nasaResponse ?? new NasaApiResponseDto { NearEarthObjects = new Dictionary<string, List<NasaAsteroidDto>>() };
        }

        // Método que filtra los asteroides peligrosos de la respuesta de la API
        private List<NasaAsteroidDto> GetHazardousAsteroids(NasaApiResponseDto response)
        {
            List<NasaAsteroidDto> allHazardousAsteroids = new List<NasaAsteroidDto>();

            foreach (KeyValuePair<string, List<NasaAsteroidDto>> entry in response.NearEarthObjects)
            {
                List<NasaAsteroidDto> dailyAsteroids = entry.Value;

                List<NasaAsteroidDto> hazardous = dailyAsteroids
                    .Where(a => a.IsPotentiallyHazardous && a.CloseApproachData.Any())
                    .ToList();

                allHazardousAsteroids.AddRange(hazardous);
            }

            return allHazardousAsteroids;
        }

        // Método que mapea un asteroide de la API de NASA a nuestro AsteroidDto
        private AsteroidDto MapToAsteroidDto(NasaAsteroidDto asteroid)
        {
            CloseApproachData approachData = asteroid.CloseApproachData.First();

            double minDiameter = asteroid.EstimatedDiameter.Kilometers.Min;
            double maxDiameter = asteroid.EstimatedDiameter.Kilometers.Max;
            double averageDiameter = (minDiameter + maxDiameter) / 2;

            double velocity = 0.0;
            bool parseSuccess = double.TryParse(approachData.RelativeVelocity.KilometersPerHour, out velocity);

            return new AsteroidDto
            {
                Nombre = asteroid.Name,
                Diametro = averageDiameter,
                Velocidad = parseSuccess ? velocity : 0.0,
                Fecha = approachData.CloseApproachDate,
                Planeta = approachData.OrbitingBody
            };
        }
    }

}
