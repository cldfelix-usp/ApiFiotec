using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using ApiFiotec.Models;
using AutoMapper;

namespace ApiFiotec.Services;

public class SolicitanteService : ISolicitanteService
{
    private readonly IMapper _mapper;
    private readonly ISolicitanteRepository _solicitanteRepository;

    public SolicitanteService(IMapper mapper, ISolicitanteRepository solicitanteRepository)
    {
        _mapper = mapper;
        _solicitanteRepository = solicitanteRepository;
    }

    public async Task<SolicitanteResponseViewModel?> PegarSolicitantePorCpf(string cpf, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(cpf))
            return null;

        cpf = cpf.Trim();

        if (cpf.Length != 11)
            return null;

        var solicitante = await _solicitanteRepository.PegarSolicitantePorCpf(cpf, cancellationToken);
        return _mapper.Map<SolicitanteResponseViewModel>(solicitante);
    }


    public async Task<bool> SolicitanteJaCadastrado(string cpf, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(cpf))
            return false;

        cpf = cpf.Trim();

        if (cpf.Length != 11)
            return false;

        return await _solicitanteRepository.SolicitanteJaCadastrado(cpf, cancellationToken);
    }

    public async Task<List<SolicitanteResponseViewModel>> PegarTodosSolicitantes(CancellationToken cancellationToken)
    {
        // Se não houver solicitação de cancelamento, continue com a execução normal
        var solicitantes =  await _solicitanteRepository.PegarTodosSolicitantes(cancellationToken);
        var mapped = _mapper.Map<List<SolicitanteResponseViewModel>>(solicitantes);
        return mapped;
    }


    public async Task<SolicitanteResponseViewModel> CadastrarSolicitante(string cpf, string nome, CancellationToken cancellationToken)
    {
  
        var mappedSolicitante = _mapper.Map<Solicitante>(new SolicitanteRequestViewModel
        {
            Cpf = cpf,
            Nome = nome
        });


        var solicitanteMapped = await _solicitanteRepository.CadastrarSolicitante(mappedSolicitante, cancellationToken);
        var mapped = _mapper.Map<SolicitanteResponseViewModel>(solicitanteMapped);
        return mapped;
    }
}