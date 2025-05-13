using ApiFiotec.Contracts;
using ApiFiotec.Infraestruture.Data;
using ApiFiotec.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFiotec.Repositories;

public class EstadosRepository : IEstadosRepository
{
    private readonly ApplicationDbContext _context;

    public EstadosRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Estado>> PegarTodosEstados(CancellationToken cancellationToken)
    {
        var estados = await _context.Estados
        .AsNoTracking()
        .ToListAsync(cancellationToken);
        return estados;
    }
}
