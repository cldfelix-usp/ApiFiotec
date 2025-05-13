using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;

namespace ApiFiotec.Contracts;

public interface ISolicitanteService
{
    Task<SolicitanteResponseViewModel?> PegarSolicitantePorCpf(string cpf, CancellationToken cancellationToken);
    Task<bool> SolicitanteJaCadastrado(string cpf, CancellationToken cancellationToken);
    Task<List<SolicitanteResponseViewModel>> PegarTodosSolicitantes( CancellationToken cancellationToken);
    Task<SolicitanteResponseViewModel> CadastrarSolicitante(string cpf, string nome, CancellationToken cancellationToken);
}