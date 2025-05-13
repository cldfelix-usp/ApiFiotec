using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Contracts;
using ApiFiotec.Models;
using AutoMapper;

namespace ApiFiotec.Services;

public class RelatorioService : IRelatoriosService
{
    private readonly IRelatorioRepository _relatorioRepository;
    private readonly IMapper _mapper;

    public RelatorioService(IRelatorioRepository relatorioRepository, IMapper mapper)
    {
        _relatorioRepository = relatorioRepository;
        _mapper = mapper;
    }



    public Task CriarRelatorioAsync(RelatorioRequestViewModel relatorioRequestViewModel)
    {
        var mapped = _mapper.Map<Relatorio>(relatorioRequestViewModel);

        return _relatorioRepository.CriarRelatorioAsync(mapped);
    }
}
