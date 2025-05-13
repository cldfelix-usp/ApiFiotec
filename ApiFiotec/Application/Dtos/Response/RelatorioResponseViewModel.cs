namespace ApiFiotec.Application.Dtos.Response;

public class RelatorioResponseViewModel
{
    public Guid Id { get; set; }

    public DateTime Data { get; set; }

    public required string Arbovirose { get; set; }

    public Guid SolicitanteId { get; set; }

    public int SemanaInicio { get; set; }
    
    public int SemanaTermino { get; set; }

    public int CodigoIbge { get; set; }
    
    public required string Municipio { get; set; }

    public required string DadosRelatorio { get; set; }
}