using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;

namespace ApiFiotec.Contracts;

public interface ISolicitanteService
{
    Task<SolicitanteResponseViewModel?> PegarSolicitantePorCpf(string cpf);
    Task<bool> SolicitanteJaCadastrado(string cpf);
    Task<List<SolicitanteResponseViewModel>> PegarTodosSolicitantes();
    Task<SolicitanteResponseViewModel> CadastrarSolicitante(string cpf, string nome);
}