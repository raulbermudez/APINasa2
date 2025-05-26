namespace APINasa2.Api.Controllers
{
    using APINasa2.Api.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("asteroids")]
    public class AsteroidsController : ControllerBase
    {
        private readonly AsteroidsService _service;

        public AsteroidsController(AsteroidsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? days)
        {
            if (!days.HasValue)
                return BadRequest(new { error = "El parámetro 'days' es obligatorio." });

            if (days < 1 || days > 7)
                return BadRequest(new { error = "El parámetro 'days' debe estar entre 1 y 7." });

            try
            {
                var result = await _service.GetTopHazardousAsteroidsAsync(days.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener datos de la NASA.", detail = ex.Message });
            }
        }
    }

}
