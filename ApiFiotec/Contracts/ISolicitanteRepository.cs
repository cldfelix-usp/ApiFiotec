using ApiFiotec.Models;

namespace ApiFiotec.Contracts;

public interface ISolicitanteRepository
{
    Task<Solicitante?> PegarSolicitantePorCpf(string cpf, CancellationToken cancellationToken);
    Task<bool> SolicitanteJaCadastrado(string cpf, CancellationToken cancellationToken);
    Task<List<Solicitante>> PegarTodosSolicitantes(CancellationToken cancellationToken);
    Task<Solicitante> CadastrarSolicitante(Solicitante solicitante, CancellationToken cancellationToken);
}