using ApiFiotec.Models;

namespace ApiFiotec.Contracts;

public interface IRelatorioRepository
{
    Task<Relatorio> CriarRelatorioAsync(Relatorio relatorio);
}
