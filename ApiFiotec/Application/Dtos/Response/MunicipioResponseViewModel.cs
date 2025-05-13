namespace ApiFiotec.Application.Dtos.Response;

public class MunicipioResponseViewModel
{
    public uint Id { get; set; }
    public string NomeMunicipio{ get; set; }
    public EstadoResponseViewModel Estado { get; set; }
}