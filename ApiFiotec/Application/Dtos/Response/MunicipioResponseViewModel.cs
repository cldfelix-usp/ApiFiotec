namespace ApiFiotec.Application.Dtos.Response;

public class MunicipioResponseViewModel
{
    public uint Id { get; set; }
    public required string NomeMunicipio{ get; set; }
    public required EstadoResponseViewModel Estado { get; set; }
}