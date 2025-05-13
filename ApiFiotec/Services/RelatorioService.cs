using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using ApiFiotec.Models;
using AutoMapper;

namespace ApiFiotec.Services;

public class RelatorioService : IRelatorioService
{
    private readonly IRelatorioRepository _relatorioRepository;
    private readonly IMapper _mapper;

    public RelatorioService(
        IRelatorioRepository relatorioRepository, 
        IMapper mapper
        )
    {
        _relatorioRepository = relatorioRepository;
        _mapper = mapper;
    }
    public async Task CriarRelatorioAsync(RelatorioRequestViewModel relatorioRequestViewModel, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<Relatorio>(relatorioRequestViewModel);
         await _relatorioRepository.CriarRelatorioAsync(mapped, cancellationToken);
    }




    public async Task<IEnumerable<RelatorioResponseViewModel>> GetRelatoriosAsync(CancellationToken cancellationToken)
    {
        var relatorios = await _relatorioRepository.GetRelatoriosAsync(cancellationToken);
        var mapped = _mapper.Map<IEnumerable<RelatorioResponseViewModel>>(relatorios);
        return mapped;
    }
}
