using ApiFiotec.Contracts;
using ApiFiotec.Infraestruture.Data;
using ApiFiotec.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFiotec.Repositories;

public class SolicitanteRepository : ISolicitanteRepository
{   
    private readonly ApplicationDbContext _context;

    public SolicitanteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Solicitante?> PegarSolicitantePorCpf(string cpf)
    {
        var solicitante = await _context.Solicitantes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Cpf == cpf);

        return solicitante;
    }

    public async Task<bool> SolicitanteJaCadastrado(string cpf)
    {
        var solicitante = await _context.Solicitantes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Cpf == cpf);

        return solicitante != null;
    }

    public async Task<List<Solicitante>> PegarTodosSolicitantes()
    {
        var solicitantes = await _context.Solicitantes
            .AsNoTracking()
            .ToListAsync();

        return solicitantes;
    }

    public async Task<Solicitante> CadastrarSolicitante(Solicitante solicitante)
    {
        _context.Solicitantes.Add(solicitante);
        await _context.SaveChangesAsync();
        return solicitante;
    }
}