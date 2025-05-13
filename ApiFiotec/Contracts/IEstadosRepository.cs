using ApiFiotec.Models;

namespace ApiFiotec.Contracts
{
    public interface IEstadosRepository
    {
        Task<List<Estado>> PegarTodosEstados(CancellationToken cancellationToken);
    }
}