using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;

namespace ApiFiotec.Contracts
{
    public interface IRelatorioService
    {
        Task CriarRelatorioAsync(RelatorioRequestViewModel relatorioRequestViewModel, CancellationToken cancellationToken);
        Task<IEnumerable<RelatorioResponseViewModel>> GetRelatoriosAsync(CancellationToken cancellationToken);
    }
}