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

    public async Task<Solicitante?> PegarSolicitantePorCpf(string cpf, CancellationToken cancellationToken)
    {
        // Verifica se o CPF é nulo ou vazio
        if (string.IsNullOrEmpty(cpf))
            return null;

        // Busca o solicitante no banco de dados
        var solicitante = await _context.Solicitantes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Cpf == cpf, cancellationToken);

        return solicitante;
    }


    public async Task<bool> SolicitanteJaCadastrado(string cpf, CancellationToken cancellationToken)
    {
        // Verifica se o CPF é nulo ou vazio
        if (string.IsNullOrEmpty(cpf))
            return false;

        // Verifica se o solicitante já está cadastrado no banco de dados
        var solicitante = await _context.Solicitantes
            .AsNoTracking()
            .AnyAsync(s => s.Cpf == cpf, cancellationToken);

        return solicitante;
    }


    public async Task<List<Solicitante>> PegarTodosSolicitantes(CancellationToken cancellationToken)    
    {
        // Busca todos os solicitantes no banco de dados
        var solicitantes = await _context.Solicitantes
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return solicitantes;
    }


    public async Task<Solicitante> CadastrarSolicitante(Solicitante solicitante, CancellationToken cancellationToken)
    {
        // Verifica se o solicitante é nulo
        ArgumentNullException.ThrowIfNull(solicitante);

        // Adiciona o solicitante ao banco de dados
        _context.Solicitantes.Add(solicitante);
        await _context.SaveChangesAsync(cancellationToken);
        return solicitante;
    }

}