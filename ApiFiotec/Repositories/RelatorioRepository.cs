using ApiFiotec.Contracts;
using ApiFiotec.Infraestruture.Data;
using ApiFiotec.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFiotec.Repositories;

public class RelatorioRepository : IRelatorioRepository
{
    private readonly ApplicationDbContext _context;

    public RelatorioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Relatorio> CriarRelatorioAsync(Relatorio relatorio, CancellationToken cancellationToken)
    {
        await _context.Relatorios.AddAsync(relatorio, cancellationToken);
        await _context.SaveChangesAsync();
        return relatorio;
    }

    public async Task<IEnumerable<Relatorio>> GetRelatoriosAsync(CancellationToken cancellationToken)
    {
        return await _context.Relatorios
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    }
}