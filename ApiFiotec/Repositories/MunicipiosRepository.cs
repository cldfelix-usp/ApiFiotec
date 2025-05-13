using ApiFiotec.Contracts;
using ApiFiotec.Infraestruture.Data;
using ApiFiotec.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFiotec.Repositories;

public class MunicipiosRepository : IMunicipiosRepository
{
    private readonly ApplicationDbContext _context;

    public MunicipiosRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Municipio>> PegarTodosMunicipios( CancellationToken cancelationToken)
    {
        var municipios = await _context.Municipios
        .AsNoTracking()
        .ToListAsync(cancelationToken);
        return municipios;
    }


    public async Task<List<Municipio>> PegarTodosMunicipiosPorEstadoAsync(uint estadoId, CancellationToken cancellationToken)
    {
        var municipios = await 
            _context.Municipios
            .AsNoTracking()
            .Where(x => x.Estado.Id == estadoId).ToListAsync();
        
        return municipios;
    }
}