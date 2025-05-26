namespace APINasa2.DTO
{
    // Este dto sera el json final que se devolvera al buscar el endpoint
    public class AsteroidDto
    {
        public string Nombre { get; set; }
        public double Diametro { get; set; }
        public double Velocidad { get; set; }
        public string Fecha { get; set; }
        public string Planeta { get; set; }
    }

}
