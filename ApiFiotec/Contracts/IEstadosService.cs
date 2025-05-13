using System.Reflection.Metadata.Ecma335;
using ApiFiotec.Application.Dtos.Response;

namespace ApiFiotec.Contracts
{
    public interface IEstadosService
    {
        Task<List<EstadoResponseViewModel>> PegarTodosEstados(CancellationToken cancellationToken);
    }
}