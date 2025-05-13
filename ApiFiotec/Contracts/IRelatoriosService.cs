using ApiFiotec.Application.Dtos.Request;

namespace ApiFiotec.Contracts
{
    public interface IRelatoriosService
    {
        Task CriarRelatorioAsync(RelatorioRequestViewModel relatorioRequestViewModel);
      
    }
}