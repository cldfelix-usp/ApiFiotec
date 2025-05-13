namespace ApiFiotec.Application.Dtos.Response;

public class SolicitanteResponseViewModel
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Cpf { get; set; }
}