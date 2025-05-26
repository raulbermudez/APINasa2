namespace APINasa2.Test
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Xunit;
    using Moq;
    using Moq.Protected;
    using System.Threading;
    using System.Text.Json;
    using global::APINasa2.Api.Services;
    using global::APINasa2.DTO;

    namespace APINasa2.Tests.Services
    {
        public class AsteroidsServiceTests
        {
            [Fact]
            public async Task GetTopHazardousAsteroidsAsync_ReturnsTop3HazardousAsteroids()
            {
                // Arrange
                var mockResponseDto = new NasaApiResponseDto
                {
                    NearEarthObjects = new Dictionary<string, List<NasaAsteroidDto>>
                {
                    {
                        DateTime.UtcNow.ToString("yyyy-MM-dd"), new List<NasaAsteroidDto>
                        {
                            CreateHazardousAsteroid("Ast1", 0.5, 1.0, "50000", "Earth"),
                            CreateHazardousAsteroid("Ast2", 0.6, 1.1, "60000", "Mars"),
                            CreateHazardousAsteroid("Ast3", 0.7, 1.2, "70000", "Venus"),
                            CreateHazardousAsteroid("Ast4", 0.8, 1.3, "80000", "Jupiter")
                        }
                    }
                }
                };

                string jsonResponse = JsonSerializer.Serialize(mockResponseDto);

                var handlerMock = new Mock<HttpMessageHandler>();
                handlerMock
                   .Protected()
                   .Setup<Task<HttpResponseMessage>>(
                       "SendAsync",
                       ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>()
                   )
                   .ReturnsAsync(new HttpResponseMessage
                   {
                       StatusCode = HttpStatusCode.OK,
                       Content = new StringContent(jsonResponse)
                   });

                var httpClient = new HttpClient(handlerMock.Object);
                var service = new AsteroidsService(httpClient);

                // Act
                var result = await service.GetTopHazardousAsteroidsAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.Equal("Ast4", result[0].Nombre); // Mayor diámetro
                Assert.Equal("Ast3", result[1].Nombre);
                Assert.Equal("Ast2", result[2].Nombre);
            }

            private NasaAsteroidDto CreateHazardousAsteroid(string name, double minDia, double maxDia, string velocity, string planet)
            {
                return new NasaAsteroidDto
                {
                    Name = name,
                    IsPotentiallyHazardous = true,
                    EstimatedDiameter = new EstimatedDiameter
                    {
                        Kilometers = new DiameterKilometers
                        {
                            Min = minDia,
                            Max = maxDia
                        }
                    },
                    CloseApproachData = new List<CloseApproachData>
                    {
                        new CloseApproachData
                        {
                            RelativeVelocity = new RelativeVelocity
                            {
                                KilometersPerHour = velocity
                            },
                            CloseApproachDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                            OrbitingBody = planet
                        }
                    }
                };
            }
        }
    }
}
