using ApiFiotec.Models;

namespace ApiFiotec.Contracts;

public interface ISolicitanteRepository
{
    Task<Solicitante?> PegarSolicitantePorCpf(string cpf);
    Task<bool> SolicitanteJaCadastrado(string cpf);
    Task<List<Solicitante>> PegarTodosSolicitantes();
    Task<Solicitante> CadastrarSolicitante(Solicitante solicitante);
}