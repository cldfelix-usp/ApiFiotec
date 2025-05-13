using ApiFiotec.Contracts;
using ApiFiotec.Infraestruture.Data;
using ApiFiotec.Models;

namespace ApiFiotec.Repositories;

public class RelatorioRepository : IRelatorioRepository
{
    private readonly ApplicationDbContext _context;

    public RelatorioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Relatorio> CriarRelatorioAsync(Relatorio relatorio)
    {
        _context.Relatorios.Add(relatorio);
        await _context.SaveChangesAsync();
        return relatorio;
    }
}