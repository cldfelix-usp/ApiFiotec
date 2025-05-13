using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using ApiFiotec.Models;
using AutoMapper;

namespace ApiFiotec.Services;

public class SolicitanteService: ISolicitanteService
{
    private readonly IMapper _mapper;
    private readonly ISolicitanteRepository _solicitanteRepository;

    public SolicitanteService(IMapper mapper, ISolicitanteRepository solicitanteRepository)
    {
        _mapper = mapper;
        _solicitanteRepository = solicitanteRepository;
    }

    public async Task<SolicitanteResponseViewModel?> PegarSolicitantePorCpf(string cpf)
    {
        var solicitante = await _solicitanteRepository.PegarSolicitantePorCpf(cpf);
        var mapped = _mapper.Map<SolicitanteResponseViewModel>(solicitante);
        return mapped;
    }

    public async Task<bool> SolicitanteJaCadastrado(string cpf)
    {
        return await _solicitanteRepository.SolicitanteJaCadastrado(cpf);
    }

    public async Task<List<SolicitanteResponseViewModel>> PegarTodosSolicitantes()
    {
        var solicitantes = await _solicitanteRepository.PegarTodosSolicitantes();
        var mapped = _mapper.Map<List<SolicitanteResponseViewModel>>(solicitantes);
        return mapped;
    }

    public async Task<SolicitanteResponseViewModel> CadastrarSolicitante(string cpf, string nome)
    {
        var solicitante = new Solicitante
        {
            Cpf = cpf,
            Nome = nome
        };
        
        var solicitanteMapped  = await _solicitanteRepository.CadastrarSolicitante(solicitante);
        var mapped = _mapper.Map<SolicitanteResponseViewModel>(solicitanteMapped);
        return mapped;
    }
}