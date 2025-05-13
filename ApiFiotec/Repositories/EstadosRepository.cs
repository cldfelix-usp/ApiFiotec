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

    public async Task<List<Estado>> PegarTodosEstados()
    {
        var estados = await _context.Estados.ToListAsync();
        return estados;
    }
}
