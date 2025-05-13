using ApiFiotec.Application.Dtos.Response;

namespace ApiFiotec.Contracts;

public interface IMunicipiosService
{
    Task<List<MunicipioResponseViewModel>> PegarTodosMunicipios(CancellationToken cancellationToken);
    Task<List<MunicipioResponseViewModel>> PegarTodosMunicipiosPorEstadoAsync(uint estadoId, CancellationToken cancelationToken);
}