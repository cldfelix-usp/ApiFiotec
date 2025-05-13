using ApiFiotec.Models;

namespace ApiFiotec.Contracts;

public interface IMunicipiosRepository
{
    Task<List<Municipio>> PegarTodosMunicipios(CancellationToken cancellationToken);

    Task<List<Municipio>> PegarTodosMunicipiosPorEstadoAsync(uint estadoId, CancellationToken cancellationToken);
}
