using ApiFiotec.Application.Dtos.Response;

namespace ApiFiotec.Contracts;

public interface IMunicipiosService
{
    Task<List<MunicipioResponseViewModel>> PegarTodosMunicipios();
    Task<List<MunicipioResponseViewModel>> PegarTodosMunicipiosPorEstadoAsync(uint estadoId, bool cancelationToken);
}