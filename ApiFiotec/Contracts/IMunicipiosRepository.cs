using ApiFiotec.Models;

namespace ApiFiotec.Contracts;

public interface IMunicipiosRepository
{
    Task<List<Municipio>> PegarTodosMunicipios();

    Task<List<Municipio>> PegarTodosMunicipiosPorEstadoAsync(uint estadoId, bool cancelationToken);
}
