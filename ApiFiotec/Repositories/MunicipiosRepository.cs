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

    public async Task<List<Municipio>> PegarTodosMunicipios()
    {
        var municipios = await _context.Municipios.ToListAsync();
        return municipios;

    }

    public async Task<List<Municipio>> PegarTodosMunicipiosPorEstadoAsync(uint estadoId, bool cancelationToken)
    {
        var municipios = await 
            _context.Municipios.Where(x => x.Estado.Id == estadoId).ToListAsync();
        
        return municipios;
    }
}